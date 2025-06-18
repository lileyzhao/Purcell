namespace PurcellLibs.Converters;

/// <summary>
/// 版本号转换器
/// </summary>
public class VersionConverter : IValueConverter
{
    private static readonly Lazy<VersionConverter> _instance = new(() => new VersionConverter());

    /// <inheritdoc cref="VersionConverter"/>
    public static IValueConverter Instance => _instance.Value;

    private VersionConverter()
    {
    }

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, CultureInfo culture, string? format)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        Type actualType = targetType.GetActualType();
        bool canBeNull = targetType.CanBeNull();

        // 验证目标类型是否为支持的类型
        if (actualType != typeof(Version))
        {
            throw new InvalidOperationException($"目标类型 {targetType.FullName} 不是支持的类型(Guid)。");
        }

        Version? defaultResult = canBeNull ? null : new Version(0, 0, 0, 0);

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
            if (Version.TryParse(strValue.AsSpan().Trim(), out Version? versionResult))
                return versionResult;
        }

        return defaultResult;
    }
}