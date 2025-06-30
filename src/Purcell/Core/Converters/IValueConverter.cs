namespace PurcellLibs.Converters;

/// <summary>
/// 值转换器的接口，用于电子表格单元格值的类型转换。
/// 电子表格单元格的数据类型是确定的，输入值是以下类型之一：null、bool、double、string、DateTime、TimeSpan。
/// 实现此接口时，应针对这些特定类型编写转换逻辑，例如使用switch-case语句处理每种可能的输入类型。
/// </summary>
public interface IValueConverter
{
    /// <summary>
    /// 将单元格值转换为指定目标类型的值
    /// </summary>
    /// <param name="value">要转换的单元格值，仅为以下类型之一：null、bool、double、string、DateTime、TimeSpan。</param>
    /// <param name="targetType">要转换到的目标类型</param>
    /// <param name="columnConfig">格式化字符串</param>
    /// <param name="culture">用于转换的文化信息，来自PurTable的设置；若PurTable未设置文化信息，则为<c>CultureInfo.InvariantCulture</c></param>
    /// <returns>
    /// 转换后的值，其类型与目标类型完全匹配。转换规则如下：
    /// <list type="bullet">
    ///   <item>目标为值类型(如bool)：返回该类型的值，转换失败时返回类型默认值(如bool为false)</item>
    ///   <item>目标为可空值类型(如bool?)：返回该类型的值，转换失败时返回null</item>
    ///   <item>目标为引用类型：返回该类型的实例，转换失败时返回null</item>
    /// </list>
    /// </returns>
    object? Convert(object? value, Type targetType, PurColumn columnConfig, CultureInfo culture);
}