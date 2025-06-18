namespace PurcellLibs;

/// <summary>
/// 列配置，定义如何在表格列与对象属性之间进行数据转换和映射，包含位置、格式和样式等配置选项。
/// 可以作为特性直接应用于属性，也能通过代码动态创建实例。
/// 支持使用流式 API 进行链式配置。
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
[PurTable(HeaderStart = "A1")]
public class PurColumn : Attribute
{
    /// <summary>
    /// 无参构造函数，创建一个默认的 <see cref="PurColumn"/> 列配置实例。
    /// </summary>
    public PurColumn()
    {
    }

    /// <summary>
    /// 带列索引参数的构造函数，创建一个指定列索引的 <see cref="PurColumn"/> 列配置实例。
    /// </summary>
    /// <param name="index">列索引，从 0 开始计数，范围为 0 - 16383。</param>
    public PurColumn(int index)
    {
        Index = index;
    }

    /// <summary>
    /// 带列名集合参数的构造函数，创建一个指定列名的 <see cref="PurColumn"/> 列配置实例。
    /// </summary>
    /// <param name="names">列名集合，用于匹配表格列名的字符串集合，会过滤掉空字符串和重复的名称。</param>
    public PurColumn(params string[] names)
    {
        _names = names.Where(n => !string.IsNullOrEmpty(n)).Distinct().ToList();
    }

    private int? _index;

    /// <summary>
    /// 列索引，范围为 0 - 16383，对应表格中的 A - ZYX 列。
    /// 例如，0 表示 A 列，1 表示 B 列。
    /// 该索引的优先级高于 <see cref="Names"/> 属性。
    /// </summary>
    public int? Index
    {
        get => _index;
        set => _index = value;
    }

    /// <summary>
    /// 列索引字母，如 "A", "B" 等，对应表格中的 A - ZYX 列。
    /// 等同于 <see cref="Index"/> 属性。
    /// 该属性的优先级高于 <see cref="Names"/> 属性。
    /// </summary>
    public string? IndexLetter
    {
        get => _index.HasValue ? CellLocator.GetColumnLetter(_index.Value) : null;
        set => _index = value != null ? CellLocator.ColumnLetterToIndex(value) : null;
    }

    private List<string> _names = [];

    /// <summary>
    /// 列名集合，用于匹配表格列名的字符串集合。
    /// 传入的集合会过滤掉空字符串和重复的名称。
    /// </summary>
    /// <exception cref="ArgumentException">当传入的集合为 null 时抛出。</exception>
    public List<string> Names
    {
        get => _names;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("列名集合不能为 null，请传入有效的字符串集合。", nameof(value));
            }

