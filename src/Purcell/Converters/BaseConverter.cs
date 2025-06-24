namespace PurcellLibs.Converters;

internal abstract class BaseConverter : IValueConverter
{
    public abstract object? Convert(object? value, Type targetType, PurColumn columnConfig, CultureInfo culture);
}