namespace PurcellLibs;

/// <summary>
/// 表格配置类 - 定义 Excel/CSV 表格文件中单个工作表的读写配置。<br />
/// 
/// <para>
/// 定义如何读取或写入 Excel/CSV 表格文件中的单个工作表。
/// 比如：操作哪张工作表？从第几行开始读取？哪些列需要处理？数据格式是什么样的？
/// 一个 PurTable 实例对应一张工作表。
/// </para>
/// 
/// <para>
/// 配置方式：
/// 可以通过特性标记、代码创建或工厂方法等多种方式进行配置，
/// 支持链式调用进行流畅的配置。
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// 既可以作为特性使用（标记在类上），也可以作为普通配置对象使用。
/// </para>
/// 
/// <para>
/// 配置优先级：
/// 当 PurTable 既通过特性（Attribute）方式配置，又在方法调用时直接传入参数配置时，
/// 将以方法参数中的配置为准，覆盖特性中定义的配置。
/// </para>
/// 
/// <para>
/// 工作表定位：
/// 每个 PurTable 实例通过 <see cref="SheetName"/>（工作表名称）或 <see cref="SheetIndex"/>（工作表索引）来唯一定位一张工作表。
/// 当同时设置时，优先使用 SheetName 进行定位。
/// </para>
/// </remarks>
public interface IPurTable
{
    /// <summary>
    /// 工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。
    /// 当同时设置了 <see cref="SheetIndex"/> 时，优先查找工作表名称来定位。
    /// </summary>
    string SheetName { get; set; }

    /// <summary>
    /// 工作表索引，用于定位要操作的工作表（索引序列含隐藏的工作表），范围为 0-255。
    /// 该索引的优先级低于 <see cref="SheetName"/> 属性。
    /// </summary>
    int SheetIndex { get; set; }

    /// <summary>
    /// 表格是否包含表头行，默认为 true。
    /// 当设置为 false 时，将不会尝试从表格中读取表头信息，而是直接从数据行开始读取。
    /// </summary>
    bool HasHeader { get; set; }

    /// <summary>
    /// 表头起始单元格位置，默认为 A1，仅在 <see cref="HasHeader"/> 为 true 时有效。
    /// 表示表头行的起始单元格，格式如 "A1"、"B2" 等。
    /// </summary>
    string HeaderStart { get; set; }

    /// <summary>
    /// 数据起始单元格位置，默认为 HeaderStart 下方单元格。<br />
    /// <para>数据起始列索引必须与表头起始列索引相同，且数据行必须位于表头行之后。</para>
    /// </summary>
    string DataStart { get; set; }

    /// <summary>
    /// 列配置集合，用于定义表格中每一列的行为和属性映射关系。
    /// 通过列配置，可以控制每列的读取、转换和导出行为，包括列名映射、数据类型转换和格式设置等。
    /// </summary>
    /// <remarks>
    /// 列配置决定了如何将表格列与对象属性进行映射，以及如何处理不同类型的数据。
    /// 可以通过代码或特性配置方式添加列配置。
    /// </remarks>
    List<PurColumn> Columns { get; set; }

    /// <summary>
    /// 读取表格时的最大读取行数限制，默认为 -1，表示不限制。
    /// </summary>
    /// <remarks>⚠️ <c>仅在读取表格时有效</c></remarks>
    int MaxReadRows { get; set; }

    /// <summary>
    /// 导出表格时的最大写入行数限制，默认为 -1，表示不限制。
    /// </summary>
    /// <remarks>⚠️ <c>仅在导出表格时有效</c></remarks>
    int MaxWriteRows { get; set; }

    /// <summary>
    /// 是否忽略值解析错误，默认为 false，解析失败时抛出异常。
    /// 当设置为 true 时，解析数据过程中遇到格式错误会尝试使用默认值或跳过。
    /// </summary>
    /// <remarks>⚠️ <c>仅在读取表格时有效</c></remarks>
    bool IgnoreParseError { get; set; }

