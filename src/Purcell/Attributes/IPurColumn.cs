namespace PurcellLibs;

/// <summary>
/// 列配置类 - 定义 Excel/CSV 表格中单个列的数据转换和映射配置。<br />
/// 
/// <para>
/// 定义如何读取或写入 Excel/CSV 表格文件中的单个列。
/// 比如：这一列对应对象的哪个属性？列的位置在哪里？数据格式怎么转换？样式如何设置？
/// 一个 PurColumn 实例对应一个列。
/// </para>
/// 
/// <para>
/// 配置方式：
/// 可以通过特性标记在属性上、代码动态创建或工厂方法等多种方式进行配置，
/// 支持链式调用进行流畅的配置设置。
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// 既可以作为特性使用（标记在属性上），也可以作为普通配置对象使用。
/// </para>
/// 
/// <para>
/// 列定位优先级：
/// 列的定位按以下优先级进行：Index（列索引）> Names（列名集合）。
/// 当设置了 Index 时，将忽略 Names 进行精确定位；否则使用 Names 进行模糊匹配。
/// </para>
/// 
/// <para>
/// 属性映射：
/// 每个 PurColumn 实例通过 PropertyName（不区分大小写）来映射到对象的具体属性，
/// 支持自动类型转换和自定义转换器，确保数据在表格列和对象属性之间正确流转。
/// </para>
/// </remarks>
public interface IPurColumn
{
    /// <summary>
    /// 列索引，范围为 0-16383，对应表格中的 A-ZYX 列，默认值 -1 表示未设置列索引使用列名匹配推断。
    /// 该索引的优先级高于 <see cref="Names"/> 属性。
    /// </summary>
    int Index { get; set; }

    /// <summary>
    /// 列索引字母，如 "A"、"B" 等，对应表格中的 A-ZYX 列，默认值 string.Empty 表示未设置列索引使用列名匹配推断。
    /// 等同于 <see cref="Index"/> 属性的字母表示形式，优先级高于 <see cref="Names"/> 属性。
    /// </summary>
    string IndexLetter { get; set; }

    /// <summary>
    /// 列名集合，用于匹配表格列名的字符串集合。
    /// 传入的集合会过滤掉空字符串和重复的名称。
    /// </summary>
    /// <exception cref="ArgumentException">当传入的集合为 null 时抛出。</exception>
    List<string> Names { get; set; }

    /// <summary>
    /// 标记该列是否为查询时的必需列，默认为 false。
    /// 当设置为 true 时，如果在表格中匹配不到该列，会抛出异常。
    /// </summary>
    bool IsRequired { get; set; } // TODO: 未实现

    /// <summary>
    /// 该列的默认值，当表格中对应列的值为空或解析失败时，会使用该默认值。
    /// </summary>
    object? DefaultValue { get; set; }

    /// <summary>
    /// 格式字符串，用于在解析和导出时指定数据格式。
    /// 例如：日期格式 "yyyy-MM-dd"、数字格式 "N2" 等。
    /// </summary>
    string? Format { get; set; }

    /// <summary>
    /// 是否对字符串值进行 Trim 操作，去除首尾空白字符，默认为 false。
    /// </summary>
    /// <remarks>⚠️ <c>仅在读取表格时有效</c></remarks>
    bool TrimValue { get; set; }

    /// <summary>
    /// 列名匹配时的策略，默认为 <see cref="MatchStrategy.IgnoreCase"/>，即忽略大小写匹配。
    /// </summary>
    /// <remarks>⚠️ <c>仅在读取表格时有效</c></remarks>
    MatchStrategy MatchStrategy { get; set; }

    /// <summary>
    /// 在查询表格数据时是否忽略该列，默认为 false。
    /// 当设置为 true 时，读取表格数据时会跳过该列，跳过不需要读取的列会提高性能。
    /// </summary>
    bool IgnoreInQuery { get; set; } // TODO: 未实现

