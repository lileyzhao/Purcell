namespace PurcellLibs.Converters;

/// <summary>
/// URI 转换器。
/// </summary>
public class UriConverter : IValueConverter
{
    // 单例实例的懒加载
    private static readonly Lazy<UriConverter> _instance = new(() => new UriConverter());

    /// <inheritdoc cref="UriConverter"/>
    public static IValueConverter Instance => _instance.Value;

    private UriConverter()
    {
    }

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, PurColumn columnConfig, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        Type actualType = targetType.GetActualType();
        bool canBeNull = targetType.CanBeNull();

        // 验证目标类型是否为支持的类型
        if (actualType != typeof(Uri))
        {
            throw new InvalidOperationException($"目标类型 {targetType.FullName} 不是支持的类型(Uri)。");
        }

        Uri? defaultResult = canBeNull ? null : new Uri("about:blank");

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
            if (Uri.TryCreate(strValue.Trim(), UriKind.RelativeOrAbsolute, out Uri? uriResult))
                return uriResult;
        }

        return defaultResult;
    }
}