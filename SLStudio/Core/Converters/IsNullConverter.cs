using System;
using System.Globalization;
using System.Windows.Data;

namespace SLStudio.Core.Converters
{
    public class IsNullConverter : IValueConverter
    {
        public bool Negate { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if (value == null)
                result = true;
            else if (value is string && string.IsNullOrEmpty(value as string))
                result = true;
            else if (string.IsNullOrEmpty(value.ToString()))
                result = true;

            return Negate ? !result : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
