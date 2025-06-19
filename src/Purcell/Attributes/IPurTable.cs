namespace PurcellLibs;

/// <summary>
/// 表格配置 - 定义 Excel/CSV 文件中单个工作表的读写配置。
/// </summary>
/// <remarks>
/// <para>
/// 定义如何读取或写入工作表，包括工作表定位、表头和数据起始位置、列映射关系等。
/// 支持特性标记、代码创建或工厂方法等多种配置方式，并支持链式调用。
/// </para>
/// <para>
/// 配置优先级：方法参数 > 特性配置。
/// 工作表定位：<see cref="SheetName"/> > <see cref="SheetIndex"/>。
/// </para>
/// </remarks>
public interface IPurTable
{
    /// <summary>
    /// 工作表名称，用于定位目标工作表。导出时作为工作表的名称。
    /// </summary>
    /// <example>
    /// <code>
    /// // 设置工作表名称
    /// table.SheetName = "用户数据";
    /// // 或使用链式调用
    /// table.WithName("用户数据");
    /// </code>
    /// </example>
    /// <remarks>
    /// 当同时设置了 <see cref="SheetIndex"/> 时，优先使用工作表名称进行定位。
    /// </remarks>
    /// <exception cref="ArgumentNullException">当设置值为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当设置值长度超过31个字符时抛出。</exception>
    string SheetName { get; set; }

    /// <summary>
    /// 工作表索引，用于定位目标工作表，范围为 0-255。
    /// </summary>
    /// <example>
    /// <code>
    /// // 操作第一个工作表（包括隐藏工作表）
    /// table.SheetIndex = 0;
    /// // 操作第三个工作表
    /// table.WithIndex(2);
    /// </code>
    /// </example>
    /// <remarks>
    /// 索引序列包含隐藏的工作表。优先级低于 <see cref="SheetName"/> 属性。
    /// <para>
    /// ⚠️ 仅在读取表格时有效。
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">当设置值不在 0-255 范围内时抛出。</exception>
    int SheetIndex { get; set; }

    /// <summary>
    /// 表格是否包含表头行，默认为 true。
    /// </summary>
    /// <example>
    /// <code>
    /// // 有表头的表格（默认），使用实际列名
    /// table.HasHeader = true;
    /// // 读取结果：{ "姓名": "张三", "年龄": 25 }
    /// // 导出结果：包含表头行和数据行
    /// 
    /// // 无表头的表格，使用 Excel 列名
    /// table.WithoutHeader();
    /// // 读取结果：{ "A": "张三", "B": 25 }
    /// // dynamic 对象：obj.A, obj.B, obj.C...
    /// // 导出结果：仅包含数据行，无表头行
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// <strong>读取时：</strong>当设置为 false 时，读取时将直接从数据行开始，不会尝试获取表头信息。
    /// </para>
    /// <para>
    /// <strong>导出时：</strong>决定导出时是否包含表头行。为 true 时会输出表头行，为 false 时仅输出数据行。
    /// </para>
    /// <para>
    /// 无表头模式下，系统会自动使用 Excel 列名（A、B、C...）作为列名：
    /// </para>
    /// <para>
    /// - 读取为字典时：键名为 "A"、"B"、"C"...
    /// </para>
    /// <para>
    /// - 读取为 dynamic 时：属性名为 A、B、C...
    /// </para>
    /// </remarks>
    bool HasHeader { get; set; }

    /// <summary>
    /// 表头起始单元格位置，默认为 A1，仅在 <see cref="HasHeader"/> 为 true 时有效。
    /// 读取时表示表头行的位置，导出时表示表头行在导出文件中的起始位置，格式如 "A1"、"B2" 等。
    /// </summary>
    /// <exception cref="ArgumentException">当设置值不是有效的单元格位置格式时抛出。</exception>
    string HeaderStart { get; set; }

    /// <summary>
    /// 数据起始单元格位置，默认为 HeaderStart 下方单元格。
    /// </summary>
    /// <example>
    /// <code>
    /// // 表头在 A1，数据从 A2 开始（默认）
    /// table.HeaderStart = "A1";
    /// // DataStart 自动为 "A2"
    /// 
    /// // 手动指定数据位置
    /// table.WithStart("B2", "B4"); // 表头B2，数据从 B4 开始
    /// </code>
    /// </example>
    /// <remarks>
    /// 数据起始列索引必须与表头起始列索引相同，且数据行必须位于表头行之后。
    /// </remarks>
    /// <exception cref="ArgumentException">当设置值不是有效的单元格位置格式时抛出。</exception>
    string DataStart { get; set; }

