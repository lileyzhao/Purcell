namespace PurcellLibs.Abstractions;

/// <summary>
/// .NET 基础数据类型枚举
/// </summary>
public enum DataType : byte
{
    /// <summary>
    /// 不支持的类型
    /// </summary>
    Unsupported = 0,

    /// <summary>
    /// System.Int32 (-2,147,483,648 to 2,147,483,647)
    /// </summary>
    Int32 = 5,

    /// <summary>
    /// System.Int64 (-9,223,372,036,854,775,808 to 9,223,372,036,854,775,807)
    /// </summary>
    Int64 = 7,

    // 浮点类型
    /// <summary>
    /// System.Single (32-bit floating point)
    /// </summary>
    Single = 10,

    /// <summary>
    /// System.Double (64-bit floating point)
    /// </summary>
    Double = 11,

    /// <summary>
    /// System.Decimal (128-bit high-precision decimal)
    /// </summary>
    Decimal = 12,

    // 字符和字符串
    /// <summary>
    /// System.Char (16-bit Unicode character)
    /// </summary>
    Char = 20,

    /// <summary>
    /// System.String (Unicode string)
    /// </summary>
    String = 21,

    // 布尔类型
    /// <summary>
    /// System.Boolean (true/false)
    /// </summary>
    Boolean = 30,

    // 日期时间类型
    /// <summary>
    /// System.DateTime (date and time)
    /// </summary>
    DateTime = 40,

    /// <summary>
    /// System.DateTimeOffset (date and time with timezone offset)
    /// </summary>
    DateTimeOffset = 41,

    /// <summary>
    /// System.TimeSpan (time interval)
    /// </summary>
    TimeSpan = 42,

#if NET6_0_OR_GREATER
    /// <summary>
    /// System.DateOnly (date without time, .NET 6+)
    /// </summary>
    DateOnly = 43,

    /// <summary>
    /// System.TimeOnly (time without date, .NET 6+)
    /// </summary>
    TimeOnly = 44,
#endif

    // 特殊类型
    /// <summary>
    /// System.Guid (globally unique identifier)
    /// </summary>
    Guid = 50,

    /// <summary>
    /// System.Object (base object type)
    /// </summary>
    Object = 60,

    /// <summary>
    /// 空值标识 (null value indicator)
    /// </summary>
    Null = byte.MaxValue
}