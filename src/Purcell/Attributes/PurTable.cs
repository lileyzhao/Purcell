namespace PurcellLibs;

/// <inheritdoc cref="IPurTable" />
[AttributeUsage(AttributeTargets.Class)]
public class PurTable : Attribute, IPurTable
{
    internal static readonly PurTable Default = new();

    /// <summary>
    /// Excel (.xls) 格式工作表允许的最大数量限制（256）。
    /// </summary>
    private const int XlsWorksheetLimit = 1 << 8;

    #region 构造函数

    /// <summary>
    /// 创建一个新的 <see cref="PurTable"/> 实例。
    /// </summary>
    public PurTable()
    {
    }

    /// <summary>
    /// 通过工作表名称创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    public PurTable(string sheetName)
    {
        SheetName = sheetName;
    }

    /// <summary>
    /// 通过工作表索引创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    public PurTable(int sheetIndex)
    {
        SheetIndex = sheetIndex;
    }

    /// <summary>
    /// 通过列配置集合和工作表索引创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="dynamicColumns">列配置集合。</param>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    public PurTable(List<PurColumn> dynamicColumns, int sheetIndex = 0)
    {
        SheetIndex = sheetIndex;
        WithColumns(dynamicColumns);
    }

    /// <summary>
    /// 通过列配置集合和工作表名称创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="dynamicColumns">列配置集合。</param>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    public PurTable(List<PurColumn> dynamicColumns, string sheetName)
    {
        SheetName = sheetName;
        WithColumns(dynamicColumns);
    }

    /// <summary>
    /// 通过 DataTable 和工作表索引创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="records">要导出的数据表。</param>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    public PurTable(DataTable records, int sheetIndex = 0)
    {
        SheetIndex = sheetIndex;
        WithRecords(records);
    }

    /// <summary>
    /// 通过 DataTable 和工作表名称创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="records">要导出的数据表。</param>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    public PurTable(DataTable records, string sheetName)
    {
        SheetName = sheetName;
        WithRecords(records);
    }

    /// <summary>
    /// 通过动态集合和工作表索引创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="records">要导出的数据集合，可以是对象集合、字典集合或其他可以转换为行数据的集合。</param>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    public PurTable(IEnumerable<dynamic?> records, int sheetIndex = 0)
    {
        SheetIndex = sheetIndex;
        WithRecords(records);
    }

    /// <summary>
    /// 通过动态集合和工作表名称创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="records">要导出的数据集合，可以是对象集合、字典集合或其他可以转换为行数据的集合。</param>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    public PurTable(IEnumerable<dynamic?> records, string sheetName)
    {
        SheetName = sheetName;
        WithRecords(records);
    }

    #endregion 构造函数

    private string _sheetName = string.Empty;

