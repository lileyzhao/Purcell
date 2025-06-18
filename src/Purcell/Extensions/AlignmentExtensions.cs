using LargeXlsx;

namespace PurcellLibs.Extensions;

/// <summary>
/// 对齐方式扩展方法
/// </summary>
internal static class AlignmentExtensions
{
    /// <summary>
    /// 将 HAlign 转换为 XlsxAlignment.Horizontal
    /// </summary>
    /// <param name="align">水平对齐方式</param>
    /// <returns>XlsxAlignment.Horizontal</returns>
    public static XlsxAlignment.Horizontal ToXlsxHorizontal(this HAlign align)
    {
        return align switch
        {
            HAlign.Default => XlsxAlignment.Horizontal.General,
            HAlign.Left => XlsxAlignment.Horizontal.Left,
            HAlign.Center => XlsxAlignment.Horizontal.Center,
            HAlign.Right => XlsxAlignment.Horizontal.Right,
            HAlign.Fill => XlsxAlignment.Horizontal.Fill,
            HAlign.Justify => XlsxAlignment.Horizontal.Justify,
            HAlign.Distributed => XlsxAlignment.Horizontal.Distributed,
            _ => XlsxAlignment.Horizontal.General
        };
    }

    /// <summary>
    /// 将 VAlign 转换为 XlsxAlignment.Vertical
    /// </summary>
    /// <param name="align">垂直对齐方式</param>
    /// <returns>XlsxAlignment.Vertical</returns>
    public static XlsxAlignment.Vertical ToXlsxVertical(this VAlign align)
    {
        return align switch
        {
            VAlign.Default => XlsxAlignment.Vertical.Bottom,
            VAlign.Top => XlsxAlignment.Vertical.Top,
            VAlign.Center => XlsxAlignment.Vertical.Center,
            VAlign.Bottom => XlsxAlignment.Vertical.Bottom,
            VAlign.Justify => XlsxAlignment.Vertical.Justify,
            VAlign.Distributed => XlsxAlignment.Vertical.Distributed,
            _ => XlsxAlignment.Vertical.Bottom
        };
    }
}