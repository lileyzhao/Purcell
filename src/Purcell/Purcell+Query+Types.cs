namespace PurcellLibs;

/**
 * 这里是 Purcell 查询表格的相关方法及重载
 */
public static partial class Purcell
{
    #region 同步查询

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<T> QueryCsv<T>(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        using IPurQuerier querier = CreateQuerier(stream, QueryType.Csv);
        foreach (T item in querier.Query<T>(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<IDictionary<string, object?>> QueryCsv(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = CreateQuerier(stream, QueryType.Csv);
        foreach (IDictionary<string, object?> item in querier.Query(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.QueryDynamic(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<dynamic> QueryCsvDynamic(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = CreateQuerier(stream, QueryType.Csv);
        foreach (dynamic item in querier.QueryDynamic(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<T> QueryXlsx<T>(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        using IPurQuerier querier = CreateQuerier(stream, QueryType.Xlsx);
        foreach (T item in querier.Query<T>(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<IDictionary<string, object?>> QueryXlsx(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = CreateQuerier(stream, QueryType.Xlsx);
        foreach (IDictionary<string, object?> item in querier.Query(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.QueryDynamic(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<dynamic> QueryXlsxDynamic(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = CreateQuerier(stream, QueryType.Xlsx);
        foreach (dynamic item in querier.QueryDynamic(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<T> QueryXlsb<T>(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        using IPurQuerier querier = CreateQuerier(stream, QueryType.Xlsb);
        foreach (T item in querier.Query<T>(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<IDictionary<string, object?>> QueryXlsb(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = CreateQuerier(stream, QueryType.Xlsb);
        foreach (IDictionary<string, object?> item in querier.Query(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.QueryDynamic(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<dynamic> QueryXlsbDynamic(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = CreateQuerier(stream, QueryType.Xlsb);
        foreach (dynamic item in querier.QueryDynamic(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<T> QueryXls<T>(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        using IPurQuerier querier = CreateQuerier(stream, QueryType.Xls);
        foreach (T item in querier.Query<T>(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<IDictionary<string, object?>> QueryXls(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = CreateQuerier(stream, QueryType.Xls);
        foreach (IDictionary<string, object?> item in querier.Query(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.QueryDynamic(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<dynamic> QueryXlsDynamic(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = CreateQuerier(stream, QueryType.Xls);
        foreach (dynamic item in querier.QueryDynamic(tableConfig, progress, cancelToken))
            yield return item;
    }

    #endregion 同步查询

    #region 异步查询

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<T>> QueryCsvAsync<T>(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        return await Task.Run(() => QueryCsv<T>(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<IDictionary<string, object?>>> QueryCsvAsync(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryCsv(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.QueryDynamic(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<dynamic>> QueryCsvDynamicAsync(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryCsvDynamic(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<T>> QueryXlsxAsync<T>(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        return await Task.Run(() => QueryXlsx<T>(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<IDictionary<string, object?>>> QueryXlsxAsync(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryXlsx(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.QueryDynamic(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<dynamic>> QueryXlsxDynamicAsync(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryXlsxDynamic(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<T>> QueryXlsbAsync<T>(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        return await Task.Run(() => QueryXlsb<T>(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<IDictionary<string, object?>>> QueryXlsbAsync(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryXlsb(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.QueryDynamic(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<dynamic>> QueryXlsbDynamicAsync(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryXlsbDynamic(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<T>> QueryXlsAsync<T>(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        return await Task.Run(() => QueryXls<T>(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<IDictionary<string, object?>>> QueryXlsAsync(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryXls(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.QueryDynamic(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<dynamic>> QueryXlsDynamicAsync(
        Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryXlsDynamic(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    #endregion 异步查询
}