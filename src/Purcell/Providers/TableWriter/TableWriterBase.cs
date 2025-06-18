namespace PurcellLibs.Providers.TableWriter;

/// <summary>
/// 表格数据写入器抽象类
/// </summary>
internal abstract class TableWriterBase : ITableWriter
{
    private int _disposed;
    private readonly Stream _stream;
    private readonly bool _ownsStream;

    /// <summary>
    /// 初始化表格数据写入器
    /// </summary>
    protected TableWriterBase(Stream stream, bool ownsStream = false)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        _ownsStream = ownsStream;
    }

    /// <inheritdoc/>
    public abstract void WriteTable(IList<PurTable> tableList,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default);

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
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 异步释放核心逻辑
    /// </summary>
    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_ownsStream) await _stream.DisposeAsync();
        await Task.CompletedTask;
    }

    /// <summary>
    /// 同步释放核心逻辑
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
        if (!disposing) return;
        if (_ownsStream) _stream.Dispose();
    }

    // 终结器
    ~TableWriterBase()
    {
        Dispose(false);
    }

    #endregion Disposable Support
}