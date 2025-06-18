#pragma warning disable CS1572 // XML 注释中有 param 标记，但是没有该名称的参数

namespace PurcellLibs.Providers;

/// <summary>
/// 表格导出器接口
/// </summary>
/// <remarks>
/// 该封装层的目的是分离实现层(TableReader/TableWriter)，确保底层实现变更时不需要变动上层调用代码，且底层可随时切换。
/// </remarks>
public interface IPurExporter : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// 将数据导出到指定的流中
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="stream">数据流</param>
    /// <param name="exportType">导出类型枚举</param>
    /// <param name="sheetDatas">要导出的工作表数据集合</param>
    /// <param name="sheetData">要导出的工作表数据</param>
    /// <param name="ownsStream">是否拥有流的释放权限（可选）</param>
    /// <param name="progress">进度报告回调（可选）</param>
    /// <param name="cancelToken">取消令牌（可选）</param>
    void Export(IList<PurTable> sheetDatas,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default);
}