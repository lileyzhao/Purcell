namespace PurcellLibs;

/// <summary>
/// 定义单元格内容的垂直对齐方式枚举。
/// </summary>
public enum VAlign
{
    /// <summary>
    /// 默认对齐方式，通常由应用程序或格式决定。
    /// </summary>
    Default = 0,

    /// <summary>
    /// 顶部对齐，内容靠上边缘对齐。
    /// </summary>
    Top = 1,

    /// <summary>
    /// 居中对齐，内容在单元格中垂直居中。
    /// </summary>
    Center = 2,

    /// <summary>
    /// 底部对齐，内容靠下边缘对齐。
    /// </summary>
    Bottom = 3,

    /// <summary>
    /// 两端对齐，内容在上下边缘之间均匀分布。
    /// </summary>
    Justify = 4,

    /// <summary>
    /// 分散对齐，内容在单元格内垂直均匀分布。
    /// </summary>
    Distributed = 5
}