namespace PurcellLibs.Providers.Excel;

/// <summary>
/// 基于 LargeXlsx 库的 Excel 表格写入器，支持高性能的大文件 XLSX 写入。
/// </summary>
public class LargeXlsxTableWriter(Stream stream) : TableWriterBase(stream)
{
    /// <summary>
    /// LargeXlsx 库的 XLSX 写入器实例。
    /// </summary>
    private readonly XlsxWriter _writer = new(stream, CompressionLevel.BestSpeed, false, false);

    /// <summary>
    /// 默认的日期时间格式。
    /// </summary>
    private readonly XlsxNumberFormat _dateFormat = new("yyyy-mm-dd HH:mm:ss");

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
                    XlsxColumn.Formatted(
                        Math.Max(ec.Width, tableStyle.MinColumnWidth),
                        1,
                        ec.IsHidden))
                .ToList();

            // 把跳列部分包含到列集合中
            xlsxColumns.InsertRange(0,
                Enumerable.Repeat(XlsxColumn.Formatted(tableStyle.MinColumnWidth), headerStart.ColumnIndex)
                    .ToList());

            // 开始写工作表: 先设置Sheet名称、xlsx列宽配置
            _writer.BeginWorksheet(
                string.IsNullOrEmpty(tableConfig.SheetName) ? $"Sheet{sheetIndex + 1}" : tableConfig.SheetName,
                columns: xlsxColumns);

            // 输出表头，先跳行，再设置表头样式，再开始行，再跳列，最后循环输出表头
            if (tableConfig.HasHeader)
            {
                _writer
                    .SkipRows(headerStart.RowIndex)
                    .SetDefaultStyle(tableStyle.HeaderStyle)
                    .BeginRow()
                    .SkipColumns(headerStart.ColumnIndex);
                foreach (PurColumn cc in tableConfig.CombinedColumns)
                {
                    if (cc.HeaderHAlign.HasValue || cc.HeaderVAlign.HasValue)
                    {
                        XlsxStyle? columnStyle =
                            tableStyle.HeaderStyle.With(new XlsxAlignment(
                                cc.HeaderHAlign?.ToXlsxHorizontal() ?? tableStyle.HeaderStyle.Alignment.HorizontalType,
                                cc.HeaderVAlign?.ToXlsxVertical() ?? tableStyle.HeaderStyle.Alignment.VerticalType
                            ));
                        _writer.Write(cc.PrimaryName ?? string.Empty, columnStyle);
                    }
                    else
                    {
                        _writer.Write(cc.PrimaryName ?? string.Empty);
                    }
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
    /// 写入行数据到工作表。
    /// </summary>
    /// <param name="tableConfig">表格配置，包含数据源和格式设置。</param>
    /// <param name="sheetIndex">工作表索引，从 0 开始。</param>
    /// <param name="progress">可选的进度报告器，报告写入位置信息。</param>
    /// <param name="cancelToken">可选的取消令牌。</param>
    private void WriteRowData(PurTable tableConfig, int sheetIndex,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        ArgumentNullException.ThrowIfNull(tableConfig.Records);

        CellLocator dataStart = tableConfig.GetDataStart();
        PurStyle tableStyle = tableConfig.GetActualStyle();

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
            foreach (PurColumn cc in tableConfig.CombinedColumns)
            {
                cancelToken.ThrowIfCancellationRequested(); // 检查任务取消

                // 跳过无属性名的列
                if (string.IsNullOrEmpty(cc.PropertyName))
                {
                    _writer.SkipColumns(1);
                    continue;
                }

                XlsxStyle? columnStyle = cc.ContentHAlign.HasValue || cc.ContentVAlign.HasValue
                    ? tableStyle.ContentStyle.With(new XlsxAlignment(
                        cc.ContentHAlign?.ToXlsxHorizontal() ?? tableStyle.ContentStyle.Alignment.HorizontalType,
                        cc.ContentVAlign?.ToXlsxVertical() ?? tableStyle.ContentStyle.Alignment.VerticalType
                    ))
                    : null;
                if (!string.IsNullOrWhiteSpace(cc.Format)) columnStyle?.With(new XlsxNumberFormat(cc.Format));

                // 获取单元格值
                object? cellValue = rowItem[cc.PropertyName];

                // 跳过空值
                if (cellValue == null || cellValue == DBNull.Value || string.IsNullOrEmpty(cellValue.ToString()))
                {
                    // _writer.SkipColumns(1);
                    _writer.Write(columnStyle);
                    continue;
                }

                // 根据不同数据类型写入单元格
                WriteCellValue(cellValue, columnStyle, tableStyle, cc);
            }

            dataIndex++; // 增加数据行计数器
        }

        progress?.Report(WritePosition.New(sheetIndex, dataIndex)); // 报告进度
    }

    /// <summary>
    /// 根据数据类型和列配置写入单元格值。
    /// </summary>
    /// <param name="cellValue">要写入的单元格值。</param>
    /// <param name="columnStyle">列的样式配置。</param>
    /// <param name="tableStyle">表格的样式配置。</param>
    /// <param name="columnConfig">列的配置信息，包含类型和格式设置。</param>
    private void WriteCellValue(object cellValue, XlsxStyle? columnStyle, PurStyle tableStyle, PurColumn columnConfig)
    {
        // 如果有格式字符串，则定义日期格式样式，否则使用列样式
        XlsxStyle? dateStyle = string.IsNullOrWhiteSpace(columnConfig.Format)
            ? (columnStyle ?? tableStyle.ContentStyle).With(_dateFormat)
            : (columnStyle ?? tableStyle.ContentStyle).With(new XlsxNumberFormat(columnConfig.Format));

        if (columnConfig.UnwrappedType == null && cellValue is bool valBool)
        {
            _writer.Write(valBool, columnStyle);
        }
        else if (columnConfig.UnwrappedType == typeof(bool) &&
                 bool.TryParse(cellValue.ToString(), out bool outBool))
        {
            _writer.Write(outBool, columnStyle);
        }
        else if (columnConfig.UnwrappedType == null && cellValue.GetType().GetActualType().IsNumericType())
        {
            if (cellValue is byte) _writer.Write(Convert.ToByte(cellValue), columnStyle);
            if (cellValue is sbyte) _writer.Write(Convert.ToSByte(cellValue), columnStyle);
            if (cellValue is short) _writer.Write(Convert.ToInt16(cellValue), columnStyle);
            if (cellValue is ushort) _writer.Write(Convert.ToUInt16(cellValue), columnStyle);
            if (cellValue is int) _writer.Write(Convert.ToInt32(cellValue), columnStyle);
            if (cellValue is uint) _writer.Write(Convert.ToInt32(cellValue), columnStyle);
            if (cellValue is long) _writer.Write(Convert.ToDecimal(cellValue), columnStyle);
            if (cellValue is ulong) _writer.Write(Convert.ToDecimal(cellValue), columnStyle);
            if (cellValue is float) _writer.Write(Convert.ToSingle(cellValue), columnStyle);
            if (cellValue is double) _writer.Write(Convert.ToDouble(cellValue), columnStyle);
            if (cellValue is decimal) _writer.Write(Convert.ToDecimal(cellValue), columnStyle);
        }
        else if (columnConfig.UnwrappedType?.IsNumericType() == true)
        {
            if (columnConfig.UnwrappedType == typeof(byte)) _writer.Write(Convert.ToByte(cellValue), columnStyle);
            if (columnConfig.UnwrappedType == typeof(sbyte)) _writer.Write(Convert.ToSByte(cellValue), columnStyle);
            if (columnConfig.UnwrappedType == typeof(short)) _writer.Write(Convert.ToInt16(cellValue), columnStyle);
            if (columnConfig.UnwrappedType == typeof(ushort)) _writer.Write(Convert.ToUInt16(cellValue), columnStyle);
            if (columnConfig.UnwrappedType == typeof(int)) _writer.Write(Convert.ToInt32(cellValue), columnStyle);
            if (columnConfig.UnwrappedType == typeof(uint)) _writer.Write(Convert.ToInt32(cellValue), columnStyle);
            if (columnConfig.UnwrappedType == typeof(long)) _writer.Write(Convert.ToDecimal(cellValue), columnStyle);
            if (columnConfig.UnwrappedType == typeof(ulong)) _writer.Write(Convert.ToDecimal(cellValue), columnStyle);
            if (columnConfig.UnwrappedType == typeof(float)) _writer.Write(Convert.ToSingle(cellValue), columnStyle);
            if (columnConfig.UnwrappedType == typeof(double)) _writer.Write(Convert.ToDouble(cellValue), columnStyle);
            if (columnConfig.UnwrappedType == typeof(decimal)) _writer.Write(Convert.ToDecimal(cellValue), columnStyle);
        }
        else if (columnConfig.UnwrappedType == null && cellValue is DateTime valDateTime)
        {
            _writer.Write(valDateTime, dateStyle);
        }
        else if (columnConfig.UnwrappedType == null && cellValue is DateTimeOffset valDateTimeOffset)
        {
            _writer.Write(Convert.ToDateTime(valDateTimeOffset), dateStyle);
        }
        else if (columnConfig.UnwrappedType == typeof(DateTime) || columnConfig.UnwrappedType == typeof(DateTimeOffset))
        {
            if (cellValue is DateTime valDateTime2)
            {
                _writer.Write(valDateTime2, dateStyle);
            }
            else if (cellValue is DateTimeOffset valDateTimeOffset2)
            {
                _writer.Write(Convert.ToDateTime(valDateTimeOffset2), dateStyle);
            }
            else
            {
                string? cellValueStr = cellValue.ToString();
                DateTime? cvDateTime = null;
                if (DateTimeConverter.Instance.Convert(cellValueStr, typeof(DateTime?), columnConfig,
                        CultureInfo.InvariantCulture)
                    is DateTime outDt)
                {
                    cvDateTime = outDt;
                }

                if (cvDateTime != null)
                {
                    if (cvDateTime.Value.ToUniversalTime() < PurConstants.Epoch1904)
                        cvDateTime = new DateTime(1900, 1, 1);
                    _writer.Write(cvDateTime.Value, dateStyle);
                }
                else
                {
                    _writer.Write(cellValueStr, columnStyle);
                }
            }
        }
        else
        {
            _writer.Write(cellValue.ToString(), columnStyle);
        }
    }

    /// <inheritdoc/>
    protected override void DisposeResources()
    {
        SafeDispose(_writer);
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeResourcesAsync()
    {
        await SafeDisposeAsync(_writer);
    }
}