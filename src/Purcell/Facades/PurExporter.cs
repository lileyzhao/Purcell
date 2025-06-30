namespace PurcellLibs;

/// <summary>
/// 表格导出器
/// </summary>
public class PurExporter : DisposableBase, IPurExporter
{
    private readonly TableWriterAdapter _writer;

    /// <summary>
    /// 初始化 PurExporter 的新实例
    /// </summary>
    /// <param name="stream">要写入的数据流</param>
    /// <param name="fileType">表格文件类型</param>
    public PurExporter(Stream stream, TableFileType fileType)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        _writer = new TableWriterAdapter(stream, fileType, false);
    }

    /// <summary>
    /// 初始化 PurExporter 的新实例
    /// </summary>
    /// <param name="filePath">导出文件的完整路径</param>
    public PurExporter(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("文件路径缺少有效的扩展名", nameof(filePath));

        TableFileType fileType = extension switch
        {
            ".csv" => TableFileType.Csv,
            ".xlsx" => TableFileType.Xlsx,
            _ => throw new NotSupportedException($"不支持导出的文件类型: {extension}")
        };

        FileStream stream = File.Create(filePath);
        _writer = new TableWriterAdapter(stream, fileType, true);
    }

    /// <inheritdoc/>
    public void Export(IList<PurTable> tableConfigs, IProgress<WritePosition>? progress = null,
        CancellationToken cancelToken = default)
    {
        _writer.WriteTable(tableConfigs, progress, cancelToken);
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