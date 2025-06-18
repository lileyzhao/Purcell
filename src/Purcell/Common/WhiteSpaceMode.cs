namespace PurcellLibs;

/// <summary>
/// 空白字符处理模式
/// </summary>
public enum WhiteSpaceMode
{
    /// <summary>
    /// 保持原始状态，不做任何处理
    /// </summary>
    Preserve = 0,

    /// <summary>
    /// 修剪首尾空格
    /// </summary>
    Trim = 1 << 0, // 1

    /// <summary>
    /// 移除所有空白字符
    /// </summary>
    RemoveAll = 1 << 1 // 2
}