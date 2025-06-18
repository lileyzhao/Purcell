using CsvHelper;

namespace PurcellLibs.Providers.TableWriter;

/// <summary>
/// 使用 CsvHelper 实现的 Csv 写入器
/// </summary>
internal class CsvHelperTableWriter : TableWriterBase
{
    private int _disposed;
    private readonly StreamWriter _streamWriter;
    private readonly CsvWriter _writer;

    /// <inheritdoc/>
    public CsvHelperTableWriter(Stream stream, bool ownsStream = false)
        : base(stream, ownsStream)
    {
        _streamWriter = new StreamWriter(stream, Encoding.UTF8);
        _writer = new CsvWriter(_streamWriter, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public override void WriteTable(IList<PurTable> tableList,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        if (tableList == null || tableList.Count == 0)
            throw new ArgumentNullException(nameof(tableList), "工作表数据集合不能为空");

        foreach ((PurTable? tableConfig, int sheetIndex) in tableList.Select((v, i) => (v, i)))
        {
            cancelToken.ThrowIfCancellationRequested(); // 检查任务取消

            if (sheetIndex > 0) return; // Csv 不支持多工作表导出

            // 输出表头
            foreach (string? colName in tableConfig.CombinedColumns.Select(ec => ec.PrimaryName))
            {
                _writer.WriteField(colName ?? string.Empty);
            }

            _writer.NextRecord(); // 换行

            int dataIndex = 0; // 数据行计数器
            foreach ((IDictionary<string, object?>? rowItem, int rowIndex) in tableConfig.Records.Select((v, i) => (v, i)))
            {
                cancelToken.ThrowIfCancellationRequested(); // 检查任务取消
                progress?.Report(WritePosition.New(sheetIndex, rowIndex)); // 报告进度

                if (tableConfig.MaxWriteRows >= 0 && dataIndex >= tableConfig.MaxWriteRows)
                    break; // 如果已达到最大写出行数，则停止写入

                // 遍历每一列
                foreach (PurColumn excelColumn in tableConfig.CombinedColumns)
                {
                    // 跳过空行或无属性名的列
                    if (rowItem == null || string.IsNullOrEmpty(excelColumn.PropertyName))
                    {
                        _writer.WriteField(string.Empty);
                        continue;
                    }

                    // 获取单元格值
                    object? cellValue = rowItem[excelColumn.PropertyName];

                    // 跳过空值
                    if (cellValue == null || string.IsNullOrEmpty(cellValue.ToString()) || cellValue == DBNull.Value)
                    {
                        _writer.WriteField(string.Empty);
                        continue;
                    }

                    // 根据不同数据类型写入单元格
                    WriteCellValue(cellValue, excelColumn);
                }

                _writer.NextRecord(); // 换行
                dataIndex++; // 增加数据行计数器
            }
        }
    }

    /// <summary>
    /// 写入单元格值
    /// </summary>
    /// <param name="cellValue">单元格值</param>
    /// <param name="colConfig">列配置</param>
    private void WriteCellValue(object cellValue, PurColumn colConfig)
    {
        if (colConfig.UnwrappedType == typeof(bool))
        {
            _writer.WriteField(Convert.ToBoolean(cellValue));
        }
        else if (colConfig.UnwrappedType?.IsNumericType() == true)
        {
            if (colConfig.UnwrappedType == typeof(byte)) _writer.WriteField(Convert.ToByte(cellValue));
            if (colConfig.UnwrappedType == typeof(sbyte)) _writer.WriteField(Convert.ToSByte(cellValue));
            if (colConfig.UnwrappedType == typeof(short)) _writer.WriteField(Convert.ToInt16(cellValue));
            if (colConfig.UnwrappedType == typeof(ushort)) _writer.WriteField(Convert.ToUInt16(cellValue));
            if (colConfig.UnwrappedType == typeof(int)) _writer.WriteField(Convert.ToInt32(cellValue));
            if (colConfig.UnwrappedType == typeof(uint)) _writer.WriteField(Convert.ToInt32(cellValue));
            if (colConfig.UnwrappedType == typeof(long)) _writer.WriteField(Convert.ToDecimal(cellValue));
            if (colConfig.UnwrappedType == typeof(ulong)) _writer.WriteField(Convert.ToDecimal(cellValue));
            if (colConfig.UnwrappedType == typeof(float)) _writer.WriteField(Convert.ToSingle(cellValue));
            if (colConfig.UnwrappedType == typeof(double)) _writer.WriteField(Convert.ToDouble(cellValue));
            if (colConfig.UnwrappedType == typeof(decimal)) _writer.WriteField(Convert.ToDecimal(cellValue));
        }
        else if (colConfig.UnwrappedType == typeof(DateTime) ||
                 colConfig.UnwrappedType == typeof(DateTimeOffset))
        {
            DateTime cellValueDataTime = Convert.ToDateTime(cellValue);
            _writer.WriteField(
                cellValueDataTime.ToUniversalTime() < PurConstants.Epoch1904
                    ? new DateTime(1900, 1, 1)
                    : Convert.ToDateTime(cellValue)
            );
        }
        else
        {
            _writer.WriteField(cellValue.ToString());
        }
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeAsyncCore()
    {
        await _writer.FlushAsync();
        await _writer.DisposeAsync();
        await _streamWriter.DisposeAsync();
        await base.DisposeAsyncCore();
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        if (!disposing) return;
        _writer.Flush();
        _writer.Dispose();
        _streamWriter.Dispose();
        base.Dispose(disposing);
    }
}