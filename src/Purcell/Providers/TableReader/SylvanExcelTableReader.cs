using Sylvan.Data.Excel;

namespace PurcellLibs.Providers.TableReader;

/// <summary>
/// 使用 Sylvan.Data.Excel 实现的 Excel 查询器
/// </summary>
internal class SylvanExcelTableReader : TableReaderBase
{
    private readonly ExcelWorkbookType _workbookType;

    /// <inheritdoc/>
    public SylvanExcelTableReader(Stream stream, QueryType queryType, bool ownsStream = false)
        : base(stream, ownsStream)
    {
        if (FileUtils.IsTextFile(stream)) throw new NotSupportedException("不是有效的Excel文件，请检查文件格式");

        _workbookType = queryType switch
        {
            QueryType.Xlsx => ExcelWorkbookType.ExcelXml,
            QueryType.Xlsb => ExcelWorkbookType.ExcelBinary,
            QueryType.Xls => ExcelWorkbookType.Excel,
            _ => throw new InvalidEnumArgumentException($"不支持的 {queryType} 类型")
        };

        // 创建 Excel 读取器
        using ExcelDataReader reader = ExcelDataReader.Create(
            stream,
            _workbookType,
            new ExcelDataReaderOptions
            {
                Schema = ExcelSchema.NoHeaders, ReadHiddenWorksheets = true, GetErrorAsNull = true, OwnsStream = false
            }
        );

        Worksheets = reader.WorksheetNames.ToList();
    }

    protected override IEnumerable<IDictionary<int, object?>> ReadCore(PurTable tableConfig, IProgress<int>? progress = null,
        CancellationToken cancelToken = default)
    {
        TableConfig = tableConfig;
        _stream.Position = 0; // 重置流位置以确保从头开始读取
        using ExcelDataReader reader = ExcelDataReader.Create( // 创建 Excel 读取器
            _stream,
            _workbookType,
            new ExcelDataReaderOptions
            {
                Schema = ExcelSchema.NoHeaders, ReadHiddenWorksheets = true, GetErrorAsNull = true, OwnsStream = false
            }
        );

        LocateSheet(TableConfig, reader); // 定位工作表

        // 获取表头和数据起始位置
        CellLocator headerStart = TableConfig.GetHeaderStart();
        CellLocator dataStart = TableConfig.GetDataStart();

        int columnLength = 0;
        int rowIndex = -1;
        while (reader.Read())
        {
            rowIndex++;
            progress?.Report(rowIndex + 1); // 报告进度
            cancelToken.ThrowIfCancellationRequested(); // 检查任务取消

            // 读取表头并把表头作为第一行数据返回且记录表头行的列数作为数据的列数
            if (TableConfig.HasHeader && rowIndex == headerStart.RowIndex)
            {
                if (reader.RowFieldCount == 0) throw new InvalidDataException($"无法解析表头：第 {rowIndex + 1} 行为空行");

                columnLength = reader.RowFieldCount; // 记录表头行列数
                Dictionary<int, object?> headerData = new();

                for (int colIndex = 0; colIndex < columnLength; colIndex++)
                {
                    headerData[colIndex] = colIndex < headerStart.ColumnIndex
                        ? null
                        : reader.GetExcelValueSafely(colIndex, rowIndex, TableConfig.IgnoreParseError);
                }

                yield return headerData;
            }

            if (rowIndex < dataStart.RowIndex) continue; // 未到达数据行直接跳过

            // ⬇⬇⬇ 读取行数据

            if (columnLength == 0) columnLength = reader.RowFieldCount;
            Dictionary<int, object?> rowData = new();

            for (int colIndex = 0; colIndex < columnLength; colIndex++)
            {
                rowData[colIndex] = colIndex < headerStart.ColumnIndex || colIndex >= reader.RowFieldCount
                    ? null
                    : reader.GetExcelValueSafely(colIndex, rowIndex, TableConfig.IgnoreParseError);
            }

            yield return rowData;
        }
    }

    /// <summary>
    /// 定位到目标工作表。根据配置优先使用表名查找，若不存在则使用表索引，
    /// 并将Excel读取器指针移动到该工作表位置。
    /// </summary>
    /// <param name="tableConfig">包含工作表定位信息的配置对象</param>
    /// <param name="reader">读取器</param>
    /// <exception cref="IndexOutOfRangeException">当指定的工作表索引超出有效范围时抛出</exception>
    private void LocateSheet(PurTable tableConfig, ExcelDataReader reader)
    {
        if (tableConfig.SheetIndex < 0)
            throw new InvalidOperationException("工作表索引不能小于0");

        int targetIndex = tableConfig.SheetIndex;

        List<string> worksheets = reader.WorksheetNames.ToList();

        if (!string.IsNullOrEmpty(tableConfig.SheetName) && worksheets.IndexOf(tableConfig.SheetName) >= 0)
            targetIndex = worksheets.IndexOf(tableConfig.SheetName);

        // 检查指定的targetIndex是否在有效范围内
        if (targetIndex > worksheets.Count - 1)
            throw new IndexOutOfRangeException($"工作表索引超出范围：索引 {targetIndex} 超过了工作表总数 {worksheets.Count}");

        // 定位到目标工作表：默认从第一个工作表开始，逐步移动到目标工作表
        for (int i = 0; i < targetIndex; i++) reader.NextResult();
    }
}