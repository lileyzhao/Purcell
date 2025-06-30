namespace PurcellLibs.Converters;

/// <summary>
/// String转换器
/// </summary>
public class StringConverter : IValueConverter
{
    private static readonly Lazy<StringConverter> _instance = new(() => new StringConverter());

    /// <inheritdoc cref="StringConverter"/>
    public static IValueConverter Instance => _instance.Value;

    private StringConverter()
    {
    }

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, PurColumn columnConfig, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        Type actualType = targetType.GetActualType();
        bool canBeNull = targetType.CanBeNull();

        // 验证目标类型是否为支持的类型
        if (actualType != typeof(string))
        {
            throw new InvalidOperationException($"目标类型 {targetType.FullName} 不是支持的类型(string)。");
        }

        // 值为null时返回默认结果
        if (value == null) return canBeNull ? null : string.Empty;

        return value.ToString();
    }
}