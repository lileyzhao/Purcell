namespace PurcellLibs;

/// <summary>
/// 定义单元格内容的水平对齐方式枚举。
/// </summary>
public enum HAlign
{
    /// <summary>
    /// 默认对齐方式，通常由应用程序或格式决定。
    /// </summary>
    Default = 0,

    /// <summary>
    /// 左对齐，内容靠左边缘对齐。
    /// </summary>
    Left = 1,

    /// <summary>
    /// 居中对齐，内容在单元格中水平居中。
    /// </summary>
    Center = 2,

    /// <summary>
    /// 右对齐，内容靠右边缘对齐。
    /// </summary>
    Right = 3,

    /// <summary>
    /// 填充对齐，内容填充整个单元格宽度。
    /// </summary>
    Fill = 4,

    /// <summary>
    /// 两端对齐，内容在左右边缘之间均匀分布。
    /// </summary>
    Justify = 5,

    /// <summary>
    /// 分散对齐，字符在单元格内均匀分布。
    /// </summary>
    Distributed = 6
}