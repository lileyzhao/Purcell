namespace PurcellLibs.Converters;

/// <summary>
/// 值转换器基类，提供转换器的通用抽象。
/// </summary>
internal abstract class BaseConverter : IValueConverter
{
    /// <inheritdoc/>
    public abstract object? Convert(object? value, Type targetType, PurColumn columnConfig, CultureInfo culture);
}