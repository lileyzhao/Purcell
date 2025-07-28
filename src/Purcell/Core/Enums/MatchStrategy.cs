namespace PurcellLibs;

/// <summary>
/// 列名匹配策略。
/// </summary>
[Flags]
public enum MatchStrategy
{
    /// <summary>
    /// 精确匹配。
    /// </summary>
    Exact = 0,

    /// <summary>
    /// 忽略大小写匹配（默认）。
    /// </summary>
    IgnoreCase = 1 << 0, // 1

    /// <summary>
    /// 包含匹配（列名包含指定字符串）。
    /// </summary>
    Contains = 1 << 1, // 2

    /// <summary>
    /// 前缀匹配（列名以指定前缀开始）。
    /// </summary>
    Prefix = 1 << 2, // 4

    /// <summary>
    /// 后缀匹配（列名以指定后缀结束）。
    /// </summary>
    Suffix = 1 << 3, // 8

    /// <summary>
    /// 正则表达式匹配。
    /// </summary>
    Regex = 1 << 4, // 16

    // 常用组合策略
    /// <summary>
    /// 忽略大小写的包含匹配。
    /// </summary>
    IgnoreCaseContains = IgnoreCase | Contains,

    /// <summary>
    /// 忽略大小写的前缀匹配。
    /// </summary>
    IgnoreCasePrefix = IgnoreCase | Prefix,

    /// <summary>
    /// 忽略大小写的后缀匹配。
    /// </summary>
    IgnoreCaseSuffix = IgnoreCase | Suffix,

    /// <summary>
    /// 忽略大小写的正则表达式匹配。
    /// </summary>
    IgnoreCaseRegex = IgnoreCase | Regex,

    /// <summary>
    /// 默认：忽略大小写匹配。
    /// </summary>
    Default = IgnoreCase
}