using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace SharpEdge.Extensions;

public static class TypeExtensions
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

        return Nullable.GetUnderlyingType(type) is not null;
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

        return !type.IsValueType || Nullable.GetUnderlyingType(type) is not null;
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
}