namespace PurcellLibs;

/// <summary>
/// 表格配置，定义如何在表格文件(Excel/CSV)与对象模型之间进行数据转换和映射。
/// 可以作为特性直接应用于类，也能通过代码动态创建实例。
/// 支持使用流式 API 进行链式配置。
/// </summary>
/// <remarks>
/// 配置优先级机制：当 PurTable 既通过特性（Attribute）方式配置，又在方法调用时直接传入参数配置时，将以方法参数中的配置为准，覆盖特性中定义的配置。
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public class PurTable : Attribute
{
    internal static readonly PurTable Default = new();

    /// <summary>
    /// 无参构造函数，创建一个默认的 <see cref="PurTable"/> 表格配置实例。
    /// </summary>
    public PurTable()
    {
    }

    /// <summary>
    /// 带工作表名称参数的构造函数，创建一个指定工作表名称的 <see cref="PurTable"/> 表格配置实例。
    /// </summary>
    /// <param name="sheetName">Excel 工作表名称，用于定位要操作的工作表。</param>
    public PurTable(string sheetName)
    {
        SheetName = sheetName;
    }

    /// <summary>
    /// 带工作表索引参数的构造函数，创建一个指定工作表索引的 <see cref="PurTable"/> 表格配置实例。
    /// </summary>
    /// <param name="sheetIndex">Excel 工作表索引，从 0 开始计数，用于定位要操作的工作表。</param>
    public PurTable(int sheetIndex)
    {
        SheetIndex = sheetIndex;
    }

    /// <summary>
    /// 工作表名称，用于定位要操作的工作表。
    /// 当同时设置了 <see cref="SheetIndex"/> 时，优先查找工作表名称。
    /// </summary>
    public string SheetName { get; set; } = string.Empty;

    /// <summary>
    /// 工作表索引，范围为 0-255，用于定位要操作的工作表(含隐藏的工作表)。
    /// 该索引的优先级低于 <see cref="SheetName"/> 属性。
    /// </summary>
    public int SheetIndex { get; set; }

    /// <summary>
    /// 表格是否包含表头行，默认为 true。
    /// 当设置为 false 时，将不会尝试从表格中读取表头信息，而是直接从数据行开始读取。
    /// </summary>
    public bool HasHeader { get; set; } = true;

    private CellLocator _headerStart = CellLocator.A1;

    /// <summary>
    /// 表头起始单元格位置，默认为 A1，仅在 <see cref="HasHeader"/> 为 true 时有效。
    /// 表示表头行的起始单元格，格式如 "A1"、"B2" 等。
    /// </summary>
    public string HeaderStart
    {
        get => _headerStart.ToString();
        set => _headerStart = new CellLocator(value);
    }

    private CellLocator _dataStart = CellLocator.Unknown;

    /// <summary>
    /// 数据起始单元格位置，默认为 A2，表示数据行的起始单元格，格式如 "A2"、"B3" 等。
    /// 数据行必须位于表头行之后。
    /// </summary>
    public string DataStart
    {
        get => _dataStart.ToString();
        set => _dataStart = new CellLocator(value);
    }

    private List<PurColumn> _columns = [];

    /// <summary>
    /// 列配置集合，包含所有映射的列配置信息。
    /// 此属性仅在运行时使用，用于存储通过代码或特性定义的列配置。
    /// </summary>
    public List<PurColumn> Columns
    {
        get => _columns;
        set
        {
            _columns = value;
            _isCombinedColumnsDirty = true;
        }
    }

    private List<PurColumn> RecordColumns { get; } = [];

    private List<PurColumn> _combinedColumns = [];
    private bool _isCombinedColumnsDirty = true;

    internal List<PurColumn> CombinedColumns
    {
        get
        {
            if (!_isCombinedColumnsDirty && _combinedColumns.Count != 0)
            {
                return _combinedColumns;
            }

            var combinedColumns = Columns.Select(col => col.Clone()).ToList();
            foreach (var col in RecordColumns)
            {
                var existingColumn = combinedColumns.FirstOrDefault(cc =>
                    cc.PropertyName.Equals(col.PropertyName, StringComparison.OrdinalIgnoreCase));

                if (existingColumn == null)
                {
                    combinedColumns.Add(col.Clone());
                }
                else
                {
                    existingColumn.AddNames(col.Names.ToArray());
                    if (existingColumn.Width <= 0) existingColumn.Width = col.Width;
                    existingColumn.Index ??= col.Index;
                    if (col.Property != null) existingColumn.WithProperty(col.Property);
                    else if (col.PropertyType != null)
                    {
                        existingColumn.WithPropertyType(col.PropertyType);
                    }
                }
            }

            _combinedColumns = combinedColumns;
            _isCombinedColumnsDirty = false;

            return _combinedColumns;
        }
    }

    /// <summary>
    /// 读取表格时，最大读取行数限制，默认为 -1，表示不限制。
    /// </summary>
    public int MaxReadRows { get; set; } = -1;

    /// <summary>
    /// 写入表格时，最大写入行数限制，默认为 -1，表示不限制。
    /// </summary>
    public int MaxWriteRows { get; set; } = -1;

    /// <summary>
    /// 是否忽略值解析错误，默认为 false。
    /// 当设置为 true 时，在解析数据过程中遇到格式错误会尝试使用默认值或跳过，而不是抛出异常中断处理。
    /// </summary>
    public bool IgnoreParseError { get; set; }

    /// <summary>
    /// [读取表格时]表头空白字符处理模式，定义如何处理表头列名中的空白字符，主要用于Dynamic或字典读取时的列名修剪。
    /// 默认为 <see cref="WhiteSpaceMode.Trim"/>，即修剪首尾空格。
    /// </summary>
    /// <remarks>
    /// 处理示例：<br />
    /// - 使用 <see cref="WhiteSpaceMode.Trim"/> 时：" User Name " → "User Name"<br />
    /// - 使用 <see cref="WhiteSpaceMode.RemoveAllSpaces"/> 时：" User Name " → "UserName"
    /// </remarks>
    public WhiteSpaceMode HeaderSpaceMode { get; set; } = WhiteSpaceMode.Trim;

    /// <summary>
    /// 日期时间格式的文化信息，默认为不变的文化信息(<c>CultureInfo.InvariantCulture</c>)。
    /// 用于在解析和格式化日期时间数据时提供文化相关的格式规则。
    /// </summary>
    public string Culture { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码，默认为 null，表示自动推测编码。
    /// 用于指定读取文件时使用的字符编码(目前仅用于读取 CSV 时使用）。
    /// </summary>
    /// <remarks>
    /// 当遇到文件内容显示乱码问题时，可以通过此设置指定正确的编码格式，
    /// 如 "utf-8"、"gb2312" 等。
    /// </remarks>
    public string? Encoding { get; set; }

    /// <summary>
    /// CSV 分隔符，默认为逗号 ","。
    /// 用于指定 CSV 文件中分隔字段的字符或字符串。
    /// </summary>
    public string CsvDelimiter { get; set; } = ",";

    /// <summary>
    /// CSV 文本限定符，默认为双引号 '"'。
    /// 用于指定 CSV 文件中包围字段内容的字符，特别是当字段内容包含分隔符时。
    /// </summary>
    public char CsvEscape { get; set; } = '"';

    /// <summary>
    /// 获取表头起始单元格位置。
    /// </summary>
    /// <returns>表头起始单元格的 <see cref="CellLocator"/> 实例。</returns>
    public CellLocator GetHeaderStart()
    {
        return _headerStart;
    }

    /// <summary>
    /// 获取数据起始单元格位置。
    /// </summary>
    /// <returns>数据起始单元格的 <see cref="CellLocator"/> 实例。</returns>
    public CellLocator GetDataStart()
    {
        return _dataStart == CellLocator.Unknown
            ? new CellLocator(_headerStart.RowIndex + 1, _headerStart.ColumnIndex)
            : _dataStart;
    }

    /// <summary>
    /// 获取日期时间格式的文化信息。
    /// </summary>
    /// <returns>表示文化信息的 <see cref="CultureInfo"/> 实例，如果未设置则返回不变的文化信息(<c>InvariantCulture</c>)。</returns>
    public CultureInfo GetCulture()
    {
        return new CultureInfo(Culture);
    }

    /// <summary>
    /// 获取 CSV 文件读取编码。
    /// </summary>
    /// <returns>表示文件编码的 <see cref="Encoding"/> 实例。</returns>
    public Encoding? GetEncoding()
    {
        return Encoding == null ? null : System.Text.Encoding.GetEncoding(Encoding);
    }

    /// <summary>
    /// 【用于导出】工作表数据集合
    /// </summary>
    public IEnumerable<IDictionary<string, object?>?> Records { get; private set; } = [];

    /// <summary>
    /// 【用于导出】指定用于列配类型推导和自动计算列宽的样本行数。系统将读取集合中的前N行数据来确定最佳列宽。默认值为5。
    /// </summary>
    /// <remarks>
    /// 增加此值可能会提高列配类型推导和自动列宽计算的准确性，但不建议设置太大。设置为 0 则不自动计算列宽。
    /// </remarks>
    public int SampleRows { get; set; } = 5;

    /// <summary>
    /// 【用于导出】是否启用表头自动筛选功能，默认值为 true。对于 Csv 格式无效！
    /// </summary>
    public bool AutoFilter { get; set; } = true;

    /// <summary>
    /// 【用于导出】工作表保护密码。对于 Csv 格式无效！
    /// </summary>
    /// <value>密码字符串，为 null 时表示不启用密码保护</value>
    public string? Password { get; set; }

    /// <summary>
    /// 【用于导出】工作表样式配置。仅对Excel类型导出有效，对于 Csv 格式无效！
    /// </summary>
    public PurStyle TableStyle { get; set; } = PurStyle.Default;

    /// <summary>
    /// 验证配置有效性，检查所有配置参数是否符合要求。
    /// 如果存在无效配置，则抛出异常。
    /// </summary>
    /// <exception cref="InvalidOperationException">当配置参数无效时抛出，异常消息包含详细的错误信息。</exception>
    public PurTable EnsureValid()
    {
        List<string> errors = [];

        // 验证工作表索引范围
        if (SheetIndex is < 0 or > 255)
        {
            errors.Add($"无效的 SheetIndex: {SheetIndex}。工作表索引必须在 0-255 范围内。");
        }

        if (_dataStart != CellLocator.Unknown)
        {
            // 验证数据区域与表头区域起始列是否相同
            if (_headerStart.ColumnIndex >= 0 && _dataStart.ColumnIndex != _headerStart.ColumnIndex)
            {
                errors.Add("数据区域的起始列必须与表头区域的起始列相同，请确保 DataStart 和 HeaderStart 的列索引一致。");
            }

            // 验证数据起始行是否有效
            if (_dataStart.RowIndex < 0)
            {
                errors.Add($"无效的数据起始行: {_dataStart.RowIndex}。数据起始行索引必须大于等于 0。");
            }

            // 验证数据起始行必须大于表头起始行
            if (_dataStart.RowIndex <= _headerStart.RowIndex)
            {
                errors.Add("数据区域的起始行必须大于表头区域的起始行，请确保 DataStartRow 大于 HeaderRow。");
            }
        }

        // 验证Culture文化信息
        if (!string.IsNullOrEmpty(Culture))
        {
            try
            {
                _ = new CultureInfo(Culture);
            }
            catch (CultureNotFoundException)
            {
                errors.Add($"无效的 Culture: {Culture}。找不到指定的文化信息，请提供有效的文化标识符，如 'en-US'、'zh-CN' 等。");
            }
        }

        // 验证编码
        if (Encoding != null)
        {
            try
            {
                _ = System.Text.Encoding.GetEncoding(Encoding);
            }
            catch (ArgumentException)
            {
                errors.Add($"无效的 Encoding: {Encoding}。此编码名称不受支持，请提供有效的编码名称，如 'utf-8'、'gb2312' 等。");
            }
        }

        // 验证CSV分隔符
        if (string.IsNullOrEmpty(CsvDelimiter))
        {
            errors.Add("CsvDelimiter 不能为空，请提供有效的分隔符，如 ','、';' 等。");
        }

        // 验证CSV文本限定符，确保CSV文本限定符不与分隔符冲突
        if (!string.IsNullOrEmpty(CsvDelimiter) && CsvDelimiter.Contains(CsvEscape))
        {
            errors.Add($"CsvEscape 字符 '{CsvEscape}' 不能包含在 CsvDelimiter '{CsvDelimiter}' 中，请确保文本限定符与分隔符不冲突。");
        }

        // 限制特定字符不能作为文本限定符，例如控制字符可能会导致解析问题
        if (char.IsControl(CsvEscape) && CsvEscape != '\t') // 允许制表符作为特例
        {
            errors.Add($"CsvEscape 不能使用控制字符 (ASCII 值: {(int)CsvEscape})，请使用可见字符作为文本限定符。");
        }

        // 如果有无效的配置则抛出异常
        if (errors.Count > 0)
        {
            var errorMessage = errors.Count switch
            {
                1 => $"PurTable 配置无效: {errors[0]}",
                <= 3 => $"PurTable 配置无效: {string.Join("; ", errors)}",
                _ => $"PurTable 配置无效: {string.Join("; ", errors.Take(3))}...等{errors.Count}个错误"
            };
            throw new InvalidOperationException(errorMessage);
        }

        return this;
    }

    #region Fluent 方法链

    /// <summary>
    /// 静态工厂方法，创建一个新的 <see cref="PurTable"/> 表格配置实例，等同于 <c>new PurTable()</c>。
    /// </summary>
    /// <returns>返回一个新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable New()
    {
        return new PurTable();
    }

    /// <summary>
    /// 静态工厂方法，通过指定的工作表名称创建一个新的 <see cref="PurTable"/> 表格配置实例。
    /// </summary>
    /// <param name="name">工作表名称，用于定位要操作的工作表。</param>
    /// <returns>返回一个新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromName(string name)
    {
        return new PurTable(name);
    }

    /// <summary>
    /// 静态工厂方法，通过指定的工作表索引创建一个新的 <see cref="PurTable"/> 表格配置实例。
    /// </summary>
    /// <param name="index">工作表索引，从 0 开始计数，用于定位要操作的工作表。</param>
    /// <returns>返回一个新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromIndex(int index)
    {
        return new PurTable(index);
    }

    /// <summary>
    /// 静态工厂方法，通过指定的列配置集合(可选工作表索引)创建一个新的 <see cref="PurTable"/> 表格配置实例。
    /// </summary>
    /// <param name="columns">列配置集合。</param>
    /// <param name="index">工作表索引，从 0 开始计数，用于定位要操作的工作表。</param>
    /// <returns>返回一个新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromColumns(List<PurColumn> columns, int index = 0)
    {
        return FromIndex(index).WithColumns(columns);
    }

    /// <summary>
    /// 静态工厂方法，通过指定的列配置集合(可选工作表名称)创建一个新的 <see cref="PurTable"/> 表格配置实例。
    /// </summary>
    /// <param name="columns">列配置集合。</param>
    /// <param name="name">工作表名称，用于定位要操作的工作表。</param>
    /// <returns>返回一个新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromColumns(List<PurColumn> columns, string name)
    {
        return FromName(name).WithColumns(columns);
    }

    /// <summary>
    /// 静态工厂方法，通过指定的数据表创建一个新的 <see cref="PurTable"/> 表格配置实例。
    /// </summary>
    /// <param name="records">数据表，包含要导出的数据记录。</param>
    /// <param name="sheetName">工作表名称，作为导出的工作表的名称。为空时使用默认名称。</param>
    /// <returns>返回一个新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromRecords(DataTable records, string sheetName = "")
    {
        return (string.IsNullOrEmpty(sheetName) ? new PurTable(sheetName) : new PurTable())
            .WithRecords(records);
    }

    /// <summary>
    /// 静态工厂方法，通过指定的数据集合创建一个新的 <see cref="PurTable"/> 表格配置实例。
    /// </summary>
    /// <typeparam name="T">数据记录的类型。</typeparam>
    /// <param name="records">数据集合，包含要导出的数据记录。</param>
    /// <param name="sheetName">工作表名称，作为导出的工作表的名称。为空时使用默认名称。</param>
    /// <returns>返回一个新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromRecords<T>(IEnumerable<T?> records, string sheetName = "")
    {
        return (!string.IsNullOrEmpty(sheetName) ? new PurTable(sheetName) : new PurTable())
            .WithRecords(records);
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的工作表名称。
    /// </summary>
    /// <param name="name">工作表名称，用于定位要操作的工作表。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithName(string name)
    {
        SheetName = name;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的工作表索引。
    /// </summary>
    /// <param name="index">工作表索引，从 0 开始计数，用于定位要操作的工作表。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithIndex(int index)
    {
        SheetIndex = index;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例是否包含表头行。
    /// </summary>
    /// <param name="hasHeader">如果设置为 true，则表示表格包含表头行。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithHasHeader(bool hasHeader)
    {
        HasHeader = hasHeader;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例不包含表头行，等同于 <c>HasHeader = false</c>。
    /// </summary>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithoutHeader()
    {
        HasHeader = false;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的表头起始单元格位置。
    /// </summary>
    /// <param name="headerStart">表头起始单元格位置，格式如 "A1"、"B2" 等。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithHeaderStart(string headerStart)
    {
        HeaderStart = headerStart;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的表头起始单元格位置。
    /// </summary>
    /// <param name="headerStart">表头起始单元格位置的 <see cref="CellLocator"/> 实例。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithHeaderStart(CellLocator headerStart)
    {
        _headerStart = headerStart;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的数据起始单元格位置。
    /// </summary>
    /// <param name="dataStart">数据起始单元格位置，格式如 "A2"、"B3" 等。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithDataStart(string dataStart)
    {
        DataStart = dataStart;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的数据起始单元格位置。
    /// </summary>
    /// <param name="dataStart">数据起始单元格位置的 <see cref="CellLocator"/> 实例。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithDataStart(CellLocator dataStart)
    {
        _dataStart = dataStart;
        return this;
    }

    /// <summary>
    /// 链式方法，同时设置当前表格配置实例的表头和数据起始单元格位置。
    /// </summary>
    /// <param name="headerStart">表头起始单元格位置，格式如 "A1"、"B2" 等。</param>
    /// <param name="dataStart">数据起始单元格位置，格式如 "A2"、"B3" 等。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithStart(string headerStart, string dataStart)
    {
        return WithHeaderStart(headerStart).WithDataStart(dataStart);
    }

    /// <summary>
    /// 链式方法，同时设置当前表格配置实例的表头和数据起始单元格位置。
    /// </summary>
    /// <param name="headerStart">表头起始单元格位置的 <see cref="CellLocator"/> 实例。</param>
    /// <param name="dataStart">数据起始单元格位置的 <see cref="CellLocator"/> 实例。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithStart(CellLocator headerStart, CellLocator dataStart)
    {
        return WithHeaderStart(headerStart).WithDataStart(dataStart);
    }

    /// <summary>
    /// 链式方法，同时设置当前表格配置实例的表头行索引和数据起始行索引。
    /// </summary>
    /// <param name="headerRow">表头行索引，从 0 开始计数。</param>
    /// <param name="dataStartRow">数据起始行索引，从 0 开始计数。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithStart(int headerRow, int dataStartRow)
    {
        return WithHeaderStart(CellLocator.Create(headerRow, 0))
            .WithDataStart(CellLocator.Create(dataStartRow, 0));
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的列配置集合。
    /// </summary>
    /// <param name="columns">列配置集合。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithColumns(List<PurColumn> columns)
    {
        _isCombinedColumnsDirty = true; // 标记组合列配置需要重新计算
        Columns = columns;
        return this;
    }

    /// <summary>
    /// 链式方法，向当前表格配置实例的列配置集合中添加一个列配置。
    /// </summary>
    /// <param name="column">要添加的列配置。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable AddColumn(PurColumn column)
    {
        _isCombinedColumnsDirty = true; // 标记组合列配置需要重新计算
        Columns.Add(column);
        return this;
    }

    /// <summary>
    /// 链式方法，向当前表格配置实例的列配置集合中添加多个列配置。
    /// </summary>
    /// <param name="columns">要添加的列配置集合。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable AddColumns(List<PurColumn> columns)
    {
        _isCombinedColumnsDirty = true; // 标记组合列配置需要重新计算
        Columns.AddRange(columns);
        return this;
    }

    /// <summary>
    /// 链式方法，向当前表格配置实例的列配置集合中添加多个列配置。
    /// </summary>
    /// <param name="columns">要添加的列配置集合。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable AddColumns(params PurColumn[] columns)
    {
        _isCombinedColumnsDirty = true; // 标记组合列配置需要重新计算
        Columns.AddRange(columns);
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的最大读取行数限制。
    /// </summary>
    /// <param name="maxReadRows">最大读取行数，默认为 -1 表示不限制。设置为非负数时将限制读取的数据行数量。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithMaxReadRows(int maxReadRows)
    {
        MaxReadRows = maxReadRows >= 0 ? maxReadRows : -1;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的最大写入行数限制。
    /// </summary>
    /// <param name="maxWriteRows">最大写入行数，默认为 -1 表示不限制。设置为非负数时将限制写入的数据行数量。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithMaxWriteRows(int maxWriteRows)
    {
        MaxWriteRows = maxWriteRows >= 0 ? maxWriteRows : -1;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的表头空白字符处理模式。
    /// </summary>
    /// <param name="headerSpaceMode">表头空白字符处理模式，定义如何处理表头列名中的空白字符，主要用于Dynamic或字典读取时的列名修剪。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    /// <remarks>
    /// 处理示例：<br />
    /// - 使用 <see cref="WhiteSpaceMode.Trim"/> 时：" User Name " → "User Name"<br />
    /// - 使用 <see cref="WhiteSpaceMode.RemoveAllSpaces"/> 时：" User Name " → "UserName"
    /// </remarks>
    public PurTable WithHeaderSpaceMode(WhiteSpaceMode headerSpaceMode)
    {
        HeaderSpaceMode = headerSpaceMode;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例是否忽略解析错误。
    /// </summary>
    /// <param name="ignoreParseError">如果设置为 true，则在解析数据过程中遇到格式错误会尝试使用默认值或跳过。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithIgnoreParseError(bool ignoreParseError)
    {
        IgnoreParseError = ignoreParseError;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的空白字符处理模式。
    /// </summary>
    /// <param name="whiteSpaceMode">空白字符处理模式。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithWhiteSpaceMode(WhiteSpaceMode whiteSpaceMode)
    {
        HeaderSpaceMode = whiteSpaceMode;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的日期时间格式文化信息。
    /// </summary>
    /// <param name="culture">日期时间格式文化信息，如 "en-US"、"zh-CN" 等。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithCulture(string culture)
    {
        Culture = culture;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的日期时间格式文化信息。
    /// </summary>
    /// <param name="culture">日期时间格式文化信息的 <see cref="CultureInfo"/> 实例。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithCulture(CultureInfo culture)
    {
        Culture = culture.Name;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的文件编码。
    /// </summary>
    /// <param name="encoding">文件编码名称，如 "utf-8"、"gb2312" 等。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithEncoding(string encoding)
    {
        Encoding = encoding;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的文件编码。
    /// </summary>
    /// <param name="encoding">文件编码的 <see cref="Encoding"/> 实例。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithEncoding(Encoding encoding)
    {
        Encoding = encoding.EncodingName;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的 CSV 文件读取分隔符。
    /// </summary>
    /// <param name="csvDelimiter">CSV 文件读取分隔符，如 ","、";" 等；默认为 ","。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithCsvDelimiter(string csvDelimiter = ",")
    {
        CsvDelimiter = csvDelimiter;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的 CSV 文件读取文本限定符。
    /// </summary>
    /// <param name="csvEscape">CSV 文件读取文本限定符，如 '"'、"'" 等；默认为 '"'。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithCsvEscape(char csvEscape = '"')
    {
        CsvEscape = csvEscape;
        return this;
    }

    /// <summary>
    /// 链式方法，为当前表格配置实例设置要导出的数据记录。
    /// 此方法会自动分析数据表结构并生成相应的列配置。
    /// </summary>
    /// <param name="dataTable">数据表，包含要导出的数据记录。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dataTable"/> 为 null 时抛出。</exception>
    /// <exception cref="InvalidOperationException">当无法从数据表中获取任何列信息时抛出。</exception>
    public PurTable WithRecords(DataTable dataTable)
    {
        ArgumentNullException.ThrowIfNull(dataTable);

        _isCombinedColumnsDirty = true; // 标记组合列配置需要重新计算

        // 计算列信息(含初始自动列宽)
        for (int idx = 0; idx < dataTable.Columns.Count; idx++)
        {
            var dc = dataTable.Columns[idx];
            PurColumn? existingColumn = RecordColumns.FirstOrDefault(ec => ec.PropertyName == dc.ColumnName);
            if (existingColumn == null)
            {
                existingColumn = PurColumn.FromProperty(dc.ColumnName)
                    .WithNames([dc.ColumnName])
                    .WithIndex(idx)
                    .WithWidth(dc.ColumnName.MeasureText())
                    .WithPropertyType(dc.DataType);
                RecordColumns.Add(existingColumn);
            }
        }

        if (RecordColumns.Count == 0) throw new InvalidOperationException("无法从数据集合中获取任何列信息");

        // 通过前 N 行数据自适应列宽， N = SampleRows
        for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
        {
            var rowItem = dataTable.Rows[rowIndex];
            if (rowIndex >= SampleRows) break;
            foreach (PurColumn col in RecordColumns)
            {
                string cellValue = ((object?)rowItem[col.PropertyName])?.ToString() ?? string.Empty;
                col.Width = Math.Max(col.Width, cellValue.MeasureText());
            }
        }

        // 转为 IDictionary<string, object?> 
        Records = dataTable.Rows.Cast<DataRow>()
            .Select(row
                => row.Table.Columns.Cast<DataColumn>().ToDictionary(
                    col => col.ColumnName,
                    col => row[col.ColumnName] == DBNull.Value ? null : row[col.ColumnName]
                ) as IDictionary<string, object?>
            );

        return this;
    }

    /// <summary>
    /// 链式方法，为当前表格配置实例设置要导出的数据记录。
    /// 此方法会自动分析数据对象的类型结构并生成相应的列配置，支持匿名类型、POCO类型和字典类型。
    /// </summary>
    /// <typeparam name="T">数据记录的类型，可以是匿名类型、POCO类型或字典类型。</typeparam>
    /// <param name="records">数据集合，包含要导出的数据记录。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="records"/> 为 null 时抛出。</exception>
    /// <exception cref="NotSupportedException">当 T 为 DataRow 类型时抛出，应使用 DataTable 作为数据源。</exception>
    /// <exception cref="InvalidOperationException">当无法从数据集合中获取任何列信息时抛出。</exception>
    /// <remarks>
    /// 此方法支持以下数据类型：
    /// <list type="bullet">
    /// <item><description>匿名类型：自动读取所有可读属性</description></item>
    /// <item><description>POCO类型：自动读取所有可读属性，支持 DisplayName 特性和 PurColumn 特性</description></item>
    /// <item><description>IDictionary&lt;string, object?&gt;：直接使用字典的键作为列名</description></item>
    /// <item><description>Object类型：运行时分析实际类型的属性</description></item>
    /// </list>
    /// </remarks>
    public PurTable WithRecords<T>(IEnumerable<T?> records)
    {
        ArgumentNullException.ThrowIfNull(records);

        _isCombinedColumnsDirty = true; // 标记组合列配置需要重新计算

        // 1. 如果T是DataRow类型，则抛出不支持的警告异常
        if (typeof(DataRow).IsAssignableFrom(typeof(T)))
        {
            throw new NotSupportedException("不支持直接使用 IEnumerable<DataRow> 类型，请使用 DataTable 作为数据源传入。");
        }

        // 2. IDictionary<string, object?> 处理路径
        if (records is IEnumerable<IDictionary<string, object?>?> dictRecords)
        {
            WithDictRecords(dictRecords);
        }
        else
        {
            WithComplexRecords(records);
        }

        // 通过前 N 行数据自适应列宽， N = SampleRows
        if (SampleRows > 0)
        {
            foreach ((var rowItem, int rowIndex) in Records.Select((v, i) => (v, i)))
            {
                if (rowIndex >= SampleRows) break;
                foreach (PurColumn col in RecordColumns)
                {
                    string cellValue = rowItem?[col.PropertyName]?.ToString() ?? string.Empty;
                    col.Width = Math.Max(col.Width, cellValue.MeasureText());
                }
            }
        }

        return this;
    }

    private void WithDictRecords(IEnumerable<IDictionary<string, object?>?> records)
    {
        Records = records;

        List<IDictionary<string, object?>?> dictLimits = Records.Take(SampleRows < 1 ? 1 : SampleRows).ToList();

        foreach (var dict in dictLimits)
        {
            if (dict == null) continue;

            for (int idx = 0; idx < dict.Keys.Count; idx++)
            {
                string key = dict.Keys.ElementAt(idx);
                object? value = dict[key];
                Type valueType = dict[key]?.GetType() ?? typeof(object);

                PurColumn? existingColumn = RecordColumns.FirstOrDefault(ec => ec.PropertyName == key);
                if (existingColumn != null && existingColumn.PropertyType == typeof(object) && value != null)
                {
                    existingColumn.WithPropertyType(value.GetType());
                }
                else if (existingColumn == null)
                {
                    existingColumn = PurColumn.FromProperty(key)
                        .WithNames([key])
                        .WithIndex(idx)
                        .WithWidth(key.MeasureText())
                        .WithPropertyType(valueType);
                    RecordColumns.Add(existingColumn);
                }
            }
        }
    }

    private void WithComplexRecords<T>(IEnumerable<T?> records)
    {
        Type typeOfT = typeof(T);

        PropertyInfo[] properties = [];

        // Object/Dynamic 处理路径
        if (typeOfT == typeof(object))
        {
            T? firstObj = records.FirstOrDefault(r => r != null);
            if (firstObj != null)
            {
                properties = firstObj.GetType().GetProperties().Where(p => p.CanRead).ToArray();
            }

            Console.WriteLine("处理Object/Dynamic类型记录");
        }
        // 匿名类型和泛型 处理路径
        else
        {
            properties = typeOfT.GetProperties()
                .Where(p => p.CanRead)
                .ToArray();
            Console.WriteLine("处理匿名类型和泛型记录");
        }

        // 计算列信息(含初始自动列宽)
        for (int idx = 0; idx < properties.Length; idx++)
        {
            var prop = properties[idx];
            PurColumn? existingColumn = RecordColumns.FirstOrDefault(ec => ec.PropertyName == prop.Name);
            if (existingColumn == null)
            {
                string colName = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? prop.Name;
                existingColumn = prop.GetCustomAttribute<PurColumn>() ??
                                 PurColumn.FromNames(colName).WithIndex(idx).WithWidth(colName.MeasureText());
                existingColumn.WithProperty(prop);
                RecordColumns.Add(existingColumn);
            }
        }

        if (RecordColumns.Count == 0) throw new InvalidOperationException("无法从数据集合中获取任何列信息");

        Records = records
            .Select(record =>
            {
                if (record == null) return null;

                Dictionary<string, object?> result = new(properties.Length);
                var recordType = record.GetType();
                foreach (PropertyInfo prop in properties)
                {
                    if (prop.DeclaringType?.IsAssignableFrom(recordType) ?? false)
                        result[prop.Name] = prop.GetValue(record) ?? prop.PropertyType.GetDefaultValue();
                    else
                        result[prop.Name] = record.GetType().GetProperty(prop.Name)?.GetValue(record)
                                            ?? prop.PropertyType.GetDefaultValue();
                }

                return result;
            });
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例的用于列配类型推导和自动列宽的样本行数。
    /// </summary>
    /// <param name="sampleRows">样本行数，系统将读取集合中的前N个项目来进行列配类型推导和自动列宽。设置为 0 则不自动计算列宽。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="sampleRows"/> 小于 0 时抛出。</exception>
    public PurTable WithSampleSize(int sampleRows)
    {
        if (sampleRows < 0) throw new ArgumentOutOfRangeException(nameof(sampleRows), "样本行数不能设置为小于0");
        SampleRows = sampleRows;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格配置实例是否启用表头自动筛选功能。
    /// </summary>
    /// <param name="autoFilter">如果设置为 true，则启用表头自动筛选功能。对于 CSV 格式无效。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithAutoFilter(bool autoFilter)
    {
        AutoFilter = autoFilter;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格的工作表保护密码。
    /// </summary>
    /// <param name="password">工作表保护密码。为 null 时表示不启用密码保护。对于 CSV 格式无效。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithPassword(string? password)
    {
        Password = password;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前表格的工作表样式配置。
    /// </summary>
    /// <param name="style">工作表样式配置，定义表格的外观和格式。</param>
    /// <returns>返回当前 <see cref="PurTable"/> 实例，以便进行链式调用。</returns>
    public PurTable WithTableStyle(PurStyle style)
    {
        TableStyle = style;
        return this;
    }

    #endregion Fluent 方法链
}