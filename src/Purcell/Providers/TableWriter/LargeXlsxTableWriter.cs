using LargeXlsx;
using SharpCompress.Compressors.Deflate;
using DateTimeConverter = PurcellLibs.Converters.DateTimeConverter;

namespace PurcellLibs.Providers.TableWriter;

/// <summary>
/// 使用 LargeXlsx 实现的 Xlsx 写入器
/// </summary>
internal class LargeXlsxTableWriter : TableWriterBase
{
    private int _disposed;
    private readonly XlsxWriter _writer;

    private readonly XlsxStyle _dateFmt = XlsxStyle.Default.With(new XlsxNumberFormat("yyyy-mm-dd HH:mm:ss"))
        .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left));

    /// <inheritdoc/>
    public LargeXlsxTableWriter(Stream stream, bool ownsStream = false)
        : base(stream, ownsStream)
    {
        _writer = new XlsxWriter(stream, CompressionLevel.BestSpeed, false, false);
    }

    /// <inheritdoc/>
    public override void WriteTable(IList<PurTable> tableConfigs,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        if (tableConfigs == null || tableConfigs.Count == 0)
            throw new ArgumentNullException(nameof(tableConfigs), "工作表数据集合不能为空");

        foreach ((PurTable tableConfig, int sheetIndex) in tableConfigs.Select((v, i) => (v, i)))
        {
            cancelToken.ThrowIfCancellationRequested(); // 检查任务取消

            CellLocator headerStart = tableConfig.GetHeaderStart();
            PurStyle tableStyle = tableConfig.GetActualStyle();
            
            // xlsx列宽
            List<XlsxColumn> xlsxColumns = tableConfig.CombinedColumns
                .Select(ec =>
                    XlsxColumn.Formatted(Math.Max(ec.Width, tableStyle.MinColumnWidth), hidden: ec.IsHidden))
                .ToList();

            // 把跳列部分包含到列集合中
            xlsxColumns.InsertRange(0,
                Enumerable.Repeat(XlsxColumn.Formatted(tableStyle.MinColumnWidth), headerStart.ColumnIndex)
                    .ToList());

            // 开始写工作表: 先设置Sheet名称、xlsx列宽配置
            _writer.BeginWorksheet(string.IsNullOrEmpty(tableConfig.SheetName) ? $"Sheet{sheetIndex + 1}" : tableConfig.SheetName,
                columns: xlsxColumns);

            // 输出表头，先跳行，再设置表头样式，再开始行，再跳列，最后循环输出表头
            if (tableConfig.HasHeader)
            {
                _writer
                    .SkipRows(headerStart.RowIndex)
                    .SetDefaultStyle(tableStyle.HeaderStyle)
                    .BeginRow()
                    .SkipColumns(headerStart.ColumnIndex);
                foreach (string colName in tableConfig.CombinedColumns.Select(ec => ec.PrimaryName ?? string.Empty))
                {
                    _writer.Write(colName);
                }
            }

            // 设置内容样式
            _writer.SetDefaultStyle(tableStyle.ContentStyle);

            // 核心：输出内容
            WriteRowData(tableConfig, sheetIndex, progress, cancelToken);

            // 设置自动筛选
            if (tableConfig.AutoFilter)
            {
                try
                {
                    _writer.SetAutoFilter(headerStart.RowIndex + 1, headerStart.ColumnIndex + 1,
                        1, tableConfig.CombinedColumns.Count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            // 设置密码保护
            if (!string.IsNullOrEmpty(tableConfig.Password))
            {
                _writer.SetSheetProtection(new XlsxSheetProtection(tableConfig.Password));
            }
        }
    }

    /// <summary>
    /// 写入行数据到工作表
    /// </summary>
    /// <param name="tableConfig">工作表数据</param>
    /// <param name="sheetIndex">工作表索引</param>
    /// <param name="progress">进度报告回调（可选）</param>
    /// <param name="cancelToken">取消令牌（可选）</param>
    private void WriteRowData(PurTable tableConfig, int sheetIndex,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        ArgumentNullException.ThrowIfNull(tableConfig.Records);

        CellLocator dataStart = tableConfig.GetDataStart();
        int dataIndex = 0; // 数据行计数器
        foreach (IDictionary<string, object?>? rowItem in tableConfig.Records)
        {
            cancelToken.ThrowIfCancellationRequested(); // 检查任务取消
            if (dataIndex % 100 == 0) progress?.Report(WritePosition.New(sheetIndex, dataIndex)); // 报告进度

            if (tableConfig.MaxWriteRows >= 0 && dataIndex >= tableConfig.MaxWriteRows)
                break; // 如果已达到最大写出行数，则停止写入

            // 跳过空行
            if (rowItem == null)
            {
                _writer.SkipRows(1);
                dataIndex++; // 增加数据行计数器
                continue;
            }

            // 开始新行并跳过指定列
            _writer.BeginRow().SkipColumns(dataStart.ColumnIndex);

            // 遍历每一列
            foreach (PurColumn excelColumn in tableConfig.CombinedColumns)
            {
                cancelToken.ThrowIfCancellationRequested(); // 检查任务取消

                // 跳过无属性名的列
                if (string.IsNullOrEmpty(excelColumn.PropertyName))
                {
                    _writer.SkipColumns(1);
                    continue;
                }

                // 获取单元格值
                object? cellValue = rowItem[excelColumn.PropertyName];

                // 跳过空值
                if (cellValue == null || cellValue == DBNull.Value || string.IsNullOrEmpty(cellValue.ToString()))
                {
                    // _writer.SkipColumns(1);
                    _writer.Write();
                    continue;
                }

                // 根据不同数据类型写入单元格
                WriteCellValue(cellValue, excelColumn);
            }

            dataIndex++; // 增加数据行计数器
        }

        progress?.Report(WritePosition.New(sheetIndex, dataIndex)); // 报告进度
    }

    /// <summary>
    /// 写入单元格值
    /// </summary>
    /// <param name="cellValue">单元格值</param>
    /// <param name="colConfig">列配置</param>
    private void WriteCellValue(object cellValue, PurColumn colConfig)
    {
        if (colConfig.UnwrappedType == null && cellValue is bool valBool)
        {
            _writer.Write(valBool);
        }
        else if (colConfig.UnwrappedType == typeof(bool) &&
                 bool.TryParse(cellValue.ToString(), out bool outBool))
        {
            _writer.Write(outBool);
        }
        else if (colConfig.UnwrappedType == null && cellValue.GetType().GetActualType().IsNumericType())
        {
            if (cellValue is byte) _writer.Write(Convert.ToByte(cellValue));
            if (cellValue is sbyte) _writer.Write(Convert.ToSByte(cellValue));
            if (cellValue is short) _writer.Write(Convert.ToInt16(cellValue));
            if (cellValue is ushort) _writer.Write(Convert.ToUInt16(cellValue));
            if (cellValue is int) _writer.Write(Convert.ToInt32(cellValue));
            if (cellValue is uint) _writer.Write(Convert.ToInt32(cellValue));
            if (cellValue is long) _writer.Write(Convert.ToDecimal(cellValue));
            if (cellValue is ulong) _writer.Write(Convert.ToDecimal(cellValue));
            if (cellValue is float) _writer.Write(Convert.ToSingle(cellValue));
            if (cellValue is double) _writer.Write(Convert.ToDouble(cellValue));
            if (cellValue is decimal) _writer.Write(Convert.ToDecimal(cellValue));
        }
        else if (colConfig.UnwrappedType?.IsNumericType() == true)
        {
            if (colConfig.UnwrappedType == typeof(byte)) _writer.Write(Convert.ToByte(cellValue));
            if (colConfig.UnwrappedType == typeof(sbyte)) _writer.Write(Convert.ToSByte(cellValue));
            if (colConfig.UnwrappedType == typeof(short)) _writer.Write(Convert.ToInt16(cellValue));
            if (colConfig.UnwrappedType == typeof(ushort)) _writer.Write(Convert.ToUInt16(cellValue));
            if (colConfig.UnwrappedType == typeof(int)) _writer.Write(Convert.ToInt32(cellValue));
            if (colConfig.UnwrappedType == typeof(uint)) _writer.Write(Convert.ToInt32(cellValue));
            if (colConfig.UnwrappedType == typeof(long)) _writer.Write(Convert.ToDecimal(cellValue));
            if (colConfig.UnwrappedType == typeof(ulong)) _writer.Write(Convert.ToDecimal(cellValue));
            if (colConfig.UnwrappedType == typeof(float)) _writer.Write(Convert.ToSingle(cellValue));
            if (colConfig.UnwrappedType == typeof(double)) _writer.Write(Convert.ToDouble(cellValue));
            if (colConfig.UnwrappedType == typeof(decimal)) _writer.Write(Convert.ToDecimal(cellValue));
        }
        else if (colConfig.UnwrappedType == null && cellValue is DateTime valDateTime)
        {
            _writer.Write(valDateTime, string.IsNullOrWhiteSpace(colConfig.Format)
                ? _dateFmt
                : XlsxStyle.Default.With(new XlsxNumberFormat(colConfig.Format))
                    .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left)));
        }
        else if (colConfig.UnwrappedType == null && cellValue is DateTimeOffset valDateTimeOffset)
        {
            _writer.Write(Convert.ToDateTime(valDateTimeOffset), _dateFmt);
        }
        else if (colConfig.UnwrappedType == typeof(DateTime) || colConfig.UnwrappedType == typeof(DateTimeOffset))
        {
            if (cellValue is DateTime valDateTime2)
            {
                _writer.Write(valDateTime2, _dateFmt);
            }
            else if (cellValue is DateTimeOffset valDateTimeOffset2)
            {
                _writer.Write(Convert.ToDateTime(valDateTimeOffset2), _dateFmt);
            }
            else
            {
                string? cellValueStr = cellValue.ToString();
                DateTime? cvDateTime = null;
                if (DateTimeConverter.Instance.Convert(cellValueStr, typeof(DateTime?), CultureInfo.InvariantCulture,
                        colConfig.Format)
                    is DateTime outDt)
                {
                    cvDateTime = outDt;
                }

                if (cvDateTime != null)
                {
                    if (cvDateTime.Value.ToUniversalTime() < PurConstants.Epoch1904)
                        cvDateTime = new DateTime(1900, 1, 1);
                    _writer.Write(cvDateTime.Value, _dateFmt);
                }
                else
                {
                    _writer.Write(cellValueStr);
                }
            }
        }
        else
        {
            _writer.Write(cellValue.ToString());
        }
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeAsyncCore()
    {
        _writer.Dispose();
        await base.DisposeAsyncCore();
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        if (!disposing) return;
        _writer.Dispose();
        base.Dispose(disposing);
    }
}