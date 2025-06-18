using LargeXlsx;

namespace PurcellLibs;

/// <summary>
/// 表格样式
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class PurStyle : Attribute
{
    /// <summary>
    /// 表头样式
    /// </summary>
    public XlsxStyle HeaderStyle { get; set; } = XlsxStyle.Default;

    /// <summary>
    /// 内容样式
    /// </summary>
    public XlsxStyle ContentStyle { get; set; } = XlsxStyle.Default;

    /// <summary>
    /// 最小列宽，默认值 10
    /// </summary>
    public double MinColumnWidth { get; set; } = 10d;

    /// <summary>
    /// 最大列宽，默认值 20
    /// </summary>
    public double MaxColumnWidth { get; set; } = 20d;

    /// <summary>
    /// 表头行高，默认值 18
    /// </summary>
    public double HeaderLineHeight { get; set; } = 18d;

    /// <summary>
    /// 内容行高，默认值 18
    /// </summary>
    public double ContentLineHeight { get; set; } = 18d;

    #region Fluent 方法链

    /// <summary>设置表头样式</summary>
    public PurStyle SetHeaderStyle(XlsxStyle headerStyle)
    {
        HeaderStyle = headerStyle;
        return this;
    }

    /// <summary>设置表头样式</summary>
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

    /// <summary>设置表头样式</summary>
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

    /// <summary>设置内容样式</summary>
    public PurStyle SetContentStyle(XlsxStyle contentStyle)
    {
        ContentStyle = contentStyle;
        return this;
    }

    /// <summary>设置内容样式</summary>
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

    /// <summary>设置内容样式</summary>
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

    /// <summary>设置表头行高</summary>
    public PurStyle SetHeaderLineHeight(double headerLineHeight)
    {
        HeaderLineHeight = headerLineHeight;
        return this;
    }

    /// <summary>设置内容行高</summary>
    public PurStyle SetContentLineHeight(double contentLineHeight)
    {
        ContentLineHeight = contentLineHeight;
        return this;
    }

    /// <summary>设置最小列宽</summary>
    public PurStyle SetMinColumnWidth(double minColumnWidth)
    {
        MinColumnWidth = minColumnWidth;
        return this;
    }

    /// <summary>设置最大列宽</summary>
    public PurStyle SetMaxColumnWidth(double maxColumnWidth)
    {
        MaxColumnWidth = maxColumnWidth;
        return this;
    }

    #endregion Fluent 方法链

    #region Preset 预设样式

    /// <summary>
    /// 默认 Default<br /><br />
    /// 🎨 文本色: #FFFFFF White<br />
    /// 🎨 背景色: #004586 Blue
    /// </summary>
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
    /// 明亮清新蓝 BrightFresh<br /><br />
    /// 🎨 文本色: #FFFFFF White<br />
    /// 🎨 背景色: #00BFFF Sky Blue
    /// </summary>
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
    /// 优雅单色 ElegantMonochrome<br /><br />
    /// 🎨 文本色: #FFFFFF White<br />
    /// 🎨 背景色: #A9A9A9 Dark Gray
    /// </summary>
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
    /// 大地色调 EarthTones<br /><br />
    /// 🎨 文本色: #FFFFFF White<br />
    /// 🎨 背景色: #808080 Gray
    /// </summary>
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
    /// 暖色调 WarmTones<br /><br />
    /// 🎨 文本色: #FFFFFF White<br />
    /// 🎨 背景色: #FF0000 Red
    /// </summary>
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
    /// 海洋蓝 OceanBlue<br /><br />
    /// 🎨 文本色: #FFFFFF White<br />
    /// 🎨 背景色: #191970 Midnight Blue
    /// </summary>
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
    /// 复古怀旧 VintageNostalgia<br /><br />
    /// 🎨 文本色: #808080 Gray<br />
    /// 🎨 背景色: #FFC0CB Pink
    /// </summary>
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
    /// 极简黑白 MinimalistBW<br /><br />
    /// 🎨 文本色: #808080 Gray<br />
    /// 🎨 背景色: #FFFFFF White
    /// </summary>
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
    /// 活力能量 VibrantEnergy
    /// 🎨 文本色: #FFFFFF White<br />
    /// 🎨 背景色: #FFA500 Orange
    /// </summary>
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
    /// 复古时尚 RetroChic<br /><br />
    /// 🎨 文本色: #FFFFFF White<br />
    /// 🎨 背景色: #DA70D6 Orchid
    /// </summary>
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
    /// 温馨秋日 CozyAutumn<br /><br />
    /// 🎨 文本色: #FFFFFF White<br />
    /// 🎨 背景色: #CD853F Peru
    /// </summary>
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
    /// 宁静自然 SereneNature<br /><br />
    /// 🎨 文本色: #FFFFFF White<br />
    /// 🎨 背景色: #2E8B57 Sea Green
    /// </summary>
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
    /// 午夜魔幻 MidnightMagic<br /><br />
    /// 🎨 文本色: #FFFFFF White<br />
    /// 🎨 背景色: #000080 Navy
    /// </summary>
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
    /// 暖阳阳光 SunnyDay<br /><br />
    /// 🎨 文本色: #808080 Gray<br />
    /// 🎨 背景色: #FFFF00 Yellow
    /// </summary>
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

    #endregion Presets 预设样式
}