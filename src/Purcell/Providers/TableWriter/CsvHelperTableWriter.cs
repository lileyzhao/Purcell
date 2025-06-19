using CsvHelper;
using CsvHelper.Configuration;

namespace PurcellLibs.Providers.TableWriter;

/// <summary>
/// 使用 CsvHelper 实现的 Csv 写入器
/// </summary>
internal class CsvHelperTableWriter : TableWriterBase
{
    /// <inheritdoc/>
    public CsvHelperTableWriter(Stream stream, bool ownsStream = false)
        : base(stream, ownsStream)
    {
    }

    /// <inheritdoc/>
    public override void WriteTable(IList<PurTable> tableConfigs,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        if (tableConfigs == null || tableConfigs.Count == 0)
            throw new ArgumentNullException(nameof(tableConfigs), "工作表数据集合不能为空");

        PurTable tableConfig = tableConfigs[0];

        using StreamWriter streamWriter = new(_stream, tableConfig.GetFileEncoding() ?? Encoding.UTF8);

        // 创建 CsvReader 配置(无头读取)
        CsvConfiguration config = new(tableConfig.GetCulture())
        {
            HasHeaderRecord = false,
            Delimiter = tableConfig.CsvDelimiter,
            Escape = tableConfig.CsvEscape
        };

        using CsvWriter writer = new(streamWriter, config);

        // 获取表头和数据起始位置
        CellLocator headerStart = tableConfig.GetHeaderStart();
        CellLocator dataStart = tableConfig.GetDataStart();

        // 输出表头
        if (tableConfig.HasHeader)
        {
            // 跳至表头起始行
            for (int i = 0; i < headerStart.RowIndex; i++)
            {
                writer.NextRecord();
            }

            // 跳至表头起始列
            for (int i = 0; i < headerStart.ColumnIndex; i++)
            {
                writer.WriteField(string.Empty);
            }

            // 输出表头列名
            foreach (string? colName in tableConfig.CombinedColumns.Select(ec => ec.PrimaryName))
            {
                writer.WriteField(colName ?? string.Empty);
            }

            writer.NextRecord(); // 换行
        }

        // 跳至数据起始行
        for (int i = 0; i < dataStart.RowIndex - headerStart.RowIndex - 1; i++)
        {
            writer.NextRecord();
        }

        int dataIndex = 0; // 数据行计数器
        foreach (IDictionary<string, object?>? rowItem in tableConfig.Records)
        {
            cancelToken.ThrowIfCancellationRequested(); // 检查任务取消
            if (dataIndex % 100 == 0) progress?.Report(WritePosition.New(0, dataIndex)); // 报告进度

            if (tableConfig.MaxWriteRows >= 0 && dataIndex >= tableConfig.MaxWriteRows)
                break; // 如果已达到最大写出行数，则停止写入

            // 跳至数据起始列
            for (int i = 0; i < dataStart.ColumnIndex; i++)
            {
                writer.WriteField(string.Empty);
            }

            // 遍历每一列
            foreach (PurColumn excelColumn in tableConfig.CombinedColumns)
            {
                // 跳过空行或无属性名的列
                if (rowItem == null || string.IsNullOrEmpty(excelColumn.PropertyName))
                {
                    writer.WriteField(string.Empty);
                    continue;
                }

                // 获取单元格值
                object? cellValue = rowItem[excelColumn.PropertyName];

                // 跳过空值
                if (cellValue == null || string.IsNullOrEmpty(cellValue.ToString()) || cellValue == DBNull.Value)
                {
                    writer.WriteField(string.Empty);
                    continue;
                }

                // 根据不同数据类型写入单元格
                WriteCellValue(cellValue, excelColumn, writer);
            }

            writer.NextRecord(); // 换行
            dataIndex++; // 增加数据行计数器
        }

        progress?.Report(WritePosition.New(0, dataIndex)); // 报告进度
        writer.Flush();
    }

    /// <summary>
    /// 写入单元格值
    /// </summary>
    /// <param name="cellValue">单元格值</param>
    /// <param name="colConfig">列配置</param>
    /// <param name="writer">写入器</param>
    private void WriteCellValue(object cellValue, PurColumn colConfig, CsvWriter writer)
    {
        if (colConfig.UnwrappedType == typeof(bool))
        {
            writer.WriteField(Convert.ToBoolean(cellValue));
        }
        else if (colConfig.UnwrappedType?.IsNumericType() == true)
        {
            if (colConfig.UnwrappedType == typeof(byte)) writer.WriteField(Convert.ToByte(cellValue));
            if (colConfig.UnwrappedType == typeof(sbyte)) writer.WriteField(Convert.ToSByte(cellValue));
            if (colConfig.UnwrappedType == typeof(short)) writer.WriteField(Convert.ToInt16(cellValue));
            if (colConfig.UnwrappedType == typeof(ushort)) writer.WriteField(Convert.ToUInt16(cellValue));
            if (colConfig.UnwrappedType == typeof(int)) writer.WriteField(Convert.ToInt32(cellValue));
            if (colConfig.UnwrappedType == typeof(uint)) writer.WriteField(Convert.ToInt32(cellValue));
            if (colConfig.UnwrappedType == typeof(long)) writer.WriteField(Convert.ToDecimal(cellValue));
            if (colConfig.UnwrappedType == typeof(ulong)) writer.WriteField(Convert.ToDecimal(cellValue));
            if (colConfig.UnwrappedType == typeof(float)) writer.WriteField(Convert.ToSingle(cellValue));
            if (colConfig.UnwrappedType == typeof(double)) writer.WriteField(Convert.ToDouble(cellValue));
            if (colConfig.UnwrappedType == typeof(decimal)) writer.WriteField(Convert.ToDecimal(cellValue));
        }
        else if (colConfig.UnwrappedType == typeof(DateTime) ||
                 colConfig.UnwrappedType == typeof(DateTimeOffset))
        {
            DateTime cellValueDataTime = Convert.ToDateTime(cellValue);
            writer.WriteField(
                cellValueDataTime.ToUniversalTime() < PurConstants.Epoch1904
                    ? new DateTime(1900, 1, 1)
                    : Convert.ToDateTime(cellValue)
            );
        }
        else
        {
            writer.WriteField(cellValue.ToString());
        }
    }
}