namespace PurcellLibs.Utilities;

/// <summary>
/// 文件工具类
/// </summary>
public static class FileUtils
{
    /// <summary>
    /// 是否文本文件
    /// </summary>
    public static bool IsTextFile(Stream stream, int sampleSize = 4196)
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
}