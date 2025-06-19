#pragma warning disable CS1572 // XML 注释中有 param 标记，但是没有该名称的参数

namespace PurcellLibs.Providers;

/// <summary>
/// 表格查询器接口 - 为查询方法提供统一的注释。
/// </summary>
public interface IPurQuerier : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// 从指定的文件或流中查询数据
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="stream">数据流</param>
    /// <param name="exportType">导出类型枚举</param>
    /// <param name="tableConfig">查询表格文件的配置（可选）</param>
    /// <param name="ownsStream">是否拥有流的释放权限（可选）</param>
    /// <param name="progress">进度报告回调（可选）</param>
    /// <param name="cancelToken">取消令牌（可选）</param>
    /// <returns>可枚举序列，包含查询到的数据项</returns>
    IEnumerable<T> Query<T>(PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new();

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    IEnumerable<IDictionary<string, object?>> Query(PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default);

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    IEnumerable<dynamic> QueryDynamic(PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default);
}