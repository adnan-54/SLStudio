using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SLStudio.Core.Converters
{
    public class ZoomFactorConverter : MarkupExtension, IValueConverter
    {
        private static ZoomFactorConverter instance;

        public override object ProvideValue(IServiceProvider serviceProvider) => instance ??= new ZoomFactorConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double valueDouble)
                return Binding.DoNothing;

            return System.Convert.ToInt32(valueDouble * 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string valueString)
                return 1.0;

            valueString = valueString.TrimEnd('%');

            if (!double.TryParse(valueString, out var valueDouble))
                return Binding.DoNothing;

            if (valueDouble <= 20)
                return 0.2;
            if (valueDouble >= 400)
                return 4.0;

            return valueDouble / 100;
        }
    }
}