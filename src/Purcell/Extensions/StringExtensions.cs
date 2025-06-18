// ReSharper disable MergeIntoPattern

namespace PurcellLibs.Extensions;

/// <summary>
/// String 扩展方法类
/// </summary>
internal static class StringExtensions
{
    public static string DefaultIfEmpty(this string? str, string defaultValue)
    {
        return string.IsNullOrEmpty(str) ? defaultValue : str;
    }

    /// <summary>
    /// 根据指定策略处理字符串中的空白字符
    /// </summary>
    /// <param name="value">要处理的字符串</param>
    /// <param name="mode">空白字符处理策略</param>
    /// <returns>处理后的字符串</returns>
    public static string ProcessWhiteSpace(this string? value, WhiteSpaceMode mode)
    {
        // 快速路径：空值检查
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        // 如果没有任何策略需要应用，直接返回原值
        if (mode != WhiteSpaceMode.Trim && mode != WhiteSpaceMode.RemoveAllSpaces)
        {
            return value;
        }

        if (mode == WhiteSpaceMode.Trim) return value.Trim();

        // 使用 ReadOnlySpan<char> 优化字符串操作
        ReadOnlySpan<char> span = value.AsSpan();

        // 计算非空白字符的数量
        int nonWhiteSpaceCount = 0;
        foreach (char c in span)
        {
            if (!char.IsWhiteSpace(c))
            {
                nonWhiteSpaceCount++;
            }
        }

        // 创建精确大小的结果字符串
        return string.Create(nonWhiteSpaceCount, value, (chars, str) =>
        {
            int writeIndex = 0;
            foreach (char c in str)
            {
                if (!char.IsWhiteSpace(c))
                {
                    chars[writeIndex++] = c;
                }
            }
        });
    }

    /// <summary>
    /// 测量文本宽度，宽字符计为2，窄字符计为1
    /// </summary>
    /// <param name="value">要测量的文本</param>
    /// <param name="adjustment">宽度调整值，默认为2</param>
    /// <returns>文本的计算宽度</returns>
    public static double MeasureText(this string? value, double adjustment = 2)
    {
        if (string.IsNullOrEmpty(value)) return 0;

        double length = 0;

        foreach (char c in value)
        {
            if (IsWideCharacter(c))
                length += 2;
            else
                length += 1;
        }

        return length + adjustment; // 使用参数化的补充长度后返回

        // 判断一个字符是否是宽字符（显示宽度为普通字符的两倍）
        bool IsWideCharacter(char ch)
        {
            int codePoint = ch;

            // 判断字符是否在常见的宽字符范围内
            return codePoint == 0x2329 || codePoint == 0x232A || // Angle brackets
                   (codePoint >= 0x1100 && codePoint <= 0x115F) || // Hangul Jamo
                   (codePoint >= 0x2E80 && codePoint <= 0x2FFB) || // CJK Radicals Supplement and Kangxi Radicals
                   (codePoint >= 0x3000 && codePoint <= 0x303E) || // CJK Symbols and Punctuation
                   (codePoint >= 0x3040 && codePoint <= 0x309F) || // Hiragana
                   (codePoint >= 0x30A0 && codePoint <= 0x30FF) || // Katakana
                   (codePoint >= 0x3100 && codePoint <= 0x312F) || // Bopomofo
                   (codePoint >= 0x3130 && codePoint <= 0x318F) || // Hangul Compatibility Jamo
                   (codePoint >= 0x3190 && codePoint <= 0x31EF) || // Kanbun, Bopomofo Extended
                   (codePoint >= 0x3200 && codePoint <= 0x32FF) || // Enclosed CJK Letters and Months
                   (codePoint >= 0x3300 && codePoint <= 0x33FF) || // CJK Compatibility
                   (codePoint >= 0x3400 && codePoint <= 0x4DBF) || // CJK Unified Ideographs Extension A
                   (codePoint >= 0x4E00 && codePoint <= 0x9FFF) || // CJK Unified Ideographs
                   (codePoint >= 0xA000 && codePoint <= 0xA4CF) || // Yi Syllables
                   (codePoint >= 0xAC00 && codePoint <= 0xD7A3) || // Hangul Syllables
                   (codePoint >= 0xF900 && codePoint <= 0xFAFF) || // CJK Compatibility Ideographs
                   (codePoint >= 0xFE10 && codePoint <= 0xFE19) || // Vertical forms
                   (codePoint >= 0xFE30 && codePoint <= 0xFE6F) || // CJK Compatibility Forms
                   (codePoint >= 0xFF00 && codePoint <= 0xFF60) || // Fullwidth ASCII variants
                   (codePoint >= 0xFFE0 && codePoint <= 0xFFE6) || // Fullwidth symbol variants
                   (codePoint >= 0x1F300 && codePoint <= 0x1F64F) || // Emoticons
                   (codePoint >= 0x1F900 && codePoint <= 0x1F9FF) || // Supplemental Symbols and Pictographs
                   (codePoint >= 0x20000 && codePoint <= 0x2FFFD) || // CJK Unified Ideographs Extension B-C-D-E-F
                   (codePoint >= 0x30000 && codePoint <= 0x3FFFD); // CJK Unified Ideographs Extension G
        }
    }
}