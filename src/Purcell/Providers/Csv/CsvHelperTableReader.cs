using CsvHelper;
using CsvHelper.Configuration;

namespace PurcellLibs.Providers.Csv;

/// <summary>
/// 使用 CsvHelper 实现的 Csv 表格查询器
/// </summary>
public class CsvHelperTableReader(Stream stream) : TableReaderBase(stream)
{
    /// <inheritdoc/>
    public override IEnumerable<string> GetWorksheets()
    {
        yield return "Sheet1";
    }

    /// <inheritdoc/>
    public override IEnumerable<IDictionary<int, object?>> ReadTable(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        if (!_stream.IsTextFile()) throw new NotSupportedException("不是有效的CSV文件，请检查文件格式");

        // 重置流位置以确保从头开始读取
        if (_stream.CanSeek) _stream.Position = 0;

        // 创建 CsvReader 配置(无头读取)
        CsvConfiguration config = new(tableConfig.GetCulture())
            { HasHeaderRecord = false, Delimiter = tableConfig.CsvDelimiter, Escape = tableConfig.CsvEscape };

        // 创建 CsvReader 读取器
        using StreamReader streamReader =
            new(_stream, tableConfig.GetFileEncoding() ?? _stream.DetectEncoding() ?? Encoding.UTF8);
        using CsvReader reader = new(streamReader, config);

        int columnLength = 0;
        int rowIndex = -1;
        foreach (IDictionary<string, object?> row in reader.GetRecords<dynamic>())
        {
            rowIndex++;
            progress?.Report(rowIndex + 1); // 报告进度
            cancelToken.ThrowIfCancellationRequested(); // 检查任务取消

            // 读取表头并把表头作为第一行数据返回且记录表头行的列数作为数据的列数
            if (tableConfig.HasHeader && rowIndex == tableConfig.GetHeaderStart().RowIndex)
            {
                if (row.Keys.Count == 0) throw new InvalidDataException($"无法解析表头：第 {rowIndex + 1} 行为空行");

                columnLength = row.Keys.Count; // 记录表头行列数
                Dictionary<int, object?> headerData = new(columnLength);

                foreach ((string key, int colIndex) in row.Keys.Select((v, i) => (v, i)))
                {
                    headerData[colIndex] = colIndex < tableConfig.GetHeaderStart().ColumnIndex ? null : row[key];
                }

                yield return headerData;
            }

            if (rowIndex < tableConfig.GetDataStart().RowIndex) continue; // 未到达数据行直接跳过

            // ⬇⬇⬇ 读取行数据

            if (columnLength == 0)
            {
                if (row.Keys.Count == 0)
                {
                    throw new InvalidDataException($"无法解析首行数据行：第 {rowIndex + 1} 行为空行");
                }

                columnLength = row.Keys.Count;
            }

            Dictionary<int, object?> rowData = new(columnLength); // CSV的值永远为string类型

            foreach ((string key, int colIndex) in row.Keys.Select((v, i) => (v, i)))
            {
                if (colIndex < tableConfig.GetHeaderStart().ColumnIndex || colIndex >= columnLength)
                    rowData[colIndex] = null;
                else
                    rowData[colIndex] = row[key];
            }

            yield return rowData;
        }
    }
}