namespace PurcellLibs;

/// <summary>
/// 为枚举值提供多值映射功能的特性。
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class PurEnum : Attribute
{
    /// <summary>
    /// 获取可映射到此枚举值的字符串值数组
    /// </summary>
    public string[] MappedValues { get; }

    /// <summary>
    /// 初始化特性实例，指定可映射到此枚举值的字符串数组
    /// </summary>
    public PurEnum()
    {
        MappedValues = [];
    }

    /// <summary>
    /// 初始化特性实例，指定可映射到此枚举值的字符串数组
    /// </summary>
    /// <param name="mappedValues">可映射到此枚举值的字符串值数组</param>
    public PurEnum(string[] mappedValues)
    {
        MappedValues = mappedValues;
    }
}