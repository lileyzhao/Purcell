namespace PurcellLibs;

/// <summary>
/// 表格查询器
/// </summary>
public class PurQuerier : IPurQuerier
{
    private int _disposed;
    private readonly ITableReader _reader;

    private ITableReader InitReader(Stream stream, QueryType queryType, bool ownsStream)
    {
        return queryType switch
        {
            QueryType.Csv => new CsvHelperTableReader(stream, ownsStream),
            QueryType.Xlsx => new SylvanExcelTableReader(stream, queryType, ownsStream),
            QueryType.Xlsb => new SylvanExcelTableReader(stream, queryType, ownsStream),
            QueryType.Xls => new SylvanExcelTableReader(stream, queryType, ownsStream),
            _ => throw new NotSupportedException($"不支持的查询类型：{queryType}")
        };
    }

    /// <inheritdoc cref="ITableReader.Worksheets" />
    public IList<string> Worksheets => _reader.Worksheets;

    /// <summary>
    /// 初始化 PurQuerier 的新实例
    /// </summary>
    public PurQuerier(Stream stream, QueryType queryType)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        _reader = InitReader(stream, queryType, false);
    }

    /// <summary>
    /// 初始化 PurQuerier 的新实例
    /// </summary>
    public PurQuerier(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("文件路径缺少有效的扩展名", nameof(filePath));

        QueryType queryType = extension switch
        {
            ".csv" => QueryType.Csv,
            ".xlsx" => QueryType.Xlsx,
            ".xlsb" => QueryType.Xlsb,
            ".xls" => QueryType.Xls,
            _ => throw new NotSupportedException($"不支持查询的文件类型: {extension}")
        };

        FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 16384,
            queryType == QueryType.Csv ? FileOptions.SequentialScan : FileOptions.None);
        _reader = InitReader(stream, queryType, true);
    }

    /// <inheritdoc/>
    public IEnumerable<T> Query<T>(PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        if (typeof(T) == typeof(object))
            throw new ArgumentException("泛型参数 T 不能为 object 或 dynamic 类型，请使用 QueryDynamic() 方法。", nameof(T));

        PurTable config = (tableConfig ?? new PurTable()).EnsureValid();
        return _reader.Read<T>(config, progress, cancelToken);
    }

    /// <inheritdoc/>
    public IEnumerable<IDictionary<string, object?>> Query(PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        PurTable config = (tableConfig ?? new PurTable()).EnsureValid();
        return _reader.ReadDict(config, progress, cancelToken);
    }

    /// <inheritdoc/>
    public IEnumerable<dynamic> QueryDynamic(PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        PurTable config = (tableConfig ?? new PurTable()).EnsureValid();
        return _reader.ReadDynamic(config, progress, cancelToken);
    }

    #region Disposable Support

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        await _reader.DisposeAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 同步释放核心逻辑
    /// </summary>
    private void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        if (!disposing) return;
        _reader.Dispose();
    }

    /// <summary>终结器</summary>
    ~PurQuerier()
    {
        Dispose(false);
    }

    #endregion Disposable Support
}