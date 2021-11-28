using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using QviKD.Types;

namespace QviKD.Functions
{
    /// <summary>
    /// Converter for binding data between <i>DDCCI.Display</i> and <i>DDCCI.IsVisible</i> property.
    /// </summary>
    [ValueConversion(typeof(Display), typeof(Visibility))]
    public class VisibilityPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException($"The {targetType} of the binding target is incompatible with Boolean data type.");

            return value is null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// Converter for binding data between <i>Display.InUse</i> and <i>Button.IsEnable</i> property.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InUsePropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException($"The {targetType} of the binding target is incompatible with Boolean data type.");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// Converter for binding data between <i>TextBlock.Text</i> and <i>Button.IsEnable</i> property.
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class EmptyStringPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException($"The {targetType} of the binding target is incompatible with String data type.");

            return value as string == string.Empty ? "(none)" : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
