namespace PurcellLibs;

/// <summary>
/// 为枚举字段提供多值字符串映射功能的特性。
/// </summary>
/// <example>
/// <code>
/// public enum Gender
/// {
///     [PurEnum("男", "M", "Male", "1")]
///     Male,
///     
///     [PurEnum("女", "F", "Female", "0")]
///     Female
/// }
/// 
/// // 解析示例：以下字符串都能正确解析为 Gender.Male
/// // "男" -> Gender.Male
/// // "M" -> Gender.Male  
/// // "Male" -> Gender.Male
/// // "1" -> Gender.Male
/// </code>
/// </example>
/// <remarks>
/// <para>
/// 用于将多个字符串值映射到同一个枚举值，便于从 Excel/CSV 文件中解析多样化的数据格式。
/// </para>
/// <para>
/// 支持一对多映射，即多个不同的字符串值都可以映射到同一个枚举值。匹配时不区分大小写。
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
public class PurEnum : Attribute
{
    /// <summary>
    /// 获取可映射到枚举值的字符串值集合。
    /// </summary>
    /// <example>
    /// <code>
    /// // 定义枚举映射
    /// [PurEnum("男", "M", "Male", "1")]
    /// Male,
    /// 
    /// // 获取映射值集合
    /// var enumField = typeof(Gender).GetField("Male");
    /// var purEnum = enumField.GetCustomAttribute&lt;PurEnum&gt;();
    /// string[] mappedValues = purEnum.MappedValues; // ["男", "M", "Male", "1"]
    /// </code>
    /// </example>
    public string[] MappedValues { get; }

    /// <summary>
    /// 初始化 <see cref="PurEnum"/> 特性实例，指定可映射到枚举值的字符串值。
    /// </summary>
    /// <example>
    /// <code>
    /// // 用户状态枚举映射
    /// public enum UserStatus
    /// {
    ///     [PurEnum("活跃", "Active", "1", "true")]
    ///     Active,
    ///     
    ///     [PurEnum("非活跃", "Inactive", "0", "false")]
    ///     Inactive,
    ///     
    ///     [PurEnum("封禁", "Banned", "Block", "-1")]
    ///     Banned
    /// }
    /// 
    /// // 简单映射（仅一个值）
    /// public enum Priority
    /// {
    ///     [PurEnum("高")]
    ///     High,
    ///     
    ///     [PurEnum("中")]
    ///     Medium,
    ///     
    ///     [PurEnum("低")]
    ///     Low
    /// }
    /// </code>
    /// </example>
    /// <param name="primaryValue">主要映射值，不能为 null。</param>
    /// <param name="additionalValues">额外的映射值，null 值将被自动过滤。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="primaryValue"/> 为 null 时抛出。</exception>
    public PurEnum(string primaryValue, params string[] additionalValues)
    {
        ArgumentNullException.ThrowIfNull(primaryValue, nameof(primaryValue));

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        IEnumerable<string> filteredAdditionalValues = additionalValues.Where(v => v != null);
        MappedValues = [primaryValue, ..filteredAdditionalValues];
    }
}