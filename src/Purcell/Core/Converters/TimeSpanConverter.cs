// ReSharper disable InconsistentNaming

namespace PurcellLibs.Converters;

/// <summary>
/// 时间间隔转换器。
/// </summary>
public class TimeSpanConverter : IValueConverter
{
    // 单例实例的懒加载
    private static readonly Lazy<TimeSpanConverter> _instance = new(() => new TimeSpanConverter());

    /// <inheritdoc cref="TimeSpanConverter"/>
    public static IValueConverter Instance => _instance.Value;

    private TimeSpanConverter()
    {
    }

    // ISO 8601 时间间隔格式的正则表达式
    private static readonly Regex ISO8601TimeSpanRegex = new(
        @"^P(?:(\d+)Y)?(?:(\d+)M)?(?:(\d+)D)?(?:T(?:(\d+)H)?(?:(\d+)M)?(?:(\d+(?:\.\d+)?)S)?)?$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, PurColumn columnConfig, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        Type actualType = targetType.GetActualType();
        bool canBeNull = targetType.CanBeNull();

        // 验证目标类型是否为支持的类型
        if (actualType != typeof(TimeSpan) && actualType != typeof(TimeOnly))
        {
            throw new InvalidOperationException($"目标类型 {actualType.FullName} 不是支持的类型(TimeSpan、TimeOnly)。");
        }

        TimeSpan? defaultResult = canBeNull ? null : TimeSpan.Zero;

        // 值为null时返回默认结果
        if (value == null) return defaultResult;

        // 快速路径 - value类型范围：null、bool、double、string、DateTime、TimeSpan
        switch (value)
        {
            case bool:
            case double d when double.IsNaN(d) || double.IsInfinity(d):
                return defaultResult;
            case double d:
                return ConvertToDateType(TimeSpan.FromSeconds(d), actualType);
            case DateTime dt:
                return ConvertToDateType(dt.TimeOfDay, actualType);
            case TimeSpan ts:
                return ConvertToDateType(ts, actualType);
        }

        // 从字符串类型转换
        if (value is string strValue && !string.IsNullOrWhiteSpace(strValue))
        {
            ReadOnlySpan<char> trimmedValue = strValue.AsSpan().Trim();

            // 使用自定义格式化字符串
            if (!string.IsNullOrWhiteSpace(columnConfig.Format)
                && TimeSpan.TryParseExact(trimmedValue, columnConfig.Format, culture, out TimeSpan parseResult))
            {
                return ConvertToDateType(parseResult, actualType) ?? defaultResult;
            }

            if (TimeSpan.TryParse(trimmedValue, culture, out parseResult))
            {
                return ConvertToDateType(parseResult, actualType) ?? defaultResult;
            }

            // 使用自定义格式化字符串
            if (!string.IsNullOrWhiteSpace(columnConfig.Format)
                && DateTime.TryParseExact(trimmedValue, columnConfig.Format, culture, DateTimeStyles.None,
                    out DateTime parseDtResult))
            {
                return ConvertToDateType(parseDtResult.TimeOfDay, actualType) ?? defaultResult;
            }

            if (DateTime.TryParse(trimmedValue, culture, out parseDtResult))
            {
                return ConvertToDateType(parseDtResult.TimeOfDay, actualType) ?? defaultResult;
            }

            if (culture.Equals(CultureInfo.InvariantCulture)
                && DateTime.TryParse(trimmedValue, CultureInfo.CurrentCulture, out parseDtResult))
            {
                return ConvertToDateType(parseDtResult.TimeOfDay, actualType) ?? defaultResult;
            }

            if (ISO8601TimeSpanRegex.IsMatch(strValue))
            {
                // 通过正则验证后再尝试解析
                try
                {
                    return ConvertToDateType(XmlConvert.ToTimeSpan(strValue), actualType);
                }
                catch
                {
                    // ignored
                }
            }
        }

        return defaultResult;
    }

    /// <summary>
    /// 将 <see cref="TimeSpan"/> 转换为指定的时间类型。
    /// </summary>
    /// <param name="timeSpan">要转换的 <see cref="TimeSpan"/>。</param>
    /// <param name="actualType">目标类型。</param>
    /// <returns>转换结果；如果转换失败则返回 <see langword="null"/>。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? ConvertToDateType(TimeSpan timeSpan, Type actualType)
    {
        if (actualType == typeof(TimeSpan))
            return timeSpan;
        if (actualType == typeof(TimeOnly))
            return TimeOnly.FromTimeSpan(timeSpan);

        return null;
    }
}