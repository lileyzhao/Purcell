namespace PurcellLibs;

/// <inheritdoc cref="IPurColumn" />
[AttributeUsage(AttributeTargets.Property)]
public class PurColumn : Attribute, IPurColumn
{
    internal static readonly PurColumn Default = new();

    #region 构造函数

    /// <summary>
    /// 创建新的 <see cref="PurColumn"/> 实例。
    /// </summary>
    public PurColumn()
    {
    }

    /// <summary>
    /// 通过列索引创建 <see cref="PurColumn"/> 实例。
    /// </summary>
    /// <param name="index">列索引（从 0 开始），范围为 0-16383。</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="index"/> 不在 0-16383 范围内时抛出。</exception>
    public PurColumn(int index)
    {
        Index = index;
    }

    /// <summary>
    /// 通过列名集合创建 <see cref="PurColumn"/> 实例。
    /// </summary>
    /// <param name="names">列名集合，用于匹配表格列名。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="names"/> 为 null 时抛出。</exception>
    public PurColumn(params string[] names)
    {
        Names = names.ToList();
    }

    /// <summary>
    /// 通过属性反射信息创建 <see cref="PurColumn"/> 实例。
    /// </summary>
    /// <param name="propertyInfo">要映射的属性的反射信息。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="propertyInfo"/> 为 null 时抛出。</exception>
    public PurColumn(PropertyInfo propertyInfo)
    {
        Property = propertyInfo;
    }

    #endregion 构造函数

    private int _index = -1;

    /// <inheritdoc cref="IPurColumn.Index" />
    public int Index
    {
        get => _index;
        set
        {
            if (value is < 0 or > 16383)
            {
                throw new ArgumentOutOfRangeException(nameof(Index), "列索引必须在 0 到 16383 之间。");
            }

            _index = value;
        }
    }

    /// <inheritdoc cref="IPurColumn.IndexLetter" />
    public string IndexLetter
    {
        get => CellLocator.GetColumnLetter(_index);
    }

    private List<string> _names = [];

    /// <inheritdoc cref="IPurColumn.Names" />
    public List<string> Names
    {
        get => _names;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("列名集合不能为 null。", nameof(value));
            }