    /// <inheritdoc cref="IPurTable.SheetName"/>
    public string SheetName
    {
        get => _sheetName;
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(SheetName), "工作表名称不能为 null。");
            }

            if (value.Length > 31)
            {
                throw new ArgumentOutOfRangeException(nameof(SheetName), "工作表名称不能超过31个字符。");
            }

            _sheetName = value;
        }
    }

    private int _sheetIndex;

    /// <inheritdoc cref="IPurTable.SheetIndex"/>
    public int SheetIndex
    {
        get => _sheetIndex;
        set
        {
            if (value is < 0 or > XlsWorksheetLimit - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(SheetIndex), $"工作表索引必须在 0-{XlsWorksheetLimit - 1} 范围内。");
            }

            _sheetIndex = value;
        }
    }

    /// <inheritdoc cref="IPurTable.HasHeader"/>
    public bool HasHeader { get; set; } = true;

    private CellLocator _headerStart = CellLocator.A1;

    /// <inheritdoc cref="IPurTable.HeaderStart"/>
    public string HeaderStart
    {
        get => _headerStart.ToString();
        set
        {
            try
            {
                _headerStart = new CellLocator(value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"无效的表头起始位置格式：'{value}'，请使用有效的单元格位置格式（如 'A1'、'B2' 等）。", nameof(HeaderStart), ex);
            }
        }
    }

    private CellLocator _dataStart = CellLocator.Unknown;

    /// <inheritdoc cref="IPurTable.DataStart"/>
    public string DataStart
    {
        get => _dataStart.ToString();
        set
        {
            try
            {
                _dataStart = new CellLocator(value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"无效的数据起始位置格式：'{value}'，请使用有效的单元格位置格式（如 'A2'、'B3' 等）。", nameof(DataStart), ex);
            }
        }
    }

    private List<PurColumn> _columns = [];

    /// <inheritdoc cref="IPurTable.Columns"/>
    public List<PurColumn> Columns
    {
        get => _columns;
        set
        {
            ArgumentNullException.ThrowIfNull(value, "列配置集合不能为 null。");

            lock (_combinedColumnsLock)
            {
                _columns = value;
                _isCombinedColumnsDirty = true;
            }
        }
    }

    private List<PurColumn> RecordColumns { get; } = [];

    /// <summary>
    /// 合并的列配置集合，包含 Columns 和 RecordColumns 的计算结果。
    /// </summary>
    private List<PurColumn> _combinedColumns = [];

    /// <summary>
    /// 标记合并列配置是否需要重新计算。
    /// </summary>
    private bool _isCombinedColumnsDirty = true;

    /// <summary>
    /// CombinedColumns 计算过程的同步锁。
    /// </summary>
    private readonly object _combinedColumnsLock = new();

    internal List<PurColumn> CombinedColumns
    {
        get
        {
            if (!_isCombinedColumnsDirty && _combinedColumns.Count != 0)
            {
                return _combinedColumns;
            }

            lock (_combinedColumnsLock)
            {
                if (!_isCombinedColumnsDirty && _combinedColumns.Count != 0)
                {
                    return _combinedColumns;
                }

                List<PurColumn> combinedColumns = Columns.Select(col => col.Clone()).ToList();
                foreach (PurColumn col in RecordColumns)
                {
                    PurColumn? existingColumn = combinedColumns.FirstOrDefault(cc =>
                        cc.PropertyName.Equals(col.PropertyName, StringComparison.OrdinalIgnoreCase));

                    if (existingColumn == null)
                    {
                        combinedColumns.Add(col.Clone());
                    }
                    else
                    {
                        existingColumn.AddNames(col.Names.ToArray());
                        if (existingColumn.Width <= 0) existingColumn.Width = col.Width;
                        if (existingColumn.Index == -1) existingColumn.Index = col.Index;
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
    }

    /// <inheritdoc cref="IPurTable.MaxReadRows"/>
    public int MaxReadRows { get; set; } = -1;

    /// <inheritdoc cref="IPurTable.MaxWriteRows"/>
    public int MaxWriteRows { get; set; } = -1;

    /// <inheritdoc cref="IPurTable.IgnoreParseError"/>
    public bool IgnoreParseError { get; set; }

    /// <inheritdoc cref="IPurTable.HeaderSpaceMode"/>
    public WhiteSpaceMode HeaderSpaceMode { get; set; } = WhiteSpaceMode.Trim;

    private CultureInfo _culture = CultureInfo.InvariantCulture;

    /// <inheritdoc cref="IPurTable.Culture"/>
    public string Culture
    {
        get => _culture.Name;
        set
        {
            try
            {
                _culture = new CultureInfo(value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"无效的区域性标识符：'{value}'，请使用有效的 CultureInfo 名称（如 'zh-CN'、'en-US' 等）。", nameof(Culture), ex);
            }
        }
    }

    private Encoding? _fileEncoding;

    /// <inheritdoc cref="IPurTable.FileEncoding"/>
    public string? FileEncoding
    {
        get => _fileEncoding?.EncodingName;
        set
        {
            if (value == null)
            {
                _fileEncoding = null;
                return;
            }

            try
            {
                _fileEncoding = Encoding.GetEncoding(value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"无效的编码名称：'{value}'，请使用有效的编码名称（如 'UTF-8'、'GBK' 等）。", nameof(FileEncoding), ex);
            }
        }
    }

    private string _csvDelimiter = ",";

    /// <inheritdoc cref="IPurTable.CsvDelimiter"/>
    public string CsvDelimiter
    {
        get => _csvDelimiter;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(CsvDelimiter), "CSV 分隔符不能为空。");

            if (value.Contains(_csvEscape))
            {
                throw new ArgumentException(
                    $"CsvDelimiter '{value}' 不能包含 CsvEscape '{_csvEscape}' 字符，请确保分隔符与文本限定符不冲突。",
                    nameof(CsvDelimiter));
            }

            _csvDelimiter = value;
        }
    }

    private char _csvEscape = '"';

    /// <inheritdoc cref="IPurTable.CsvEscape"/>
    public char CsvEscape
    {
        get => _csvEscape;
        set
        {
            if (value == '\0')
                throw new ArgumentNullException(nameof(CsvEscape), "CSV 文本限定符不能为 null 字符。");

            if (char.IsControl(value) && value != '\t')
            {
                throw new ArgumentException(
                    $"CsvEscape '{value}' 不能使用控制字符 (ASCII 值: {(int)value})，请使用可见字符作为文本限定符。",
                    nameof(CsvEscape));
            }

            if (!string.IsNullOrEmpty(_csvDelimiter) && _csvDelimiter.Contains(value))
            {
                throw new ArgumentException(
                    $"CsvEscape '{value}' 不能与 CsvDelimiter '{_csvDelimiter}' 中的字符冲突，请确保文本限定符与分隔符不冲突。",
                    nameof(CsvEscape));
            }

            _csvEscape = value;
        }
    }

    /// <inheritdoc cref="IPurTable.Records"/>
    public IEnumerable<IDictionary<string, object?>?> Records { get; private set; } = [];

    private int _sampleRows = 5;

    /// <inheritdoc cref="IPurTable.SampleRows"/>
    public int SampleRows
    {
        get => _sampleRows;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(SampleRows), "样本行数不能小于 0。0 表示不自动计算列宽，仅使用列名长度作为初始宽度。");
            _sampleRows = value;
        }
    }

    /// <inheritdoc cref="IPurTable.AutoFilter"/>
    public bool AutoFilter { get; set; } = true;

    /// <inheritdoc cref="IPurTable.Password"/>
    public string? Password { get; set; }

    /// <inheritdoc cref="IPurTable.TableStyle"/>
    public PurStyle TableStyle { get; set; } = PurStyle.Default;

    /// <summary>
    /// 元素类型，用于泛型集合导出时指定数据类型。对于非泛型集合、匿名集合，此属性为 null。
    /// </summary>
    internal Type? ElementType { get; set; }

    /// <inheritdoc cref="IPurTable.GetHeaderStart"/>
    public CellLocator GetHeaderStart()
    {
        return _headerStart;
    }

    /// <inheritdoc cref="IPurTable.GetDataStart"/>
    public CellLocator GetDataStart()
    {
        if (_headerStart.ColumnIndex >= 0 && _dataStart != CellLocator.Unknown)
        {
            if (_dataStart.ColumnIndex != _headerStart.ColumnIndex)
                throw new ArgumentException(
                    "数据起始单元格列索引必须与表头起始单元格的列索引相同，比如 HeaderStart=B1 则 DataStart 必须为 B 列。",
                    nameof(DataStart));
            if (_dataStart.RowIndex <= _headerStart.RowIndex)
                throw new ArgumentException(
                    "数据起始行必须大于表头起始行，比如 HeaderStart=B2 则 DataStart 必须为 B3 及以上，不能为 B1 或 B2。",
                    nameof(DataStart));
        }

        return _dataStart == CellLocator.Unknown
            ? new CellLocator(_headerStart.RowIndex + 1, _headerStart.ColumnIndex)
            : _dataStart;
    }

    /// <inheritdoc cref="IPurTable.GetCulture"/>
    public CultureInfo GetCulture()
    {
        return _culture;
    }

    /// <inheritdoc cref="IPurTable.GetEncoding"/>
    public Encoding? GetEncoding()
    {
        return _fileEncoding;
    }

    #region 静态工厂方法

    /// <summary>
    /// 创建一个新的 <see cref="PurTable"/> 实例，等价于 <c>new PurTable()</c>。
    /// </summary>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable New()
    {
        return new PurTable();
    }

    /// <summary>
    /// 通过工作表名称创建 <see cref="PurTable"/> 实例，等价于 <c>new PurTable(sheetName)</c>。
    /// </summary>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable From(string sheetName)
    {
        return new PurTable(sheetName);
    }

    /// <summary>
    /// 通过工作表索引创建 <see cref="PurTable"/> 实例，等价于 <c>new PurTable(sheetIndex)</c>。
    /// </summary>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable From(int sheetIndex)
    {
        return new PurTable(sheetIndex);
    }

    /// <summary>
    /// 通过列配置集合和工作表索引创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="dynamicColumns">列配置集合。</param>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable From(List<PurColumn> dynamicColumns, int sheetIndex = 0)
    {
        return new PurTable(dynamicColumns, sheetIndex);
    }

    /// <summary>
    /// 通过列配置集合和工作表名称创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="dynamicColumns">列配置集合。</param>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable From(List<PurColumn> dynamicColumns, string sheetName)
    {
        return new PurTable(dynamicColumns, sheetName);
    }

    /// <summary>
    /// 通过 DataTable 和工作表索引创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="records">要导出的数据表。</param>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable From(DataTable records, int sheetIndex = 0)
    {
        return new PurTable(records, sheetIndex);
    }

    /// <summary>
    /// 通过 DataTable 和工作表名称创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="records">要导出的数据表。</param>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable From(DataTable records, string sheetName)
    {
        return new PurTable(records, sheetName);
    }

    /// <summary>
    /// 通过泛型集合和工作表索引创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <typeparam name="T">数据类型。</typeparam>
    /// <param name="records">要导出的数据集合，可以是对象集合、字典集合或其他可以转换为行数据的集合。</param>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable From<T>(IEnumerable<T?> records, int sheetIndex = 0)
    {
        return new PurTable(sheetIndex).WithRecords(records);
    }

    /// <summary>
    /// 通过泛型集合和工作表名称创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <typeparam name="T">数据类型。</typeparam>
    /// <param name="records">要导出的数据集合，可以是对象集合、字典集合或其他可以转换为行数据的集合。</param>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable From<T>(IEnumerable<T?> records, string sheetName)
    {
        return new PurTable(sheetName).WithRecords(records);
    }

    /// <summary>
    /// 通过工作表名称创建 <see cref="PurTable"/> 实例，等价于 <c>new PurTable(sheetName)</c>。
    /// </summary>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromName(string sheetName)
    {
        return new PurTable(sheetName);
    }

    /// <summary>
    /// 通过工作表索引创建 <see cref="PurTable"/> 实例，等价于 <c>new PurTable(sheetIndex)</c>。
    /// </summary>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromIndex(int sheetIndex)
    {
        return new PurTable(sheetIndex);
    }

    /// <summary>
    /// 通过列配置集合和工作表索引创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="dynamicColumns">列配置集合。</param>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromColumns(List<PurColumn> dynamicColumns, int sheetIndex = 0)
    {
        return new PurTable(dynamicColumns, sheetIndex);
    }

    /// <summary>
    /// 通过列配置集合和工作表名称创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="dynamicColumns">列配置集合。</param>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromColumns(List<PurColumn> dynamicColumns, string sheetName)
    {
        return new PurTable(dynamicColumns, sheetName);
    }

    /// <summary>
    /// 通过 DataTable 和工作表索引创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="records">要导出的数据表。</param>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromRecords(DataTable records, int sheetIndex = 0)
    {
        return new PurTable(records, sheetIndex);
    }

    /// <summary>
    /// 通过 DataTable 和工作表名称创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="records">要导出的数据表。</param>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromRecords(DataTable records, string sheetName)
    {
        return new PurTable(records, sheetName);
    }

    /// <summary>
    /// 通过泛型集合和工作表索引创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <typeparam name="T">数据类型。</typeparam>
    /// <param name="records">要导出的数据集合，可以是对象集合、字典集合或其他可以转换为行数据的集合。</param>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromRecords<T>(IEnumerable<T?> records, int sheetIndex = 0)
    {
        return new PurTable(sheetIndex).WithRecords(records);
    }

    /// <summary>
    /// 通过泛型集合和工作表名称创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <typeparam name="T">数据类型。</typeparam>
    /// <param name="records">要导出的数据集合，可以是对象集合、字典集合或其他可以转换为行数据的集合。</param>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    /// <returns>返回新的 <see cref="PurTable"/> 实例。</returns>
    public static PurTable FromRecords<T>(IEnumerable<T?> records, string sheetName)
    {
        return new PurTable(sheetName).WithRecords(records);
    }

    #endregion 静态工厂方法

    #region Fluent API

    /// <inheritdoc />
    public PurTable WithName(string sheetName)
    {
        SheetName = sheetName;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithIndex(int sheetIndex)
    {
        SheetIndex = sheetIndex;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithHasHeader(bool hasHeader)
    {
        HasHeader = hasHeader;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithoutHeader()
    {
        HasHeader = false;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithHeaderStart(string headerStart)
    {
        HeaderStart = headerStart;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithHeaderStart(CellLocator headerStart)
    {
        if (headerStart == CellLocator.Unknown)
            throw new ArgumentException("HeaderStart 不能为 CellLocator.Unknown", nameof(headerStart));

        _headerStart = headerStart;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithHeaderStart(int rowIndex, int columnIndex)
    {
        _headerStart = CellLocator.Create(rowIndex, columnIndex);
        return this;
    }

    /// <inheritdoc />
    public PurTable WithDataStart(string dataStart)
    {
        DataStart = dataStart;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithDataStart(CellLocator dataStart)
    {
        _dataStart = dataStart;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithDataStart(int rowIndex, int columnIndex)
    {
        _dataStart = CellLocator.Create(rowIndex, columnIndex);
        return this;
    }

    /// <inheritdoc />
    public PurTable WithStart(string headerStart, string dataStart)
    {
        return WithHeaderStart(headerStart).WithDataStart(dataStart);
    }

    /// <inheritdoc />
    public PurTable WithStart(CellLocator headerStart, CellLocator dataStart)
    {
        return WithHeaderStart(headerStart).WithDataStart(dataStart);
    }

    /// <inheritdoc />
    public PurTable WithStart(int headerRow, int dataStartRow)
    {
        return WithHeaderStart(CellLocator.Create(headerRow, 0))
            .WithDataStart(CellLocator.Create(dataStartRow, 0));
    }

    /// <inheritdoc />
    public PurTable WithColumns(List<PurColumn> dynamicColumns)
    {
        Columns = dynamicColumns;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithColumns(PurColumn[] dynamicColumns)
    {
        Columns = dynamicColumns.ToList();
        return this;
    }

    /// <inheritdoc />
    public PurTable AddColumns(List<PurColumn> dynamicColumns)
    {
        lock (_combinedColumnsLock)
        {
            _isCombinedColumnsDirty = true;
            Columns.AddRange(dynamicColumns);
        }

        return this;
    }

    /// <inheritdoc />
    public PurTable AddColumns(PurColumn[] dynamicColumns)
    {
        lock (_combinedColumnsLock)
        {
            _isCombinedColumnsDirty = true;
            Columns.AddRange(dynamicColumns);
        }

        return this;
    }

    /// <inheritdoc />
    public PurTable AddColumn(PurColumn column)
    {
        lock (_combinedColumnsLock)
        {
            _isCombinedColumnsDirty = true;
            Columns.Add(column);
        }

        return this;
    }

    /// <inheritdoc />
    public PurTable WithMaxReadRows(int maxReadRows)
    {
        MaxReadRows = maxReadRows >= 0 ? maxReadRows : -1;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithMaxWriteRows(int maxWriteRows)
    {
        MaxWriteRows = maxWriteRows >= 0 ? maxWriteRows : -1;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithHeaderSpaceMode(WhiteSpaceMode headerSpaceMode)
    {
        HeaderSpaceMode = headerSpaceMode;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithIgnoreParseError(bool ignoreParseError)
    {
        IgnoreParseError = ignoreParseError;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithCulture(string culture)
    {
        Culture = culture;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithCulture(CultureInfo culture)
    {
        Culture = culture.Name;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithFileEncoding(string fileEncoding)
    {
        FileEncoding = fileEncoding;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithFileEncoding(Encoding encoding)
    {
        FileEncoding = encoding.EncodingName;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithCsvDelimiter(string csvDelimiter = ",")
    {
        CsvDelimiter = csvDelimiter;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithCsvEscape(char csvEscape = '"')
    {
        CsvEscape = csvEscape;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithRecords(DataTable dataTable)
    {
        ArgumentNullException.ThrowIfNull(dataTable);

        lock (_combinedColumnsLock)
        {
            _isCombinedColumnsDirty = true;
        }

        // 基于 DataTable 列结构生成列配置信息，包含初始列宽计算
        for (int idx = 0; idx < dataTable.Columns.Count; idx++)
        {
            DataColumn dc = dataTable.Columns[idx];
            PurColumn? existingColumn = RecordColumns.FirstOrDefault(ec => ec.PropertyName == dc.ColumnName);
            if (existingColumn != null) continue;

            existingColumn = PurColumn.FromProperty(dc.ColumnName)
                .WithNames(dc.ColumnName)
                .WithIndex(idx)
                .WithWidth(dc.ColumnName.MeasureText())
                .WithPropertyType(dc.DataType);
            RecordColumns.Add(existingColumn);
        }

        if (RecordColumns.Count == 0) throw new InvalidOperationException("无法从数据集合中获取任何列信息");

        // 基于样本行数据进行列宽自适应计算
        for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
        {
            DataRow rowItem = dataTable.Rows[rowIndex];
            if (rowIndex >= SampleRows) break;
            foreach (PurColumn cc in RecordColumns)
            {
                string? cellValue = ((object?)rowItem[cc.PropertyName])?.ToString();
                if (!string.IsNullOrEmpty(cellValue))
                {
                    cc.Width = Math.Max(cc.Width, cellValue.MeasureText());
                }
            }
        }

        // 将 DataTable 行数据转换为字典集合格式
        string[] columnNames = dataTable.Columns.Cast<DataColumn>().Select(dc => dc.ColumnName).ToArray();
        Records = dataTable.Rows.Cast<DataRow>()
            .Select(row =>
            {
                Dictionary<string, object?> dict = new(columnNames.Length);
                for (int i = 0; i < columnNames.Length; i++)
                {
                    object value = row[i];
                    dict[columnNames[i]] = value == DBNull.Value ? null : value;
                }

                return dict;
            });

        return this;
    }

    /// <inheritdoc />
    public PurTable WithRecords<T>(IEnumerable<T?> records)
    {
        ArgumentNullException.ThrowIfNull(records);

        lock (_combinedColumnsLock)
        {
            _isCombinedColumnsDirty = true;
        }

        // 验证泛型类型 T，不支持 DataRow 类型
        if (typeof(DataRow).IsAssignableFrom(typeof(T)))
        {
            throw new NotSupportedException("不支持直接使用 IEnumerable<DataRow> 类型，请使用 DataTable 作为数据源传入。");
        }

        // 根据数据类型选择处理策略
        if (records is IEnumerable<IDictionary<string, object?>?> dictRecords)
        {
            WithDictRecords(dictRecords);
        }
        else
        {
            WithComplexRecords(records);
        }

        // 基于样本行数据进行列宽自适应计算
        if (SampleRows > 0)
        {
            foreach ((IDictionary<string, object?>? dict, int idx) in Records.Select((v, i) => (v, i)))
            {
                if (idx >= SampleRows) break;
                foreach (PurColumn cc in RecordColumns)
                {
                    string? cellValue = dict?[cc.PropertyName]?.ToString();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        cc.Width = Math.Max(cc.Width, cellValue.MeasureText());
                    }
                }
            }
        }

        return this;
    }

    private void WithDictRecords(IEnumerable<IDictionary<string, object?>?> records)
    {
        Records = records;

        List<IDictionary<string, object?>> dictLimits = Records
            .Where(d => d != null)
            .Select(d => d!)
            .Take(SampleRows < 1 ? 1 : SampleRows)
            .ToList();

        foreach (IDictionary<string, object?> dict in dictLimits)
        {
            int idx = 0;
            foreach ((string key, object? value) in dict)
            {
                Type valueType = value?.GetType() ?? typeof(object);

                PurColumn? existingColumn = RecordColumns.FirstOrDefault(ec => ec.PropertyName == key);
                if (existingColumn != null && existingColumn.PropertyType == typeof(object) && value != null)
                {
                    existingColumn.WithPropertyType(valueType);
                }
                else if (existingColumn == null)
                {
                    existingColumn = PurColumn.FromProperty(key)
                        .WithNames(key)
                        .WithIndex(idx)
                        .WithWidth(key.MeasureText())
                        .WithPropertyType(valueType);
                    RecordColumns.Add(existingColumn);
                }

                idx++;
            }
        }
    }

    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> TypePropsCache = new();
    private const int MaxCacheSize = 100; // 限制缓存大小防止内存泄漏

    /// <summary>
    /// 安全地获取类型的属性信息，包含缓存大小限制
    /// </summary>
    private static PropertyInfo[] GetCachedProperties(Type type)
    {
        if (TypePropsCache.TryGetValue(type, out PropertyInfo[]? cached))
        {
            return cached;
        }

        if (TypePropsCache.Count > MaxCacheSize)
        {
            List<Type> keysToRemove = TypePropsCache.Keys.Take(MaxCacheSize / 2).ToList();
            foreach (Type key in keysToRemove)
            {
                TypePropsCache.TryRemove(key, out _);
            }
        }

        return TypePropsCache.GetOrAdd(type, t => t.GetProperties().Where(p => p.CanRead).ToArray());
    }

    private void WithComplexRecords<T>(IEnumerable<T?> records)
    {
        Type typeOfT = typeof(T);
        PropertyInfo[] properties = [];
        bool isAnonymousType = false;

        // 处理 Object/Dynamic 类型，基于运行时类型获取属性信息
        if (typeOfT == typeof(object))
        {
            T? firstItem = records.FirstOrDefault(r => r != null);
            if (firstItem != null)
            {
                Type itemType = firstItem.GetType();
                isAnonymousType = itemType.IsAnonymousType();
                properties = isAnonymousType
                    ? itemType.GetProperties().Where(p => p.CanRead).ToArray()
                    : GetCachedProperties(itemType);
            }
        }
        // 处理已知类型（匿名类型和泛型），基于编译时类型获取属性信息
        else
        {
            isAnonymousType = typeOfT.IsAnonymousType();
            properties = isAnonymousType
                ? typeOfT.GetProperties().Where(p => p.CanRead).ToArray()
                : GetCachedProperties(typeOfT);
        }

        // 基于属性信息生成列配置，包含初始列宽计算
        for (int idx = 0; idx < properties.Length; idx++)
        {
            PropertyInfo prop = properties[idx];
            PurColumn? existingColumn = RecordColumns.FirstOrDefault(ec => ec.PropertyName == prop.Name);
            if (existingColumn != null) continue;

            string colName = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? prop.Name;
            existingColumn = prop.GetCustomAttribute<PurColumn>() ??
                             PurColumn.From(colName).WithIndex(idx).WithWidth(colName.MeasureText());
            existingColumn.WithProperty(prop);
            RecordColumns.Add(existingColumn);
        }

        if (RecordColumns.Count == 0) throw new InvalidOperationException("无法从数据集合中获取任何列信息");

        Records = records
            .Select(record =>
            {
                if (record == null) return null;

                if (isAnonymousType)
                {
                    return properties.ToDictionary(
                        pk => pk.Name,
                        pv => record.GetType().GetProperty(pv.Name)?.GetValue(record)
                              ?? pv.PropertyType.GetDefaultValue()
                    );
                }

                Dictionary<string, object?> result = new(properties.Length);
                foreach (PropertyInfo prop in properties)
                {
                    try
                    {
                        result[prop.Name] = prop.GetValue(record) ?? prop.PropertyType.GetDefaultValue();
                    }
                    catch
                    {
                        result[prop.Name] = record.GetType().GetProperty(prop.Name)?.GetValue(record)
                                            ?? prop.PropertyType.GetDefaultValue();
                    }
                }

                return result;
            });
    }

    /// <inheritdoc />
    public PurTable WithSampleRows(int sampleRows)
    {
        SampleRows = sampleRows;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithAutoFilter(bool autoFilter)
    {
        AutoFilter = autoFilter;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithPassword(string? password)
    {
        Password = password;
        return this;
    }

    /// <inheritdoc />
    public PurTable WithTableStyle(PurStyle style)
    {
        TableStyle = style;
        return this;
    }

    #endregion Fluent API
}