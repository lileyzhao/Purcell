namespace PurcellLibs;

/// <summary>
/// 列配置接口 - 定义 Excel/CSV 表格中单个列的数据转换和映射配置。
/// </summary>
/// <remarks>
/// <para>
/// 定义如何读取或写入 Excel/CSV 表格文件中的单个列，包括列定位、属性映射、数据格式转换、样式设置等。
/// 支持特性标记、代码动态创建、工厂方法等多种配置方式，并支持链式调用。
/// </para>
/// <para>
/// 列定位优先级：Index（列索引）> Names（列名集合）。
/// 设置 Index 时将忽略 Names 进行精确定位；否则使用 Names 进行模糊匹配。
/// </para>
/// <para>
/// 属性映射：
/// 通过 PropertyName（不区分大小写）映射到对象的具体属性，
/// 支持自动类型转换和自定义转换器，确保数据在表格列和对象属性之间正确流转。
/// </para>
/// </remarks>
public interface IPurColumn
{
    /// <summary>
    /// 列索引，范围为 0-16383，对应表格中的 A 到 ZYX 列，默认值 -1 表示未设置列索引，使用列名匹配推断。
    /// </summary>
    /// <example>
    /// <code>
    /// // 设置为第一列 (A列)
    /// column.Index = 0;
    /// // 设置为第三列 (C列)
    /// column.WithIndex(2);
    /// // 设置为第26列 (Z列)
    /// column.WithIndex(25);
    /// // 重置为未设置状态
    /// column.Index = -1;
    /// </code>
    /// </example>
    /// <remarks>
    /// 列索引优先级高于 <see cref="Names"/> 属性。设置列索引后将忽略列名进行精确定位。
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">当设置值不在 0-16383 范围内时抛出。</exception>
    int Index { get; set; }

    /// <summary>
    /// 列索引字母表示，如 "A"、"B" 等，对应表格中的 A 到 ZYX 列，默认值为空字符串表示未设置列索引，使用列名匹配推断。
    /// </summary>
    /// <example>
    /// <code>
    /// // 设置为A列
    /// column.IndexLetter = "A";
    /// // 设置为C列
    /// column.WithIndexLetter("C");
    /// // 设置为Z列
    /// column.WithIndexLetter("Z");
    /// // 设置为AA列（第27列）
    /// column.IndexLetter = "AA";
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// 等同于 <see cref="Index"/> 属性的字母表示形式，优先级高于 <see cref="Names"/> 属性。
    /// </para>
    /// <para>
    /// 设置此属性会自动更新 <see cref="Index"/> 属性的值。
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentException">当传入的列索引字母为空或仅包含空白字符时抛出。</exception>
    string IndexLetter { get; set; }

    /// <summary>
    /// 列名集合，用于匹配表格列名的字符串集合。
    /// 传入的集合会过滤掉空字符串和重复的名称。
    /// </summary>
    /// <example>
    /// <code>
    /// // 设置多个列名进行匹配
    /// column.Names = new List&lt;string&gt; { "姓名", "用户名", "Name" };
    /// // 读取时会按顺序尝试匹配："姓名" → "用户名" → "Name"
    /// 
    /// // 或使用链式调用
    /// column.WithNames("邮箱", "电子邮件", "Email", "E-mail");
    /// 
    /// // 添加更多匹配名称
    /// column.AddName("用户邮箱").AddName("联系邮箱");
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// 优先级低于 <see cref="Index"/> 和 <see cref="IndexLetter"/> 属性。
    /// </para>
    /// <para>
    /// 匹配时按列表顺序进行，第一个匹配成功的名称会被使用。
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentException">当传入的集合为 null 时抛出。</exception>
    List<string> Names { get; set; }

    /// <summary>
    /// 标记列是否为查询时的必需列，默认为 false。
    /// </summary>
    /// <example>
    /// <code>
    /// // 设置为必需列
    /// column.IsRequired = true;
    /// // 如果表格中没有匹配的列名或索引，将抛出异常
    /// 
    /// // 设置为可选列（默认）
    /// column.WithIsRequired(false);
    /// // 如果匹配不到，将跳过此列或使用默认值
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// 设置为 true 时，表格中匹配不到列将抛出异常。用于确保关键数据字段在表格中存在。
    /// </para>
    /// <para>
    /// ⚠️ 仅在读取表格时有效。
    /// </para>
    /// </remarks>
    bool IsRequired { get; set; } // TODO: 功能未实现

