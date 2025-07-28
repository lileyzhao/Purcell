namespace PurcellLibs;

/// <inheritdoc cref="IPurTable" />
[AttributeUsage(AttributeTargets.Class)]
public class PurTable : Attribute, IPurTable
{
    /// <summary>
    /// 获取默认的 <see cref="PurTable"/> 实例，用于内部默认配置。
    /// </summary>
    internal static readonly PurTable Default = new();

    /// <summary>
    /// Excel (.xls) 格式工作表允许的最大数量限制，值为 256。
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
    /// <exception cref="ArgumentNullException">当 <paramref name="sheetName"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="sheetName"/> 长度超过31个字符时抛出。</exception>
    public PurTable(string sheetName)
    {
        SheetName = sheetName;
    }

    /// <summary>
    /// 通过工作表索引创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="sheetIndex"/> 不在 0-255 范围内时抛出。</exception>
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
    /// 通过数据集合和工作表索引创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="records">要导出的数据集合，可以是对象集合、字典集合或其他可以转换为行数据的集合。</param>
    /// <param name="sheetIndex">工作表索引（从 0 开始），用于定位要操作的工作表（索引序列含隐藏的工作表）。</param>
    public PurTable(IEnumerable<object?> records, int sheetIndex = 0)
    {
        SheetIndex = sheetIndex;
        WithRecords(records);
    }

