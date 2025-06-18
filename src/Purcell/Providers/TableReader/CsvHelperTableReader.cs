using CsvHelper;
using CsvHelper.Configuration;

namespace PurcellLibs.Providers.TableReader;

/// <summary>
/// 使用 CsvHelper 实现的 Csv 查询器
/// </summary>
internal class CsvHelperTableReader : TableReaderBase
{
    /// <inheritdoc/>
    public CsvHelperTableReader(Stream stream, bool ownsStream = false)
        : base(stream, ownsStream)
    {
        if (!FileUtils.IsTextFile(stream)) throw new NotSupportedException("不是有效的CSV文件，请检查文件格式");
        Worksheets = ["Sheet1"];
    }

    /// <inheritdoc/>
    protected override IEnumerable<IDictionary<int, object?>> ReadCore(PurTable tableConfig, IProgress<int>? progress = null,
        CancellationToken cancelToken = default)
    {
        _tableConfig = tableConfig;

        // 创建 CsvReader 配置(无头读取)
        CsvConfiguration config = new(tableConfig.GetCulture())
        {
            HasHeaderRecord = false,
            Delimiter = tableConfig.CsvDelimiter,
            Escape = tableConfig.CsvEscape
        };

        // 获取表头和数据起始位置
        CellLocator headerStart = _tableConfig.GetHeaderStart();
        CellLocator dataStart = _tableConfig.GetDataStart();

        // 创建 CsvReader 读取器
        using StreamReader streamReader = new(_stream,
            tableConfig.GetEncoding() ?? EncodingUtils.DetectEncoding(_stream) ?? Encoding.UTF8);
        using CsvReader reader = new(streamReader, config);

        int columnLength = 0;
        int rowIndex = -1;
        foreach (IDictionary<string, object?> row in reader.GetRecords<dynamic>())
        {
            rowIndex++;
            progress?.Report(rowIndex + 1); // 报告进度
            cancelToken.ThrowIfCancellationRequested(); // 检查任务取消

            // 读取表头并把表头作为第一行数据返回且记录表头行的列数作为数据的列数
            if (_tableConfig.HasHeader && rowIndex == headerStart.RowIndex)
            {
                if (row.Keys.Count == 0) throw new InvalidDataException($"无法解析表头：第 {rowIndex + 1} 行为空行");

                columnLength = row.Keys.Count; // 记录表头行列数
                Dictionary<int, object?> headerData = new();

                foreach ((string key, int colIndex) in row.Keys.Select((v, i) => (v, i)))
                {
                    headerData[colIndex] = colIndex < headerStart.ColumnIndex ? null : row[key];
                }

                yield return headerData;
            }

            if (rowIndex < dataStart.RowIndex) continue; // 未到达数据行直接跳过

            // ⬇⬇⬇ 读取行数据

            if (columnLength == 0) columnLength = row.Keys.Count;
            Dictionary<int, object?> rowData = new(); // CSV的值永远为string类型

            foreach ((string key, int colIndex) in row.Keys.Select((v, i) => (v, i)))
            {
                if (colIndex < headerStart.ColumnIndex || colIndex >= columnLength)
                    rowData[colIndex] = null;
                else
                    rowData[colIndex] = row[key];
            }

            yield return rowData;
        }
    }
}