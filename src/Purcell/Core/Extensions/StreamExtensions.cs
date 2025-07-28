namespace PurcellLibs.Extensions;

/// <summary>
/// 流扩展方法，提供文本文件检测和编码识别功能。
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// 检测流是否为文本文件。
    /// </summary>
    /// <param name="stream">要检测的流。</param>
    /// <param name="sampleSize">用于检测的样本字节数，默认为 4196。</param>
    /// <returns>如果流是文本文件则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    public static bool IsTextFile(this Stream stream, int sampleSize = 4196)
    {
        long originalPosition = stream.Position;
        try
        {
            stream.Position = 0;

            // 如果文件为空，按约定视为文本文件
            if (stream.Length == 0)
                return true;

            // 读取样本数据
            byte[] buffer = new byte[Math.Min(sampleSize, stream.Length)];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            // 检查是否包含空字节（通常表示二进制文件）
            for (int i = 0; i < bytesRead; i++)
            {
                // 大多数文本文件不会包含NULL字节
                if (buffer[i] == 0)
                    return false;
            }

            // 检查是否包含控制字符（ASCII 0-31，除了常见的如TAB, CR, LF等）
            for (int i = 0; i < bytesRead; i++)
            {
                byte b = buffer[i];
                if (b < 32 && b != 9 && b != 10 && b != 13)
                    return false;
            }

            return true;
        }
        finally
        {
            // 恢复原始位置
            stream.Position = originalPosition;
        }
    }

    /// <summary>
    /// 检测文本流的编码格式。
    /// </summary>
    /// <param name="stream">要检测的流。</param>
    /// <returns>检测到的编码；如果无法确定则返回 <see langword="null"/>。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentException">流不支持读取或查找操作。</exception>
    public static Encoding? DetectEncoding(this Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        if (!stream.CanRead || !stream.CanSeek)
            throw new ArgumentException("流必须支持读取和查找操作", nameof(stream));

        // 保存原始位置
        long originalPosition = stream.Position;

        try
        {
            // 重置到流开始
            stream.Position = 0;

            // 首先检查 BOM
            Encoding? encodingFromBom = DetectEncodingByBom(stream);
            if (encodingFromBom != null)
                return encodingFromBom;

            // 无 BOM，使用统计方法检测
            return DetectEncodingWithoutBom(stream);
        }
        finally
        {
            // 恢复原始位置
            stream.Position = originalPosition;
        }
    }

    #region Encoding Detection Methods

    /// <summary>
    /// UTF-8 BOM 标记。
    /// </summary>
    private static readonly byte[] Utf8Bom = [0xEF, 0xBB, 0xBF];
    
    /// <summary>
    /// UTF-16 LE BOM 标记。
    /// </summary>
    private static readonly byte[] Utf16LeBom = [0xFF, 0xFE];
    
    /// <summary>
    /// UTF-16 BE BOM 标记。
    /// </summary>
    private static readonly byte[] Utf16BeBom = [0xFE, 0xFF];
    
    /// <summary>
    /// UTF-32 LE BOM 标记。
    /// </summary>
    private static readonly byte[] Utf32LeBom = [0xFF, 0xFE, 0x00, 0x00];
    
    /// <summary>
    /// UTF-32 BE BOM 标记。
    /// </summary>
    private static readonly byte[] Utf32BeBom = [0x00, 0x00, 0xFE, 0xFF];
    
    /// <summary>
    /// 用于编码检测的采样大小。
    /// </summary>
    private const int SampleSize = 4096;
    
    /// <summary>
    /// 支持的编码列表，用于编码检测。
    /// </summary>
    private static readonly Encoding[] SupportedEncodings;

    static StreamExtensions()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        SupportedEncodings =
        [
            Encoding.UTF8,
            Encoding.Unicode, // UTF-16 LE
            Encoding.BigEndianUnicode, // UTF-16 BE
            Encoding.UTF32, // UTF-32 LE
            Encoding.GetEncoding("UTF-32BE"), // UTF-32 BE
            Encoding.GetEncoding("GB18030"), // 中文环境
            Encoding.GetEncoding("Big5"), // 繁体中文
            Encoding.GetEncoding("shift_jis"), // 日文环境
            Encoding.GetEncoding("euc-kr"), // 韩文环境
            Encoding.GetEncoding("windows-1251"), // 俄文环境
            Encoding.GetEncoding("windows-1252"), // 西欧语言
            Encoding.ASCII
        ];
    }

    /// <summary>
    /// 通过 BOM (Byte Order Mark) 检测编码。
    /// </summary>
    /// <param name="stream">要检测的流。</param>
    /// <returns>检测到的编码；如果没有 BOM 则返回 <see langword="null"/>。</returns>
    private static Encoding? DetectEncodingByBom(Stream stream)
    {
        // 读取足够的字节来检测所有可能的 BOM
        byte[] bom = new byte[4];
        int bytesRead = stream.Read(bom, 0, 4);

        // 重置流位置
        stream.Position = 0;

        if (bytesRead >= 4)
        {
            // 检查 UTF-32 (BE/LE)
            if (BytesEqual(bom, Utf32LeBom))
                return Encoding.UTF32;
            if (BytesEqual(bom, Utf32BeBom))
                return Encoding.GetEncoding("UTF-32BE");
        }

        if (bytesRead >= 3)
        {
            // 检查 UTF-8
            if (BytesEqual(bom.Take(3).ToArray(), Utf8Bom))
                return Encoding.UTF8;
        }

        if (bytesRead >= 2)
        {
            // 检查 UTF-16 (LE/BE)
            if (BytesEqual(bom.Take(2).ToArray(), Utf16LeBom))
                return Encoding.Unicode;
            if (BytesEqual(bom.Take(2).ToArray(), Utf16BeBom))
                return Encoding.BigEndianUnicode;
        }

        // 没有找到 BOM
        return null;
    }

    /// <summary>
    /// 比较两个字节数组是否相等。
    /// </summary>
    /// <param name="a">第一个字节数组。</param>
    /// <param name="b">第二个字节数组。</param>
    /// <returns>如果两个数组相等则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    private static bool BytesEqual(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            return false;

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
                return false;
        }

        return true;
    }

    /// <summary>
    /// 对没有 BOM 的文本进行编码检测。
    /// </summary>
    /// <param name="stream">要检测的流。</param>
    /// <returns>最可能的编码；如果无法确定则返回 <see langword="null"/>。</returns>
    private static Encoding? DetectEncodingWithoutBom(Stream stream)
    {
        // 读取样本数据
        byte[] sample = ReadSample(stream, SampleSize);
        if (sample.Length == 0)
            return null;

        // 首先检测是否为 UTF-8（无 BOM）
        if (IsValidUtf8(sample))
            return Encoding.UTF8;

        // 检测是否为 UTF-16/UTF-32
        if (IsLikelyUtf16(sample))
        {
            // 通过奇偶字节模式判断 LE 还是 BE
            return HasMoreNullEvenBytes(sample) ? Encoding.BigEndianUnicode : Encoding.Unicode;
        }

        if (IsLikelyUtf32(sample))
        {
            // UTF-32 检测逻辑
            return HasUtf32LePattern(sample) ? Encoding.UTF32 : Encoding.GetEncoding("UTF-32BE");
        }

        // 对其他编码进行概率分析
        return DetectEncodingByFrequencyAnalysis(sample);
    }

    /// <summary>
    /// 读取流的样本数据。
    /// </summary>
    /// <param name="stream">要读取的流。</param>
    /// <param name="maxBytes">最大读取字节数。</param>
    /// <returns>读取的样本字节数组。</returns>
    private static byte[] ReadSample(Stream stream, int maxBytes)
    {
        stream.Position = 0;
        byte[] buffer = new byte[maxBytes];
        int bytesRead = stream.Read(buffer, 0, maxBytes);

        if (bytesRead < maxBytes)
        {
            Array.Resize(ref buffer, bytesRead);
        }

        return buffer;
    }

    /// <summary>
    /// 检测数据是否为有效的 UTF-8 编码。
    /// </summary>
    /// <param name="data">要检测的字节数组。</param>
    /// <returns>如果数据符合 UTF-8 编码规范则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    private static bool IsValidUtf8(byte[] data)
    {
        int i = 0;
        while (i < data.Length)
        {
            if (data[i] <= 0x7F) // ASCII 范围
            {
                i++;
                continue;
            }

            // 多字节序列开始
            if ((data[i] & 0xE0) == 0xC0) // 2 字节序列
            {
                if (i + 1 >= data.Length || (data[i + 1] & 0xC0) != 0x80)
                    return false;
                i += 2;
            }
            else if ((data[i] & 0xF0) == 0xE0) // 3 字节序列
            {
                if (i + 2 >= data.Length || (data[i + 1] & 0xC0) != 0x80 || (data[i + 2] & 0xC0) != 0x80)
                    return false;
                i += 3;
            }
            else if ((data[i] & 0xF8) == 0xF0) // 4 字节序列
            {
                if (i + 3 >= data.Length || (data[i + 1] & 0xC0) != 0x80 ||
                    (data[i + 2] & 0xC0) != 0x80 || (data[i + 3] & 0xC0) != 0x80)
                    return false;
                i += 4;
            }
            else
            {
                return false; // 无效的 UTF-8 序列
            }
        }

        return true;
    }

    /// <summary>
    /// 检测数据是否可能为 UTF-16 编码。
    /// </summary>
    /// <param name="data">要检测的字节数组。</param>
    /// <returns>如果数据可能是 UTF-16 编码则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    private static bool IsLikelyUtf16(byte[] data)
    {
        if (data.Length < 4)
            return false;

        // 检查是否有规律的空字节模式（UTF-16 的特征）
        int nullBytes = 0;
        for (int i = 0; i < data.Length; i += 2)
        {
            if (i + 1 < data.Length && (data[i] == 0 || data[i + 1] == 0))
                nullBytes++;
        }

        // 如果超过 30% 的双字节中有空字节，可能是 UTF-16
        return nullBytes / (data.Length / 2.0) > 0.3;
    }

    /// <summary>
    /// 检测是否有更多的偶数位置为空字节，用于判断 UTF-16 BE/LE。
    /// </summary>
    /// <param name="data">要检测的字节数组。</param>
    /// <returns>如果偶数位置的空字节更多则返回 <see langword="true"/>（表示 BE）；否则返回 <see langword="false"/>（表示 LE）。</returns>
    private static bool HasMoreNullEvenBytes(byte[] data)
    {
        int evenNulls = 0, oddNulls = 0;
        for (int i = 0; i < data.Length - 1; i += 2)
        {
            if (data[i] == 0) evenNulls++;
            if (data[i + 1] == 0) oddNulls++;
        }

        return evenNulls > oddNulls;
    }

    /// <summary>
    /// 检测数据是否可能为 UTF-32 编码。
    /// </summary>
    /// <param name="data">要检测的字节数组。</param>
    /// <returns>如果数据可能是 UTF-32 编码则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    private static bool IsLikelyUtf32(byte[] data)
    {
        if (data.Length < 8)
            return false;

        // 检查是否有规律的空字节模式（UTF-32 的特征）
        int validPatterns = 0;
        for (int i = 0; i < data.Length - 3; i += 4)
        {
            // UTF-32 通常有连续的空字节
            if ((data[i] == 0 && data[i + 1] == 0) || (data[i + 2] == 0 && data[i + 3] == 0))
                validPatterns++;
        }

        // 如果超过 40% 的四个字节组有特定模式，可能是 UTF-32
        return validPatterns / (data.Length / 4.0) > 0.4;
    }

    /// <summary>
    /// 检测是否符合 UTF-32 LE 的模式。
    /// </summary>
    /// <param name="data">要检测的字节数组。</param>
    /// <returns>如果符合 UTF-32 LE 模式则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    private static bool HasUtf32LePattern(byte[] data)
    {
        int lePattern = 0, bePattern = 0;
        for (int i = 0; i < data.Length - 3; i += 4)
        {
            // UTF-32 LE 模式: 有效字节在低位
            if (data[i] != 0 && data[i + 1] == 0 && data[i + 2] == 0 && data[i + 3] == 0)
                lePattern++;
            // UTF-32 BE 模式: 有效字节在高位
            if (data[i] == 0 && data[i + 1] == 0 && data[i + 2] == 0 && data[i + 3] != 0)
                bePattern++;
        }

        return lePattern > bePattern;
    }

    /// <summary>
    /// 通过频率分析检测可能的编码。
    /// </summary>
    /// <param name="sample">要分析的样本字节数组。</param>
    /// <returns>最可能的编码；如果无法确定则返回 <see langword="null"/>。</returns>
    private static Encoding? DetectEncodingByFrequencyAnalysis(byte[] sample)
    {
        Dictionary<Encoding, double> confidenceScores = new();

        foreach (Encoding encoding in SupportedEncodings)
        {
            try
            {
                // 尝试解码然后重新编码
                string decoded = encoding.GetString(sample);
                byte[] reEncoded = encoding.GetBytes(decoded);

                // 计算重新编码后与原样本的匹配度
                double matchScore = CalculateMatchScore(sample, reEncoded);

                // 应用特定编码的启发式规则
                matchScore = ApplyEncodingHeuristics(encoding, sample, matchScore);

                confidenceScores[encoding] = matchScore;
            }
            catch
            {
                // 解码失败，跳过此编码
                confidenceScores[encoding] = 0;
            }
        }

        // 获取置信度最高的编码
        KeyValuePair<Encoding, double> bestMatch = confidenceScores.OrderByDescending(kv => kv.Value).FirstOrDefault();

        // 如果最高置信度低于阈值，返回 null
        return bestMatch.Value > 0.6 ? bestMatch.Key : null;
    }

    /// <summary>
    /// 计算两个字节数组的匹配度。
    /// </summary>
    /// <param name="original">原始字节数组。</param>
    /// <param name="reEncoded">重新编码后的字节数组。</param>
    /// <returns>匹配度（介于 0 到 1 之间）。</returns>
    private static double CalculateMatchScore(byte[] original, byte[] reEncoded)
    {
        int matchLength = Math.Min(original.Length, reEncoded.Length);
        int matches = 0;

        for (int i = 0; i < matchLength; i++)
        {
            if (original[i] == reEncoded[i])
                matches++;
        }

        // 计算匹配百分比
        return (double)matches / matchLength;
    }

    /// <summary>
    /// 应用特定编码的启发式规则来调整匹配分数。
    /// </summary>
    /// <param name="encoding">要调整的编码。</param>
    /// <param name="sample">样本字节数组。</param>
    /// <param name="baseScore">基础匹配分数。</param>
    /// <returns>调整后的匹配分数。</returns>
    private static double ApplyEncodingHeuristics(Encoding encoding, byte[] sample, double baseScore)
    {
        string encodingName = encoding.WebName.ToLowerInvariant();
        double adjustedScore = baseScore;

        // 中文编码特征检测 (GB18030/Big5)
        if (encodingName == "gb18030" || encodingName == "big5")
        {
            // 检查是否包含中文编码的特征字节
            if (HasChineseEncodingCharacteristics(sample, encodingName))
            {
                adjustedScore += 0.1; // 提高中文编码的权重
            }
        }
        // 日文编码特征检测 (Shift-JIS)
        else if (encodingName == "shift_jis")
        {
            if (HasJapaneseEncodingCharacteristics(sample))
            {
                adjustedScore += 0.1;
            }
        }
        // 韩文编码特征检测 (EUC-KR)
        else if (encodingName == "euc-kr")
        {
            if (HasKoreanEncodingCharacteristics(sample))
            {
                adjustedScore += 0.1;
            }
        }
        // 西里尔字母编码特征检测 (Windows-1251)
        else if (encodingName == "windows-1251")
        {
            if (HasCyrillicEncodingCharacteristics(sample))
            {
                adjustedScore += 0.1;
            }
        }
        // 西欧语言编码特征检测 (Windows-1252)
        else if (encodingName == "windows-1252")
        {
            if (HasWesternEncodingCharacteristics(sample))
            {
                adjustedScore += 0.1;
            }
        }
        // UTF-8 无 BOM 的特征检测
        else if (encodingName == "utf-8")
        {
            // UTF-8 的多字节序列特征
            if (HasUtf8MultibyteSequences(sample))
            {
                adjustedScore += 0.15;
            }
        }

        return adjustedScore;
    }

    /// <summary>
    /// 检测是否包含中文编码的特征字节。
    /// </summary>
    /// <param name="sample">要检测的样本字节数组。</param>
    /// <param name="encodingName">编码名称（gb18030 或 big5）。</param>
    /// <returns>如果包含中文编码特征则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    private static bool HasChineseEncodingCharacteristics(byte[] sample, string encodingName)
    {
        int count = 0;

        if (encodingName == "gb18030")
        {
            // GB18030 特征: 高字节通常在 0x81-0xFE 范围，低字节在 0x40-0xFE
            for (int i = 0; i < sample.Length - 1; i++)
            {
                if (sample[i] >= 0x81 && sample[i] <= 0xFE &&
                    sample[i + 1] >= 0x40 && sample[i + 1] <= 0xFE)
                {
                    count++;
                }
            }
        }
        else if (encodingName == "big5")
        {
            // Big5 特征: 高字节通常在 0xA1-0xF9，低字节在 0x40-0x7E 或 0xA1-0xFE
            for (int i = 0; i < sample.Length - 1; i++)
            {
                if (sample[i] >= 0xA1 && sample[i] <= 0xF9 &&
                    ((sample[i + 1] >= 0x40 && sample[i + 1] <= 0x7E) ||
                     (sample[i + 1] >= 0xA1 && sample[i + 1] <= 0xFE)))
                {
                    count++;
                }
            }
        }

        // 如果特征字节对超过样本的 5%，认为可能是该编码
        return count > sample.Length * 0.05;
    }

    /// <summary>
    /// 检测是否包含日文编码的特征字节。
    /// </summary>
    /// <param name="sample">要检测的样本字节数组。</param>
    /// <returns>如果包含日文编码特征则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    private static bool HasJapaneseEncodingCharacteristics(byte[] sample)
    {
        int count = 0;

        // Shift-JIS 特征: 高字节在 0x81-0x9F 或 0xE0-0xEF，低字节在 0x40-0xFC
        for (int i = 0; i < sample.Length - 1; i++)
        {
            if (((sample[i] >= 0x81 && sample[i] <= 0x9F) || (sample[i] >= 0xE0 && sample[i] <= 0xEF)) &&
                sample[i + 1] >= 0x40 && sample[i + 1] <= 0xFC)
            {
                count++;
            }
        }

        return count > sample.Length * 0.05;
    }

    /// <summary>
    /// 检测是否包含韩文编码的特征字节。
    /// </summary>
    /// <param name="sample">要检测的样本字节数组。</param>
    /// <returns>如果包含韩文编码特征则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    private static bool HasKoreanEncodingCharacteristics(byte[] sample)
    {
        int count = 0;

        // EUC-KR 特征: 高字节在 0xA1-0xFE，低字节在 0xA1-0xFE
        for (int i = 0; i < sample.Length - 1; i++)
        {
            if (sample[i] >= 0xA1 && sample[i] <= 0xFE &&
                sample[i + 1] >= 0xA1 && sample[i + 1] <= 0xFE)
            {
                count++;
            }
        }

        return count > sample.Length * 0.05;
    }

    /// <summary>
    /// 检测是否包含西里尔字母编码的特征字节。
    /// </summary>
    /// <param name="sample">要检测的样本字节数组。</param>
    /// <returns>如果包含西里尔字母编码特征则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    private static bool HasCyrillicEncodingCharacteristics(byte[] sample)
    {
        int cyrillicChars = 0;

        // Windows-1251 西里尔字母范围: 0xC0-0xFF
        foreach (byte b in sample)
        {
            if (b >= 0xC0)
            {
                cyrillicChars++;
            }
        }

        // 如果西里尔字母字符超过样本的 10%，可能是 Windows-1251
        return cyrillicChars > sample.Length * 0.1;
    }

    /// <summary>
    /// 检测是否包含西欧语言编码的特征字节。
    /// </summary>
    /// <param name="sample">要检测的样本字节数组。</param>
    /// <returns>如果包含西欧语言编码特征则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    private static bool HasWesternEncodingCharacteristics(byte[] sample)
    {
        int westernChars = 0;

        // Windows-1252 西欧语言特征字节: 0x80-0x9F
        foreach (byte b in sample)
        {
            if (b is >= 0x80 and <= 0x9F)
            {
                westernChars++;
            }
        }

        // 检查常见西欧重音字符
        int accentedChars = 0;
        foreach (byte b in sample)
        {
            if (b is >= 0xC0 and <= 0xD6 or >= 0xD8 and <= 0xF6 or >= 0xF8)
            {
                accentedChars++;
            }
        }

        // 结合两种特征进行判断
        return westernChars > sample.Length * 0.02 || accentedChars > sample.Length * 0.05;
    }

    /// <summary>
    /// 检测是否包含 UTF-8 多字节序列。
    /// </summary>
    /// <param name="sample">要检测的样本字节数组。</param>
    /// <returns>如果包含足够的 UTF-8 多字节序列则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    private static bool HasUtf8MultibyteSequences(byte[] sample)
    {
        int multibyteSequences = 0;

        for (int i = 0; i < sample.Length; i++)
        {
            // 检测 UTF-8 多字节序列的起始字节
            if ((sample[i] & 0xE0) == 0xC0) // 2 字节序列
            {
                if (i + 1 < sample.Length && (sample[i + 1] & 0xC0) == 0x80)
                {
                    multibyteSequences++;
                    i += 1;
                }
            }
            else if ((sample[i] & 0xF0) == 0xE0) // 3 字节序列
            {
                if (i + 2 < sample.Length &&
                    (sample[i + 1] & 0xC0) == 0x80 &&
                    (sample[i + 2] & 0xC0) == 0x80)
                {
                    multibyteSequences++;
                    i += 2;
                }
            }
            else if ((sample[i] & 0xF8) == 0xF0) // 4 字节序列
            {
                if (i + 3 < sample.Length &&
                    (sample[i + 1] & 0xC0) == 0x80 &&
                    (sample[i + 2] & 0xC0) == 0x80 &&
                    (sample[i + 3] & 0xC0) == 0x80)
                {
                    multibyteSequences++;
                    i += 3;
                }
            }
        }

        // 如果有足够的多字节序列，可能是 UTF-8
        return multibyteSequences > 5;
    }

    #endregion Encoding Detection Methods
}