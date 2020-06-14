using Humanizer;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SLStudio.Core.Converters
{
    public class HumanizeDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
                return date.Humanize(false);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}