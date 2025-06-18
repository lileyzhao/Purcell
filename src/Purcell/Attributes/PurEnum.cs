namespace PurcellLibs;

/// <summary>
/// 为枚举字段提供多值字符串映射功能的特性（Attribute）。
/// <para>
/// 可用于将多个字符串值映射到同一个枚举值，便于解析和转换。
/// 适用于如 Excel/CSV 等表格数据与枚举类型的灵活映射。
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class PurEnum : Attribute
{
    /// <summary>
    /// 获取可映射到此枚举值的字符串值集合。
    /// <para>
    /// 例如：可将“男”、“M”、“Male”都映射为 Gender.Male。
    /// </para>
    /// </summary>
    public string[] MappedValues { get; }

    /// <summary>
    /// 初始化 <see cref="PurEnum"/> 特性实例，默认无映射值。
    /// </summary>
    public PurEnum()
    {
        MappedValues = [];
    }

    /// <summary>
    /// 初始化 <see cref="PurEnum"/> 特性实例，指定可映射到此枚举值的字符串数组。
    /// </summary>
    /// <param name="mappedValues">可映射到此枚举值的字符串值数组。例如：["男", "M", "Male"]。</param>
    public PurEnum(string[] mappedValues)
    {
        MappedValues = mappedValues;
    }
}