// ReSharper disable InconsistentNaming

namespace PurcellLibs.Converters;

/// <summary>
/// 布尔值转换器
/// </summary>
public sealed class BooleanConverter : IValueConverter
{
    private static readonly Lazy<BooleanConverter> _instance = new(() => new BooleanConverter());

    /// <inheritdoc cref="BooleanConverter"/>
    public static IValueConverter Instance => _instance.Value;

    private BooleanConverter()
    {
    }

    // 定义各种常见的布尔值表达方式及其对应的布尔值
    private readonly Dictionary<string, bool> _predefinedValues = new(StringComparer.OrdinalIgnoreCase)
    {
        // 英文常见表达
        ["true"] = true, ["false"] = false,
        ["t"] = true, ["f"] = false,
        ["yes"] = true, ["no"] = false,
        ["y"] = true, ["n"] = false,
        ["on"] = true, ["off"] = false,
        // 中文常见表达
        ["真"] = true, ["假"] = false,
        ["是"] = true, ["否"] = false,
        ["有"] = true, ["无"] = false
    };

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, CultureInfo culture, string? format)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        Type actualType = targetType.GetActualType();
        bool canBeNull = targetType.CanBeNull();

        // 验证目标类型是否为支持的类型
        if (actualType != typeof(bool))
        {
            throw new InvalidOperationException($"目标类型 {targetType.FullName} 不是支持的类型(bool)。");
        }

        bool? defaultResult = canBeNull ? null : false;

        // 值为null时返回默认结果
        if (value == null) return defaultResult;

        // 快速路径 - value类型范围：null、bool、double、string、DateTime、TimeSpan
        switch (value)
        {
            case bool b:
                return b;
            case double d when double.IsNaN(d) || double.IsInfinity(d):
                return defaultResult;
            case double d:
                return Math.Abs(d) > double.Epsilon;
            case DateTime dt:
                return dt > DateTime.MinValue;
            case TimeSpan ts:
                return ts > TimeSpan.Zero;
        }

        // 从字符串类型转换
        if (value is string strValue && !string.IsNullOrWhiteSpace(strValue))
        {
            ReadOnlySpan<char> trimmedValue = strValue.AsSpan().Trim();

            // 标准解析
            if (bool.TryParse(trimmedValue, out bool standardResult))
                return standardResult;

            // 数值解析
            if (double.TryParse(trimmedValue, out double doubleResult))
                return Math.Abs(doubleResult) > double.Epsilon;

            // 预定义值查找 - 先尝试使用原始字符串查找预定义值，减少不必要的字符串分配
            if (_predefinedValues.TryGetValue(strValue, out bool predefinedResult1))
                return predefinedResult1;

            // 预定义值查找 - 如果原始查找失败且有空白字符，再尝试使用 trimmed 版本
            if (strValue.Length != trimmedValue.Length)
            {
                if (_predefinedValues.TryGetValue(trimmedValue.ToString(), out bool trimmedResult))
                    return trimmedResult;
            }
        }

        return defaultResult;
    }
}