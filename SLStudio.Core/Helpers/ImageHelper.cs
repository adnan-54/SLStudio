using System.Windows;
using System.Windows.Controls;

namespace SLStudio.Core
{
    public static class ImageHelper
    {
        public static DependencyProperty ImageSourceProperty = DependencyProperty.RegisterAttached("ImageSource", typeof(object), typeof(ImageHelper), new PropertyMetadata(string.Empty, OnImageSourceChanged));

        public static void SetImageSource(DependencyObject d, object value)
        {
            d.SetValue(ImageSourceProperty, value);
        }

        public static object GetImageSource(DependencyObject d)
        {
            return d.GetValue(ImageSourceProperty);
        }

        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Image element)
                element.SetResourceReference(Image.SourceProperty, e.NewValue);
        }
    }
}