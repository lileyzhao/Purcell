namespace PurcellLibs.Converters;

/// <summary>
/// 数字转换器
/// </summary>
public class NumericConverter : IValueConverter
{
    private static readonly Lazy<NumericConverter> _instance = new(() => new NumericConverter());

    /// <inheritdoc cref="NumericConverter"/>
    public static IValueConverter Instance => _instance.Value;

    private NumericConverter()
    {
    }

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, PurColumn columnConfig, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        bool canBeNull = targetType.CanBeNull();
        Type actualType = targetType.GetActualType();

        // 验证目标类型是否为支持的类型
        if (!actualType.IsNumericType())
        {
            throw new InvalidOperationException($"目标类型 {targetType.FullName} 不是支持的类型(基本数值类型)。");
        }

        object? defaultResult = canBeNull ? null : actualType.GetDefaultValue();

        // 值为null时返回默认结果
        if (value == null) return defaultResult;

        // 快速路径 - value类型范围：null、bool、double、string、DateTime、TimeSpan
        switch (value)
        {
            case bool:
            case double d when double.IsNaN(d) || double.IsInfinity(d):
            case DateTime:
            case TimeSpan:
                return defaultResult;
        }

        double? doubleValue = null;

        // 从数值类型转换
        if (value is double dbValue)
        {
            doubleValue = dbValue;
        }

        // 从字符串转换
        if (value is string strValue && !string.IsNullOrWhiteSpace(strValue))
        {
            ReadOnlySpan<char> trimmedValue = strValue.AsSpan().Trim();

            if (double.TryParse(trimmedValue, NumberStyles.Any, culture, out double parsedValue))
            {
                doubleValue = parsedValue;
            }

            if (culture.Equals(CultureInfo.InvariantCulture)
                && double.TryParse(trimmedValue, NumberStyles.Any, CultureInfo.CurrentCulture, out parsedValue))
            {
                doubleValue = parsedValue;
            }
        }

        // 如果成功获取到双精度值，根据目标类型进行转换
        if (doubleValue.HasValue)
        {
            double val = doubleValue.Value;

            // 尝试解析为数字
            if (actualType == typeof(byte)) return val.ToByteSafe();
            if (actualType == typeof(sbyte)) return val.ToSByteSafe();
            if (actualType == typeof(short)) return val.ToInt16Safe();
            if (actualType == typeof(ushort)) return val.ToUInt16Safe();
            if (actualType == typeof(int)) return val.ToInt32Safe();
            if (actualType == typeof(uint)) return val.ToUInt32Safe();
            if (actualType == typeof(long)) return val.ToInt64Safe();
            if (actualType == typeof(ulong)) return val.ToUInt64Safe();
            if (actualType == typeof(float)) return val.ToSingleSafe();
            if (actualType == typeof(double)) return val;
            if (actualType == typeof(decimal)) return val.ToDecimalSafe();
        }

        return defaultResult;
    }
}