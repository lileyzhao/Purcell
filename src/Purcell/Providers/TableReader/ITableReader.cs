namespace PurcellLibs.Providers.TableReader;

/// <summary>
/// 表格数据读取器接口
/// </summary>
public interface ITableReader : IDisposable, IAsyncDisposable
{
    /// <summary>工作表集合</summary>
    IList<string> Worksheets { get; set; }

    /// <summary>
    /// 从指定的流中查询表格数据
    /// </summary>
    /// <param name="tableConfig">查询表格文件的配置</param>
    /// <param name="progress">进度报告回调（可选）</param>
    /// <param name="cancelToken">取消令牌（可选）</param>
    IEnumerable<T> Read<T>(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new();

    /// <summary>
    /// 从指定的流中查询表格数据
    /// </summary>
    /// <param name="tableConfig">查询表格文件的配置</param>
    /// <param name="progress">进度报告回调（可选）</param>
    /// <param name="cancelToken">取消令牌（可选）</param>
    IEnumerable<IDictionary<string, object?>> ReadDict(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default);

    /// <summary>
    /// 从指定的流中查询表格数据
    /// </summary>
    /// <param name="tableConfig">查询表格文件的配置</param>
    /// <param name="progress">进度报告回调（可选）</param>
    /// <param name="cancelToken">取消令牌（可选）</param>
    IEnumerable<dynamic> ReadDynamic(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default);
}