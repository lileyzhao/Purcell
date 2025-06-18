namespace PurcellLibs;

/**
 * 这里是 Purcell 查询表格的相关方法及重载
 */
public static partial class Purcell
{
    #region 同步导出

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(IList<PurTable> sheetDatas, Stream stream, ExportType exportType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(stream, exportType);
        exporter.Export(sheetDatas, progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(IList<PurTable> sheetDatas, string filePath,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(filePath);
        exporter.Export(sheetDatas, progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(PurTable sheetData, Stream stream, ExportType exportType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(stream, exportType);
        exporter.Export([sheetData], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(PurTable sheetData, string filePath,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(filePath);
        exporter.Export([sheetData], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export<T>(IEnumerable<T> collection, Stream stream, ExportType exportType,
        string sheetName = "", string writeStart = "", bool autoFilter = true, string password = "",
        PurStyle? sheetStyle = null,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        PurTable sheetData = PurTable.FromRecords(collection, sheetName);
        if (!string.IsNullOrEmpty(writeStart)) sheetData.WithHeaderStart(writeStart);
        if (!autoFilter) sheetData.WithAutoFilter(autoFilter);
        if (!string.IsNullOrEmpty(password)) sheetData.WithPassword(password);
        if (sheetStyle != null) sheetData.WithTableStyle(sheetStyle);

        using IPurExporter exporter = CreateExporter(stream, exportType);
        exporter.Export([sheetData], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export<T>(IEnumerable<T> collection, string filePath,
        string sheetName = "", string writeStart = "", bool autoFilter = true, string password = "",
        PurStyle? sheetStyle = null,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
        where T : class, new()
    {
        PurTable sheetData = PurTable.FromRecords(collection, sheetName);
        if (!string.IsNullOrEmpty(writeStart)) sheetData.WithHeaderStart(writeStart);
        if (!autoFilter) sheetData.WithAutoFilter(autoFilter);
        if (!string.IsNullOrEmpty(password)) sheetData.WithPassword(password);
        if (sheetStyle != null) sheetData.WithTableStyle(sheetStyle);

        using IPurExporter exporter = CreateExporter(filePath);
        exporter.Export([sheetData], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(DataTable collection, Stream stream, ExportType exportType,
        string sheetName = "", string writeStart = "", bool autoFilter = true, string password = "",
        PurStyle? sheetStyle = null,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        PurTable sheetData = PurTable.FromRecords(collection, sheetName);
        if (!string.IsNullOrEmpty(writeStart)) sheetData.WithHeaderStart(writeStart);
        if (!autoFilter) sheetData.WithAutoFilter(autoFilter);
        if (!string.IsNullOrEmpty(password)) sheetData.WithPassword(password);
        if (sheetStyle != null) sheetData.WithTableStyle(sheetStyle);

        using IPurExporter exporter = CreateExporter(stream, exportType);
        exporter.Export([sheetData], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(DataTable collection, string filePath,
        string sheetName = "", string writeStart = "", bool autoFilter = true, string password = "",
        PurStyle? sheetStyle = null,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        PurTable sheetData = PurTable.FromRecords(collection, sheetName);
        if (!string.IsNullOrEmpty(writeStart)) sheetData.WithHeaderStart(writeStart);
        if (!autoFilter) sheetData.WithAutoFilter(autoFilter);
        if (!string.IsNullOrEmpty(password)) sheetData.WithPassword(password);
        if (sheetStyle != null) sheetData.WithTableStyle(sheetStyle);

        using IPurExporter exporter = CreateExporter(filePath);
        exporter.Export([sheetData], progress, cancelToken);
    }

    #endregion 同步导出

    #region 异步导出

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(IList<PurTable> sheetDatas, Stream stream, ExportType exportType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(sheetDatas, stream, exportType, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(IList<PurTable> sheetDatas, string filePath,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(sheetDatas, filePath, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(PurTable sheetData, Stream stream, ExportType exportType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(sheetData, stream, exportType, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(PurTable sheetData, string filePath,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(sheetData, filePath, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    #endregion 异步导出
}