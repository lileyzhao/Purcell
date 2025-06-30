namespace PurcellLibs.Providers;

/// <summary>
/// 表格读取器接口，定义表格数据读取的基本操作。
/// </summary>
public interface ITableReader : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// 获取文件中所有工作表的名称。
    /// </summary>
    /// <returns>工作表名称序列。对于 CSV 文件，通常返回空序列或单个默认名称。</returns>
    IEnumerable<string> GetWorksheets();

    /// <summary>
    /// 读取表格数据，返回以列索引为键的字典序列。
    /// </summary>
    /// <param name="tableConfig">表格配置信息，包含工作表名称、数据范围等设置。</param>
    /// <param name="progress">可选的进度报告器，报告已读取的行数。</param>
    /// <param name="cancelToken">可选的取消令牌。</param>
    /// <returns>字典序列，每个字典代表一行数据，键为列索引（从0开始），值为单元格原始数据。</returns>
    IEnumerable<IDictionary<int, object?>> ReadTable(PurTable tableConfig,
        IProgress<int>? progress = null, CancellationToken cancelToken = default);
}