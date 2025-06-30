namespace PurcellLibs.Extensions;

/// <summary>
/// 提供对齐方式枚举的扩展方法，用于转换为第三方库的对齐类型。
/// </summary>
internal static class AlignmentExtensions
{
    /// <summary>
    /// 将 <see cref="HAlign"/> 转换为 <see cref="XlsxAlignment.Horizontal"/>，用于 Excel 导出时的水平对齐设置。
    /// </summary>
    /// <param name="align">要转换的水平对齐方式。</param>
    /// <returns>对应的 <see cref="XlsxAlignment.Horizontal"/> 枚举值。</returns>
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
    /// 将 <see cref="VAlign"/> 转换为 <see cref="XlsxAlignment.Vertical"/>，用于 Excel 导出时的垂直对齐设置。
    /// </summary>
    /// <param name="align">要转换的垂直对齐方式。</param>
    /// <returns>对应的 <see cref="XlsxAlignment.Vertical"/> 枚举值。</returns>
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