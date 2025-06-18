using System.Net;
using BooleanConverter = PurcellLibs.Converters.BooleanConverter;
using DateTimeConverter = PurcellLibs.Converters.DateTimeConverter;
using EnumConverter = PurcellLibs.Converters.EnumConverter;
using GuidConverter = PurcellLibs.Converters.GuidConverter;
using StringConverter = PurcellLibs.Converters.StringConverter;
using TimeSpanConverter = PurcellLibs.Converters.TimeSpanConverter;
using VersionConverter = PurcellLibs.Converters.VersionConverter;

namespace PurcellLibs.Extensions;

/// <summary>
/// 反射相关扩展方法类
/// </summary>
internal static class ReflectionExtensions
{
    // 缓存默认值以提高性能
    private static readonly ConcurrentDictionary<Type, object?> _defaultValueCache = new();

    /// <summary>
    /// 检查类型是否为 <see cref="Nullable{T}"/>
    /// </summary>
    /// <param name="type">要检查的类型</param>
    /// <returns>如果是 <see cref="Nullable{T}"/> 则返回 true，否则返回 false</returns>
    /// <exception cref="ArgumentNullException">当 type 为 null 时抛出</exception>
    private static bool IsNullable(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return Nullable.GetUnderlyingType(type) != null;
    }

    /// <summary>
    /// 获取类型的实际类型（如果是 <see cref="Nullable{T}"/> 则返回 T，否则返回原类型）
    /// </summary>
    /// <param name="type">要获取实际类型的类型</param>
    /// <returns>实际类型</returns>
    /// <exception cref="ArgumentNullException">当 type 为 null 时抛出</exception>
    public static Type GetActualType(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return Nullable.GetUnderlyingType(type) ?? type;
    }

    /// <summary>
    /// 检查类型是否可以为 null（引用类型或 <see cref="Nullable{T}"/>）
    /// </summary>
    /// <param name="type">要检查的类型</param>
    /// <returns>如果可以为 null 则返回 true，否则返回 false</returns>
    /// <exception cref="ArgumentNullException">当 type 为 null 时抛出</exception>
    public static bool CanBeNull(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
    }

