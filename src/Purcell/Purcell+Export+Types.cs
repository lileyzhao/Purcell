namespace PurcellLibs;

/**
 * 这里是 Purcell 查询表格的相关方法及重载
 */
public static partial class Purcell
{
    #region 同步导出

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportCsv(PurTable sheetData, Stream stream,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(stream, ExportType.Csv);
        exporter.Export([sheetData], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportXlsx(IList<PurTable> sheetDatas, Stream stream,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(stream, ExportType.Xlsx);
        exporter.Export(sheetDatas, progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportXlsx(PurTable sheetData, Stream stream,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(stream, ExportType.Xlsx);
        exporter.Export([sheetData], progress, cancelToken);
    }

    #endregion 同步导出

    #region 异步导出

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportCsvAsync(PurTable sheetData, Stream stream,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => ExportCsv(sheetData, stream, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportXlsxAsync(IList<PurTable> sheetDatas, Stream stream,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => ExportXlsx(sheetDatas, stream, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportXlsxAsync(PurTable sheetData, Stream stream,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => ExportXlsx(sheetData, stream, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    #endregion 异步导出
}