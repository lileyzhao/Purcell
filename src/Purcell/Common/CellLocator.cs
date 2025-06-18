namespace PurcellLibs;

/// <summary>
/// Excel单元格定位器，提供行列索引方式定位Excel中的单元格
/// </summary>
public readonly struct CellLocator : IEquatable<CellLocator>
{
    /// <summary>
    /// A1表示法字符串正则表达式
    /// </summary>
    private static readonly Regex A1NotationRegex = new(@"^(?<column>[A-Za-z]{1,3})(?<row>[1-9][0-9]*)$", RegexOptions.Compiled);

    /// <summary>
    /// 基于0的行索引
    /// </summary>
    public int RowIndex { get; }

    /// <summary>
    /// 基于0的列索引
    /// </summary>
    public int ColumnIndex { get; }

    /// <summary>
    /// 创建单元格的定位器
    /// </summary>
    /// <param name="rowIndex">基于0的行索引</param>
    /// <param name="columnIndex">基于0的列索引</param>
    public CellLocator(int rowIndex, int columnIndex)
    {
        if (!(rowIndex == -1 && columnIndex == -1))
        {
            if (rowIndex < 0 || columnIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "行索引和列索引不能为负数");
            if (columnIndex > PurConstants.XlsxColumnLimit - 1)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), $"列索引超出范围(0-{PurConstants.XlsxColumnLimit - 1})");
        }

        RowIndex = rowIndex;
        ColumnIndex = columnIndex;
    }

    /// <summary>
    /// 从A1表示法创建单元格定位器（如A1, B2）
    /// </summary>
    /// <param name="a1Notation">A1表示法字符串</param>
    public CellLocator(string a1Notation)
    {
        if (string.IsNullOrWhiteSpace(a1Notation))
            throw new ArgumentException("表头/数据起始行的A1表示法字符串不能为空", nameof(a1Notation));

        Match match = A1NotationRegex.Match(a1Notation);
        if (!match.Success)
            throw new FormatException($"无效的A1表示法: {a1Notation}");

        int rowIndex = int.Parse(match.Groups["row"].Value) - 1;
        int columnIndex = ColumnLetterToIndex(match.Groups["column"].Value);

        if (columnIndex > PurConstants.XlsxColumnLimit)
            throw new ArgumentOutOfRangeException(nameof(columnIndex), $"列索引超出范围(0-{PurConstants.XlsxColumnLimit})");

        RowIndex = rowIndex;
        ColumnIndex = columnIndex;
    }

    /// <summary>
    /// 创建单元格的定位器
    /// </summary>
    /// <param name="rowIndex">基于0的行索引</param>
    /// <param name="columnIndex">基于0的列索引</param>
    public static CellLocator Create(int rowIndex, int columnIndex)
    {
        return new CellLocator(rowIndex, columnIndex);
    }

    /// <summary>
    /// 从A1表示法创建单元格定位器（如A1, B2）
    /// </summary>
    /// <param name="a1Notation">A1表示法字符串</param>
    public static CellLocator Create(string a1Notation)
    {
        return new CellLocator(a1Notation);
    }

    /// <summary>
    /// 表示单元格 A1 (0,0)
    /// </summary>
    public static readonly CellLocator A1 = new("A1");

    /// <summary>
    /// 表示单元格 B1 (0,1)
    /// </summary>
    public static readonly CellLocator A2 = new("A2");

    /// <summary>
    /// 表示未知/未设置
    /// </summary>
    public static readonly CellLocator Unknown = new(-1, -1);

    /// <summary>
    /// 将列字母转换为列索引（基于0）
    /// </summary>
    public static int ColumnLetterToIndex(string columnLetter)
    {
        int result = 0;
        foreach (char ct in columnLetter)
        {
            result = result * 26 + (char.ToUpperInvariant(ct) - 'A') + 1;
        }

        return result - 1;
    }

    /// <summary>
    /// 获取列字母表示
    /// </summary>
    public static string GetColumnLetter(int columnIndex)
    {
        columnIndex++;
        string columnLetter = string.Empty;
        while (columnIndex > 0)
        {
            int remainder = (columnIndex - 1) % 26;
            columnLetter = (char)('A' + remainder) + columnLetter;
            columnIndex = (columnIndex - remainder) / 26;
        }

        return columnLetter;
    }

    /// <summary>
    /// 创建一个新的单元格定位器，表示当前定位器向指定方向偏移
    /// </summary>
    /// <param name="rowOffset">行偏移量</param>
    /// <param name="columnOffset">列偏移量</param>
    /// <returns>新的单元格定位器</returns>
    public CellLocator Offset(int rowOffset, int columnOffset)
    {
        return new CellLocator(RowIndex + rowOffset, ColumnIndex + columnOffset);
    }

    /// <summary>
    /// 返回单元格的A1表示法
    /// </summary>
    public override string ToString()
    {
        return $"{GetColumnLetter(ColumnIndex)}{RowIndex + 1}";
    }

    /// <inheritdoc/>
    public bool Equals(CellLocator other)
    {
        return RowIndex == other.RowIndex && ColumnIndex == other.ColumnIndex;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is CellLocator other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(RowIndex, ColumnIndex);
    }

    /// <summary>==</summary>
    public static bool operator ==(CellLocator left, CellLocator right)
    {
        return left.Equals(right);
    }

    /// <summary>==</summary>
    public static bool operator !=(CellLocator left, CellLocator right)
    {
        return !left.Equals(right);
    }
}