    /// <summary>
    /// 获取任意类型的默认值（带缓存优化）
    /// </summary>
    /// <param name="type">要获取默认值的类型</param>
    /// <returns>该类型的默认值</returns>
    /// <exception cref="ArgumentNullException">当 type 为 null 时抛出</exception>
    public static object? GetDefaultValue(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return _defaultValueCache.GetOrAdd(type, t =>
        {
            // 引用类型和可空值类型的默认值都是 null
            if (!t.IsValueType || t.IsNullable())
            {
                return null;
            }

            if (t == typeof(DateTime)) return new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (t == typeof(DateTimeOffset)) return new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero);
            if (t == typeof(DateOnly)) return new DateOnly(1900, 1, 1);

            // 不可空值类型使用 Activator.CreateInstance 创建默认值
            return Activator.CreateInstance(t);
        });
    }

    /// <summary>
    /// 检查类型是否为匿名类型
    /// </summary>
    /// <param name="type">要检查的类型</param>
    /// <returns>如果是匿名类型则返回 true，否则返回 false</returns>
    /// <exception cref="ArgumentNullException">当 type 为 null 时抛出</exception>
    public static bool IsAnonymousType(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        // 匿名类型通常带有 CompilerGeneratedAttribute
        // 并且名称包含一些特殊字符如 "<>f__AnonymousType"
        return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false) &&
               type.Name.Contains("AnonymousType") &&
               type.Name.StartsWith("<>");
    }

    /// <summary>
    /// 检查类型是否为数值类型
    /// </summary>
    /// <param name="type">要检查的类型</param>
    /// <param name="includeNullable">是否包含可空数值类型，默认为 true</param>
    /// <returns>如果是数值类型则返回 true，否则返回 false</returns>
    /// <exception cref="ArgumentNullException">当 type 为 null 时抛出</exception>
    public static bool IsNumericType(this Type type, bool includeNullable = true)
    {
        ArgumentNullException.ThrowIfNull(type);

        // 如果不包含可空类型且当前类型是可空类型，则返回 false
        if (!includeNullable && type.IsNullable())
        {
            return false;
        }

        // 获取实际类型（处理可空类型）
        Type actualType = type.GetActualType();

        return actualType == typeof(byte) ||
               actualType == typeof(sbyte) ||
               actualType == typeof(short) ||
               actualType == typeof(ushort) ||
               actualType == typeof(int) ||
               actualType == typeof(uint) ||
               actualType == typeof(long) ||
               actualType == typeof(ulong) ||
               actualType == typeof(float) ||
               actualType == typeof(double) ||
               actualType == typeof(decimal);
    }

    /// <summary>
    /// 获取属性上指定类型的特性实例
    /// </summary>
    /// <typeparam name="T">特性类型</typeparam>
    /// <param name="propertyInfo">属性信息</param>
    /// <param name="inherit">是否搜索继承的特性</param>
    /// <returns>特性实例，如果未找到则返回 null</returns>
    /// <exception cref="ArgumentNullException">当 propertyInfo 为 null 时抛出</exception>
    public static T? GetCustomAttribute<T>(this PropertyInfo propertyInfo, bool inherit = true)
        where T : Attribute
    {
        ArgumentNullException.ThrowIfNull(propertyInfo);

        return propertyInfo.GetCustomAttribute(typeof(T), inherit) as T;
    }

    /// <summary>
    /// 获取属性上指定类型的所有特性实例
    /// </summary>
    /// <typeparam name="T">特性类型</typeparam>
    /// <param name="propertyInfo">属性信息</param>
    /// <param name="inherit">是否搜索继承的特性</param>
    /// <returns>特性实例数组</returns>
    /// <exception cref="ArgumentNullException">当 propertyInfo 为 null 时抛出</exception>
    public static T[] GetCustomAttributes<T>(this PropertyInfo propertyInfo, bool inherit = true)
        where T : Attribute
    {
        ArgumentNullException.ThrowIfNull(propertyInfo);

        return propertyInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
    }

    /// <summary>
    /// 检查属性是否定义了指定类型的特性
    /// </summary>
    /// <typeparam name="T">特性类型</typeparam>
    /// <param name="propertyInfo">属性信息</param>
    /// <param name="inherit">是否搜索继承的特性</param>
    /// <returns>如果定义了特性则返回 true，否则返回 false</returns>
    /// <exception cref="ArgumentNullException">当 propertyInfo 为 null 时抛出</exception>
    public static bool HasCustomAttribute<T>(this PropertyInfo propertyInfo, bool inherit = true)
        where T : Attribute
    {
        ArgumentNullException.ThrowIfNull(propertyInfo);

        return propertyInfo.IsDefined(typeof(T), inherit);
    }

    /// <summary>
    /// 尝试获取属性上指定类型的特性实例
    /// </summary>
    /// <typeparam name="T">特性类型</typeparam>
    /// <param name="propertyInfo">属性信息</param>
    /// <param name="attribute">输出的特性实例</param>
    /// <param name="inherit">是否搜索继承的特性</param>
    /// <returns>如果找到特性则返回 true，否则返回 false</returns>
    /// <exception cref="ArgumentNullException">当 propertyInfo 为 null 时抛出</exception>
    public static bool TryGetCustomAttribute<T>(this PropertyInfo propertyInfo, out T? attribute, bool inherit = true)
        where T : Attribute
    {
        ArgumentNullException.ThrowIfNull(propertyInfo);

        attribute = propertyInfo.GetCustomAttribute<T>(inherit);

        return attribute != null;
    }

    /// <summary>
    /// 尝试获取指定类型的内置值转换器
    /// </summary>
    /// <param name="type">要转换到的目标类型</param>
    /// <param name="converter">转换器实例</param>
    /// <returns>如果找到转换器则返回 true，否则返回 false</returns>
    public static bool TryGetValueConverter(this Type type, [NotNullWhen(true)] out IValueConverter? converter)
    {
        ArgumentNullException.ThrowIfNull(type);

        Type actualType = type.GetActualType();

        converter = actualType switch
        {
            _ when actualType == typeof(bool) => BooleanConverter.Instance,
            _ when actualType.IsNumericType() => NumericConverter.Instance,
            _ when actualType == typeof(string) => StringConverter.Instance,
            _ when actualType == typeof(DateTime) || actualType == typeof(DateTimeOffset) || actualType == typeof(DateOnly) =>
                DateTimeConverter.Instance,
            _ when actualType == typeof(TimeSpan) || actualType == typeof(TimeOnly) => TimeSpanConverter.Instance,
            _ when actualType == typeof(Guid) => GuidConverter.Instance,
            _ when actualType == typeof(IPAddress) => IPAddressConverter.Instance,
            _ when actualType == typeof(Uri) => UriConverter.Instance,
            _ when actualType == typeof(Version) => VersionConverter.Instance,
            _ when actualType.IsEnum => EnumConverter.Instance,
            _ => null
        };

        return converter != null;
    }
}