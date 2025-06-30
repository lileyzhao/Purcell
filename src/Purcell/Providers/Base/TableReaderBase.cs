namespace PurcellLibs.Providers;

/// <summary>
/// 表格读取器基类，提供表格读取的通用功能和流管理。
/// </summary>
public abstract class TableReaderBase(Stream stream) : DisposableBase, ITableReader
{
    /// <summary>
    /// 用于读取数据的底层流。
    /// </summary>
    protected readonly Stream _stream = stream ?? throw new ArgumentNullException(nameof(stream));

    /// <inheritdoc/>
    public abstract IEnumerable<string> GetWorksheets();

    /// <inheritdoc/>
    public abstract IEnumerable<IDictionary<int, object?>> ReadTable(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default);
}