    /// <summary>
    /// 列的默认值，表格中对应列的值为空或解析失败时使用此默认值。
    /// </summary>
    /// <example>
    /// <code>
    /// // 设置字符串默认值
    /// column.DefaultValue = "未知";
    /// // 设置数字默认值
    /// column.WithDefaultValue(0);
    /// // 设置日期默认值
    /// column.WithDefaultValue(DateTime.Now);
    /// // 设置布尔默认值
    /// column.WithDefaultValue(false);
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// 默认值的类型应与目标属性类型兼容，系统自动进行类型转换。
    /// </para>
    /// <para>
    /// 单元格为空或解析失败时，使用默认值而不是抛出异常。
    /// </para>
    /// </remarks>
    object? DefaultValue { get; set; }

    /// <summary>
    /// 格式字符串，用于在解析和导出时指定数据格式。
    /// 例如：日期格式 "yyyy-MM-dd"、数字格式 "N2" 等。
    /// </summary>
    /// <example>
    /// <code>
    /// // 日期格式
    /// column.Format = "yyyy-MM-dd";
    /// // 数字格式（保留2位小数）
    /// column.WithFormat("N2");
    /// // 百分比格式
    /// column.WithFormat("P1");
    /// // 货币格式
    /// column.WithFormat("C2");
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// 格式字符串遵循 .NET 标准格式规范，支持日期时间、数字、货币等格式。
    /// </para>
    /// <para>
    /// 读取时用于解析字符串为强类型值，导出时用于格式化显示。
    /// </para>
    /// </remarks>
    string? Format { get; set; }

    /// <summary>
    /// 是否对字符串值进行 Trim 操作去除首尾空白字符，默认为 false。
    /// </summary>
    /// <example>
    /// <code>
    /// // 启用 Trim 操作
    /// column.TrimValue = true;
    /// // 读取结果："  张三  " → "张三"
    /// 
    /// // 禁用 Trim 操作（默认）
    /// column.WithTrimValue(false);
    /// // 读取结果："  张三  " → "  张三  "
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// 启用后，自动去除字符串值的首尾空白字符（包括空格、制表符、换行符等）。
    /// </para>
    /// <para>
    /// ⚠️ 仅在读取表格时有效。
    /// </para>
    /// </remarks>
    bool TrimValue { get; set; }

    /// <summary>
    /// 列名匹配时的策略，默认为 <see cref="MatchStrategy.IgnoreCase"/>，即忽略大小写匹配。
    /// </summary>
    /// <example>
    /// <code>
    /// // 忽略大小写匹配（默认）
    /// column.MatchStrategy = MatchStrategy.IgnoreCase;
    /// // "UserName" 可以匹配 "username"、"USERNAME"
    /// 
    /// // 精确匹配
    /// column.WithMatchStrategy(MatchStrategy.Exact);
    /// // "UserName" 只能匹配 "UserName"
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// 匹配策略影响 <see cref="Names"/> 集合与表格列名的比较方式：
    /// </para>
    /// <para>
    /// - <see cref="MatchStrategy.IgnoreCase"/>：忽略大小写差异
    /// </para>
    /// <para>
    /// - <see cref="MatchStrategy.Exact"/>：完全精确匹配
    /// </para>
    /// <para>
    /// ⚠️ 仅在读取表格时有效。
    /// </para>
    /// </remarks>
    MatchStrategy MatchStrategy { get; set; }

    /// <summary>
    /// 在查询表格数据时是否忽略此列，默认为 false。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 设置为 true 时，读取表格数据时跳过此列，可提高性能。
    /// </para>
    /// <para>
    /// ⚠️ 仅在读取表格时有效。
    /// </para>
    /// </remarks>
    bool IgnoreInQuery { get; set; } // TODO: 功能未实现

    /// <summary>
    /// 在导出表格数据时是否忽略此列，默认为 false。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 设置为 true 时，导出表格时不包含此列。
    /// </para>
    /// <para>
    /// ⚠️ 仅在导出表格时有效。
    /// </para>
    /// </remarks>
    bool IgnoreInExport { get; set; } // TODO: 功能未实现

