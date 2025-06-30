namespace PurcellLibs;

/// <summary>
/// Purcell 扩展方法类
/// </summary>
public static partial class PurcellExtension
{
    #region 同步查询

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<T> Query<T>(this Stream stream, TableFileType fileType, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        using IPurQuerier querier = new PurQuerier(stream, fileType);
        foreach (T item in querier.Query<T>(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<T> QueryCsv<T>(this Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        using IPurQuerier querier = new PurQuerier(stream, TableFileType.Csv);
        foreach (T item in querier.Query<T>(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<T> QueryXlsx<T>(this Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        using IPurQuerier querier = new PurQuerier(stream, TableFileType.Xlsx);
        foreach (T item in querier.Query<T>(tableConfig, progress, cancelToken))
            yield return item;
    }


    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<T> QueryXls<T>(this Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        using IPurQuerier querier = new PurQuerier(stream, TableFileType.Xls);
        foreach (T item in querier.Query<T>(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<T> Query<T>(this string filePath, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        using IPurQuerier querier = new PurQuerier(filePath);
        foreach (T item in querier.Query<T>(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<IDictionary<string, object?>> Query(
        this Stream stream, TableFileType fileType,
        PurTable? tableConfig = null, IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = new PurQuerier(stream, fileType);
        foreach (IDictionary<string, object?> item in querier.Query(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<IDictionary<string, object?>> QueryCsv(
        this Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = new PurQuerier(stream, TableFileType.Csv);
        foreach (IDictionary<string, object?> item in querier.Query(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<IDictionary<string, object?>> QueryXlsx(
        this Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = new PurQuerier(stream, TableFileType.Xlsx);
        foreach (IDictionary<string, object?> item in querier.Query(tableConfig, progress, cancelToken))
            yield return item;
    }


    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<IDictionary<string, object?>> QueryXls(
        this Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = new PurQuerier(stream, TableFileType.Xls);
        foreach (IDictionary<string, object?> item in querier.Query(tableConfig, progress, cancelToken))
            yield return item;
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static IEnumerable<IDictionary<string, object?>> Query(
        this string filePath, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurQuerier querier = new PurQuerier(filePath);
        foreach (IDictionary<string, object?> item in querier.Query(tableConfig, progress, cancelToken))
            yield return item;
    }

    #endregion 同步查询

    #region 异步查询

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<T>> QueryAsync<T>(this Stream stream, TableFileType fileType,
        PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        return await Task.Run(() => Query<T>(stream, fileType, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<T>> QueryCsvAsync<T>(this Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        return await Task.Run(() => QueryCsv<T>(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<T>> QueryXlsxAsync<T>(this Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        return await Task.Run(() => QueryXlsx<T>(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }


    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<T>> QueryXlsAsync<T>(this Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        return await Task.Run(() => QueryXls<T>(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query{T}(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<T>> QueryAsync<T>(this string filePath, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        return await Task.Run(() => Query<T>(filePath, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<IDictionary<string, object?>>> QueryAsync(
        this Stream stream, TableFileType fileType, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => Query(stream, fileType, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<IDictionary<string, object?>>> QueryCsvAsync(
        this Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryCsv(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<IDictionary<string, object?>>> QueryXlsxAsync(
        this Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryXlsx(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }


    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<IDictionary<string, object?>>> QueryXlsAsync(
        this Stream stream, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => QueryXls(stream, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurQuerier.Query(PurTable,IProgress{int},CancellationToken)"/>
    public static async Task<IEnumerable<IDictionary<string, object?>>> QueryAsync(
        this string filePath, PurTable? tableConfig = null,
        IProgress<int>? progress = null, CancellationToken cancelToken = default)
    {
        return await Task.Run(() => Query(filePath, tableConfig, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    #endregion 异步查询
}