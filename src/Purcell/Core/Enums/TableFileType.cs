namespace PurcellLibs;

/// <summary>
/// 支持查询的表格文件格式类型枚举。
/// </summary>
/// <remarks>
/// 此枚举定义了支持查询的表格文件格式类型，包括：
/// <list type="bullet">
/// <item><description>CSV 文本格式 (.csv)</description></item>
/// <item><description>现代的 Excel 2007+ 格式 (.xlsx)</description></item>
/// <item><description>古老的 Excel 97-2003 格式 (.xls)</description></item>
/// </list>
/// </remarks>
[Flags]
public enum TableFileType
{
    /// <summary>
    /// 未知或未指定的文件类型。
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// 逗号分隔值文本格式 (.csv)。
    /// </summary>
    /// <remarks>
    /// <para>纯文本格式，使用逗号（或其他分隔符）分隔数据。</para>
    /// <para>优点：</para>
    /// <list type="bullet">
    /// <item><description>文件体积小</description></item>
    /// <item><description>跨平台兼容性最好</description></item>
    /// <item><description>便于文本编辑和程序处理</description></item>
    /// </list>
    /// <para>限制：</para>
    /// <list type="bullet">
    /// <item><description>不支持格式化、公式和多工作表</description></item>
    /// <item><description>可能存在编码问题</description></item>
    /// <item><description>对包含逗号的数据需要特殊处理</description></item>
    /// </list>
    /// </remarks>
    Csv = 1 << 0, // = 1

    /// <summary>
    /// Excel 2007+ Office Open XML 格式 (.xlsx)。
    /// </summary>
    /// <remarks>
    /// <para>基于 XML 的开放格式，最大支持 1,048,576 行和 16,384 列。</para>
    /// <para>文件较小，跨平台兼容性好。</para>
    /// <para>推荐使用此格式作为默认选择。</para>
    /// </remarks>
    Xlsx = 1 << 1, // = 2

    /// <summary>
    /// Excel 97-2003 工作表格式 (.xls)。
    /// </summary>
    /// <remarks>
    /// <para>BIFF8 格式，最大支持 65,536 行和 256 列。</para>
    /// <para>文件大小限制为 2GB。</para>
    /// <para>兼容性好，但功能较为有限。</para>
    /// </remarks>
    Xls = 1 << 2, // = 4

    // 常用组合策略
    /// <summary>
    /// 表示所有支持查询的Excel格式的文件类型组合（.xlsx、.xls）。
    /// </summary>
    ExcelTypes = Xlsx | Xls,

    /// <summary>
    /// 表示所有支持查询的电子表格的文件类型组合（.csv、.xlsx、.xls）。
    /// </summary>
    SpreadsheetTypes = Csv | Xlsx | Xls
}