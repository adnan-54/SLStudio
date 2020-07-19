using System;
using System.Globalization;
using System.Windows.Data;

namespace SLStudio.Core.Converters
{
    public class IconSourceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is IconSourceType sourceType))
                return false;

            return sourceType switch
            {
                IconSourceType.FromUri => value is Uri,
                IconSourceType.FromResource => value is string,
                _ => false
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public enum IconSourceType
    {
        FromUri,
        FromResource
    }
}