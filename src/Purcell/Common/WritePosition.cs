namespace PurcellLibs;

/// <summary>
/// 表示写表格文件处理过程中的位置信息，用于写进度报告
/// </summary>
public class WritePosition
{
    /// <summary>
    /// 当前正在处理的工作表索引
    /// </summary>
    public readonly int SheetIndex;

    /// <summary>
    /// 当前正在处理的行索引，实际上每50行才会报告一次进度
    /// </summary>
    public readonly int RowIndex;

    /// <summary>
    /// 写表格文件处理过程中的位置信息
    /// </summary>
    /// <param name="sheetIndex">工作表索引（从0开始）</param>
    /// <param name="rowIndex">行索引（从0开始）</param>
    public WritePosition(int sheetIndex, int rowIndex)
    {
        SheetIndex = sheetIndex;
        RowIndex = rowIndex;
    }

    /// <summary>
    /// 写表格文件处理过程中的位置信息
    /// </summary>
    /// <param name="sheetIndex">工作表索引（从0开始）</param>
    /// <param name="rowIndex">行索引（从0开始）</param>
    public static WritePosition New(int sheetIndex, int rowIndex)
    {
        return new WritePosition(sheetIndex, rowIndex);
    }
}