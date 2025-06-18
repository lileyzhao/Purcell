namespace PurcellLibs.Converters;

/// <summary>
/// 枚举转换器
/// </summary>
public class EnumConverter : IValueConverter
{
    private static readonly Lazy<EnumConverter> _instance = new(() => new EnumConverter());

    /// <inheritdoc cref="EnumConverter"/>
    public static IValueConverter Instance => _instance.Value;

    private EnumConverter()
    {
    }

    private static readonly ConcurrentDictionary<Type, EnumValueMappings> EnumMetadataCache = new();

    private class EnumValueMappings
    {
        public Dictionary<string, object> NameMapping { get; } = new(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, object> DescriptionMapping { get; } = new(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, object> EnumMemberMapping { get; } = new(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, object> PurEnumMapping { get; } = new(StringComparer.OrdinalIgnoreCase);
    }

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, CultureInfo culture, string? format)
    {
        ArgumentNullException.ThrowIfNull(targetType);

        Type actualType = targetType.GetActualType();
        bool canBeNull = targetType.CanBeNull();

        // 验证目标类型是否为支持的类型
        if (!actualType.IsEnum)
        {
            throw new InvalidOperationException($"目标类型 {targetType.FullName} 不是支持的类型(枚举类型)。");
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

        // 从数值类型转换
        if (value is double dblValue and > 0 and < long.MaxValue)
        {
            if (Enum.IsDefined(actualType, (long)dblValue))
                return Enum.ToObject(actualType, (long)dblValue);
        }

        // 从字符串类型转换
        if (value is string strValue && !string.IsNullOrWhiteSpace(strValue))
        {
            string trimmedValue = strValue.Trim();
            if (Enum.TryParse(actualType, trimmedValue, true, out object? enumResult))
                return enumResult;

            // 获取枚举映射
            EnumValueMappings mappings = EnumMetadataCache.GetOrAdd(actualType, CreateEnumMappings);

            // 按名称匹配（作为Enum.TryParse的备选，处理一些特殊情况）
            if (mappings.NameMapping.TryGetValue(trimmedValue, out enumResult))
            {
                return enumResult;
            }

            // 按 PurEnum 匹配
            if (mappings.PurEnumMapping.TryGetValue(trimmedValue, out enumResult))
            {
                return enumResult;
            }

            // 按 Description 匹配
            if (mappings.DescriptionMapping.TryGetValue(trimmedValue, out enumResult))
            {
                return enumResult;
            }

            // 按 DisplayName 匹配
            if (mappings.EnumMemberMapping.TryGetValue(trimmedValue, out enumResult))
            {
                return enumResult;
            }
        }

        return null;
    }

    private EnumValueMappings CreateEnumMappings(Type type)
    {
        EnumValueMappings metadata = new();

        foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            object? enumValue = field.GetValue(null);

            if (enumValue == null) continue; // 跳过未定义的枚举值

            // 添加枚举名称映射
            metadata.NameMapping[field.Name] = enumValue;

            // 添加 PurEnum 特性映射
            PurEnum? purEnumAttr = field.GetCustomAttribute<PurEnum>();
            if (purEnumAttr is { MappedValues.Length: > 0 })
            {
                foreach (string value in purEnumAttr.MappedValues)
                {
                    if (string.IsNullOrEmpty(value)) continue;
                    metadata.PurEnumMapping.TryAdd(value, enumValue);
                }
            }

            // 添加 Description 特性映射
            DescriptionAttribute? descAttr = field.GetCustomAttribute<DescriptionAttribute>();
            if (descAttr != null && !string.IsNullOrEmpty(descAttr.Description))
            {
                metadata.DescriptionMapping.TryAdd(descAttr.Description, enumValue);
            }

            // 添加 DisplayName 特性映射
            EnumMemberAttribute? enumMemberAttr = field.GetCustomAttribute<EnumMemberAttribute>();
            if (enumMemberAttr != null && !string.IsNullOrEmpty(enumMemberAttr.Value))
            {
                metadata.EnumMemberMapping.TryAdd(enumMemberAttr.Value, enumValue);
            }
        }

        return metadata;
    }
}