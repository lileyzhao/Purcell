namespace PurcellLibs;

/// <summary>
/// Purcell 常量
/// </summary>
public static class PurConstants
{
    /// <summary>
    /// Excel (.xlsx) 格式工作表中允许的最大工作表数量。
    /// </summary>
    /// <remarks>
    /// <para>理论最大值为 2,147,483,647 (int.MaxValue)。</para>
    /// <para>实际限制：</para>
    /// <list type="bullet">
    ///     <item>受系统可用内存限制</item>
    ///     <item>受Excel应用程序性能影响</item>
    ///     <item>建议实际使用不超过1000个工作表以确保性能</item>
    /// </list>
    /// </remarks>
    /// <seealso cref="XlsWorksheetLimit"/>
    public const int XlsxWorksheetLimit = int.MaxValue;

    /// <summary>
    /// Excel (.xls) 格式工作表中允许的最大工作表数量。
    /// </summary>
    /// <remarks>
    /// <para>固定限制值为 256 (2^8)。</para>
    /// <para>技术细节：</para>
    /// <list type="bullet">
    ///     <item>适用于Excel 97-2003二进制文件格式</item>
    ///     <item>此限制是由文件格式规范决定的硬性限制</item>
    ///     <item>超出此限制将导致文件无法保存</item>
    /// </list>
    /// </remarks>
    /// <seealso cref="XlsxWorksheetLimit"/>
    public const short XlsWorksheetLimit = 1 << 8;

    /// <summary>
    /// Excel 1900 日期系统的起始日期。
    /// </summary>
    /// <remarks>
    /// <para>日期：1899-12-30</para>
    /// <para>用途：</para>
    /// <list type="bullet">
    /// <item><description>用于 Windows Excel 默认日期计算</description></item>
    /// <item><description>处理 1900 日期系统的日期转换</description></item>
    /// </list>
    /// </remarks>
    public static readonly DateTime Epoch1900 = new(1899, 12, 30);

    /// <summary>
    /// 一天的时钟周期数（Ticks）- double 类型。
    /// </summary>
    /// <remarks>
    /// <para>常量值：864,000,000,000.0</para>
    /// <para>计算说明：</para>
    /// <list type="bullet">
    ///     <item><description>1秒 = 10,000,000 ticks</description></item>
    ///     <item><description>1分钟 = 60秒 = 600,000,000 ticks</description></item>
    ///     <item><description>1小时 = 60分钟 = 36,000,000,000 ticks</description></item>
    ///     <item><description>1天 = 24小时 = 864,000,000,000 ticks</description></item>
    /// </list>
    /// <para>用途：用于 Excel 日期转换中的浮点数计算。</para>
    /// </remarks>
    /// <seealso cref="DayTicksL"/>
    public static readonly double DayTicks = 864000000000.0;
    
    /// <summary>
    /// 一天的时钟周期数（Ticks）- long 类型。
    /// </summary>
    /// <remarks>
    /// <para>常量值：864,000,000,000</para>
    /// <para>计算说明：</para>
    /// <list type="bullet">
    ///     <item><description>1秒 = 10,000,000 ticks</description></item>
    ///     <item><description>1分钟 = 60秒 = 600,000,000 ticks</description></item>
    ///     <item><description>1小时 = 60分钟 = 36,000,000,000 ticks</description></item>
    ///     <item><description>1天 = 24小时 = 864,000,000,000 ticks</description></item>
    /// </list>
    /// <para>用途：用于 Excel 日期转换中的精确整数计算和时间偏移处理。</para>
    /// </remarks>
    /// <seealso cref="DayTicks"/>
    public static readonly long DayTicksL = 864000000000L;

    /// <summary>
    /// Excel 1904 日期系统的起始日期。
    /// </summary>
    /// <remarks>
    /// <para>日期：1904-01-01</para>
    /// <para>用途：</para>
    /// <list type="bullet">
    /// <item><description>用于 Mac Excel 默认日期计算</description></item>
    /// <item><description>处理 1904 日期系统的日期转换</description></item>
    /// </list>
    /// </remarks>
    public static readonly DateTime Epoch1904 = new(1904, 1, 1);
}