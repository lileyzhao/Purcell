using LargeXlsx;

namespace PurcellLibs;

/// <summary>
/// Excel 表格样式配置，用于定义 Excel 表格的表头、内容样式及列宽、行高等参数。
/// </summary>
/// <example>
/// <code>
/// // 使用预设样式
/// table.WithTableStyle(PurStyle.BrightFresh);
/// 
/// // 自定义样式配置
/// var customStyle = new PurStyle()
///     .SetHeaderStyle(Color.White, Color.Blue)
///     .SetMinColumnWidth(15)
///     .SetMaxColumnWidth(30);
/// 
/// // 在工作表配置中使用
/// table.WithTableStyle(customStyle);
/// 
/// // 导出时使用样式
/// Purcell.Export(data, "output.xlsx", table);
/// </code>
/// </example>
/// <remarks>
/// <para>
/// 仅适用于 Excel 文件的样式设置，通过代码方式配置，支持链式调用进行流畅设置。
/// </para>
/// <para>
/// 提供多种预设样式，包括 Default、BrightFresh、ElegantMonochrome 等，满足不同的视觉需求。
/// </para>
/// <para>
/// ⚠️ 对 CSV 格式无效。
/// </para>
/// </remarks>
public class PurStyle
{
    /// <summary>
    /// 表头单元格样式，默认为 <see cref="XlsxStyle.Default"/>。
    /// </summary>
    public XlsxStyle HeaderStyle { get; set; } = XlsxStyle.Default;

    /// <summary>
    /// 内容单元格样式，默认为 <see cref="XlsxStyle.Default"/>。
    /// </summary>
    public XlsxStyle ContentStyle { get; set; } = XlsxStyle.Default;

    /// <summary>
    /// 最小列宽，单位为字符宽度，默认值为 10。
    /// 中文或全角字符算 2 个宽度单位，英文或数字算 1 个宽度单位。
    /// </summary>
    public double MinColumnWidth { get; set; } = 10d;

    /// <summary>
    /// 最大列宽，单位为字符宽度，默认值为 20。
    /// 中文或全角字符算 2 个宽度单位，英文或数字算 1 个宽度单位。
    /// </summary>
    public double MaxColumnWidth { get; set; } = 20d;

    /// <summary>
    /// 表头行高，单位为磅（point），默认值为 18。
    /// </summary>
    public double HeaderLineHeight { get; set; } = 18d;

    /// <summary>
    /// 内容行高，单位为磅（point），默认值为 18。
    /// </summary>
    public double ContentLineHeight { get; set; } = 18d;

    #region Fluent API

    /// <summary>
    /// 设置表头单元格样式。
    /// </summary>
    /// <param name="headerStyle">表头样式对象。</param>
    /// <returns>返回当前 <see cref="PurStyle"/> 实例，支持链式调用。</returns>
    public PurStyle SetHeaderStyle(XlsxStyle headerStyle)
    {
        HeaderStyle = headerStyle;
        return this;
    }

