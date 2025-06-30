namespace PurcellLibs;

/**
 * 这里是 Purcell 查询表格的相关方法及重载
 */
public static partial class Purcell
{
    #region 同步导出

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(IList<PurTable> tableConfigs, Stream stream, TableFileType fileType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(stream, fileType);
        exporter.Export(tableConfigs, progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(PurTable tableConfig, Stream stream, TableFileType fileType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(stream, fileType);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(IList<PurTable> tableConfigs, string filePath,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(filePath);
        exporter.Export(tableConfigs, progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(PurTable tableConfig, string filePath,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(filePath);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export<T>(IEnumerable<T?> records, Stream stream, TableFileType fileType,
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

        using IPurExporter exporter = CreateExporter(stream, fileType);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(DataTable records, Stream stream, TableFileType fileType,
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

        using IPurExporter exporter = CreateExporter(stream, fileType);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export<T>(IEnumerable<T?> records, string filePath,
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

        using IPurExporter exporter = CreateExporter(filePath);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void Export(DataTable records, string filePath,
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

        using IPurExporter exporter = CreateExporter(filePath);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportCsv(PurTable tableConfig, Stream stream,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(stream, TableFileType.Csv);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportCsv<T>(IEnumerable<T?> records, Stream stream,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        Export(
            records, stream, TableFileType.Csv,
            sheetName, hasHeader, headerStart, dataStart,
            columns, maxWriteRows, fileEncoding,
            csvDelimiter, csvEscape, sampleRows, autoFilter,
            password, tableStyle, presetStyle, progress, cancelToken
        );
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportCsv(DataTable dataTable, Stream stream,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        Export(
            dataTable, stream, TableFileType.Csv,
            sheetName, hasHeader, headerStart, dataStart,
            columns, maxWriteRows, fileEncoding,
            csvDelimiter, csvEscape, sampleRows, autoFilter,
            password, tableStyle, presetStyle, progress, cancelToken
        );
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportXlsx(IList<PurTable> tableConfigs, Stream stream,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(stream, TableFileType.Xlsx);
        exporter.Export(tableConfigs, progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportXlsx(PurTable tableConfig, Stream stream,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        using IPurExporter exporter = CreateExporter(stream, TableFileType.Xlsx);
        exporter.Export([tableConfig], progress, cancelToken);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportXlsx<T>(IEnumerable<T?> records, Stream stream,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        Export(
            records, stream, TableFileType.Xlsx,
            sheetName, hasHeader, headerStart, dataStart,
            columns, maxWriteRows, fileEncoding,
            csvDelimiter, csvEscape, sampleRows, autoFilter,
            password, tableStyle, presetStyle, progress, cancelToken
        );
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static void ExportXlsx(DataTable dataTable, Stream stream,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        Export(
            dataTable, stream, TableFileType.Xlsx,
            sheetName, hasHeader, headerStart, dataStart,
            columns, maxWriteRows, fileEncoding,
            csvDelimiter, csvEscape, sampleRows, autoFilter,
            password, tableStyle, presetStyle, progress, cancelToken
        );
    }

    #endregion 同步导出

    #region 异步导出

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(IList<PurTable> tableConfigs, Stream stream, TableFileType fileType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(tableConfigs, stream, fileType, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(PurTable tableConfig, Stream stream, TableFileType fileType,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(tableConfig, stream, fileType, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(IList<PurTable> tableConfigs, string filePath,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(tableConfigs, filePath, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(PurTable tableConfig, string filePath,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(tableConfig, filePath, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync<T>(IEnumerable<T?> records, Stream stream, TableFileType fileType,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    records, stream, fileType,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(DataTable dataTable, Stream stream, TableFileType fileType,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    dataTable, stream, fileType,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync<T>(IEnumerable<T?> records, string filePath,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    records, filePath,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportAsync(DataTable dataTable, string filePath,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    dataTable, filePath,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportCsvAsync(PurTable tableConfig, Stream stream,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => ExportCsv(tableConfig, stream, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportCsvAsync<T>(IEnumerable<T?> records, Stream stream,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    records, stream, TableFileType.Csv,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportCsvAsync(DataTable dataTable, Stream stream,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    dataTable, stream, TableFileType.Csv,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportXlsxAsync(IList<PurTable> tableConfigs, Stream stream,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => ExportXlsx(tableConfigs, stream, progress, cancelToken), cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportXlsxAsync(PurTable tableConfig, Stream stream,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => ExportXlsx(tableConfig, stream, progress, cancelToken), cancelToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportXlsxAsync<T>(IEnumerable<T?> records, Stream stream,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    records, stream, TableFileType.Xlsx,
                    sheetName, hasHeader, headerStart, dataStart,
                    columns, maxWriteRows, fileEncoding,
                    csvDelimiter, csvEscape, sampleRows, autoFilter,
                    password, tableStyle, presetStyle, progress, cancelToken
                ),
                cancelToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="IPurExporter.Export(IList{PurTable},IProgress{WritePosition},CancellationToken)"/>
    public static async Task ExportXlsxAsync(DataTable dataTable, Stream stream,
        string sheetName = "", bool hasHeader = true, string headerStart = "A1", string dataStart = "",
        List<PurColumn>? columns = null, int maxWriteRows = -1, string? fileEncoding = null,
        string csvDelimiter = ",", char csvEscape = '"', int sampleRows = 5, bool autoFilter = true,
        string password = "", PurStyle? tableStyle = null, PresetStyle presetStyle = PresetStyle.Default,
        IProgress<WritePosition>? progress = null, CancellationToken cancelToken = default)
    {
        await Task.Run(() => Export(
                    dataTable, stream, TableFileType.Xlsx,
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