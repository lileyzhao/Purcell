using Sylvan.Data.Excel;

namespace PurcellLibs.Providers.TableReader;

/// <summary>
/// 使用 Sylvan.Data.Excel 实现的 Excel 查询器
/// </summary>
internal class SylvanExcelTableReader : TableReaderBase
{
    private int _disposed;
    private readonly ExcelDataReader _reader;

    /// <inheritdoc/>
    public SylvanExcelTableReader(Stream stream, QueryType queryType, bool ownsStream = false)
        : base(stream, ownsStream)
    {
        if (FileUtils.IsTextFile(stream)) throw new NotSupportedException("不是有效的Excel文件，请检查文件格式");

        ExcelWorkbookType workbookType = queryType switch
        {
            QueryType.Xlsx => ExcelWorkbookType.ExcelXml,
            QueryType.Xlsb => ExcelWorkbookType.ExcelBinary,
            QueryType.Xls => ExcelWorkbookType.Excel,
            _ => throw new InvalidEnumArgumentException($"不支持的 {queryType} 类型")
        };

        // 创建 Excel 读取器
        _reader = ExcelDataReader.Create(
            stream,
            workbookType,
            new ExcelDataReaderOptions { Schema = ExcelSchema.NoHeaders, ReadHiddenWorksheets = true, GetErrorAsNull = true }
        );

        Worksheets = _reader.WorksheetNames.ToList();
    }

    protected override IEnumerable<IDictionary<int, object?>> ReadCore(PurTable tableConfig, IProgress<int>? progress = null,
        CancellationToken cancelToken = default)
    {
        tableConfig.EnsureValid();
        _tableConfig = tableConfig;
        LocateSheet(_tableConfig); // 定位工作表

        // 获取表头和数据起始位置
        CellLocator headerStart = _tableConfig.GetHeaderStart();
        CellLocator dataStart = _tableConfig.GetDataStart();

        int columnLength = 0;
        int rowIndex = -1;
        while (_reader.Read())
        {
            rowIndex++;
            progress?.Report(rowIndex + 1); // 报告进度
            cancelToken.ThrowIfCancellationRequested(); // 检查任务取消

            // 读取表头并把表头作为第一行数据返回且记录表头行的列数作为数据的列数
            if (_tableConfig.HasHeader && rowIndex == headerStart.RowIndex)
            {
                if (_reader.RowFieldCount == 0) throw new InvalidDataException($"无法解析表头：第 {rowIndex + 1} 行为空行");

                columnLength = _reader.RowFieldCount; // 记录表头行列数
                Dictionary<int, object?> headerData = new();

                for (int colIndex = 0; colIndex < columnLength; colIndex++)
                {
                    headerData[colIndex] = colIndex < headerStart.ColumnIndex
                        ? null
                        : _reader.GetExcelValueSafely(colIndex, rowIndex, _tableConfig.IgnoreParseError);
                }

                yield return headerData;
            }

            if (rowIndex < dataStart.RowIndex) continue; // 未到达数据行直接跳过

            // ⬇⬇⬇ 读取行数据

            if (columnLength == 0) columnLength = _reader.RowFieldCount;
            Dictionary<int, object?> rowData = new();

            for (int colIndex = 0; colIndex < columnLength; colIndex++)
            {
                rowData[colIndex] = colIndex < headerStart.ColumnIndex || colIndex >= _reader.RowFieldCount
                    ? null
                    : _reader.GetExcelValueSafely(colIndex, rowIndex, _tableConfig.IgnoreParseError);
            }

            yield return rowData;
        }
    }

    /// <summary>
    /// 定位到目标工作表。根据配置优先使用表名查找，若不存在则使用表索引，
    /// 并将Excel读取器指针移动到该工作表位置。
    /// </summary>
    /// <param name="tableConfig">包含工作表定位信息的配置对象</param>
    /// <exception cref="IndexOutOfRangeException">当指定的工作表索引超出有效范围时抛出</exception>
    private void LocateSheet(PurTable tableConfig)
    {
        if (tableConfig.SheetIndex < 0)
            throw new InvalidOperationException("工作表索引不能小于0");

        int targetIndex = tableConfig.SheetIndex;

        if (!string.IsNullOrEmpty(tableConfig.SheetName) && Worksheets.IndexOf(tableConfig.SheetName) >= 0)
            targetIndex = Worksheets.IndexOf(tableConfig.SheetName);

        // 检查指定的targetIndex是否在有效范围内
        if (targetIndex > Worksheets.Count - 1)
            throw new IndexOutOfRangeException($"工作表索引超出范围：索引 {targetIndex} 超过了工作表总数 {Worksheets.Count}");

        // 定位到目标工作表：默认从第一个工作表开始，逐步移动到目标工作表
        for (int i = 0; i < targetIndex; i++) _reader.NextResult();
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeAsyncCore()
    {
        await _reader.DisposeAsync();
        await base.DisposeAsyncCore();
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        if (!disposing) return;
        _reader.Dispose();
        base.Dispose(disposing);
    }
}