    /// <summary>
    /// 在导出表格数据时是否忽略该列，默认为 false。
    /// 当设置为 true 时，导出表格时不会包含该列。
    /// </summary>
    bool IgnoreInExport { get; set; } // TODO: 未实现

    /// <summary>
    /// 映射的属性名称。
    /// </summary>
    string PropertyName { set; get; }

    /// <summary>
    /// 值转换器实例，用于自定义数据类型转换。
    /// </summary>
    IValueConverter? ValueConverter { get; set; }

    /// <summary>
    /// 列宽，导出表格时该列的宽度。
    /// 中文或全角字符算 2 个宽度单位，英文或数字算 1 个宽度单位。
    /// </summary>
    /// <remarks>⚠️ <c>仅在导出表格时有效</c></remarks>
    double Width { get; set; }

    /// <summary>
    /// 导出 Excel 时表头的水平对齐方式。
    /// </summary>
    /// <remarks>⚠️ <c>仅在导出表格时有效</c></remarks>
    HAlign HeaderHAlign { get; set; } // TODO: 此功能尚未实现

    /// <summary>
    /// 导出 Excel 时表头的垂直对齐方式。
    /// </summary>
    /// <remarks>⚠️ <c>仅在导出表格时有效</c></remarks>
    VAlign HeaderVAlign { get; set; } // TODO: 此功能尚未实现

    /// <summary>
    /// 导出 Excel 时单元格内容的水平对齐方式。
    /// </summary>
    /// <remarks>⚠️ <c>仅在导出表格时有效</c></remarks>
    HAlign ContentHAlign { get; set; } // TODO: 此功能尚未实现

    /// <summary>
    /// 导出 Excel 时单元格内容的垂直对齐方式。
    /// </summary>
    /// <remarks>⚠️ <c>仅在导出表格时有效</c></remarks>
    VAlign ContentVAlign { get; set; } // TODO: 此功能尚未实现

    /// <summary>
    /// 导出 Excel 时是否隐藏该列，默认为 false。
    /// </summary>
    /// <remarks>⚠️ <c>仅在导出表格时有效</c></remarks>
    bool IsHidden { get; set; } // TODO: 此功能尚未实现

    /// <summary>
    /// 链式设置当前列配置实例的列索引。
    /// </summary>
    /// <param name="index">列索引，从 0 开始计数，范围为 0-16383。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithIndex(int index);

    /// <summary>
    /// 链式设置当前列配置实例的列索引。
    /// </summary>
    /// <param name="indexLetter">列索引字母，如 "A"、"B" 等。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentException">当传入的列索引字母为空或仅包含空白字符时抛出。</exception>
    PurColumn WithIndex(string indexLetter);

    /// <summary>
    /// 链式设置当前列配置实例的列索引字母。
    /// </summary>
    /// <param name="indexLetter">列索引字母，如 "A"、"B" 等。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentException">当传入的列索引字母为空或仅包含空白字符时抛出。</exception>
    PurColumn WithIndexLetter(string indexLetter);

    /// <summary>
    /// 链式覆盖当前列配置实例的列名集合。
    /// </summary>
    /// <param name="names">用于替换的列名集合。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithNames(IEnumerable<string> names);

    /// <summary>
    /// 链式覆盖当前列配置实例的列名集合。
    /// </summary>
    /// <param name="names">用于替换的列名集合。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithNames(params string[] names);

    /// <summary>
    /// 链式向当前列配置实例的列名集合中添加一个列名。
    /// </summary>
    /// <param name="name">要添加的列名。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn AddName(string name);

    /// <summary>
    /// 链式向当前列配置实例的列名集合中添加多个列名。
    /// </summary>
    /// <param name="names">要添加的列名集合。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn AddNames(params string[] names);

    /// <summary>
    /// 链式设置当前列配置实例在查询时是否为必需列。
    /// </summary>
    /// <param name="isRequired">是否为必需列。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithIsRequired(bool isRequired);