    /// <summary>
    /// 列配置集合，用于定义表格中每一列的行为和属性映射关系。
    /// </summary>
    /// <example>
    /// <code>
    /// // 手动配置列映射
    /// table.Columns = new List&lt;PurColumn&gt;
    /// {
    ///     PurColumn.From("用户名").WithPropertyName("UserName"),
    ///     PurColumn.From("创建日期").WithPropertyName("CreateTime").WithDateFormat("yyyy-MM-dd")
    /// };
    /// 
    /// // 或使用链式调用
    /// table.AddColumn(PurColumn.From("邮箱").WithPropertyName("Email"));
    /// </code>
    /// </example>
    /// <remarks>
    /// 控制每列的读取、转换和导出行为，包括列名映射、数据类型转换和格式设置等。
    /// 可以通过代码或特性配置方式添加。
    /// </remarks>
    /// <exception cref="ArgumentNullException">当设置值为 null 时抛出。</exception>
    List<PurColumn> Columns { get; set; }

    /// <summary>
    /// 读取时的最大行数限制，默认为 -1（不限制）。
    /// </summary>
    /// <remarks>⚠️ 仅在读取表格时有效。</remarks>
    int MaxReadRows { get; set; }

    /// <summary>
    /// 导出时的最大行数限制，默认为 -1（不限制）。
    /// </summary>
    /// <remarks>⚠️ 仅在导出表格时有效。</remarks>
    int MaxWriteRows { get; set; }

    /// <summary>
    /// 是否忽略数据解析错误，默认为 false。
    /// 当设置为 true 时，解析失败会使用默认值或跳过，而不抛出异常。
    /// </summary>
    /// <remarks>⚠️ 仅在读取表格时有效。</remarks>
    bool IgnoreParseError { get; set; }

    /// <summary>
    /// 表头空白字符处理模式，默认为 <see cref="WhiteSpaceMode.Trim"/>。
    /// 主要用于动态类型或字典类型读取时的列名清理。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 处理示例：
    /// </para>
    /// <para>
    /// - 使用 <see cref="WhiteSpaceMode.Trim"/> 时：" User Name \n" → "User Name"
    /// </para>
    /// <para>
    /// - 使用 <see cref="WhiteSpaceMode.RemoveAll"/> 时：" User Name \n" → "UserName"
    /// </para>
    /// <para>
    /// ⚠️ 仅在读取表格时有效。
    /// </para>
    /// </remarks>
    WhiteSpaceMode HeaderSpaceMode { get; set; }

    /// <summary>
    /// 区域性标识符，默认为与语言无关的区域性标识符（<see cref="CultureInfo.InvariantCulture"/>）。
    /// 用于解析或格式化日期、数字、货币等数据。
    /// </summary>
    /// <example>
    /// <code>
    /// // 中文环境，适用于中国用户
    /// table.Culture = "zh-CN";
    /// 
    /// // 英文环境，适用于美国用户
    /// table.WithCulture("en-US");
    /// 
    /// // 影响日期格式：2023/12/25 vs 25/12/2023
    /// </code>
    /// </example>
    /// <exception cref="ArgumentException">当设置值不是有效的区域性标识符时抛出。</exception>
    string Culture { get; set; }

    /// <summary>
    /// 文件编码，默认为 null（读取时自动检测，导出时为UTF-8）。
    /// 用于指定读取或导出文件时的字符编码，目前仅支持 CSV 文件。
    /// </summary>
    /// <exception cref="ArgumentException">当设置值不是有效的编码名称时抛出。</exception>
    string? FileEncoding { get; set; }

    /// <summary>
    /// CSV 分隔符，默认为逗号 ","。
    /// 用于指定 CSV 文件中分隔字段的字符或字符串。
    /// </summary>
    /// <example>
    /// <code>
    /// // 逗号分隔（默认）
    /// table.CsvDelimiter = ",";
    /// 
    /// // 分号分隔，常用于欧洲
    /// table.WithCsvDelimiter(";");
    /// 
    /// // Tab 分隔
    /// table.WithCsvDelimiter("\t");
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">当设置值为 null 或空字符串时抛出。</exception>
    /// <exception cref="ArgumentException">当设置值包含文本限定符字符时抛出。</exception>
    string CsvDelimiter { get; set; }

    /// <summary>
    /// CSV 文本限定符，默认为双引号 '"'。
    /// 用于包围包含特殊字符（如分隔符、换行符）的字段内容。
    /// </summary>
    /// <exception cref="ArgumentNullException">当设置值为 null 字符时抛出。</exception>
    /// <exception cref="ArgumentException">当设置值为控制字符或与分隔符冲突时抛出。</exception>
    char CsvEscape { get; set; }

    /// <summary>
    /// 要导出的数据集合，表示将要写入到表格中的记录数据。
    /// 可以是对象集合、字典集合或其他可以转换为行数据的集合。
    /// </summary>
    /// <remarks>⚠️ 仅在导出表格时有效。</remarks>
    IEnumerable<IDictionary<string, object?>?> Records { get; }

    /// <summary>
    /// 样本行数，系统分析集合中的前 N 个元素来进行列类型推断和自动列宽计算，默认值为 5。
    /// </summary>
    /// <example>
    /// <code>
    /// // 使用前10行数据计算列宽
    /// table.SampleRows = 10;
    /// 
    /// // 禁用自动列宽计算，提高性能
    /// table.WithSampleRows(0);
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// 设置为 0 则不进行自动列宽计算，仅使用列名长度作为初始宽度。
    /// </para>
    /// <para>
    /// 值越大计算越准确，但性能开销也越大。
    /// </para>
    /// <para>
    /// ⚠️ 仅在导出表格时有效。
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">当设置值小于 0 时抛出。</exception>
    int SampleRows { get; set; }

