using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Appearance;

namespace MyWPFApp.Helpers;

internal class EnumToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is not String enumString)
        {
            throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
        }

        if (!Enum.IsDefined(typeof(ApplicationTheme), value))
        {
            throw new ArgumentException("ExceptionEnumToBooleanConverterValueMustBeAnEnum");
        }

        ApplicationTheme enumValue = Enum.Parse<ApplicationTheme>(enumString);

        return enumValue.Equals(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is not String enumString)
        {
            throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
        }

        return Enum.Parse<ApplicationTheme>(enumString);
    }
}