            _names = value.Where(n => !string.IsNullOrEmpty(n)).Distinct().ToList();
        }
    }

    /// <summary>
    /// 获取列名集合中的第一个名称，即主列名。
    /// 在列名匹配时具有最高优先级，在表格导出时用作表头列名。
    /// 如果列名集合为空，则返回 null。
    /// </summary>
    internal string? PrimaryName => _names.Count > 0 ? _names[0] : null;

    /// <inheritdoc cref="IPurColumn.IsRequired" />
    public bool IsRequired { get; set; }

    /// <inheritdoc cref="IPurColumn.DefaultValue" />
    public object? DefaultValue { get; set; }

    /// <inheritdoc cref="IPurColumn.Format" />
    public string? Format { get; set; }

    /// <inheritdoc cref="IPurColumn.TrimValue" />
    public bool TrimValue { get; set; }

    /// <inheritdoc cref="IPurColumn.MatchStrategy" />
    public MatchStrategy MatchStrategy { get; set; } = MatchStrategy.IgnoreCase;

    /// <inheritdoc cref="IPurColumn.IgnoreInQuery" />
    public bool IgnoreInQuery { get; set; } // TODO: 此功能尚未实现

    /// <inheritdoc cref="IPurColumn.IgnoreInExport" />
    public bool IgnoreInExport { get; set; } // TODO: 此功能尚未实现

    private string _propertyName = string.Empty;

    /// <inheritdoc cref="IPurColumn.PropertyName" />
    public string PropertyName
    {
        get => _propertyName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("映射的属性名称不能为 null 或仅包含空白字符。", nameof(PropertyName));
            }

            _propertyName = value;
        }
    }

    /// <inheritdoc cref="IPurColumn.ValueConverter" />
    public IValueConverter? ValueConverter
    {
        get;
        set;
        // 允许设置为 null，与可空类型声明保持一致
    }

    private Type? _propertyType;

    /// <summary>
    /// 映射属性的类型信息。
    /// </summary>
    internal Type? PropertyType
    {
        get => _propertyType;
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(PropertyType), "属性类型不能为 null。");
            }

            UnwrappedType = value.GetActualType();
            _propertyType = value;
        }
    }

    /// <summary>
    /// 属性的非可空基础类型。
    /// </summary>
    internal Type? UnwrappedType { get; private set; }

    private PropertyInfo? _property;

    /// <summary>
    /// 映射属性的反射信息，用于动态读写属性值。
    /// </summary>
    internal PropertyInfo? Property
    {
        get => _property;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("传入的属性反射信息不能为 null。", nameof(Property));
            }

            _property = value;
            PropertyType = value.PropertyType;
            UnwrappedType = value.PropertyType.GetActualType();
            PropertyName = value.Name;
        }
    }

    /// <inheritdoc cref="IPurColumn.Width" />
    public double Width { get; set; }

    /// <summary>
    /// 标记列的宽度是否为自定义设置。
    /// </summary>
    internal bool IsCustomWidth { get; set; }

    /// <inheritdoc cref="IPurColumn.HeaderHAlign" />
    public HAlign? HeaderHAlign { get; set; }

    /// <inheritdoc cref="IPurColumn.HeaderVAlign" />
    public VAlign? HeaderVAlign { get; set; }

    /// <inheritdoc cref="IPurColumn.ContentHAlign" />
    public HAlign? ContentHAlign { get; set; }

    /// <inheritdoc cref="IPurColumn.ContentVAlign" />
    public VAlign? ContentVAlign { get; set; }

    /// <inheritdoc cref="IPurColumn.IsHidden" />
    public bool IsHidden { get; set; }

    #region 静态工厂方法

    /// <summary>
    /// 创建新的 <see cref="PurColumn"/> 实例，等价于 <c>new PurColumn()</c>。
    /// </summary>
    /// <returns>返回新的 <see cref="PurColumn"/> 实例。</returns>
    public static PurColumn New()
    {
        return new PurColumn();
    }

    /// <summary>
    /// 通过列索引创建 <see cref="PurColumn"/> 实例，等价于 <c>new PurColumn(index)</c>。
    /// </summary>
    /// <param name="index">列索引，从 0 开始计数，范围为 0-16383。</param>
    /// <returns>返回新的 <see cref="PurColumn"/> 实例。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="index"/> 不在 0-16383 范围内时抛出。</exception>
    public static PurColumn From(int index)
    {
        return new PurColumn(index);
    }

    /// <summary>
    /// 通过列名集合创建 <see cref="PurColumn"/> 实例，等价于 <c>new PurColumn(names)</c>。
    /// </summary>
    /// <param name="names">列名集合，用于匹配表格列名。</param>
    /// <returns>返回新的 <see cref="PurColumn"/> 实例。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="names"/> 为 null 时抛出。</exception>
    public static PurColumn From(params string[] names)
    {
        return new PurColumn(names);
    }

    /// <summary>
    /// 通过属性反射信息创建 <see cref="PurColumn"/> 实例，等价于 <c>new PurColumn(propertyInfo)</c>。
    /// </summary>
    /// <param name="propertyInfo">要映射的属性的反射信息。</param>
    /// <returns>返回新的 <see cref="PurColumn"/> 实例。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="propertyInfo"/> 为 null 时抛出。</exception>
    public static PurColumn From(PropertyInfo propertyInfo)
    {
        return new PurColumn(propertyInfo);
    }

    /// <summary>
    /// 通过列索引创建 <see cref="PurColumn"/> 实例，等价于 <c>new PurColumn(index)</c>。
    /// </summary>
    /// <param name="index">列索引，从 0 开始计数，范围为 0-16383。</param>
    /// <returns>返回新的 <see cref="PurColumn"/> 实例。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="index"/> 不在 0-16383 范围内时抛出。</exception>
    public static PurColumn FromIndex(int index)
    {
        return new PurColumn(index);
    }

    /// <summary>
    /// 通过列索引字母创建 <see cref="PurColumn"/> 实例。
    /// </summary>
    /// <param name="indexLetter">列索引字母，如 "A"、"B" 等。</param>
    /// <returns>返回新的 <see cref="PurColumn"/> 实例。</returns>
    /// <exception cref="ArgumentException">当传入的列索引字母为空或仅包含空白字符时抛出。</exception>
    public static PurColumn FromIndex(string indexLetter)
    {
        return new PurColumn().WithIndex(indexLetter);
    }

    /// <summary>
    /// 通过列名集合创建 <see cref="PurColumn"/> 实例，等价于 <c>new PurColumn(names)</c>。
    /// </summary>
    /// <param name="names">列名集合，用于匹配表格列名。</param>
    /// <returns>返回新的 <see cref="PurColumn"/> 实例。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="names"/> 为 null 时抛出。</exception>
    public static PurColumn FromNames(params string[] names)
    {
        return new PurColumn(names);
    }

    /// <summary>
    /// 通过属性名称创建 <see cref="PurColumn"/> 实例。
    /// </summary>
    /// <param name="propertyName">要映射的属性名称。</param>
    /// <returns>返回新的 <see cref="PurColumn"/> 实例。</returns>
    /// <exception cref="ArgumentException">当传入的属性名称为空或仅包含空白字符时抛出。</exception>
    public static PurColumn FromProperty(string propertyName)
    {
        return new PurColumn { PropertyName = propertyName };
    }

    /// <summary>
    /// 通过属性反射信息创建 <see cref="PurColumn"/> 实例，等价于 <c>new PurColumn(propertyInfo)</c>。
    /// </summary>
    /// <param name="propertyInfo">要映射的属性的反射信息。</param>
    /// <returns>返回新的 <see cref="PurColumn"/> 实例。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="propertyInfo"/> 为 null 时抛出。</exception>
    public static PurColumn FromProperty(PropertyInfo propertyInfo)
    {
        return new PurColumn(propertyInfo);
    }

    #endregion 静态工厂方法

    #region Fluent API

    /// <inheritdoc cref="IPurColumn.WithIndex(int)" />
    public PurColumn WithIndex(int index)
    {
        Index = index;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithIndex(string)" />
    public PurColumn WithIndex(string indexLetter)
    {
        Index = CellLocator.ColumnLetterToIndex(indexLetter);
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithNames(IEnumerable{string})" />
    public PurColumn WithNames(IEnumerable<string> names)
    {
        Names = names.ToList();
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithNames(string[])" />
    public PurColumn WithNames(params string[] names)
    {
        Names = names.ToList();
        return this;
    }

    /// <inheritdoc cref="IPurColumn.AddName" />
    public PurColumn AddName(string name)
    {
        if (!string.IsNullOrEmpty(name) && !_names.Contains(name))
            _names.Add(name);

        return this;
    }

    /// <inheritdoc cref="IPurColumn.AddNames" />
    public PurColumn AddNames(params string[] names)
    {
        foreach (string name in names.Where(n => !string.IsNullOrEmpty(n)).Distinct())
        {
            if (!_names.Contains(name)) _names.Add(name);
        }

        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithIsRequired" />
    public PurColumn WithIsRequired(bool isRequired)
    {
        IsRequired = isRequired;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithDefaultValue" />
    public PurColumn WithDefaultValue(object defaultValue)
    {
        DefaultValue = defaultValue;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithFormat" />
    public PurColumn WithFormat(string format)
    {
        Format = format;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithTrimValue" />
    public PurColumn WithTrimValue(bool trimValue)
    {
        TrimValue = trimValue;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithMatchStrategy" />
    public PurColumn WithMatchStrategy(MatchStrategy matchStrategy)
    {
        MatchStrategy = matchStrategy;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithIgnoreInQuery" />
    public PurColumn WithIgnoreInQuery(bool ignore)
    {
        IgnoreInQuery = ignore;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithIgnoreInExport" />
    public PurColumn WithIgnoreInExport(bool ignore)
    {
        IgnoreInExport = ignore;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithProperty(string)" />
    public PurColumn WithProperty(string propertyName)
    {
        PropertyName = propertyName;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithProperty(PropertyInfo)" />
    public PurColumn WithProperty(PropertyInfo propertyInfo)
    {
        Property = propertyInfo;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.SetConverter" />
    public PurColumn SetConverter(IValueConverter? converter)
    {
        ValueConverter = converter;
        return this;
    }

    /// <summary>
    /// 链式设置当前列配置实例映射的属性类型。
    /// </summary>
    /// <param name="propertyType">要映射的属性类型。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="propertyType"/> 为 null 时抛出。</exception>
    internal PurColumn WithPropertyType(Type propertyType)
    {
        PropertyType = propertyType;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithWidth" />
    public PurColumn WithWidth(double width)
    {
        Width = width;
        IsCustomWidth = true;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithHeaderHAlign" />
    public PurColumn WithHeaderHAlign(HAlign headerHAlign)
    {
        HeaderHAlign = headerHAlign;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithHeaderVAlign" />
    public PurColumn WithHeaderVAlign(VAlign headerVAlign)
    {
        HeaderVAlign = headerVAlign;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithContentHAlign" />
    public PurColumn WithContentHAlign(HAlign contentHAlign)
    {
        ContentHAlign = contentHAlign;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithContentVAlign" />
    public PurColumn WithContentVAlign(VAlign contentVAlign)
    {
        ContentVAlign = contentVAlign;
        return this;
    }

    /// <inheritdoc cref="IPurColumn.WithIsHidden" />
    public PurColumn WithIsHidden(bool isHidden)
    {
        IsHidden = isHidden;
        return this;
    }

    #endregion Fluent API

    /// <inheritdoc cref="IPurColumn.Clone" />
    public PurColumn Clone()
    {
        PurColumn clone = (PurColumn)MemberwiseClone();
        clone._names = new List<string>(_names);
        return clone;
    }
}