    /// <summary>
    /// 是否启用 Excel 表头自动筛选功能，默认值为 true。
    /// </summary>
    /// <remarks>⚠️ 仅在导出表格时有效，对 CSV 格式无效。</remarks>
    bool AutoFilter { get; set; }

    /// <summary>
    /// Excel 工作表保护密码。
    /// </summary>
    /// <remarks>⚠️ 仅在导出表格时有效，对 CSV 格式无效。</remarks>
    string? Password { get; set; }

    /// <summary>
    /// 预设表格样式，用于特性场景下的样式设置，默认为 Default（未设置）。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 此属性专门用于特性使用场景，运行时会自动映射到对应的 <see cref="PurStyle"/> 实例。
    /// </para>
    /// <para>
    /// ⚠️ 仅在导出表格时有效，对 CSV 格式无效。
    /// </para>
    /// </remarks>
    PresetStyle PresetStyle { get; set; }

    /// <summary>
    /// Excel 工作表样式，默认为 <see cref="PurStyle.Default"/>。
    /// </summary>
    /// <remarks>⚠️ 仅在导出表格时有效，对 CSV 格式无效。</remarks>
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
    /// <exception cref="ArgumentException">当数据起始位置与表头起始位置不匹配时抛出。</exception>
    CellLocator GetDataStart();

    /// <summary>
    /// 获取区域性标识符，默认为与语言无关的区域性标识符（<see cref="CultureInfo.InvariantCulture"/>）。
    /// 用于解析或格式化日期、数字、货币等数据。
    /// </summary>
    /// <returns>表示区域性标识符的 <see cref="CultureInfo"/> 实例，如果未设置则返回与语言无关的区域性标识符。</returns>
    CultureInfo GetCulture();

    /// <summary>
    /// 获取文件编码，默认为 null（读取时自动检测，导出时为UTF-8）。
    /// 用于指定读取或导出文件时的字符编码，目前仅支持 CSV 文件。
    /// </summary>
    /// <returns>表示文件编码的 <see cref="Encoding"/> 实例，如果未设置则返回 null。</returns>
    Encoding? GetFileEncoding();

    /// <summary>
    /// 获取实际的样式实例。
    /// </summary>
    /// <returns>根据优先级规则确定的 <see cref="PurStyle"/> 实例。</returns>
    /// <remarks>
    /// <para>
    /// 优先级规则：<see cref="TableStyle"/>（如果不是默认值）> <see cref="PresetStyle"/>（如果不是默认值）> <see cref="PurStyle.Default"/>
    /// </para>
    /// </remarks>
    PurStyle GetActualStyle();

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
    /// <param name="hasHeader">为 true 表示包含表头行。读取时从表头行之后开始读取数据，导出时会输出表头行。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithHasHeader(bool hasHeader);

    /// <summary>
    /// 链式设置为不包含表头行，等价于 HasHeader = false。
    /// 读取时直接从数据行开始，导出时不输出表头行。
    /// </summary>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithoutHeader();

    /// <summary>
    /// 链式设置表头起始单元格位置。
    /// </summary>
    /// <param name="headerStart">表头起始单元格位置，格式如 "A1"、"B2"。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="headerStart"/> 为 CellLocator.Unknown 时抛出。</exception>
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
    /// <exception cref="InvalidOperationException">当无法从数据表中获取任何列信息时抛出。</exception>
    PurTable WithRecords(DataTable dataTable);

    /// <summary>
    /// 链式设置要导出的数据（泛型集合）。自动分析类型结构并生成列配置。
    /// </summary>
    /// <typeparam name="T">数据类型。</typeparam>
    /// <param name="records">要导出的数据集合，可以是对象集合、字典集合或其他可以转换为行数据的集合。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <example>
    /// <code>
    /// // 对象集合
    /// var users = new List&lt;User&gt; { new User { Name = "张三", Age = 25 } };
    /// table.WithRecords(users);
    /// 
    /// // 匿名对象
    /// var data = new[] { new { 姓名 = "李四", 年龄 = 30 } };
    /// table.WithRecords(data);
    /// 
    /// // 字典集合
    /// var dicts = new List&lt;Dictionary&lt;string, object&gt;&gt;();
    /// table.WithRecords(dicts);
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">当 <paramref name="records"/> 为 null 时抛出。</exception>
    /// <exception cref="NotSupportedException">当 T 为 DataRow 类型时抛出，应使用 DataTable 作为数据源。</exception>
    /// <exception cref="InvalidOperationException">当无法从数据集合中获取任何列信息时抛出。</exception>
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

    /// <summary>
    /// 链式设置预设工作表样式配置。
    /// </summary>
    /// <param name="presetStyle">预设工作表样式配置。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurTable WithPresetStyle(PresetStyle presetStyle);
}