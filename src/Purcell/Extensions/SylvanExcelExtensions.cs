using Sylvan.Data.Excel;

namespace PurcellLibs.Extensions;

/// <summary>
/// Sylvan.Data.Excel 扩展方法类
/// </summary>
internal static class SylvanExcelExtensions
{
    /// <summary>
    /// 将Excel单元格值安全地转换为.NET类型对象
    /// </summary>
    /// <param name="reader">Excel数据读取器</param>
    /// <param name="columnIndex">要读取的列索引</param>
    /// <param name="rowIndex">当前行索引(仅用于错误提示)</param>
    /// <param name="ignoreParseError">是否在值解析失败时抛出异常，默认为 false</param>
    /// <returns>转换后的值。单元格为空或转换失败时返回 null</returns>
    /// <remarks>
    /// 根据Excel数据类型智能转换:
    /// <list type="bullet">
    ///   <item><description>空指针 → Null</description></item>
    ///   <item><description>布尔值 → bool</description></item>
    ///   <item><description>日期 → DateTime</description></item>
    ///   <item><description>日期 → TimeSpan</description></item>
    ///   <item><description>数值 → double</description></item>
    ///   <item><description>字符串 → string</description></item>
    /// </list>
    /// </remarks>
    public static object? GetExcelValueSafely(this ExcelDataReader reader,
        int columnIndex,
        int rowIndex,
        bool ignoreParseError)
    {
        bool isDbNull = reader.IsDBNull(columnIndex);
        if (isDbNull) return null;

        object excelValue = reader.GetExcelValue(columnIndex);
        ExcelDataType excelDataType = reader.GetExcelDataType(columnIndex);
        FormatKind? formatKind = reader.GetFormat(columnIndex)?.Kind;

        try
        {
            // 根据类型返回适当的值
            switch (excelDataType)
            {
                case ExcelDataType.Boolean:
                    if (excelValue is bool outBoolean) return outBoolean;
                    return reader.GetBoolean(columnIndex);
                case ExcelDataType.DateTime:
                    if (excelValue is DateTime outDateTime) return outDateTime;
                    return reader.GetDateTime(columnIndex);
                case ExcelDataType.Numeric:
                    double doubleValue = excelValue is double outDouble ? outDouble : reader.GetDouble(columnIndex);
                    if (formatKind is not (FormatKind.Date or FormatKind.Time)) return doubleValue;
                    object? numDateObject = doubleValue.ToExcelDate();
                    if (numDateObject is not DateTime numDateValue) return null;
                    if (numDateValue.Ticks % TimeSpan.TicksPerSecond == 9_999_999) // tick 9999999 视为 double 精度问题
                        numDateValue = numDateValue.AddTicks(1);
                    return formatKind is FormatKind.Date ? numDateValue : numDateValue.TimeOfDay;
                case ExcelDataType.String:
                    return reader.GetString(columnIndex);
                case ExcelDataType.Null:
                case ExcelDataType.Error:
                default:
                    return null;
            }
        }
        catch
        {
            if (!ignoreParseError) throw;

#if DEBUG
            // 警告输出
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[警告] Excel转换错误 (行:{rowIndex}列:{columnIndex}): {ex.Message}");
            Console.ResetColor();
#endif
            return null;
        }
    }
}