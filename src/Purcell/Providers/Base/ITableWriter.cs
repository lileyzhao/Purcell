namespace PurcellLibs.Providers;

/// <summary>
/// 表格写入器接口，定义表格数据写入的基本操作。
/// </summary>
public interface ITableWriter : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// 将表格配置数据写入到目标文件中。
    /// </summary>
    /// <param name="tableConfigs">表格配置列表，每个配置包含数据源、工作表名称、格式设置等信息。</param>
    /// <param name="progress">可选的进度报告器，报告当前写入位置信息。</param>
    /// <param name="cancelToken">可选的取消令牌。</param>
    void WriteTable(IList<PurTable> tableConfigs,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default);
}