namespace PurcellLibs;

/// <summary>
/// 预设表格样式枚举，用于特性场景下的样式选择。
/// </summary>
/// <example>
/// <code>
/// // 在特性中使用预设样式
/// [PurTable(PresetStyle = PresetStyle.BrightFresh)]
/// public class User
/// {
///     public string Name { get; set; }
///     public int Age { get; set; }
/// }
/// </code>
/// </example>
/// <remarks>
/// <para>
/// 此枚举专门用于特性场景，提供编译时类型安全的样式选择。
/// </para>
/// <para>
/// 每个枚举值对应 <see cref="PurStyle"/> 中的一个预设样式实例。
/// </para>
/// </remarks>
public enum PresetStyle
{
    /// <summary>
    /// 默认样式（未设置）。
    /// </summary>
    Default = 0,

    /// <summary>
    /// 明亮清新蓝样式。
    /// </summary>
    BrightFresh,

    /// <summary>
    /// 优雅单色样式。
    /// </summary>
    ElegantMonochrome,

    /// <summary>
    /// 大地色调样式。
    /// </summary>
    EarthTones,

    /// <summary>
    /// 暖色调样式。
    /// </summary>
    WarmTones,

    /// <summary>
    /// 海洋蓝样式。
    /// </summary>
    OceanBlue,

    /// <summary>
    /// 复古怀旧样式。
    /// </summary>
    VintageNostalgia,

    /// <summary>
    /// 极简黑白样式。
    /// </summary>
    MinimalistBw,

    /// <summary>
    /// 活力能量样式。
    /// </summary>
    VibrantEnergy,

    /// <summary>
    /// 复古时尚样式。
    /// </summary>
    RetroChic,

    /// <summary>
    /// 温馨秋日样式。
    /// </summary>
    CozyAutumn,

    /// <summary>
    /// 宁静自然样式。
    /// </summary>
    SereneNature,

    /// <summary>
    /// 午夜魔幻样式。
    /// </summary>
    MidnightMagic,

    /// <summary>
    /// 暖阳阳光样式。
    /// </summary>
    SunnyDay
}