    /// <summary>
    /// 表头空白字符处理模式，默认为 <see cref="WhiteSpaceMode.Trim"/>。
    /// 主要用于动态类型或字典类型读取时的列名处理。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 处理示例：<br />
    /// - 使用 <see cref="WhiteSpaceMode.Trim"/> 时：" User Name \n" → "User Name"<br />
    /// - 使用 <see cref="WhiteSpaceMode.RemoveAll"/> 时：" User Name \n" → "UserName"
    /// </para>
    /// <br />⚠️ <c>仅在读取表格时有效</c>
    /// </remarks>
    WhiteSpaceMode HeaderSpaceMode { get; set; }

    /// <summary>
    /// 文化信息，默认为与语言无关的文化信息（<see cref="CultureInfo.InvariantCulture"/>）。
    /// 用于解析或格式化如日期、数字、货币等数据。
    /// </summary>
    string Culture { get; set; }

    /// <summary>
    /// 文件编码，默认为 null，表示自动检测编码。
    /// 用于指定读取文件时使用的字符编码（目前仅用于读取 CSV 时使用）。
    /// </summary>
    string? FileEncoding { get; set; }

    /// <summary>
    /// CSV 分隔符，默认为逗号 ","。
    /// 用于指定 CSV 文件中分隔字段的字符或字符串。
    /// </summary>
    string CsvDelimiter { get; set; }

    /// <summary>
    /// CSV 文本限定符，默认为双引号 '"'。
    /// 用于指定 CSV 文件中包围字段内容的字符，特别是当字段内容包含分隔符时。
    /// </summary>
    char CsvEscape { get; set; }

    /// <summary>
    /// 要导出的数据集合，表示将要写入到表格中的记录数据。
    /// 可以是对象集合、字典集合或其他可以转换为行数据的集合。
    /// </summary>
    /// <remarks>⚠️ <c>仅在导出表格时有效</c></remarks>
    IEnumerable<IDictionary<string, object?>?> Records { get; }

    /// <summary>
    /// 样本行数，系统将分析集合中的前N个元素来进行列类型推断和自动列宽计算，默认值为5。
    /// 设置为 0 则不进行自动列宽计算，仅使用列名长度作为初始宽度。
    /// </summary>
    /// <remarks>⚠️ <c>仅在导出表格时有效</c></remarks>
    int SampleRows { get; set; }

    /// <summary>
    /// 是否启用 Excel 表头自动筛选功能，默认值为 true。
    /// </summary>
    /// <remarks>⚠️ <c>仅在导出表格时有效，对 CSV 格式无效</c></remarks>
    bool AutoFilter { get; set; }

    /// <summary>
    /// Excel 工作表保护密码。
    /// </summary>
    /// <remarks>⚠️ <c>仅在导出表格时有效，对 CSV 格式无效</c></remarks>
    string? Password { get; set; }

    /// <summary>
    /// Excel 工作表样式，默认为 <see cref="PurStyle.Default"/>。
    /// </summary>
    /// <remarks>⚠️ <c>仅在导出表格时有效，对 CSV 格式无效</c></remarks>
    PurStyle TableStyle { get; set; }

    /// <summary>
    /// 获取表头起始单元格位置。
    /// </summary>
    /// <returns>表头起始单元格的 <see cref="CellLocator"/> 实例。</returns>
    CellLocator GetHeaderStart();

    /// <summary>
    /// 获取数据起始单元格位置。
    /// </summary>
    /// <returns>数据起始单元格的 <see cref="CellLocator"/> 实例。</returns>
    /// <exception cref="ArgumentException">当数据起始单元格列索引与表头起始单元格列索引不同时抛出，或当数据起始行小于等于表头起始行时抛出。</exception>
    CellLocator GetDataStart();

    /// <summary>
    /// 获取文化信息。
    /// </summary>
    /// <returns>表示文化信息的 <see cref="CultureInfo"/> 实例，如果未设置则返回与语言无关的文化信息（<c>InvariantCulture</c>）。</returns>
    CultureInfo GetCulture();

    /// <summary>
    /// 获取 CSV 文件读取编码。
    /// </summary>
    /// <returns>表示文件编码的 <see cref="Encoding"/> 实例，如果未设置则返回 null。</returns>
    Encoding? GetEncoding();

