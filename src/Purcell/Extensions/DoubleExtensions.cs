namespace PurcellLibs.Extensions;

/// <summary>
/// Double 扩展方法类
/// </summary>
internal static class DoubleExtensions
{
    /// <summary>
    /// 安全地将 double 转换为 long，处理边界情况和溢出
    /// </summary>
    /// <param name="value">要转换的 double 值</param>
    /// <param name="defaultValue">当转换失败时返回的默认值</param>
    /// <returns>转换后的 long 值，或在无法转换时返回默认值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ToInt64Safe(this double value, long defaultValue = 0)
    {
        // 处理特殊值
        if (double.IsNaN(value) || double.IsInfinity(value))
            return defaultValue;

        return value switch
        {
            // 检查范围
            > long.MaxValue => long.MaxValue,
            < long.MinValue => long.MinValue,
            // 移除小数部分
            _ => (long)Math.Truncate(value)
        };
    }

    /// <summary>
    /// 安全地将 double 转换为 ulong，处理边界情况和溢出
    /// </summary>
    /// <param name="value">要转换的 double 值</param>
    /// <param name="defaultValue">当转换失败时返回的默认值</param>
    /// <returns>转换后的 ulong 值，或在无法转换时返回默认值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ToUInt64Safe(this double value, ulong defaultValue = 0)
    {
        // 处理特殊值
        if (double.IsNaN(value) || double.IsInfinity(value))
            return defaultValue;

        return value switch
        {
            // 检查范围
            > ulong.MaxValue => ulong.MaxValue,
            < 0 => 0,
            // 移除小数部分
            _ => (ulong)Math.Truncate(value)
        };
    }

    /// <summary>
    /// 安全地将 double 转换为 int，处理边界情况和溢出
    /// </summary>
    /// <param name="value">要转换的 double 值</param>
    /// <param name="defaultValue">当转换失败时返回的默认值</param>
    /// <returns>转换后的 int 值，或在无法转换时返回默认值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt32Safe(this double value, int defaultValue = 0)
    {
        // 处理特殊值
        if (double.IsNaN(value) || double.IsInfinity(value))
            return defaultValue;

        return value switch
        {
            // 检查范围
            > int.MaxValue => int.MaxValue,
            < int.MinValue => int.MinValue,
            // 移除小数部分
            _ => (int)Math.Truncate(value)
        };
    }

    /// <summary>
    /// 安全地将 double 转换为 uint，处理边界情况和溢出
    /// </summary>
    /// <param name="value">要转换的 double 值</param>
    /// <param name="defaultValue">当转换失败时返回的默认值</param>
    /// <returns>转换后的 uint 值，或在无法转换时返回默认值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ToUInt32Safe(this double value, uint defaultValue = 0)
    {
        // 处理特殊值
        if (double.IsNaN(value) || double.IsInfinity(value))
            return defaultValue;

        return value switch
        {
            // 检查范围
            > uint.MaxValue => uint.MaxValue,
            < 0 => 0,
            // 移除小数部分
            _ => (uint)Math.Truncate(value)
        };
    }

    /// <summary>
    /// 安全地将 double 转换为 short，处理边界情况和溢出
    /// </summary>
    /// <param name="value">要转换的 double 值</param>
    /// <param name="defaultValue">当转换失败时返回的默认值</param>
    /// <returns>转换后的 short 值，或在无法转换时返回默认值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short ToInt16Safe(this double value, short defaultValue = 0)
    {
        // 处理特殊值
        if (double.IsNaN(value) || double.IsInfinity(value))
            return defaultValue;

        return value switch
        {
            // 检查范围
            > short.MaxValue => short.MaxValue,
            < short.MinValue => short.MinValue,
            // 移除小数部分
            _ => (short)Math.Truncate(value)
        };
    }

    /// <summary>
    /// 安全地将 double 转换为 ushort，处理边界情况和溢出
    /// </summary>
    /// <param name="value">要转换的 double 值</param>
    /// <param name="defaultValue">当转换失败时返回的默认值</param>
    /// <returns>转换后的 ushort 值，或在无法转换时返回默认值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ToUInt16Safe(this double value, ushort defaultValue = 0)
    {
        // 处理特殊值
        if (double.IsNaN(value) || double.IsInfinity(value))
            return defaultValue;

        return value switch
        {
            // 检查范围
            > ushort.MaxValue => ushort.MaxValue,
            < 0 => 0,
            // 移除小数部分
            _ => (ushort)Math.Truncate(value)
        };
    }

