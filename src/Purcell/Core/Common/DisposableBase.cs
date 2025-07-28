namespace PurcellLibs.Common;

/// <summary>
/// 提供 IDisposable 和 IAsyncDisposable 的通用实现基类。
/// 支持管理多个资源及其条件释放逻辑。
/// </summary>
public abstract class DisposableBase : IDisposable, IAsyncDisposable
{
    private volatile bool _disposed;

    /// <summary>
    /// 用于同步 Dispose 方法的锁对象。
    /// </summary>
    private readonly object _disposeLock = new();

    /// <summary>
    /// 用于异步 DisposeAsync 方法的信号量锁。
    /// </summary>
    private readonly SemaphoreSlim _asyncDisposeLock = new(1, 1);

    /// <summary>
    /// 获取一个值，指示对象是否已被释放。
    /// </summary>
    protected bool IsDisposed => _disposed;

    /// <summary>
    /// 释放所有资源。
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        lock (_disposeLock)
        {
            if (_disposed)
                return;

            try
            {
                DisposeResources();
            }
            finally
            {
                try
                {
                    _asyncDisposeLock.Dispose();
                }
                finally
                {
                    _disposed = true;
                }
            }
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 异步释放所有资源。
    /// </summary>
    /// <returns>表示异步操作的 <see cref="ValueTask"/>。</returns>
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        try
        {
            await _asyncDisposeLock.WaitAsync().ConfigureAwait(false);
        }
        catch (ObjectDisposedException)
        {
            // SemaphoreSlim 已被释放，说明 Dispose() 已经被调用。
            return;
        }

        try
        {
            if (_disposed)
                return;

            await DisposeResourcesAsync().ConfigureAwait(false);
            _disposed = true;
        }
        finally
        {
            try
            {
                _asyncDisposeLock.Release();
            }
            finally
            {
                _asyncDisposeLock.Dispose();
            }
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 释放托管资源。子类应重写此方法来释放具体的资源。
    /// </summary>
    protected virtual void DisposeResources()
    {
        // 子类实现具体的资源释放逻辑。
    }

    /// <summary>
    /// 异步释放托管资源。子类应重写此方法来异步释放具体的资源。
    /// </summary>
    /// <returns>表示异步操作的 <see cref="ValueTask"/>。</returns>
    protected virtual ValueTask DisposeResourcesAsync()
    {
        // 子类实现具体的异步资源释放逻辑。
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// 确保对象未被释放，如果已释放则抛出异常。
    /// 此方法用于在执行操作前验证对象状态。
    /// </summary>
    /// <exception cref="ObjectDisposedException">当对象已被释放时抛出此异常。</exception>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().Name);
    }

    /// <summary>
    /// 安全释放 <see cref="IDisposable"/> 对象，忽略释放过程中的异常。
    /// 此方法确保即使释放过程出现异常也不会影响调用方。
    /// </summary>
    /// <param name="disposable">要释放的 <see cref="IDisposable"/> 对象。如果为 <see langword="null"/>，则不执行任何操作。</param>
    /// <param name="shouldDispose">指示是否应该释放该对象。如果为 <see langword="false"/>，则不执行释放操作。</param>
    protected static void SafeDispose(IDisposable? disposable, bool shouldDispose = true)
    {
        if (disposable == null || !shouldDispose)
            return;

        try
        {
            disposable.Dispose();
        }
        catch
        {
            // 忽略释放过程中的异常。
        }
    }

    /// <summary>
    /// 安全异步释放 <see cref="IAsyncDisposable"/> 对象，忽略释放过程中的异常。
    /// 此方法确保即使释放过程出现异常也不会影响调用方。
    /// </summary>
    /// <param name="disposable">要释放的 <see cref="IAsyncDisposable"/> 对象。如果为 <see langword="null"/>，则不执行任何操作。</param>
    /// <param name="shouldDispose">指示是否应该释放该对象。如果为 <see langword="false"/>，则不执行释放操作。</param>
    /// <returns>表示异步操作的 <see cref="ValueTask"/>。</returns>
    protected static async ValueTask SafeDisposeAsync(IAsyncDisposable? disposable, bool shouldDispose = true)
    {
        if (disposable == null || !shouldDispose)
            return;

        try
        {
            await disposable.DisposeAsync().ConfigureAwait(false);
        }
        catch
        {
            // 忽略释放过程中的异常。
        }
    }

    /// <summary>
    /// 安全释放实现 <see cref="IDisposable"/> 的对象，自动检测对象类型。
    /// 忽略释放过程中的异常。
    /// </summary>
    /// <param name="disposable">要释放的对象。如果为 <see langword="null"/>，则不执行任何操作。</param>
    /// <param name="shouldDispose">指示是否应该释放该对象。如果为 <see langword="false"/>，则不执行释放操作。</param>
    protected static void SafeDispose(object? disposable, bool shouldDispose = true)
    {
        if (disposable == null || !shouldDispose)
            return;

        try
        {
            if (disposable is IDisposable syncDisposable)
            {
                syncDisposable.Dispose();
            }
        }
        catch
        {
            // 忽略释放过程中的异常。
        }
    }

    /// <summary>
    /// 安全异步释放同时实现 <see cref="IDisposable"/> 和 <see cref="IAsyncDisposable"/> 的对象。
    /// 优先使用 <see cref="IAsyncDisposable.DisposeAsync"/> 方法，如果不支持则降级到 <see cref="IDisposable.Dispose"/> 方法。
    /// 忽略释放过程中的异常。
    /// </summary>
    /// <param name="disposable">要释放的对象。如果为 <see langword="null"/>，则不执行任何操作。</param>
    /// <param name="shouldDispose">指示是否应该释放该对象。如果为 <see langword="false"/>，则不执行释放操作。</param>
    /// <returns>表示异步操作的 <see cref="ValueTask"/>。</returns>
    protected static async ValueTask SafeDisposeAsync(object? disposable, bool shouldDispose = true)
    {
        if (disposable == null || !shouldDispose)
            return;

        try
        {
            if (disposable is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
            }
            else if (disposable is IDisposable syncDisposable)
            {
                syncDisposable.Dispose();
            }
        }
        catch
        {
            // 忽略释放过程中的异常。
        }
    }
}