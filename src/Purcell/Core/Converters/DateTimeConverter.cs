// ReSharper disable InconsistentNaming

namespace PurcellLibs.Converters;

/// <summary>
/// 日期时间转换器。
/// </summary>
/// <remarks>
/// <para>支持格式：ISO8601、Unix 时间戳、多国日期格式、Excel 序列日期。</para>
/// <para>性能特性：预编译格式、零分配路径、类型缓存、边界检查。</para>
/// </remarks>
public class DateTimeConverter : IValueConverter
{
    // 单例实例的懒加载
    private static readonly Lazy<DateTimeConverter> _instance = new(() => new DateTimeConverter());

    /// <inheritdoc cref="DateTimeConverter"/>
    public static IValueConverter Instance => _instance.Value;

    private DateTimeConverter()
    {
    }

    // 时间戳边界常量 - DateTime(1900,1,1) 的秒数
    private const long MinUnixSeconds = -2208988800L;

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, PurColumn columnConfig, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        bool canBeNull = targetType.CanBeNull();
        Type actualType = targetType.GetActualType();

        // 验证目标类型是否为支持的类型
        if (actualType != typeof(DateTime) && actualType != typeof(DateTimeOffset) && actualType != typeof(DateOnly))
        {
            throw new InvalidOperationException(
                $"目标类型 {targetType.Name} 不是支持的类型(DateTime、DateTimeOffset、DateOnly)。");
        }

        object? defaultResult = canBeNull ? null : actualType.GetDefaultValue();

        // 值为null时返回默认结果
        if (value == null) return defaultResult;

        // 快速路径 - value类型范围：null、bool、double、string、DateTime、TimeSpan
        switch (value)
        {
            case bool:
            case double d when double.IsNaN(d) || double.IsInfinity(d):
                return defaultResult;
            case DateTime dt:
                return ConvertToDateType(dt, actualType);
            case TimeSpan ts:
                return ConvertToTimeType(ts, actualType);
        }

        // 从数值类型转换
        if (value is double doubleVal && TryParseTimestamp((long)doubleVal, out DateTime dvResult))
        {
            return ConvertToDateType(dvResult, actualType) ?? defaultResult;
        }

        // 从字符串类型转换
        if (value is string strValue && !string.IsNullOrWhiteSpace(strValue))
        {
            ReadOnlySpan<char> trimmedValue = strValue.AsSpan().Trim();

            if (double.TryParse(trimmedValue, out doubleVal) && TryParseTimestamp((long)doubleVal, out dvResult))
            {
                return ConvertToDateType(dvResult, actualType) ?? defaultResult;
            }

            // 使用自定义格式化字符串
            if (!string.IsNullOrWhiteSpace(columnConfig.Format)
                && DateTime.TryParseExact(trimmedValue, columnConfig.Format, culture, DateTimeStyles.None,
                    out DateTime parseResult))
            {
                return ConvertToDateType(parseResult, actualType) ?? defaultResult;
            }

            if (DateTime.TryParse(trimmedValue, culture, DateTimeStyles.None, out parseResult))
            {
                return ConvertToDateType(parseResult, actualType) ?? defaultResult;
            }

            if (culture.Equals(CultureInfo.InvariantCulture)
                && DateTime.TryParse(trimmedValue, CultureInfo.CurrentCulture, out parseResult))
            {
                return ConvertToDateType(parseResult, actualType) ?? defaultResult;
            }
        }

        return defaultResult;
    }

    /// <summary>
    /// 解析 Unix 时间戳为 <see cref="DateTime"/>。
    /// </summary>
    /// <param name="timestamp">Unix 时间戳（秒）。</param>
    /// <param name="result">解析结果。</param>
    /// <returns>如果解析成功则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    private static bool TryParseTimestamp(long timestamp, out DateTime result)
    {
        result = default;

        try
        {
            // 秒级时间戳检查
            if (timestamp >= MinUnixSeconds)
            {
                result = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
                return result >= DateTime.MinValue && result <= DateTime.MaxValue;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            // 时间戳超出.NET支持范围
        }

        return false;
    }

    /// <summary>
    /// 将 <see cref="DateTime"/> 转换为指定的日期时间类型。
    /// </summary>
    /// <param name="dateTime">要转换的 <see cref="DateTime"/>。</param>
    /// <param name="targetType">目标类型。</param>
    /// <returns>转换结果；如果转换失败则返回 <see langword="null"/>。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? ConvertToDateType(DateTime dateTime, Type targetType)
    {
        if (targetType == typeof(DateTime))
            return dateTime;
        if (targetType == typeof(DateTimeOffset))
            return new DateTimeOffset(dateTime);
        if (targetType == typeof(DateOnly))
            return DateOnly.FromDateTime(dateTime);

        return null;
    }

    /// <summary>
    /// 将 <see cref="TimeSpan"/> 转换为指定的日期时间类型。
    /// </summary>
    /// <param name="timeSpan">要转换的 <see cref="TimeSpan"/>。</param>
    /// <param name="targetType">目标类型。</param>
    /// <returns>转换结果；如果转换失败则返回 <see langword="null"/>。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? ConvertToTimeType(TimeSpan timeSpan, Type targetType)
    {
        if (targetType == typeof(DateTime))
            return DateTime.MinValue.Add(timeSpan);
        if (targetType == typeof(DateTimeOffset))
            return DateTimeOffset.MinValue.Add(timeSpan);
        if (targetType == typeof(DateOnly))
            return DateOnly.MinValue;

        return null;
    }
}