    /// <summary>
    /// 链式设置当前列配置实例的默认值。
    /// </summary>
    /// <param name="defaultValue">当表格中对应列的值为空或解析失败时使用的默认值。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithDefaultValue(object defaultValue);

    /// <summary>
    /// 链式设置当前列配置实例的格式字符串。
    /// </summary>
    /// <param name="format">用于解析和导出数据的格式字符串。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithFormat(string format);

    /// <summary>
    /// 链式设置当前列配置实例在查询时是否对字符串值进行 Trim 操作。
    /// </summary>
    /// <param name="trimValue">是否对字符串值进行 Trim 操作。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithTrimValue(bool trimValue);

    /// <summary>
    /// 链式设置当前列配置实例的列名匹配策略。
    /// </summary>
    /// <param name="matchStrategy">列名匹配时使用的比较策略。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithMatchStrategy(MatchStrategy matchStrategy);

    /// <summary>
    /// 链式设置当前列配置实例在查询时是否忽略该列。
    /// </summary>
    /// <param name="ignore">是否在查询表格数据时跳过该列。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithIgnoreInQuery(bool ignore);

    /// <summary>
    /// 链式设置当前列配置实例在导出时是否忽略该列。
    /// </summary>
    /// <param name="ignore">是否在导出表格数据时不包含该列。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithIgnoreInExport(bool ignore);

    /// <summary>
    /// 链式设置当前列配置实例映射的属性名称。
    /// </summary>
    /// <param name="propertyName">要映射的属性名称。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentException">当传入的属性名称为空或仅包含空白字符时抛出。</exception>
    PurColumn WithProperty(string propertyName);

    /// <summary>
    /// 链式设置当前列配置实例映射的属性反射信息。
    /// </summary>
    /// <param name="propertyInfo">要映射的属性的反射信息。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentException">当传入的属性反射信息为 null 时抛出。</exception>
    PurColumn WithProperty(PropertyInfo propertyInfo);

    /// <summary>
    /// 链式设置值转换器。
    /// </summary>
    /// <param name="converter">值转换器实例。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn SetConverter(IValueConverter converter);

    /// <summary>
    /// 链式设置当前列配置实例在导出 Excel 时的列宽度。
    /// </summary>
    /// <param name="width">列的宽度，中文或全角字符算 2 个宽度单位，英文或数字算 1 个宽度单位。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithWidth(double width);

    /// <summary>
    /// 链式设置当前列配置实例在导出 Excel 时表头的水平对齐方式。
    /// </summary>
    /// <param name="headerHAlign">表头的水平对齐方式。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithHeaderHAlign(HAlign headerHAlign);

    /// <summary>
    /// 链式设置当前列配置实例在导出 Excel 时表头的垂直对齐方式。
    /// </summary>
    /// <param name="headerVAlign">表头的垂直对齐方式。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithHeaderVAlign(VAlign headerVAlign);

    /// <summary>
    /// 链式设置当前列配置实例在导出 Excel 时单元格内容的水平对齐方式。
    /// </summary>
    /// <param name="contentHAlign">单元格内容的水平对齐方式。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithContentHAlign(HAlign contentHAlign);

    /// <summary>
    /// 链式设置当前列配置实例在导出 Excel 时单元格内容的垂直对齐方式。
    /// </summary>
    /// <param name="contentVAlign">单元格内容的垂直对齐方式。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithContentVAlign(VAlign contentVAlign);

    /// <summary>
    /// 链式设置当前列配置实例在导出 Excel 时是否隐藏该列。
    /// </summary>
    /// <param name="isHidden">是否在导出 Excel 时隐藏该列。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithIsHidden(bool isHidden);

    /// <summary>
    /// 克隆当前列配置实例。
    /// </summary>
    /// <returns>返回当前实例的浅拷贝。</returns>
    PurColumn Clone();
}