            _names = value.Where(n => !string.IsNullOrEmpty(n) && !_names.Contains(n)).ToList();
        }
    }

    /// <summary>
    /// 获取列名集合中的第一个名称，也就是主列名。
    /// 在匹配列名时具有最高优先级，在导出表格时会作为表头列名。
    /// 如果列名集合为空，则返回 null。
    /// </summary>
    internal string? PrimaryName => _names.Count > 0 ? _names[0] : null;

    /// <summary>
    /// 标记该列是否为查询时的必需列，默认为 false。
    /// 当设置为 true 时，如果在表格中匹配不到该列，会抛出异常。
    /// </summary>
    public bool IsRequired { get; set; } // TODO: 未实现 IsRequired

    /// <summary>
    /// 该列的默认值，当表格中对应列的值为空或解析失败时，会使用该默认值。
    /// </summary>
    public object? DefaultValue { get; set; }

    /// <summary>
    /// 格式字符串，用于在解析和导出时指定格式。
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// 是否对字符串值进行 Trim 操作，去除首尾空白字符，默认为 false。
    /// </summary>
    public bool TrimValue { get; set; }

    /// <summary>
    /// 列名匹配时的策略，默认为 <see cref="MatchStrategy.IgnoreCase"/>，即忽略大小写匹配。
    /// </summary>
    public MatchStrategy MatchStrategy { get; set; } = MatchStrategy.IgnoreCase;

    /// <summary>
    /// 在查询表格数据时是否忽略该列，默认为 false。
    /// 当设置为 true 时，读取表格数据时会跳过该列，跳过不需要读取的列会提高性能。
    /// </summary>
    public bool IgnoreInQuery { get; set; } // TODO: 未实现 IgnoreInQuery

    /// <summary>
    /// 在导出表格数据时是否忽略该列，默认为 false。
    /// 当设置为 true 时，导出表格时不会包含该列。
    /// </summary>
    public bool IgnoreInExport { get; set; } // TODO: 未实现 IgnoreInExport

    /// <summary>
    /// 映射的属性名称。
    /// </summary>
    public string PropertyName { set; get; } = string.Empty;

    /// <summary>
    /// 值转换器实例，仅在运行时使用。
    /// </summary>
    public IValueConverter? ValueConverter { get; set; }

    /// <summary>
    /// 映射的属性类型，仅在运行时使用。
    /// </summary>
    internal Type? PropertyType { get; set; }

    /// <summary>
    /// 属性的非可空基础类型，仅在运行时使用。
    /// </summary>
    internal Type? UnwrappedType { get; set; }

    /// <summary>
    /// 映射属性的反射信息，用于动态读写属性值。
    /// </summary>
    internal PropertyInfo? Property { get; set; }

    /// <summary>
    /// 列宽，导出表格时该列的宽度。
    /// 中文或全角字符算 2 个宽度单位，英文或数字算 1 个宽度单位。
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// 标记该列的宽度是否为自定义设置。
    /// </summary>
    internal bool IsCustomWidth { get; set; }

    /// <summary>
    /// 导出 Excel 时表头的水平对齐方式。
    /// </summary>
    public HAlign HeaderHAlign { get; set; } // TODO: 未实现 HeaderHAlign

    /// <summary>
    /// 导出 Excel 时表头的垂直对齐方式。
    /// </summary>
    public VAlign HeaderVAlign { get; set; } // TODO: 未实现 HeaderVAlign

    /// <summary>
    /// 导出 Excel 时单元格内容的水平对齐方式。
    /// </summary>
    public HAlign ContentHAlign { get; set; } // TODO: 未实现 ContentHAlign

    /// <summary>
    /// 导出 Excel 时单元格内容的垂直对齐方式。
    /// </summary>
    public VAlign ContentVAlign { get; set; } // TODO: 未实现 ContentVAlign

    /// <summary>
    /// 导出 Excel 时是否隐藏该列，默认为 false。
    /// </summary>
    public bool IsHidden { get; set; } // TODO: 未实现 IsHidden

    #region Fluent 方法链

    /// <summary>
    /// 静态工厂方法，通过指定的列索引创建一个新的 <see cref="PurColumn"/> 列配置实例。
    /// </summary>
    /// <param name="index">列索引，从 0 开始计数，范围为 0 - 16383。</param>
    /// <returns>返回一个新的 <see cref="PurColumn"/> 实例。</returns>
    public static PurColumn FromIndex(int index)
    {
        return new PurColumn(index);
    }

    /// <summary>
    /// 静态工厂方法，通过指定的列索引字母创建一个新的 <see cref="PurColumn"/> 列配置实例。
    /// </summary>
    /// <param name="indexLetter">列索引字母，如 "A", "B" 等。</param>
    /// <returns>返回一个新的 <see cref="PurColumn"/> 实例。</returns>
    /// <exception cref="ArgumentException">当传入的列索引字母为空或仅包含空白字符时抛出。</exception>
    public static PurColumn FromIndex(string indexLetter)
    {
        if (string.IsNullOrWhiteSpace(indexLetter))
        {
            throw new ArgumentException("列索引字母不能为空或仅包含空白字符，请传入有效的列索引字母。", nameof(indexLetter));
        }

        return new PurColumn(CellLocator.ColumnLetterToIndex(indexLetter));
    }

    /// <summary>
    /// 静态工厂方法，通过指定的列名集合创建一个新的 <see cref="PurColumn"/> 列配置实例。
    /// </summary>
    /// <param name="names">列名集合，用于匹配表格列名的字符串集合。</param>
    /// <returns>返回一个新的 <see cref="PurColumn"/> 实例。</returns>
    public static PurColumn FromNames(params string[] names)
    {
        return new PurColumn(names);
    }

    /// <summary>
    /// 静态工厂方法，通过指定的属性名称创建一个新的列配置实例。
    /// </summary>
    /// <param name="propertyName">要映射的属性名称。</param>
    /// <returns>返回一个新的 <see cref="PurColumn"/> 实例。</returns>
    /// <exception cref="ArgumentException">当传入的属性名称为空或仅包含空白字符时抛出。</exception>
    public static PurColumn FromProperty(string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException("属性名称不能为空或仅包含空白字符，请传入有效的属性名称。", nameof(propertyName));
        }

        return new PurColumn { PropertyName = propertyName };
    }

    /// <summary>
    /// 静态工厂方法，通过指定的属性名称创建一个新的列配置实例。
    /// </summary>
    /// <param name="propertyInfo">要映射的属性的反射信息。</param>
    /// <returns>返回一个新的 <see cref="PurColumn"/> 实例。</returns>
    /// <exception cref="ArgumentException">当传入的属性名称为空或仅包含空白字符时抛出。</exception>
    public static PurColumn FromProperty(PropertyInfo propertyInfo)
    {
        return new PurColumn().WithProperty(propertyInfo);
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例的列索引。
    /// </summary>
    /// <param name="index">列索引，从 0 开始计数，范围为 0 - 16383。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithIndex(int index)
    {
        _index = index;
        return this;
    }

    /// <summary>
    /// 链式方法，通过指定的列索引字母设置当前列配置实例的列索引。
    /// </summary>
    /// <param name="indexLetter">列索引字母，如 "A", "B" 等。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    /// <exception cref="ArgumentException">当传入的列索引字母为空或仅包含空白字符时抛出。</exception>
    public PurColumn WithIndex(string indexLetter)
    {
        if (string.IsNullOrWhiteSpace(indexLetter))
        {
            throw new ArgumentException("列索引字母不能为空或仅包含空白字符，请传入有效的列索引字母。", nameof(indexLetter));
        }

        return WithIndex(CellLocator.ColumnLetterToIndex(indexLetter));
    }

    /// <summary>
    /// 链式方法，覆盖当前列配置实例的列名集合。
    /// 会过滤掉空字符串和已存在的名称。
    /// </summary>
    /// <param name="names">用于替换的列名集合。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithNames(IEnumerable<string> names)
    {
        _names = names.Where(n => !string.IsNullOrEmpty(n)).Distinct().ToList();
        return this;
    }

    /// <summary>
    /// 链式方法，向当前列配置实例的列名集合中添加一个列名。
    /// </summary>
    /// <param name="name">要添加的列名。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn AddName(string name)
    {
        if (!_names.Contains(name)) _names.Add(name);
        return this;
    }

    /// <summary>
    /// 链式方法，向当前列配置实例的列名集合中添加多个列名。
    /// 会过滤掉空字符串和已存在的名称。
    /// </summary>
    /// <param name="names">要添加的列名集合。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn AddNames(params string[] names)
    {
        foreach (string name in names.Where(n => !string.IsNullOrEmpty(n)).Distinct())
        {
            if (!_names.Contains(name)) _names.Add(name);
        }

        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例在查询时是否为必需列。
    /// </summary>
    /// <param name="isRequired">如果设置为 true，则该列在查询时必须存在，否则会抛出异常。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithIsRequired(bool isRequired)
    {
        IsRequired = isRequired;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例的默认值。
    /// </summary>
    /// <param name="defaultValue">当表格中对应列的值为空时使用的默认值。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithDefaultValue(object defaultValue)
    {
        DefaultValue = defaultValue;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例的格式字符串。
    /// </summary>
    /// <param name="format">用于解析和导出数据的格式字符串。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithFormat(string format)
    {
        Format = format;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例在查询时是否对字符串值进行 Trim 操作。
    /// </summary>
    /// <param name="trimValue">如果设置为 true，则对字符串值进行 Trim 操作，去除首尾空白字符。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithTrimValue(bool trimValue)
    {
        TrimValue = trimValue;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例的列名匹配策略。
    /// </summary>
    /// <param name="matchStrategy">列名匹配时使用的比较策略。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithMatchStrategy(MatchStrategy matchStrategy)
    {
        MatchStrategy = matchStrategy;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例在查询时是否忽略该列。
    /// </summary>
    /// <param name="ignore">如果设置为 true，则在查询表格数据时跳过该列。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithIgnoreInQuery(bool ignore)
    {
        IgnoreInQuery = ignore;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例在导出时是否忽略该列。
    /// </summary>
    /// <param name="ignore">如果设置为 true，则在导出表格数据时不包含该列。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithIgnoreInExport(bool ignore)
    {
        IgnoreInExport = ignore;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例映射的属性名称。
    /// </summary>
    /// <param name="propertyName">要映射的属性名称。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    /// <exception cref="ArgumentException">当传入的属性名称为空或仅包含空白字符时抛出。</exception>
    public PurColumn WithProperty(string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException("映射的属性名称不能为空或仅包含空白字符，请传入有效的属性名称。", nameof(propertyName));
        }

        PropertyName = propertyName;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例映射的属性反射信息。
    /// </summary>
    /// <param name="propertyInfo">要映射的属性的反射信息。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    /// <exception cref="ArgumentException">当传入的属性反射信息为 null 时抛出。</exception>
    internal PurColumn WithProperty(PropertyInfo propertyInfo)
    {
        if (propertyInfo == null)
        {
            throw new ArgumentException("传入的属性反射信息不能为 null，请提供有效的 PropertyInfo 实例。", nameof(propertyInfo));
        }

        Property = propertyInfo;
        PropertyType = propertyInfo.PropertyType;
        UnwrappedType = propertyInfo.PropertyType.GetActualType();
        WithProperty(propertyInfo.Name);
        return this;
    }

    /// <summary>
    /// 设置转换器函数
    /// </summary>
    /// <param name="converter">转换器函数。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn SetConverter(IValueConverter converter)
    {
        ValueConverter = converter ?? throw new ArgumentNullException(nameof(converter));
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例映射的属性类型。
    /// </summary>
    /// <param name="propertyType">要映射的属性类型。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    internal PurColumn WithPropertyType(Type propertyType)
    {
        PropertyType = propertyType;
        UnwrappedType = propertyType.GetActualType();
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例在导出 Excel 时的列宽度。
    /// </summary>
    /// <param name="width">列的宽度，中文或全角字符算 2 个宽度单位，英文或数字算 1 个宽度单位。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithWidth(double width)
    {
        Width = width;
        IsCustomWidth = true;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例在导出 Excel 时表头的水平对齐方式。
    /// </summary>
    /// <param name="headerHAlign">表头的水平对齐方式。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithHeaderHAlign(HAlign headerHAlign)
    {
        HeaderHAlign = headerHAlign;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例在导出 Excel 时表头的垂直对齐方式。
    /// </summary>
    /// <param name="headerVAlign">表头的垂直对齐方式。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithHeaderVAlign(VAlign headerVAlign)
    {
        HeaderVAlign = headerVAlign;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例在导出 Excel 时单元格内容的水平对齐方式。
    /// </summary>
    /// <param name="contentHAlign">单元格内容的水平对齐方式。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithContentHAlign(HAlign contentHAlign)
    {
        ContentHAlign = contentHAlign;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例在导出 Excel 时单元格内容的垂直对齐方式。
    /// </summary>
    /// <param name="contentVAlign">单元格内容的垂直对齐方式。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithContentVAlign(VAlign contentVAlign)
    {
        ContentVAlign = contentVAlign;
        return this;
    }

    /// <summary>
    /// 链式方法，设置当前列配置实例在导出 Excel 时是否隐藏该列。
    /// </summary>
    /// <param name="isHidden">如果设置为 true，则在导出 Excel 时隐藏该列。</param>
    /// <returns>返回当前 <see cref="PurColumn"/> 实例，以便进行链式调用。</returns>
    public PurColumn WithIsHidden(bool isHidden)
    {
        IsHidden = isHidden;
        return this;
    }

    #endregion Fluent 方法链
    
    /// <summary>
    /// 克隆当前列配置实例，返回一个新的 <see cref="PurColumn"/> 实例。
    /// </summary>
    public PurColumn Clone()
    {
        PurColumn clone = (PurColumn)this.MemberwiseClone();
        clone._names = new List<string>(_names);
        return clone;
    }
}