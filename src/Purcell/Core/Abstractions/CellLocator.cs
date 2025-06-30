namespace PurcellLibs;

/// <summary>
/// Excel 单元格定位器，提供行列索引方式定位 Excel 中的单元格。
/// </summary>
public readonly struct CellLocator : IEquatable<CellLocator>
{
    /// <summary>
    /// Excel (.xlsx) 文件格式支持的最大列数。
    /// </summary>
    private const int XlsxColumnLimit = 1 << 14;

    /// <summary>
    /// A1 表示法字符串正则表达式。
    /// </summary>
    private static readonly Regex A1NotationRegex =
        new(@"^(?<column>[A-Za-z]{1,3})(?<row>[1-9][0-9]*)$", RegexOptions.Compiled);

    /// <summary>
    /// 获取基于 0 的行索引。
    /// </summary>
    /// <value>行索引，从 0 开始计数。</value>
    public int RowIndex { get; }

    /// <summary>
    /// 获取基于 0 的列索引。
    /// </summary>
    /// <value>列索引，从 0 开始计数。</value>
    public int ColumnIndex { get; }

    /// <summary>
    /// 初始化 <see cref="CellLocator"/> 结构的新实例。
    /// </summary>
    /// <param name="rowIndex">基于 0 的行索引。</param>
    /// <param name="columnIndex">基于 0 的列索引。</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="rowIndex"/> 或 <paramref name="columnIndex"/> 为负数，或 <paramref name="columnIndex"/> 超出 Excel 支持的列数范围时抛出。</exception>
    public CellLocator(int rowIndex, int columnIndex)
    {
        if (!(rowIndex == -1 && columnIndex == -1))
        {
            if (rowIndex < 0 || columnIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "行索引和列索引不能为负数");
            if (columnIndex > XlsxColumnLimit - 1)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), $"列索引超出范围(0-{XlsxColumnLimit - 1})");
        }

        RowIndex = rowIndex;
        ColumnIndex = columnIndex;
    }

    /// <summary>
    /// 使用 A1 表示法初始化 <see cref="CellLocator"/> 结构的新实例。
    /// </summary>
    /// <param name="a1Notation">A1 表示法字符串（如 "A1"、"B2"）。</param>
    /// <exception cref="ArgumentException">当 <paramref name="a1Notation"/> 为 <see langword="null"/>、空字符串或仅包含空白字符时抛出。</exception>
    /// <exception cref="FormatException">当 <paramref name="a1Notation"/> 不是有效的 A1 表示法格式时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当列索引超出 Excel 支持的列数范围时抛出。</exception>
    public CellLocator(string a1Notation)
    {
        if (string.IsNullOrWhiteSpace(a1Notation))
            throw new ArgumentException("表头/数据起始行的A1表示法字符串不能为空", nameof(a1Notation));

        Match match = A1NotationRegex.Match(a1Notation);
        if (!match.Success)
            throw new FormatException($"无效的A1表示法: {a1Notation}");

        int rowIndex = int.Parse(match.Groups["row"].Value) - 1;
        int columnIndex = ExcelHelper.ToColumnIndex(match.Groups["column"].Value);

        if (columnIndex > XlsxColumnLimit)
            throw new ArgumentOutOfRangeException(nameof(columnIndex), $"列索引超出范围(0-{XlsxColumnLimit})");

        RowIndex = rowIndex;
        ColumnIndex = columnIndex;
    }

    /// <summary>
    /// 创建新的 <see cref="CellLocator"/> 实例。
    /// </summary>
    /// <param name="rowIndex">基于 0 的行索引。</param>
    /// <param name="columnIndex">基于 0 的列索引。</param>
    /// <returns>新的 <see cref="CellLocator"/> 实例。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="rowIndex"/> 或 <paramref name="columnIndex"/> 为负数，或 <paramref name="columnIndex"/> 超出 Excel 支持的列数范围时抛出。</exception>
    public static CellLocator Create(int rowIndex, int columnIndex)
    {
        return new CellLocator(rowIndex, columnIndex);
    }

    /// <summary>
    /// 使用 A1 表示法创建新的 <see cref="CellLocator"/> 实例。
    /// </summary>
    /// <param name="a1Notation">A1 表示法字符串（如 "A1"、"B2"）。</param>
    /// <returns>新的 <see cref="CellLocator"/> 实例。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="a1Notation"/> 为 <see langword="null"/>、空字符串或仅包含空白字符时抛出。</exception>
    /// <exception cref="FormatException">当 <paramref name="a1Notation"/> 不是有效的 A1 表示法格式时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当列索引超出 Excel 支持的列数范围时抛出。</exception>
    public static CellLocator Create(string a1Notation)
    {
        return new CellLocator(a1Notation);
    }

    /// <summary>
    /// 表示单元格 A1 (0,0)。
    /// </summary>
    public static readonly CellLocator A1 = new("A1");

    /// <summary>
    /// 表示单元格 A2 (1,0)。
    /// </summary>
    public static readonly CellLocator A2 = new("A2");

    /// <summary>
    /// 表示未知或未设置的单元格位置。
    /// </summary>
    public static readonly CellLocator Unknown = new(-1, -1);

    /// <summary>
    /// 创建一个新的 <see cref="CellLocator"/>，表示当前定位器向指定方向偏移后的位置。
    /// </summary>
    /// <param name="rowOffset">行偏移量。</param>
    /// <param name="columnOffset">列偏移量。</param>
    /// <returns>偏移后的新 <see cref="CellLocator"/> 实例。</returns>
    public CellLocator Offset(int rowOffset, int columnOffset)
    {
        return new CellLocator(RowIndex + rowOffset, ColumnIndex + columnOffset);
    }

    /// <summary>
    /// 返回单元格的 A1 表示法字符串。
    /// </summary>
    /// <returns>A1 表示法字符串；如果位置未知则返回空字符串。</returns>
    public override string ToString()
    {
        return ColumnIndex == -1 ? "" : $"{ExcelHelper.ToColumnLetter(ColumnIndex)}{RowIndex + 1}";
    }

    /// <summary>
    /// 确定指定的 <see cref="CellLocator"/> 是否等于当前实例。
    /// </summary>
    /// <param name="other">要与当前实例进行比较的 <see cref="CellLocator"/>。</param>
    /// <returns>如果指定的 <see cref="CellLocator"/> 等于当前实例，则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public bool Equals(CellLocator other)
    {
        return RowIndex == other.RowIndex && ColumnIndex == other.ColumnIndex;
    }

    /// <summary>
    /// 确定指定的对象是否等于当前实例。
    /// </summary>
    /// <param name="obj">要与当前实例进行比较的对象。</param>
    /// <returns>如果指定的对象是 <see cref="CellLocator"/> 且等于当前实例，则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public override bool Equals(object? obj)
    {
        return obj is CellLocator other && Equals(other);
    }

    /// <summary>
    /// 获取当前实例的哈希代码。
    /// </summary>
    /// <returns>当前实例的哈希代码。</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(RowIndex, ColumnIndex);
    }

    /// <summary>
    /// 确定两个 <see cref="CellLocator"/> 实例是否相等。
    /// </summary>
    /// <param name="left">要比较的第一个 <see cref="CellLocator"/>。</param>
    /// <param name="right">要比较的第二个 <see cref="CellLocator"/>。</param>
    /// <returns>如果两个实例相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool operator ==(CellLocator left, CellLocator right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// 确定两个 <see cref="CellLocator"/> 实例是否不相等。
    /// </summary>
    /// <param name="left">要比较的第一个 <see cref="CellLocator"/>。</param>
    /// <param name="right">要比较的第二个 <see cref="CellLocator"/>。</param>
    /// <returns>如果两个实例不相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool operator !=(CellLocator left, CellLocator right)
    {
        return !left.Equals(right);
    }
}