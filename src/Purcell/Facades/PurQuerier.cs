namespace PurcellLibs;

/// <summary>
/// 表格查询器
/// </summary>
public class PurQuerier : DisposableBase, IPurQuerier
{
    private readonly TableReaderAdapter _reader;

    /// <summary>
    /// 初始化 PurQuerier 的新实例
    /// </summary>
    /// <param name="stream">要读取的数据流</param>
    /// <param name="queryType">表格文件类型</param>
    public PurQuerier(Stream stream, TableFileType queryType)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        _reader = new TableReaderAdapter(stream, queryType, false);
    }

    /// <summary>
    /// 初始化 PurQuerier 的新实例
    /// </summary>
    /// <param name="filePath">要查询的文件完整路径</param>
    public PurQuerier(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("文件路径缺少有效的扩展名", nameof(filePath));

        TableFileType fileType = extension switch
        {
            ".csv" => TableFileType.Csv,
            ".xlsx" => TableFileType.Xlsx,
            ".xls" => TableFileType.Xls,
            _ => throw new NotSupportedException($"不支持查询的文件类型: {extension}")
        };

        FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 16384,
            fileType == TableFileType.Csv ? FileOptions.SequentialScan : FileOptions.None);
        _reader = new TableReaderAdapter(stream, fileType, true);
    }

    // /// <inheritdoc cref="ITableReader.Worksheets" />
    // public IList<string> Worksheets => _reader.Worksheets;


    /// <inheritdoc/>
    public IEnumerable<T> Query<T>(PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        if (typeof(T) == typeof(object))
            throw new ArgumentException("泛型参数 T 不能为 object 或 dynamic 类型，请使用 QueryDynamic() 方法。", nameof(T));

        PurTable config = tableConfig ?? new PurTable();
        return _reader.ReadTyped<T>(config, progress, cancelToken);
    }

    /// <inheritdoc/>
    public IEnumerable<IDictionary<string, object?>> Query(PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        PurTable config = tableConfig ?? new PurTable();
        return _reader.ReadDict(config, progress, cancelToken);
    }

    /// <inheritdoc/>
    public IEnumerable<dynamic> QueryDynamic(PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        PurTable config = tableConfig ?? new PurTable();
        return _reader.ReadDynamic(config, progress, cancelToken);
    }

    /// <inheritdoc/>
    public IEnumerable<dynamic> QueryShapeless(PurTable? tableConfig = null, IProgress<int>? progress = null,
        CancellationToken cancelToken = default)
    {
        PurTable config = tableConfig ?? new PurTable();
        return _reader.ReadShapeless(config, progress, cancelToken);
    }

    /// <inheritdoc/>
    protected override void DisposeResources()
    {
        SafeDispose(_reader);
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeResourcesAsync()
    {
        await SafeDisposeAsync(_reader);
    }
}