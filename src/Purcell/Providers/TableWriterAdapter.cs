namespace PurcellLibs.Providers;

/// <summary>
/// 表格写入适配器，提供统一的表格数据写入接口。将底层的文件格式写入器包装为统一的数据输出层。
/// </summary>
public class TableWriterAdapter(Stream stream, TableFileType fileType, bool ownsStream) : DisposableBase
{
    /// <summary>
    /// 底层表格写入器，根据文件类型创建相应的实现。
    /// </summary>
    private readonly ITableWriter _writer = fileType switch
    {
        TableFileType.Csv => new CsvHelperTableWriter(stream),
        TableFileType.Xlsx => new LargeXlsxTableWriter(stream),
        _ => throw new NotSupportedException($"不支持导出的表格类型：{fileType}")
    };

    /// <summary>
    /// 将表格配置数据写入到目标流中。
    /// </summary>
    /// <param name="tableConfigs">表格配置列表，每个配置包含数据源和格式设置。</param>
    /// <param name="progress">可选的进度报告器，报告写入位置信息。</param>
    /// <param name="cancelToken">可选的取消令牌。</param>
    public void WriteTable(IList<PurTable> tableConfigs,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        _writer.WriteTable(tableConfigs, progress, cancelToken);
    }

    /// <inheritdoc/>
    protected override void DisposeResources()
    {
        SafeDispose(_writer);
        SafeDispose(stream, ownsStream);
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeResourcesAsync()
    {
        await SafeDisposeAsync(_writer);
        await SafeDisposeAsync(stream, ownsStream);
    }
}