    /// <summary>
    /// 通过数据集合和工作表名称创建 <see cref="PurTable"/> 实例。
    /// </summary>
    /// <param name="records">要导出的数据集合，可以是对象集合、字典集合或其他可以转换为行数据的集合。</param>
    /// <param name="sheetName">工作表名称，用于定位要操作的工作表，导出时也用作导出的工作表名。</param>
    public PurTable(IEnumerable<object?> records, string sheetName)
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
                throw new ArgumentException($"无效的表头起始位置格式：'{value}'，请使用有效的单元格位置格式（如 'A1'、'B2' 等）。", nameof(HeaderStart),
                    ex);
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
                _dataStart = value == string.Empty ? CellLocator.Unknown : new CellLocator(value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"无效的数据起始位置格式：'{value}'，请使用有效的单元格位置格式（如 'A2'、'B3' 等）。", nameof(DataStart),
                    ex);
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
            value.ThrowIfArgumentNull("列配置集合不能为 null。", nameof(Columns));
            lock (_combinedColumnsLock)
            {
                _columns = value;
                _isCombinedColumnsDirty = true;
            }
        }
    }

    private List<PurColumn> RecordColumns { get; } = [];

    private List<PurColumn> _combinedColumns = [];
    private bool _isCombinedColumnsDirty = true;

    /// <summary>
    /// <see cref="CombinedColumns"/> 计算过程的线程同步锁对象。
    /// </summary>
    private readonly object _combinedColumnsLock = new();

    /// <summary>
    /// 获取最终的合并列配置集合，按需计算并缓存结果。
    /// 优先级：手动配置的 <see cref="Columns"/> > 自动推断的 <see cref="RecordColumns"/>。
    /// </summary>
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

    /// <inheritdoc cref="IPurTable.HeaderSpaceMode"/>
    public WhiteSpaceMode HeaderSpaceMode { get; set; } = WhiteSpaceMode.Trim;

    /// <inheritdoc cref="IPurTable.IgnoreParseError"/>
    public bool IgnoreParseError { get; set; }

    private CultureInfo _culture = CultureInfo.InvariantCulture;

    /// <inheritdoc cref="IPurTable.Culture"/>
    public string Culture
    {
        get => _culture.Name;
        set
        {
            try
            {
                _culture = CultureInfo.GetCultureInfo(value);
                if (_culture.IsNeutralCulture || _culture.CultureTypes.HasFlag(CultureTypes.NeutralCultures))
                {
                    // 检查是否在已知区域性列表中
                    CultureInfo[] knownCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                    if (!knownCultures.Any(c =>
                            string.Equals(c.Name, _culture.Name, StringComparison.OrdinalIgnoreCase)))
                        throw new ArgumentException(
                            $"无效的区域性标识符：'{value}'，请使用具体的区域性标识符（如 'zh-CN'、'en-US' 等）。", nameof(Culture));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"无效的区域性标识符：'{value}'，请使用有效的区域性标识符（如 'zh-CN'、'en-US' 等）。", nameof(Culture),
                    ex);
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
                try
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    _fileEncoding = Encoding.GetEncoding(value);
                }
                catch (Exception)
                {
                    throw new ArgumentException($"无效的编码名称：'{value}'，请使用有效的编码名称（如 'UTF-8'、'GBK' 等）。",
                        nameof(FileEncoding), ex);
                }
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

    /// <inheritdoc cref="IPurTable.PresetStyle"/>
    public PresetStyle PresetStyle { get; set; } = PresetStyle.Default;

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

    /// <inheritdoc cref="IPurTable.GetFileEncoding"/>
    public Encoding? GetFileEncoding()
    {
        return _fileEncoding;
    }

    /// <inheritdoc cref="IPurTable.GetActualStyle"/>
    public PurStyle GetActualStyle()
    {
        // 1. 如果 TableStyle 不是默认值，优先使用 TableStyle
        if (!ReferenceEquals(TableStyle, PurStyle.Default))
            return TableStyle;

        // 2. 如果 PresetStyle 不是默认值，使用 PresetStyle
        if (PresetStyle != PresetStyle.Default)
        {
            return PresetStyle switch
            {
                PresetStyle.Default => PurStyle.Default,
                PresetStyle.BrightFresh => PurStyle.BrightFresh,
                PresetStyle.ElegantMonochrome => PurStyle.ElegantMonochrome,
                PresetStyle.EarthTones => PurStyle.EarthTones,
                PresetStyle.WarmTones => PurStyle.WarmTones,
                PresetStyle.OceanBlue => PurStyle.OceanBlue,
                PresetStyle.VintageNostalgia => PurStyle.VintageNostalgia,
                PresetStyle.MinimalistBw => PurStyle.MinimalistBw,
                PresetStyle.VibrantEnergy => PurStyle.VibrantEnergy,
                PresetStyle.RetroChic => PurStyle.RetroChic,
                PresetStyle.CozyAutumn => PurStyle.CozyAutumn,
                PresetStyle.SereneNature => PurStyle.SereneNature,
                PresetStyle.MidnightMagic => PurStyle.MidnightMagic,
                PresetStyle.SunnyDay => PurStyle.SunnyDay,
                _ => PurStyle.Default
            };
        }

        // 3. 都是默认值，返回默认样式
        return PurStyle.Default;
    }

    #region 静态工厂方法

    /// <inheritdoc cref="PurTable()"/>
    public static PurTable New()
    {
        return new PurTable();
    }

    /// <inheritdoc cref="PurTable(string)"/>
    public static PurTable From(string sheetName)
    {
        return new PurTable(sheetName);
    }

    /// <inheritdoc cref="PurTable(int)"/>
    public static PurTable From(int sheetIndex)
    {
        return new PurTable(sheetIndex);
    }

    /// <inheritdoc cref="PurTable(System.Collections.Generic.List{PurcellLibs.PurColumn},int)"/>
    public static PurTable From(List<PurColumn> dynamicColumns, int sheetIndex = 0)
    {
        return new PurTable(dynamicColumns, sheetIndex);
    }

    /// <inheritdoc cref="PurTable(System.Collections.Generic.List{PurcellLibs.PurColumn},string)"/>
    public static PurTable From(List<PurColumn> dynamicColumns, string sheetName)
    {
        return new PurTable(dynamicColumns, sheetName);
    }

    /// <inheritdoc cref="PurTable(System.Data.DataTable,int)"/>
    public static PurTable From(DataTable records, int sheetIndex = 0)
    {
        return new PurTable(records, sheetIndex);
    }

    /// <inheritdoc cref="PurTable(System.Data.DataTable,string)"/>
    public static PurTable From(DataTable records, string sheetName)
    {
        return new PurTable(records, sheetName);
    }

    /// <inheritdoc cref="PurTable(System.Collections.Generic.IEnumerable{object?},int)"/>
    public static PurTable From<T>(IEnumerable<T?> records, int sheetIndex = 0)
    {
        return new PurTable(sheetIndex).WithRecords(records);
    }

    /// <inheritdoc cref="PurTable(System.Collections.Generic.IEnumerable{object?},string)"/>
    public static PurTable From<T>(IEnumerable<T?> records, string sheetName)
    {
        return new PurTable(sheetName).WithRecords(records);
    }

    /// <inheritdoc cref="PurTable(string)"/>
    public static PurTable FromName(string sheetName)
    {
        return new PurTable(sheetName);
    }

    /// <inheritdoc cref="PurTable(int)"/>
    public static PurTable FromIndex(int sheetIndex)
    {
        return new PurTable(sheetIndex);
    }

    /// <inheritdoc cref="PurTable(System.Collections.Generic.List{PurcellLibs.PurColumn},int)"/>
    public static PurTable FromColumns(List<PurColumn> dynamicColumns, int sheetIndex = 0)
    {
        return new PurTable(dynamicColumns, sheetIndex);
    }

    /// <inheritdoc cref="PurTable(System.Collections.Generic.List{PurcellLibs.PurColumn},string)"/>
    public static PurTable FromColumns(List<PurColumn> dynamicColumns, string sheetName)
    {
        return new PurTable(dynamicColumns, sheetName);
    }

    /// <inheritdoc cref="PurTable(System.Data.DataTable,int)"/>
    public static PurTable FromRecords(DataTable records, int sheetIndex = 0)
    {
        return new PurTable(records, sheetIndex);
    }

    /// <inheritdoc cref="PurTable(System.Data.DataTable,string)"/>
    public static PurTable FromRecords(DataTable records, string sheetName)
    {
        return new PurTable(records, sheetName);
    }

    /// <inheritdoc cref="PurTable(System.Collections.Generic.IEnumerable{object?},int)"/>
    public static PurTable FromRecords<T>(IEnumerable<T?> records, int sheetIndex = 0)
    {
        return new PurTable(sheetIndex).WithRecords(records);
    }

    /// <inheritdoc cref="PurTable(System.Collections.Generic.IEnumerable{object?},string)"/>
    public static PurTable FromRecords<T>(IEnumerable<T?> records, string sheetName)
    {
        return new PurTable(sheetName).WithRecords(records);
    }

    #endregion 静态工厂方法

    #region Fluent API

    /// <inheritdoc cref="IPurTable.WithName(string)" />
    public PurTable WithName(string sheetName)
    {
        SheetName = sheetName;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithIndex(int)" />
    public PurTable WithIndex(int sheetIndex)
    {
        SheetIndex = sheetIndex;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithHasHeader(bool)" />
    public PurTable WithHasHeader(bool hasHeader)
    {
        HasHeader = hasHeader;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithoutHeader" />
    public PurTable WithoutHeader()
    {
        HasHeader = false;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithHeaderStart(string)" />
    public PurTable WithHeaderStart(string headerStart)
    {
        HeaderStart = headerStart;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithHeaderStart(CellLocator)" />
    public PurTable WithHeaderStart(CellLocator headerStart)
    {
        if (headerStart == CellLocator.Unknown)
            throw new ArgumentException("HeaderStart 不能为 CellLocator.Unknown", nameof(headerStart));

        _headerStart = headerStart;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithHeaderStart(int, int)" />
    public PurTable WithHeaderStart(int rowIndex, int columnIndex)
    {
        _headerStart = CellLocator.Create(rowIndex, columnIndex);
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithDataStart(string)" />
    public PurTable WithDataStart(string dataStart)
    {
        DataStart = dataStart;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithDataStart(CellLocator)" />
    public PurTable WithDataStart(CellLocator dataStart)
    {
        _dataStart = dataStart;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithDataStart(int, int)" />
    public PurTable WithDataStart(int rowIndex, int columnIndex)
    {
        _dataStart = CellLocator.Create(rowIndex, columnIndex);
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithStart(string, string)" />
    public PurTable WithStart(string headerStart, string dataStart)
    {
        return WithHeaderStart(headerStart).WithDataStart(dataStart);
    }

    /// <inheritdoc cref="IPurTable.WithStart(CellLocator, CellLocator)" />
    public PurTable WithStart(CellLocator headerStart, CellLocator dataStart)
    {
        return WithHeaderStart(headerStart).WithDataStart(dataStart);
    }

    /// <inheritdoc cref="IPurTable.WithStart(int, int)" />
    public PurTable WithStart(int headerRow, int dataStartRow)
    {
        return WithHeaderStart(CellLocator.Create(headerRow, 0))
            .WithDataStart(CellLocator.Create(dataStartRow, 0));
    }

    /// <inheritdoc cref="IPurTable.WithColumns(List{PurColumn})" />
    public PurTable WithColumns(List<PurColumn> dynamicColumns)
    {
        Columns = dynamicColumns;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithColumns(PurColumn[])" />
    public PurTable WithColumns(PurColumn[] dynamicColumns)
    {
        Columns = dynamicColumns.ToList();
        return this;
    }

    /// <inheritdoc cref="IPurTable.AddColumns(List{PurColumn})" />
    public PurTable AddColumns(List<PurColumn> dynamicColumns)
    {
        lock (_combinedColumnsLock)
        {
            _isCombinedColumnsDirty = true;
            Columns.AddRange(dynamicColumns);
        }

        return this;
    }

    /// <inheritdoc cref="IPurTable.AddColumns(PurColumn[])" />
    public PurTable AddColumns(PurColumn[] dynamicColumns)
    {
        lock (_combinedColumnsLock)
        {
            _isCombinedColumnsDirty = true;
            Columns.AddRange(dynamicColumns);
        }

        return this;
    }

    /// <inheritdoc cref="IPurTable.AddColumn(PurColumn)" />
    public PurTable AddColumn(PurColumn column)
    {
        lock (_combinedColumnsLock)
        {
            _isCombinedColumnsDirty = true;
            Columns.Add(column);
        }

        return this;
    }

    /// <inheritdoc cref="IPurTable.WithMaxReadRows(int)" />
    public PurTable WithMaxReadRows(int maxReadRows)
    {
        MaxReadRows = maxReadRows >= 0 ? maxReadRows : -1;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithMaxWriteRows(int)" />
    public PurTable WithMaxWriteRows(int maxWriteRows)
    {
        MaxWriteRows = maxWriteRows >= 0 ? maxWriteRows : -1;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithHeaderSpaceMode(WhiteSpaceMode)" />
    public PurTable WithHeaderSpaceMode(WhiteSpaceMode headerSpaceMode)
    {
        HeaderSpaceMode = headerSpaceMode;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithIgnoreParseError(bool)" />
    public PurTable WithIgnoreParseError(bool ignoreParseError)
    {
        IgnoreParseError = ignoreParseError;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithCulture(string)" />
    public PurTable WithCulture(string culture)
    {
        Culture = culture;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithCulture(CultureInfo)" />
    public PurTable WithCulture(CultureInfo culture)
    {
        Culture = culture.Name;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithFileEncoding(string)" />
    public PurTable WithFileEncoding(string fileEncoding)
    {
        FileEncoding = fileEncoding;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithFileEncoding(Encoding)" />
    public PurTable WithFileEncoding(Encoding encoding)
    {
        FileEncoding = encoding.EncodingName;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithCsvDelimiter(string)" />
    public PurTable WithCsvDelimiter(string csvDelimiter = ",")
    {
        CsvDelimiter = csvDelimiter;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithCsvEscape(char)" />
    public PurTable WithCsvEscape(char csvEscape = '"')
    {
        CsvEscape = csvEscape;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithRecords(DataTable)" />
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
            if (rowIndex >= SampleRows) break;
            DataRow rowItem = dataTable.Rows[rowIndex];
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
                if (row.ItemArray.All(r => r == DBNull.Value || r == null)) return null;

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

    /// <inheritdoc cref="IPurTable.WithRecords{T}(IEnumerable{T})" />
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

    /// <summary>
    /// 处理字典类型的数据集合，从字典键中提取列信息。
    /// </summary>
    /// <param name="records">字典类型的数据集合。</param>
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

    // 类型属性信息的静态缓存，用于提升反射性能。
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> TypePropsCache = new();

    /// <summary>
    /// 缓存大小的最大限制，防止内存泄漏。
    /// </summary>
    private const int MaxCacheSize = 100;

    /// <summary>
    /// 安全地获取类型的属性信息，包含缓存大小限制。
    /// 使用 LRU 策略清理缓存，防止内存泄漏。
    /// </summary>
    /// <param name="type">要获取属性信息的类型。</param>
    /// <returns>该类型的所有可读属性数组。</returns>
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

        // 使用工厂方法获取属性信息，只返回可读属性
        return TypePropsCache.GetOrAdd(type, t => t.GetProperties().Where(p => p.CanRead).ToArray());
    }

    /// <summary>
    /// 处理复杂类型的数据集合，通过反射获取属性信息。
    /// 支持匿名类型、一般类型和 object 类型。
    /// </summary>
    /// <typeparam name="T">数据类型。</typeparam>
    /// <param name="records">数据集合。</param>
    private void WithComplexRecords<T>(IEnumerable<T?> records)
    {
        Type typeOfT = typeof(T);
        PropertyInfo[] properties = [];
        bool isAnonymousType = false;

        // 处理 Object/Dynamic 类型，需要基于运行时类型获取属性信息
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

        // 将复杂对象转换为字典格式，便于统一处理
        Records = records
            .Select(record =>
            {
                if (record == null) return null;

                // 匿名类型需要通过运行时类型获取属性值
                if (isAnonymousType)
                {
                    return properties.ToDictionary(
                        pk => pk.Name,
                        pv => record.GetType().GetProperty(pv.Name)?.GetValue(record)
                              ?? pv.PropertyType.GetDefaultValue()
                    );
                }

                // 一般类型直接使用编译时属性信息，失败时降级为运行时获取
                return properties.ToDictionary(
                    pk => pk.Name,
                    pv =>
                    {
                        try
                        {
                            return pv.GetValue(record) ?? pv.PropertyType.GetDefaultValue();
                        }
                        catch
                        {
                            // 降级策略：通过运行时类型获取属性值
                            return record.GetType().GetProperty(pv.Name)?.GetValue(record)
                                   ?? pv.PropertyType.GetDefaultValue();
                        }
                    });
            });
    }

    /// <inheritdoc cref="IPurTable.WithSampleRows(int)" />
    public PurTable WithSampleRows(int sampleRows)
    {
        SampleRows = sampleRows;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithAutoFilter(bool)" />
    public PurTable WithAutoFilter(bool autoFilter)
    {
        AutoFilter = autoFilter;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithPassword(string)" />
    public PurTable WithPassword(string? password)
    {
        Password = password;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithTableStyle" />
    public PurTable WithTableStyle(PurStyle style)
    {
        TableStyle = style;
        return this;
    }

    /// <inheritdoc cref="IPurTable.WithPresetStyle(PurcellLibs.PresetStyle)" />
    public PurTable WithPresetStyle(PresetStyle presetStyle)
    {
        PresetStyle = presetStyle;
        return this;
    }

    #endregion Fluent API
}