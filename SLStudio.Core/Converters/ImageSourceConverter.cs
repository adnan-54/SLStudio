using SharpVectors.Converters;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SLStudio.Core.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        private static readonly SvgImageConverterExtension svgConverter = new SvgImageConverterExtension();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
                if (WpfHelpers.TryFindResource(path, out DrawingImage image))
                    return image;
                else
                if (Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out Uri result))
                    value = result;

            if (value is Uri uri)
            {
                if (Path.HasExtension(uri.AbsoluteUri) && Path.GetExtension(uri.AbsoluteUri) == ".svg")
                    return svgConverter.Convert(uri, targetType, parameter, culture);

                return new BitmapImage(uri);
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}