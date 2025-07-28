namespace PurcellLibs.Converters;

/// <summary>
/// 值转换器的接口，用于电子表格单元格值的类型转换。
/// </summary>
/// <remarks>
/// <para>电子表格单元格的数据类型是确定的，输入值是以下类型之一：<see langword="null"/>、<see langword="bool"/>、<see langword="double"/>、<see langword="string"/>、<see cref="DateTime"/>、<see cref="TimeSpan"/>。</para>
/// <para>实现此接口时，应针对这些特定类型编写转换逻辑，例如使用 switch-case 语句处理每种可能的输入类型。</para>
/// </remarks>
public interface IValueConverter
{
    /// <summary>
    /// 将单元格值转换为指定目标类型的值。
    /// </summary>
    /// <param name="value">要转换的单元格值，仅为以下类型之一：<see langword="null"/>、<see langword="bool"/>、<see langword="double"/>、<see langword="string"/>、<see cref="DateTime"/>、<see cref="TimeSpan"/>。</param>
    /// <param name="targetType">要转换到的目标类型。</param>
    /// <param name="columnConfig">列配置信息，包含格式化字符串等。</param>
    /// <param name="culture">用于转换的文化信息，来自 <see cref="PurTable"/> 的设置；若 <see cref="PurTable"/> 未设置文化信息，则为 <see cref="CultureInfo.InvariantCulture"/>。</param>
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