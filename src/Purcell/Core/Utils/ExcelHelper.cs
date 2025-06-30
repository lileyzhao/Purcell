namespace PurcellLibs.Utils;

/// <summary>
/// 提供 Excel 列索引字母和列索引转换的辅助方法。
/// </summary>
public static class ExcelHelper
{
    /// <summary>
    /// 将 Excel 列索引字母转换为基于零的列索引。
    /// </summary>
    /// <param name="columnLetter">Excel 列索引字母，如 "A"、"B"、"AA" 等。</param>
    /// <returns>对应的基于零的列索引。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="columnLetter"/> 为 <see langword="null"/>、空字符串或包含无效字符时抛出。</exception>
    public static int ToColumnIndex(string columnLetter)
    {
        if (string.IsNullOrWhiteSpace(columnLetter))
            throw new ArgumentException("列名不能为空", nameof(columnLetter));

        int result = 0;
        foreach (char ct in columnLetter)
        {
            result = result * 26 + (char.ToUpperInvariant(ct) - 'A') + 1;
        }

        return result - 1;
    }

    /// <summary>
    /// 将基于零的列索引转换为 Excel 列索引字母。
    /// </summary>
    /// <param name="columnIndex">基于零的列索引。</param>
    /// <returns>对应的 Excel 列索引字母，如 "A"、"B"、"AA" 等。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="columnIndex"/> 为负数时抛出。</exception>
    public static string ToColumnLetter(int columnIndex)
    {
        if (columnIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(columnIndex), "列索引不能为负数");

        columnIndex++;
        string columnName = string.Empty;
        while (columnIndex > 0)
        {
            int remainder = (columnIndex - 1) % 26;
            columnName = (char)('A' + remainder) + columnName;
            columnIndex = (columnIndex - remainder) / 26;
        }

        return columnName;
    }
}