    /// <summary>
    /// 安全地将 double 转换为 byte，处理边界情况和溢出
    /// </summary>
    /// <param name="value">要转换的 double 值</param>
    /// <param name="defaultValue">当转换失败时返回的默认值</param>
    /// <returns>转换后的 byte 值，或在无法转换时返回默认值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ToByteSafe(this double value, byte defaultValue = 0)
    {
        // 处理特殊值
        if (double.IsNaN(value) || double.IsInfinity(value))
            return defaultValue;

        return value switch
        {
            // 检查范围
            > byte.MaxValue => byte.MaxValue,
            < 0 => 0,
            // 移除小数部分
            _ => (byte)Math.Truncate(value)
        };
    }

    /// <summary>
    /// 安全地将 double 转换为 sbyte，处理边界情况和溢出
    /// </summary>
    /// <param name="value">要转换的 double 值</param>
    /// <param name="defaultValue">当转换失败时返回的默认值</param>
    /// <returns>转换后的 sbyte 值，或在无法转换时返回默认值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte ToSByteSafe(this double value, sbyte defaultValue = 0)
    {
        // 处理特殊值
        if (double.IsNaN(value) || double.IsInfinity(value))
            return defaultValue;

        return value switch
        {
            // 检查范围
            > sbyte.MaxValue => sbyte.MaxValue,
            < sbyte.MinValue => sbyte.MinValue,
            // 移除小数部分
            _ => (sbyte)Math.Truncate(value)
        };
    }

    /// <summary>
    /// 安全地将 double 转换为 decimal，处理边界情况和溢出
    /// </summary>
    /// <param name="value">要转换的 double 值</param>
    /// <param name="defaultValue">当转换失败时返回的默认值</param>
    /// <returns>转换后的 decimal 值，或在无法转换时返回默认值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal ToDecimalSafe(this double value, decimal defaultValue = 0)
    {
        // 处理特殊值
        if (double.IsNaN(value) || double.IsInfinity(value))
            return defaultValue;

        // 硬编码 decimal 的最大/最小值的 double 近似值
        // 这些值略小于实际边界，提供安全余量
        const double maxDecimalAsDouble = 7.9e28;
        const double minDecimalAsDouble = -7.9e28;

        return value switch
        {
            // 检查范围
            > maxDecimalAsDouble => decimal.MaxValue,
            < minDecimalAsDouble => decimal.MinValue,
            _ => (decimal)value
        };
    }

    /// <summary>
    /// 安全地将 double 转换为 float，处理边界情况和溢出
    /// </summary>
    /// <param name="value">要转换的 double 值</param>
    /// <param name="defaultValue">当转换失败时返回的默认值</param>
    /// <returns>转换后的 float 值，或在无法转换时返回默认值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ToSingleSafe(this double value, float defaultValue = 0)
    {
        // 处理特殊值
        if (double.IsNaN(value))
            return float.NaN;

        if (double.IsPositiveInfinity(value))
            return float.PositiveInfinity;

        if (double.IsNegativeInfinity(value))
            return float.NegativeInfinity;

        return value switch
        {
            // 检查范围
            > float.MaxValue => float.MaxValue,
            < float.MinValue => float.MinValue,
            _ => (float)value
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime? ToExcelDate(this double value)
    {
        if (value is >= 60.0 and < 61.0 or < -657435.0 or >= 2958466.0)
        {
            // Excel 1900 日期系统的特殊处理
            // 1900年2月29日是一个错误日期，Excel错误地将其视为有效日期
            return null;
        }

        long saltTicks = value < 61 ? PurConstants.DayTicksL : 0; // 处理 Excel 1900 日期系统闰年Bug的偏移
        return PurConstants.Epoch1900.AddTicks(saltTicks + (long)(value * PurConstants.DayTicks));
    }
}