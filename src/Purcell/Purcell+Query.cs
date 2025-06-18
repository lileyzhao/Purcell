namespace PurcellLibs;

/**
 * 这里是 Purcell 查询表格的相关方法及重载
 */
public static partial class Purcell
{
    #region 同步查询

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<T> Query<T>(
        Stream stream, QueryType queryType, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        using IPurQuerier querier = CreateQuerier(stream, queryType);
        foreach (T item in querier.Query<T>(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<T> Query<T>(
        string filePath, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        using IPurQuerier querier = CreateQuerier(filePath);
        foreach (T item in querier.Query<T>(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<IDictionary<string, object?>> Query(
        Stream stream, QueryType queryType, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = CreateQuerier(stream, queryType);
        foreach (IDictionary<string, object?> item in querier.Query(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<IDictionary<string, object?>> Query(
        string filePath, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = CreateQuerier(filePath);
        foreach (IDictionary<string, object?> item in querier.Query(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.QueryDynamic(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<dynamic> QueryDynamic(
        Stream stream, QueryType queryType, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = CreateQuerier(stream, queryType);
        foreach (dynamic item in querier.QueryDynamic(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.QueryDynamic(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<dynamic> QueryDynamic(
        string filePath, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = CreateQuerier(filePath);
        foreach (dynamic item in querier.QueryDynamic(tableConfig, progress, cancelToken))
            yield return item;
    }

    #endregion 同步查询

    #region 异步查询

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<T>> QueryAsync<T>(
        Stream stream, QueryType queryType, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        return await Task.Run(() => Query<T>(stream, queryType, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<T>> QueryAsync<T>(
        string filePath, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        return await Task.Run(() => Query<T>(filePath, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<IDictionary<string, object?>>> QueryAsync(
        Stream stream, QueryType queryType, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => Query(stream, queryType, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<IDictionary<string, object?>>> QueryAsync(
        string filePath, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => Query(filePath, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.QueryDynamic(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<dynamic>> QueryDynamicAsync(
        Stream stream, QueryType queryType, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryDynamic(stream, queryType, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.QueryDynamic(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<dynamic>> QueryDynamicAsync(
        string filePath, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryDynamic(filePath, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    #endregion 异步查询
}