namespace PurcellLibs.Converters;

/// <summary>
/// Guid转换器
/// </summary>
public class GuidConverter : IValueConverter
{
    private static readonly Lazy<GuidConverter> _instance = new(() => new GuidConverter());

    /// <inheritdoc cref="GuidConverter"/>
    public static IValueConverter Instance => _instance.Value;

    private GuidConverter()
    {
    }

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, CultureInfo culture, string? format)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        Type actualType = targetType.GetActualType();
        bool canBeNull = targetType.CanBeNull();

        // 验证目标类型是否为支持的类型
        if (actualType != typeof(Guid))
        {
            throw new InvalidOperationException($"目标类型 {targetType.FullName} 不是支持的类型(Guid)。");
        }

        Guid? defaultResult = canBeNull ? null : Guid.Empty;

        // 值为null时返回默认结果
        if (value == null) return defaultResult;

        // 快速路径 - value类型范围：null、bool、double、string、DateTime、TimeSpan
        switch (value)
        {
            case bool:
            case double:
            case DateTime:
            case TimeSpan:
                return defaultResult;
        }

        // 从字符串类型转换
        if (value is string strValue && !string.IsNullOrWhiteSpace(strValue))
        {
            ReadOnlySpan<char> trimmedValue = strValue.AsSpan().Trim();

            // 使用自定义格式化字符串
            if (!string.IsNullOrWhiteSpace(format)
                && Guid.TryParseExact(trimmedValue, format, out Guid parseResult))
            {
                return parseResult;
            }

            if (Guid.TryParse(trimmedValue, out parseResult))
                return parseResult;
        }

        return defaultResult;
    }
}