    /// <summary>
    /// 链式设置工作表名称。
    /// </summary>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithName(string sheetName);

    /// <summary>
    /// 链式设置工作表索引。
    /// </summary>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithIndex(int sheetIndex);

    /// <summary>
    /// 链式设置是否包含表头行。
    /// </summary>
    /// <param name="hasHeader">为 true 表示包含表头行。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithHasHeader(bool hasHeader);

    /// <summary>
    /// 链式设置为不包含表头行，等价于 HasHeader = false。
    /// </summary>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithoutHeader();

    /// <summary>
    /// 链式设置表头起始单元格位置。
    /// </summary>
    /// <param name="headerStart">表头起始单元格位置，格式如 "A1"、"B2"。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithHeaderStart(string headerStart);

    /// <summary>
    /// 链式设置表头起始单元格位置。
    /// </summary>
    /// <param name="headerStart">表头起始单元格位置的 <see cref="CellLocator"/> 实例。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="headerStart"/> 为 <see cref="CellLocator.Unknown"/> 时抛出。</exception>
    PurTable WithHeaderStart(CellLocator headerStart);

    /// <summary>
    /// 链式设置表头起始单元格位置。
    /// </summary>
    /// <param name="rowIndex">行索引（从 0 开始）。</param>
    /// <param name="columnIndex">列索引（从 0 开始）。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithHeaderStart(int rowIndex, int columnIndex);

    /// <summary>
    /// 链式设置数据起始单元格位置。
    /// </summary>
    /// <param name="dataStart">数据起始单元格位置，格式如 "A2"、"B3"。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithDataStart(string dataStart);

    /// <summary>
    /// 链式设置数据起始单元格位置。
    /// </summary>
    /// <param name="dataStart">数据起始单元格位置的 <see cref="CellLocator"/> 实例。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithDataStart(CellLocator dataStart);

    /// <summary>
    /// 链式设置数据起始单元格位置。
    /// </summary>
    /// <param name="rowIndex">行索引（从 0 开始）。</param>
    /// <param name="columnIndex">列索引（从 0 开始）。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithDataStart(int rowIndex, int columnIndex);

    /// <summary>
    /// 链式同时设置表头和数据起始单元格位置。
    /// </summary>
    /// <param name="headerStart">表头起始单元格位置，格式如 "A1"、"B2"。</param>
    /// <param name="dataStart">数据起始单元格位置，格式如 "A2"、"B3"。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithStart(string headerStart, string dataStart);

    /// <summary>
    /// 链式同时设置表头和数据起始单元格位置。
    /// </summary>
    /// <param name="headerStart">表头起始单元格位置的 <see cref="CellLocator"/> 实例。</param>
    /// <param name="dataStart">数据起始单元格位置的 <see cref="CellLocator"/> 实例。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithStart(CellLocator headerStart, CellLocator dataStart);

    /// <summary>
    /// 链式同时设置表头行索引和数据起始行索引。
    /// </summary>
    /// <param name="headerRow">表头行索引（从 0 开始，列索引为 0）。</param>
    /// <param name="dataStartRow">数据起始行索引（从 0 开始，列索引为 0）。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithStart(int headerRow, int dataStartRow);

    /// <summary>
    /// 链式设置列配置集合。
    /// </summary>
    /// <param name="dynamicColumns">列配置集合。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithColumns(List<PurColumn> dynamicColumns);

    /// <summary>
    /// 链式设置列配置集合。
    /// </summary>
    /// <param name="dynamicColumns">列配置集合。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithColumns(PurColumn[] dynamicColumns);

    /// <summary>
    /// 链式添加多个列配置。
    /// </summary>
    /// <param name="dynamicColumns">要添加的列配置集合。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable AddColumns(List<PurColumn> dynamicColumns);

    /// <summary>
    /// 链式添加多个列配置。
    /// </summary>
    /// <param name="dynamicColumns">要添加的列配置集合。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable AddColumns(PurColumn[] dynamicColumns);

    /// <summary>
    /// 链式添加单个列配置。
    /// </summary>
    /// <param name="column">要添加的列配置。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable AddColumn(PurColumn column);