    /// <summary>
    /// 映射的属性名称。
    /// </summary>
    /// <example>
    /// <code>
    /// // 设置属性映射
    /// column.PropertyName = "UserName";
    /// // 或使用链式调用
    /// column.WithProperty("UserName");
    /// </code>
    /// </example>
    /// <exception cref="ArgumentException">当设置值为空或仅包含空白字符时抛出。</exception>
    string PropertyName { set; get; }

    /// <summary>
    /// 值转换器实例，用于自定义数据类型转换。
    /// </summary>
    /// <example>
    /// <code>
    /// // 设置自定义日期转换器
    /// column.ValueConverter = new CustomDateConverter();
    /// 
    /// // 设置枚举转换器
    /// column.SetConverter(new EnumConverter&lt;UserStatus&gt;());
    /// 
    /// // 设置布尔值转换器
    /// column.SetConverter(new BoolConverter("是", "否"));
    /// 
    /// // 清除转换器（使用默认转换）
    /// column.SetConverter(null);
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// 自定义转换器用于处理特殊的数据类型转换需求，如枚举、复杂对象等。
    /// </para>
    /// <para>
    /// 设置为 null 时，将使用内置的默认类型转换机制。
    /// </para>
    /// </remarks>
    IValueConverter? ValueConverter { get; set; }

    /// <summary>
    /// 列宽，导出表格时此列的宽度。
    /// 中文或全角字符算 2 个宽度单位，英文或数字算 1 个宽度单位。
    /// </summary>
    /// <example>
    /// <code>
    /// // 设置列宽为20个单位
    /// column.Width = 20;
    /// // 或使用链式调用
    /// column.WithWidth(25.5);
    /// // 自动宽度（使用0或负数）
    /// column.WithWidth(0);
    /// </code>
    /// </example>
    /// <remarks>
    /// <para>
    /// 宽度值为0或负数时，根据内容自动计算列宽。
    /// </para>
    /// <para>
    /// ⚠️ 仅在导出表格时有效。
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">当设置值为 NaN 或无穷大时抛出。</exception>
    double Width { get; set; }

    /// <summary>
    /// 导出 Excel 时表头的水平对齐方式。
    /// </summary>
    /// <remarks>⚠️ 仅在导出表格时有效。</remarks>
    HAlign HeaderHAlign { get; set; } // TODO: 此功能尚未实现

    /// <summary>
    /// 导出 Excel 时表头的垂直对齐方式。
    /// </summary>
    /// <remarks>⚠️ 仅在导出表格时有效。</remarks>
    VAlign HeaderVAlign { get; set; } // TODO: 此功能尚未实现

    /// <summary>
    /// 导出 Excel 时单元格内容的水平对齐方式。
    /// </summary>
    /// <remarks>⚠️ 仅在导出表格时有效。</remarks>
    HAlign ContentHAlign { get; set; } // TODO: 此功能尚未实现

    /// <summary>
    /// 导出 Excel 时单元格内容的垂直对齐方式。
    /// </summary>
    /// <remarks>⚠️ 仅在导出表格时有效。</remarks>
    VAlign ContentVAlign { get; set; } // TODO: 此功能尚未实现

    /// <summary>
    /// 导出 Excel 时是否隐藏列，默认为 false。
    /// </summary>
    /// <remarks>⚠️ 仅在导出表格时有效。</remarks>
    bool IsHidden { get; set; } // TODO: 此功能尚未实现

    /// <summary>
    /// 链式设置列索引。
    /// </summary>
    /// <param name="index">列索引，从 0 开始计数，范围为 0-16383。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当索引不在 0-16383 范围内时抛出。</exception>
    PurColumn WithIndex(int index);

    /// <summary>
    /// 链式设置列索引。
    /// </summary>
    /// <param name="indexLetter">列索引字母，如 "A"、"B" 等。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentException">当传入的列索引字母为空或仅包含空白字符时抛出。</exception>
    PurColumn WithIndex(string indexLetter);

    /// <summary>
    /// 链式设置列索引字母。
    /// </summary>
    /// <param name="indexLetter">列索引字母，如 "A"、"B" 等。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentException">当传入的列索引字母为空或仅包含空白字符时抛出。</exception>
    PurColumn WithIndexLetter(string indexLetter);

