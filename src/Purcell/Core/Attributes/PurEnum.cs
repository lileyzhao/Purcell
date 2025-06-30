// ReSharper disable ClassNeverInstantiated.Global

namespace PurcellLibs;

/// <summary>
/// 为枚举字段提供多值字符串映射功能的特性，支持从多种不同的字符串表示形式解析为相同的枚举值。
/// </summary>
/// <remarks>
/// <para>
/// 此特性用于将多个字符串值映射到同一个枚举值，便于从表格文件中解析多样化的数据格式。
/// </para>
/// <para>
/// 支持一对多映射，即多个不同的字符串值都可以映射到同一个枚举值。匹配时不区分大小写。
/// </para>
/// </remarks>
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
[AttributeUsage(AttributeTargets.Field)]
public class PurEnum : Attribute
{
    /// <summary>
    /// 获取可映射到枚举值的字符串值集合。
    /// </summary>
    /// <value>
    /// 包含所有映射字符串值的数组，第一个元素为主要映射值，后续元素为额外映射值。
    /// </value>
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
    /// <param name="primaryValue">主要映射值，不能为 <see langword="null"/>。</param>
    /// <param name="additionalValues">额外的映射值，<see langword="null"/> 值将被自动过滤。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="primaryValue"/> 为 <see langword="null"/> 时抛出。</exception>
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
    public PurEnum(string primaryValue, params string[] additionalValues)
    {
        primaryValue.ThrowIfArgumentNull("主要映射值不能为 null。");

        MappedValues = [primaryValue, ..additionalValues.Where(v => (string?)v != null)];
    }
}