    /// <summary>
    /// 链式设置最大读取行数。
    /// </summary>
    /// <param name="maxReadRows">最大读取行数，-1 表示不限制。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithMaxReadRows(int maxReadRows);

    /// <summary>
    /// 链式设置最大写入行数。
    /// </summary>
    /// <param name="maxWriteRows">最大写入行数，-1 表示不限制。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithMaxWriteRows(int maxWriteRows);

    /// <summary>
    /// 链式设置表头空白字符处理模式。
    /// </summary>
    /// <param name="headerSpaceMode">表头空白字符处理模式。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithHeaderSpaceMode(WhiteSpaceMode headerSpaceMode);

    /// <summary>
    /// 链式设置是否忽略解析错误。
    /// </summary>
    /// <param name="ignoreParseError">是否忽略解析错误。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithIgnoreParseError(bool ignoreParseError);

    /// <summary>
    /// 链式设置文化信息。
    /// </summary>
    /// <param name="culture">文化信息，如 "en-US"、"zh-CN"。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithCulture(string culture);

    /// <summary>
    /// 链式设置文化信息。
    /// </summary>
    /// <param name="culture">文化信息的 <see cref="CultureInfo"/> 实例。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithCulture(CultureInfo culture);

    /// <summary>
    /// 链式设置文件编码。
    /// </summary>
    /// <param name="fileEncoding">文件编码名称，如 "utf-8"、"gb2312"。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithFileEncoding(string fileEncoding);

    /// <summary>
    /// 链式设置文件编码。
    /// </summary>
    /// <param name="encoding">文件编码的 <see cref="Encoding"/> 实例。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithFileEncoding(Encoding encoding);

    /// <summary>
    /// 链式设置 CSV 文件分隔符。
    /// </summary>
    /// <param name="csvDelimiter">CSV 分隔符，默认为 ","。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithCsvDelimiter(string csvDelimiter = ",");

    /// <summary>
    /// 链式设置 CSV 文件文本限定符。
    /// </summary>
    /// <param name="csvEscape">CSV 文本限定符，默认为 '"'。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithCsvEscape(char csvEscape = '"');

    /// <summary>
    /// 链式设置要导出的数据（DataTable）。自动分析结构并生成列配置。
    /// </summary>
    /// <param name="dataTable">要导出的数据表。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dataTable"/> 为 null 时抛出。</exception>
    /// <exception cref="InvalidOperationException">当无法获取任何列信息时抛出。</exception>
    PurTable WithRecords(DataTable dataTable);

    /// <summary>
    /// 链式设置要导出的数据（泛型集合）。自动分析类型结构并生成列配置。
    /// </summary>
    /// <typeparam name="T">数据类型。</typeparam>
    /// <param name="records">要导出的数据集合，可以是对象集合、字典集合或其他可以转换为行数据的集合。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="records"/> 为 null 时抛出。</exception>
    /// <exception cref="NotSupportedException">当 T 为 DataRow 类型时抛出，应使用 DataTable 作为数据源。</exception>
    /// <exception cref="InvalidOperationException">当无法获取任何列信息时抛出。</exception>
    PurTable WithRecords<T>(IEnumerable<T?> records);

    /// <summary>
    /// 链式设置用于列类型推断和自动列宽的样本行数。
    /// </summary>
    /// <param name="sampleRows">样本行数，0 表示不自动计算列宽。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="sampleRows"/> 小于 0 时抛出。</exception>
    PurTable WithSampleRows(int sampleRows);

    /// <summary>
    /// 链式设置是否启用表头自动筛选。
    /// </summary>
    /// <param name="autoFilter">是否启用表头自动筛选（CSV 格式无效）。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithAutoFilter(bool autoFilter);

    /// <summary>
    /// 链式设置工作表保护密码。
    /// </summary>
    /// <param name="password">保护密码，为 null 表示不启用（CSV 格式无效）。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithPassword(string? password);

    /// <summary>
    /// 链式设置工作表样式配置。
    /// </summary>
    /// <param name="style">工作表样式配置。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithTableStyle(PurStyle style);
}