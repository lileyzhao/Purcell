namespace PurcellLibs.Converters;

/// <summary>
/// IP地址转换器
/// </summary>
public class IPAddressConverter : IValueConverter
{
    private static readonly Lazy<IPAddressConverter> _instance = new(() => new IPAddressConverter());

    /// <inheritdoc cref="IPAddressConverter"/>
    public static IValueConverter Instance => _instance.Value;

    private IPAddressConverter()
    {
    }

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, PurColumn columnConfig, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        Type actualType = targetType.GetActualType();
        bool canBeNull = targetType.CanBeNull();

        // 验证目标类型是否为支持的类型
        if (actualType != typeof(Version))
        {
            throw new InvalidOperationException($"目标类型 {targetType.FullName} 不是支持的类型(Guid)。");
        }

        IPAddress? defaultResult = canBeNull ? null : IPAddress.None;

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
            if (IPAddress.TryParse(strValue.AsSpan().Trim(), out IPAddress? uriResult))
                return uriResult;
        }

        return defaultResult;
    }
}