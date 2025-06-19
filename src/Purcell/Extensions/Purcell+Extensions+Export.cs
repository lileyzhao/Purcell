namespace PurcellLibs;

/// <summary>
/// Purcell 扩展方法类
/// </summary>
public static partial class PurcellExtension
{
    #region 同步导出

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(this Stream stream, IList<PurTable> tableConfigs, ExportType exportType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(stream, exportType);
        exporter.Export(tableConfigs, progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(this Stream stream, PurTable tableConfig, ExportType exportType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(stream, exportType);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(this string filePath, IList<PurTable> tableConfigs,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(filePath);
        exporter.Export(tableConfigs, progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(this string filePath, PurTable tableConfig,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(filePath);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export<T>(this Stream stream, IEnumerable<T?> records, ExportType exportType,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        PurTable tableConfig = PurTable.From(records, sheetName)
            .WithHasHeader(hasHeader)
            .WithHeaderStart(headerStart)
            .WithDataStart(dataStart)
            .WithMaxWriteRows(maxWriteRows)
            .WithCsvDelimiter(csvDelimiter)
            .WithCsvEscape(csvEscape)
            .WithSampleRows(sampleRows)
            .WithAutoFilter(autoFilter)
            .WithPresetStyle(presetStyle);
        if (columns != null) tableConfig.WithColumns(columns);
        if (!string.IsNullOrEmpty(fileEncoding)) tableConfig.WithFileEncoding(fileEncoding);
        if (!string.IsNullOrEmpty(password)) tableConfig.WithPassword(password);
        if (tableStyle != null) tableConfig.WithTableStyle(tableStyle);

        using IPurExporter exporter = Purcell.CreateExporter(stream, exportType);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(this Stream stream, DataTable records, ExportType exportType,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        PurTable tableConfig = PurTable.From(records, sheetName)
            .WithHasHeader(hasHeader)
            .WithHeaderStart(headerStart)
            .WithDataStart(dataStart)
            .WithMaxWriteRows(maxWriteRows)
            .WithCsvDelimiter(csvDelimiter)
            .WithCsvEscape(csvEscape)
            .WithSampleRows(sampleRows)
            .WithAutoFilter(autoFilter)
            .WithPresetStyle(presetStyle);
        if (columns != null) tableConfig.WithColumns(columns);
        if (!string.IsNullOrEmpty(fileEncoding)) tableConfig.WithFileEncoding(fileEncoding);
        if (!string.IsNullOrEmpty(password)) tableConfig.WithPassword(password);
        if (tableStyle != null) tableConfig.WithTableStyle(tableStyle);

        using IPurExporter exporter = Purcell.CreateExporter(stream, exportType);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export<T>(this string filePath, IEnumerable<T?> records,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        PurTable tableConfig = PurTable.From(records, sheetName)
            .WithHasHeader(hasHeader)
            .WithHeaderStart(headerStart)
            .WithDataStart(dataStart)
            .WithMaxWriteRows(maxWriteRows)
            .WithCsvDelimiter(csvDelimiter)
            .WithCsvEscape(csvEscape)
            .WithSampleRows(sampleRows)
            .WithAutoFilter(autoFilter)
            .WithPresetStyle(presetStyle);
        if (columns != null) tableConfig.WithColumns(columns);
        if (!string.IsNullOrEmpty(fileEncoding)) tableConfig.WithFileEncoding(fileEncoding);
        if (!string.IsNullOrEmpty(password)) tableConfig.WithPassword(password);
        if (tableStyle != null) tableConfig.WithTableStyle(tableStyle);

        using IPurExporter exporter = Purcell.CreateExporter(filePath);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(this string filePath, DataTable records,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        PurTable tableConfig = PurTable.From(records, sheetName)
            .WithHasHeader(hasHeader)
            .WithHeaderStart(headerStart)
            .WithDataStart(dataStart)
            .WithMaxWriteRows(maxWriteRows)
            .WithCsvDelimiter(csvDelimiter)
            .WithCsvEscape(csvEscape)
            .WithSampleRows(sampleRows)
            .WithAutoFilter(autoFilter)
            .WithPresetStyle(presetStyle);
        if (columns != null) tableConfig.WithColumns(columns);
        if (!string.IsNullOrEmpty(fileEncoding)) tableConfig.WithFileEncoding(fileEncoding);
        if (!string.IsNullOrEmpty(password)) tableConfig.WithPassword(password);
        if (tableStyle != null) tableConfig.WithTableStyle(tableStyle);

        using IPurExporter exporter = Purcell.CreateExporter(filePath);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportCsv(this Stream stream, PurTable tableConfig,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(stream, ExportType.Csv);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportCsv<T>(this Stream stream, IEnumerable<T?> records,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        Export(
            stream, records, ExportType.Csv,
            sheetName, hasHeader, headerStart, dataStart,
            columns, maxWriteRows, fileEncoding,
            csvDelimiter, csvEscape, sampleRows, autoFilter,
            password, tableStyle, presetStyle, progress, cancelToken
        );
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportCsv(this Stream stream, DataTable dataTable,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        Export(
            stream, dataTable, ExportType.Csv,
            sheetName, hasHeader, headerStart, dataStart,
            columns, maxWriteRows, fileEncoding,
            csvDelimiter, csvEscape, sampleRows, autoFilter,
            password, tableStyle, presetStyle, progress, cancelToken
        );
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportXlsx(this Stream stream, IList<PurTable> tableConfigs,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(stream, ExportType.Xlsx);
        exporter.Export(tableConfigs, progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportXlsx(this Stream stream, PurTable tableConfig,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = Purcell.CreateExporter(stream, ExportType.Xlsx);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportXlsx<T>(this Stream stream, IEnumerable<T?> records,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        Export(
            stream, records, ExportType.Xlsx,
            sheetName, hasHeader, headerStart, dataStart,
            columns, maxWriteRows, fileEncoding,
            csvDelimiter, csvEscape, sampleRows, autoFilter,
            password, tableStyle, presetStyle, progress, cancelToken
        );
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportXlsx(this Stream stream, DataTable dataTable,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        Export(
            stream, dataTable, ExportType.Xlsx,
            sheetName, hasHeader, headerStart, dataStart,
            columns, maxWriteRows, fileEncoding,
            csvDelimiter, csvEscape, sampleRows, autoFilter,
            password, tableStyle, presetStyle, progress, cancelToken
        );
    }

    #endregion 同步导出

    #region 异步导出

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(this Stream stream, IList<PurTable> tableConfigs, ExportType exportType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(stream, tableConfigs, exportType, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(this Stream stream, PurTable tableConfig, ExportType exportType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(stream, tableConfig, exportType, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(this string filePath, IList<PurTable> tableConfigs,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(filePath, tableConfigs, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(this string filePath, PurTable tableConfig,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(filePath, tableConfig, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync<T>(this Stream stream, IEnumerable<T?> records, ExportType exportType,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    stream, records, exportType,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(this Stream stream, DataTable dataTable, ExportType exportType,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    stream, dataTable, exportType,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync<T>(this string filePath, IEnumerable<T?> records,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    filePath, records,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(this string filePath, DataTable dataTable,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    filePath, dataTable,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportCsvAsync(this Stream stream, PurTable tableConfig,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => ExportCsv(stream, tableConfig, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportCsvAsync<T>(this Stream stream, IEnumerable<T?> records,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    stream, records, ExportType.Csv,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportCsvAsync(this Stream stream, DataTable dataTable,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    stream, dataTable, ExportType.Csv,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportXlsxAsync(this Stream stream, IList<PurTable> tableConfigs,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => ExportXlsx(stream, tableConfigs, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportXlsxAsync(this Stream stream, PurTable tableConfig,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => ExportXlsx(stream, tableConfig, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportXlsxAsync<T>(this Stream stream, IEnumerable<T?> records,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    stream, records, ExportType.Xlsx,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportXlsxAsync(this Stream stream, DataTable dataTable,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    stream, dataTable, ExportType.Xlsx,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    #endregion 异步导出
}