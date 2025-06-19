namespace PurcellLibs.Providers.TableWriter;

/// <summary>
/// 表格数据写入器接口
/// </summary>
public interface ITableWriter : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// 将数据写入表格文件
    /// </summary>
    /// <param name="tableConfigs">工作表集合</param>
    /// <param name="progress">进度报告回调（可选）</param>
    /// <param name="cancelToken">取消令牌（可选）</param>
    void WriteTable(IList<PurTable> tableConfigs,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default);
}