    /// <summary>
    /// 设置表头单元格样式。
    /// </summary>
    /// <param name="textColor">文本颜色。</param>
    /// <param name="fillColor">填充色。</param>
    /// <param name="fontFamily">字体名称，默认 "Calibri"。</param>
    /// <param name="fontSize">字体大小，默认 11。</param>
    /// <param name="fontBold">是否加粗，默认 true。</param>
    /// <param name="fontItalic">是否斜体，默认 false。</param>
    /// <param name="fontStrike">是否删除线，默认 false。</param>
    /// <param name="fontUnderline">下划线样式，默认无。</param>
    /// <param name="horizontal">水平对齐方式，默认左对齐。</param>
    /// <param name="vertical">垂直对齐方式，默认居中。</param>
    /// <returns>返回当前 <see cref="PurStyle"/> 实例，支持链式调用。</returns>
    public PurStyle SetHeaderStyle(Color textColor, Color fillColor, string fontFamily = "Calibri", double fontSize = 11,
        bool fontBold = true,
        bool fontItalic = false, bool fontStrike = false, XlsxFont.Underline fontUnderline = XlsxFont.Underline.None,
        XlsxAlignment.Horizontal horizontal = XlsxAlignment.Horizontal.Left,
        XlsxAlignment.Vertical vertical = XlsxAlignment.Vertical.Center)
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(textColor).WithName(fontFamily).WithSize(fontSize).WithBold(fontBold)
                .WithItalic(fontItalic)
                .WithStrike(fontStrike).WithUnderline(fontUnderline))
            .With(new XlsxFill(fillColor))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(horizontal, vertical));
        return this;
    }

    /// <summary>
    /// 设置表头单元格样式。
    /// </summary>
    /// <param name="font">字体样式对象。</param>
    /// <param name="fill">填充样式对象。</param>
    /// <param name="horizontal">水平对齐方式，默认左对齐。</param>
    /// <param name="vertical">垂直对齐方式，默认居中。</param>
    /// <returns>返回当前 <see cref="PurStyle"/> 实例，支持链式调用。</returns>
    public PurStyle SetHeaderStyle(XlsxFont font, XlsxFill fill,
        XlsxAlignment.Horizontal horizontal = XlsxAlignment.Horizontal.Left,
        XlsxAlignment.Vertical vertical = XlsxAlignment.Vertical.Center)
    {
        HeaderStyle = XlsxStyle.Default
            .With(font)
            .With(fill)
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(horizontal, vertical));
        return this;
    }

    /// <summary>
    /// 设置内容单元格样式。
    /// </summary>
    /// <param name="contentStyle">内容样式对象。</param>
    /// <returns>返回当前 <see cref="PurStyle"/> 实例，支持链式调用。</returns>
    public PurStyle SetContentStyle(XlsxStyle contentStyle)
    {
        ContentStyle = contentStyle;
        return this;
    }

    /// <summary>
    /// 设置内容单元格样式。
    /// </summary>
    /// <param name="textColor">文本颜色。</param>
    /// <param name="fillColor">填充色。</param>
    /// <param name="fontFamily">字体名称，默认 "Calibri"。</param>
    /// <param name="fontSize">字体大小，默认 11。</param>
    /// <param name="fontBold">是否加粗，默认 true。</param>
    /// <param name="fontItalic">是否斜体，默认 false。</param>
    /// <param name="fontStrike">是否删除线，默认 false。</param>
    /// <param name="fontUnderline">下划线样式，默认无。</param>
    /// <param name="horizontal">水平对齐方式，默认左对齐。</param>
    /// <param name="vertical">垂直对齐方式，默认居中。</param>
    /// <returns>返回当前 <see cref="PurStyle"/> 实例，支持链式调用。</returns>
    public PurStyle SetContentStyle(Color textColor, Color fillColor, string fontFamily = "Calibri", double fontSize = 11,
        bool fontBold = true,
        bool fontItalic = false, bool fontStrike = false, XlsxFont.Underline fontUnderline = XlsxFont.Underline.None,
        XlsxAlignment.Horizontal horizontal = XlsxAlignment.Horizontal.Left,
        XlsxAlignment.Vertical vertical = XlsxAlignment.Vertical.Center)
    {
        ContentStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(textColor).WithName(fontFamily).WithSize(fontSize).WithBold(fontBold)
                .WithItalic(fontItalic)
                .WithStrike(fontStrike).WithUnderline(fontUnderline))
            .With(new XlsxFill(fillColor))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(horizontal, vertical));
        return this;
    }

    /// <summary>
    /// 设置内容单元格样式。
    /// </summary>
    /// <param name="font">字体样式对象。</param>
    /// <param name="fill">填充样式对象。</param>
    /// <param name="horizontal">水平对齐方式，默认左对齐。</param>
    /// <param name="vertical">垂直对齐方式，默认居中。</param>
    /// <returns>返回当前 <see cref="PurStyle"/> 实例，支持链式调用。</returns>
    public PurStyle SetContentStyle(XlsxFont font, XlsxFill fill,
        XlsxAlignment.Horizontal horizontal = XlsxAlignment.Horizontal.Left,
        XlsxAlignment.Vertical vertical = XlsxAlignment.Vertical.Center)
    {
        ContentStyle = XlsxStyle.Default
            .With(font)
            .With(fill)
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(horizontal, vertical));
        return this;
    }

    /// <summary>
    /// 设置表头行高。
    /// </summary>
    /// <param name="headerLineHeight">表头行高，单位为磅（point）。</param>
    /// <returns>返回当前 <see cref="PurStyle"/> 实例，支持链式调用。</returns>
    public PurStyle SetHeaderLineHeight(double headerLineHeight)
    {
        HeaderLineHeight = headerLineHeight;
        return this;
    }

    /// <summary>
    /// 设置内容行高。
    /// </summary>
    /// <param name="contentLineHeight">内容行高，单位为磅（point）。</param>
    /// <returns>返回当前 <see cref="PurStyle"/> 实例，支持链式调用。</returns>
    public PurStyle SetContentLineHeight(double contentLineHeight)
    {
        ContentLineHeight = contentLineHeight;
        return this;
    }

    /// <summary>
    /// 设置最小列宽。
    /// </summary>
    /// <param name="minColumnWidth">最小列宽，单位为字符宽度。</param>
    /// <returns>返回当前 <see cref="PurStyle"/> 实例，支持链式调用。</returns>
    public PurStyle SetMinColumnWidth(double minColumnWidth)
    {
        MinColumnWidth = minColumnWidth;
        return this;
    }

    /// <summary>
    /// 设置最大列宽。
    /// </summary>
    /// <param name="maxColumnWidth">最大列宽，单位为字符宽度。</param>
    /// <returns>返回当前 <see cref="PurStyle"/> 实例，支持链式调用。</returns>
    public PurStyle SetMaxColumnWidth(double maxColumnWidth)
    {
        MaxColumnWidth = maxColumnWidth;
        return this;
    }

    #endregion Fluent API

    #region 预设样式

    /// <summary>
    /// 默认样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#FFFFFF（白色）
    /// </para>
    /// <para>
    /// 背景色：#004586（蓝色）
    /// </para>
    /// </remarks>
    public static readonly PurStyle Default = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0xFF, 0xFF, 0xFF)))
            .With(new XlsxFill(Color.FromArgb(0x00, 0x45, 0x86)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 明亮清新蓝样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#FFFFFF（白色）
    /// </para>
    /// <para>
    /// 背景色：#00BFFF（天空蓝）
    /// </para>
    /// </remarks>
    public static readonly PurStyle BrightFresh = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0xFF, 0xFF, 0xFF)))
            .With(new XlsxFill(Color.FromArgb(0x00, 0xBF, 0xFF)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 优雅单色样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#FFFFFF（白色）
    /// </para>
    /// <para>
    /// 背景色：#A9A9A9（深灰色）
    /// </para>
    /// </remarks>
    public static readonly PurStyle ElegantMonochrome = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0xFF, 0xFF, 0xFF)))
            .With(new XlsxFill(Color.FromArgb(0xA9, 0xA9, 0xA9)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 大地色调样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#FFFFFF（白色）
    /// </para>
    /// <para>
    /// 背景色：#808080（灰色）
    /// </para>
    /// </remarks>
    public static readonly PurStyle EarthTones = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0xFF, 0xFF, 0xFF)))
            .With(new XlsxFill(Color.FromArgb(0x80, 0x80, 0x80)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 暖色调样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#FFFFFF（白色）
    /// </para>
    /// <para>
    /// 背景色：#FF0000（红色）
    /// </para>
    /// </remarks>
    public static readonly PurStyle WarmTones = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0xFF, 0xFF, 0xFF)))
            .With(new XlsxFill(Color.FromArgb(0xFF, 0x00, 0x00)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 海洋蓝样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#FFFFFF（白色）
    /// </para>
    /// <para>
    /// 背景色：#191970（午夜蓝）
    /// </para>
    /// </remarks>
    public static readonly PurStyle OceanBlue = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0xFF, 0xFF, 0xFF)))
            .With(new XlsxFill(Color.FromArgb(0x19, 0x19, 0x70)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 复古怀旧样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#808080（灰色）
    /// </para>
    /// <para>
    /// 背景色：#FFC0CB（粉色）
    /// </para>
    /// </remarks>
    public static readonly PurStyle VintageNostalgia = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0x80, 0x80, 0x80)))
            .With(new XlsxFill(Color.FromArgb(0xFF, 0xC0, 0xCB)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 极简黑白样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#808080（灰色）
    /// </para>
    /// <para>
    /// 背景色：#FFFFFF（白色）
    /// </para>
    /// </remarks>
    public static readonly PurStyle MinimalistBw = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0x80, 0x80, 0x80)))
            .With(new XlsxFill(Color.FromArgb(0xFF, 0xFF, 0xFF)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 活力能量样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#FFFFFF（白色）
    /// </para>
    /// <para>
    /// 背景色：#FFA500（橙色）
    /// </para>
    /// </remarks>
    public static readonly PurStyle VibrantEnergy = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0xFF, 0xFF, 0xFF)))
            .With(new XlsxFill(Color.FromArgb(0xFF, 0xA5, 0x00)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 复古时尚样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#FFFFFF（白色）
    /// </para>
    /// <para>
    /// 背景色：#DA70D6（兰花紫）
    /// </para>
    /// </remarks>
    public static readonly PurStyle RetroChic = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0xFF, 0xFF, 0xFF)))
            .With(new XlsxFill(Color.FromArgb(0xDA, 0x70, 0xD6)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 温馨秋日样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#FFFFFF（白色）
    /// </para>
    /// <para>
    /// 背景色：#CD853F（秘鲁棕）
    /// </para>
    /// </remarks>
    public static readonly PurStyle CozyAutumn = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0xFF, 0xFF, 0xFF)))
            .With(new XlsxFill(Color.FromArgb(0xCD, 0x85, 0x3F)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 宁静自然样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#FFFFFF（白色）
    /// </para>
    /// <para>
    /// 背景色：#2E8B57（海绿色）
    /// </para>
    /// </remarks>
    public static readonly PurStyle SereneNature = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0xFF, 0xFF, 0xFF)))
            .With(new XlsxFill(Color.FromArgb(0x2E, 0x8B, 0x57)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 午夜魔幻样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#FFFFFF（白色）
    /// </para>
    /// <para>
    /// 背景色：#000080（海军蓝）
    /// </para>
    /// </remarks>
    public static readonly PurStyle MidnightMagic = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0xFF, 0xFF, 0xFF)))
            .With(new XlsxFill(Color.FromArgb(0x00, 0x00, 0x80)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    /// <summary>
    /// 暖阳阳光样式。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 文本色：#808080（灰色）
    /// </para>
    /// <para>
    /// 背景色：#FFFF00（黄色）
    /// </para>
    /// </remarks>
    public static readonly PurStyle SunnyDay = new()
    {
        HeaderStyle = XlsxStyle.Default
            .With(XlsxFont.Default.With(Color.FromArgb(0x80, 0x80, 0x80)))
            .With(new XlsxFill(Color.FromArgb(0xFF, 0xFF, 0x00)))
            .With(XlsxStyle.Default.Border)
            .With(XlsxStyle.Default.NumberFormat)
            .With(new XlsxAlignment(XlsxAlignment.Horizontal.Left, XlsxAlignment.Vertical.Center)),
        ContentStyle = XlsxStyle.Default
    };

    #endregion 预设样式
}