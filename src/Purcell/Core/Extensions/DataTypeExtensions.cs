namespace PurcellLibs.Extensions;

/// <summary>
/// 提供 DataType 枚举与 .NET Type 之间的转换扩展方法。
/// </summary>
public static class DataTypeExtensions
{
    /// <summary>
    /// DataType 到 .NET Type 的映射表。
    /// </summary>
    private static readonly Dictionary<DataType, Type> TypeMap = new()
    {
        [DataType.Unsupported] = typeof(object),
        [DataType.Int32] = typeof(int),
        [DataType.Int64] = typeof(long),
        [DataType.Single] = typeof(float),
        [DataType.Double] = typeof(double),
        [DataType.Decimal] = typeof(decimal),
        [DataType.Char] = typeof(char),
        [DataType.String] = typeof(string),
        [DataType.Boolean] = typeof(bool),
        [DataType.DateTime] = typeof(DateTime),
        [DataType.DateTimeOffset] = typeof(DateTimeOffset),
        [DataType.TimeSpan] = typeof(TimeSpan),
        [DataType.DateOnly] = typeof(DateOnly),
        [DataType.TimeOnly] = typeof(TimeOnly),
        [DataType.Guid] = typeof(Guid)
    };

    /// <summary>
    /// .NET Type 到 DataType 的反向映射表。
    /// </summary>
    private static readonly Dictionary<Type, DataType> ReverseTypeMap =
        TypeMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    /// <summary>
    /// 获取 <see cref="DataType"/> 对应的 .NET <see cref="Type"/>。
    /// </summary>
    /// <param name="dataType">要转换的 <see cref="DataType"/> 值。</param>
    /// <returns>对应的 .NET <see cref="Type"/>；如果找不到映射则返回 <see cref="object"/> 类型。</returns>
    public static Type ToType(this DataType dataType)
    {
        return TypeMap.TryGetValue(dataType, out Type? type) ? type : typeof(object);
    }

    /// <summary>
    /// 从 .NET <see cref="Type"/> 获取对应的 <see cref="DataType"/>。
    /// </summary>
    /// <param name="type">要转换的 .NET <see cref="Type"/>。</param>
    /// <returns>对应的 <see cref="DataType"/>；如果找不到映射或 <paramref name="type"/> 为 <see langword="null"/> 则返回 <see cref="DataType.Unsupported"/>。</returns>
    public static DataType ToDataType(this Type type)
    {
        if (type == null!) return DataType.Unsupported;

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
        }

        return ReverseTypeMap.GetValueOrDefault(type, DataType.Unsupported);
    }

    /// <summary>
    /// 检查指定的 <see cref="DataType"/> 是否为数值类型。
    /// </summary>
    /// <param name="dataType">要检查的 <see cref="DataType"/> 值。</param>
    /// <returns>如果 <paramref name="dataType"/> 为数值类型（Int32、Int64、Single、Double、Decimal）则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    public static bool IsNumeric(this DataType dataType)
    {
        return dataType switch
        {
            DataType.Int32 or DataType.Int64 or DataType.Single or DataType.Double or DataType.Decimal => true,
            _ => false
        };
    }
}