namespace PurcellLibs;

/// <summary>
/// Purcell 扩展方法类
/// </summary>
public static partial class PurcellExtension
{
    #region 同步导出

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(this Stream stream, IList<PurTable> sheetDatas, ExportType exportType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(stream, exportType);
        exporter.Export(sheetDatas, progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(this Stream stream, PurTable sheetData, ExportType exportType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(stream, exportType);
        exporter.Export([sheetData], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(this string filePath, IList<PurTable> sheetDatas,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(filePath);
        exporter.Export(sheetDatas, progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(this string filePath, PurTable sheetData,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(filePath);
        exporter.Export([sheetData], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportCsv(this Stream stream, PurTable sheetData,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(stream, ExportType.Csv);
        exporter.Export([sheetData], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportXlsx(this Stream stream, IList<PurTable> sheetDatas,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(stream, ExportType.Xlsx);
        exporter.Export(sheetDatas, progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportXlsx(this Stream stream, PurTable sheetData,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(stream, ExportType.Xlsx);
        exporter.Export([sheetData], progress, cancelToken);
    }

    #endregion 同步导出

    #region 异步导出

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(this Stream stream, IList<PurTable> sheetDatas, ExportType exportType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(stream, sheetDatas, exportType, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(this Stream stream, PurTable sheetData, ExportType exportType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(stream, sheetData, exportType, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(this string filePath, IList<PurTable> sheetDatas,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(filePath, sheetDatas, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(this string filePath, PurTable sheetData,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(filePath, sheetData, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportCsvAsync(this Stream stream, PurTable sheetData,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => ExportCsv(stream, sheetData, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportXlsxAsync(this Stream stream, IList<PurTable> sheetDatas,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => ExportXlsx(stream, sheetDatas, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportXlsxAsync(this Stream stream, PurTable sheetData,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => ExportXlsx(stream, sheetData, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    #endregion 异步导出
}