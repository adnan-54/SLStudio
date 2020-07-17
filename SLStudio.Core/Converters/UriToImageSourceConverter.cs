using SharpVectors.Converters;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SLStudio.Core.Converters
{
    public class UriToImageSourceConverter : IValueConverter
    {
        private static SvgImageConverterExtension svgConverter;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (!(value is string) && !(value is Uri)))
                return Binding.DoNothing;

            var uri = value is Uri newUri ? newUri : new Uri(value.ToString());
            if (Path.HasExtension(uri.AbsoluteUri) && Path.GetExtension(uri.AbsoluteUri) == ".svg")
            {
                if (svgConverter == null)
                    svgConverter = new SvgImageConverterExtension();
                return svgConverter.Convert(uri, targetType, parameter, culture);
            }
            return new BitmapImage(uri);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}