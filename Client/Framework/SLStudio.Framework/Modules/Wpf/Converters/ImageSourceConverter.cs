using SharpVectors.Converters;
using SLStudio.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SLStudio
{
    public class ImageSourceConverter : MarkupExtension, IValueConverter
    {
        private static readonly ILogger logger = LogManager.GetLogger<ImageSourceConverter>();
        private static readonly SvgImageConverterExtension svgConverter = new();
        private static ImageSourceConverter instance;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance ??= new ImageSourceConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return value switch
                {
                    ImageSource imageSource => imageSource,
                    string key => TryGetFromKey(key),
                    Uri uri => TryGetFromUri(uri),
                    _ => Binding.DoNothing
                };
            }
            catch (Exception ex)
            {
                logger.Warn(ex);
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        private static ImageSource TryGetFromKey(string key)
        {
            return Uri.TryCreate(key, UriKind.RelativeOrAbsolute, out var uri)
                ? TryGetFromUri(uri)
                : WpfHelpers.TryFindResource(key, out ImageSource imageSource) ? imageSource : default;
        }

        private static ImageSource TryGetFromUri(Uri uri)
        {
            return IsSvgFile(uri) ? FromSvg(uri) : new BitmapImage(uri);
        }

        private static bool IsSvgFile(Uri uri)
        {
            var path = uri.AbsoluteUri;
            return !string.IsNullOrEmpty(path) && Path.GetExtension(path).EndsWith(".svg", StringComparison.InvariantCultureIgnoreCase);
        }

        private static ImageSource FromSvg(Uri uri)
        {
            return svgConverter.Convert(uri, null, null, null) as ImageSource;
        }
    }
}
