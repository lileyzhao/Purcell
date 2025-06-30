namespace PurcellLibs.Providers;

/// <summary>
/// 表格写入器基类，提供表格写入的通用功能和流管理。
/// </summary>
public abstract class TableWriterBase(Stream stream) : DisposableBase, ITableWriter
{
    /// <summary>
    /// 用于写入数据的底层流。
    /// </summary>
    protected readonly Stream _stream = stream ?? throw new ArgumentNullException(nameof(stream));

    /// <inheritdoc/>
    public abstract void WriteTable(IList<PurTable> tableConfigs,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default);
}