    /// <summary>
    /// 链式覆盖列名集合。
    /// </summary>
    /// <param name="names">用于替换的列名集合。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentNullException">当传入的集合为 null 时抛出。</exception>
    PurColumn WithNames(IEnumerable<string> names);

    /// <summary>
    /// 链式覆盖列名集合。
    /// </summary>
    /// <param name="names">用于替换的列名集合。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentNullException">当传入的数组为 null 时抛出。</exception>
    PurColumn WithNames(params string[] names);

    /// <summary>
    /// 链式向列名集合中添加一个列名。
    /// </summary>
    /// <param name="name">要添加的列名。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn AddName(string name);

    /// <summary>
    /// 链式向列名集合中添加多个列名。
    /// </summary>
    /// <param name="names">要添加的列名集合。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn AddNames(params string[] names);

    /// <summary>
    /// 链式设置在查询时是否为必需列。
    /// </summary>
    /// <param name="isRequired">是否为必需列。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithIsRequired(bool isRequired);

    /// <summary>
    /// 链式设置默认值。
    /// </summary>
    /// <param name="defaultValue">当表格中对应列的值为空或解析失败时使用的默认值。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithDefaultValue(object defaultValue);

    /// <summary>
    /// 链式设置格式字符串。
    /// </summary>
    /// <param name="format">用于解析和导出数据的格式字符串。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithFormat(string format);

    /// <summary>
    /// 链式设置在查询时是否对字符串值进行 Trim 操作。
    /// </summary>
    /// <param name="trimValue">是否对字符串值进行 Trim 操作。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithTrimValue(bool trimValue);

    /// <summary>
    /// 链式设置列名匹配策略。
    /// </summary>
    /// <param name="matchStrategy">列名匹配时使用的比较策略。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithMatchStrategy(MatchStrategy matchStrategy);

    /// <summary>
    /// 链式设置在查询时是否忽略此列。
    /// </summary>
    /// <param name="ignore">是否在查询表格数据时跳过此列。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithIgnoreInQuery(bool ignore);

    /// <summary>
    /// 链式设置在导出时是否忽略此列。
    /// </summary>
    /// <param name="ignore">是否在导出表格数据时不包含此列。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithIgnoreInExport(bool ignore);

    /// <summary>
    /// 链式设置映射的属性名称。
    /// </summary>
    /// <param name="propertyName">要映射的属性名称。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    /// <exception cref="ArgumentException">当传入的属性名称为空或仅包含空白字符时抛出。</exception>
    PurColumn WithProperty(string propertyName);

    /// <summary>
    /// 链式设置映射的属性反射信息。
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
    /// 链式设置在导出 Excel 时的列宽度。
    /// </summary>
    /// <param name="width">列的宽度，中文或全角字符算 2 个宽度单位，英文或数字算 1 个宽度单位。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithWidth(double width);

    /// <summary>
    /// 链式设置在导出 Excel 时表头的水平对齐方式。
    /// </summary>
    /// <param name="headerHAlign">表头的水平对齐方式。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithHeaderHAlign(HAlign headerHAlign);

    /// <summary>
    /// 链式设置在导出 Excel 时表头的垂直对齐方式。
    /// </summary>
    /// <param name="headerVAlign">表头的垂直对齐方式。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithHeaderVAlign(VAlign headerVAlign);

    /// <summary>
    /// 链式设置在导出 Excel 时单元格内容的水平对齐方式。
    /// </summary>
    /// <param name="contentHAlign">单元格内容的水平对齐方式。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithContentHAlign(HAlign contentHAlign);

    /// <summary>
    /// 链式设置在导出 Excel 时单元格内容的垂直对齐方式。
    /// </summary>
    /// <param name="contentVAlign">单元格内容的垂直对齐方式。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithContentVAlign(VAlign contentVAlign);

    /// <summary>
    /// 链式设置在导出 Excel 时是否隐藏此列。
    /// </summary>
    /// <param name="isHidden">是否在导出 Excel 时隐藏此列。</param>
    /// <returns>返回当前实例以支持链式调用。</returns>
    PurColumn WithIsHidden(bool isHidden);

    /// <summary>
    /// 克隆列配置实例。
    /// </summary>
    /// <returns>返回当前实例的浅拷贝。</returns>
    PurColumn Clone();
}