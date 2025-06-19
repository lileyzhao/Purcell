namespace PurcellLibs;

/// <summary>
/// 表格导出器
/// </summary>
public class PurExporter : IPurExporter
{
    private int _disposed;
    private readonly ITableWriter _writer;

    private ITableWriter InitWriter(Stream stream, ExportType exportType, bool ownsStream)
    {
        return exportType switch
        {
            ExportType.Csv => new CsvHelperTableWriter(stream, ownsStream),
            ExportType.Xlsx => new LargeXlsxTableWriter(stream, ownsStream),
            _ => throw new NotSupportedException($"不支持的查询类型：{exportType}")
        };
    }

    /// <summary>
    /// 初始化 PurExporter 的新实例
    /// </summary>
    public PurExporter(Stream stream, ExportType exportType)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        _writer = InitWriter(stream, exportType, false);
    }

    /// <summary>
    /// 初始化 PurExporter 的新实例
    /// </summary>
    public PurExporter(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("文件路径缺少有效的扩展名", nameof(filePath));

        ExportType exportType = extension switch
        {
            ".csv" => ExportType.Csv,
            ".xlsx" => ExportType.Xlsx,
            _ => throw new NotSupportedException($"不支持导出的文件类型: {extension}")
        };

        FileStream stream = File.Create(filePath);
        _writer = InitWriter(stream, exportType, true);
    }

    /// <inheritdoc/>
    public void Export(IList<PurTable> tableConfigs, IProgress<WritePosition>? progress = null,
        CancellationToken cancelToken = default)
    {
        _writer.WriteTable(tableConfigs, progress, cancelToken);
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
        await _writer.DisposeAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 同步释放核心逻辑
    /// </summary>
    private void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        if (!disposing) return;
        _writer.Dispose();
    }

    /// <summary>终结器</summary>
    ~PurExporter()
    {
        Dispose(false);
    }

    #endregion Disposable Support
}