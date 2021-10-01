using ICSharpCode.AvalonEdit.Editing;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace SLStudio.Core
{
    public static class WpfHelpers
    {
        //AvalonDock
        public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.RegisterAttached("CaretBrush", typeof(Brush), typeof(WpfHelpers), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(15, 15, 15)), OnCaretBrushChanged));

        public static void SetCaretBrush(DependencyObject d, Brush value)
        {
            d.SetValue(CaretBrushProperty, value);
        }

        public static Brush GetCaretBrush(DependencyObject d)
        {
            return (Brush)d.GetValue(CaretBrushProperty);
        }

        private static void OnCaretBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextArea textArea || e.NewValue is not Brush brush)
                return;
            textArea.Caret.CaretBrush = brush;
        }

        //WindowStartupLocation
        public static readonly DependencyProperty WindowStartupLocationProperty = DependencyProperty.RegisterAttached("WindowStartupLocation", typeof(WindowStartupLocation), typeof(WpfHelpers), new UIPropertyMetadata(WindowStartupLocation.Manual, OnWindowStartupLocationChanged));

        public static void SetWindowStartupLocation(DependencyObject DepObject, WindowStartupLocation value)
        {
            DepObject.SetValue(WindowStartupLocationProperty, value);
        }

        public static WindowStartupLocation GetWindowStartupLocation(DependencyObject DepObject)
        {
            return (WindowStartupLocation)DepObject.GetValue(WindowStartupLocationProperty);
        }

        private static void OnWindowStartupLocationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Window window)
                window.WindowStartupLocation = GetWindowStartupLocation(window);
        }

        //Watermark
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(WpfHelpers), new FrameworkPropertyMetadata(string.Empty));

        public static void SetWatermark(DependencyObject element, string value)
        {
            element.SetValue(WatermarkProperty, value);
        }

        public static string GetWatermark(DependencyObject element)
        {
            return (string)element.GetValue(WatermarkProperty);
        }

        //RightContent
        public static readonly DependencyProperty RightContentProperty = DependencyProperty.RegisterAttached("RightContent", typeof(object), typeof(WpfHelpers), new FrameworkPropertyMetadata());

        public static void SetRightContent(DependencyObject element, object value)
        {
            element.SetValue(RightContentProperty, value);
        }

        public static object GetRightContent(DependencyObject element)
        {
            return element.GetValue(RightContentProperty);
        }

        //Helpers
        public static bool TryFindResource<TResource>(object resourceKey, out TResource resource) where TResource : class
        {
            resource = Application.Current.TryFindResource(resourceKey) as TResource;
            return resource != null;
        }

        public static bool TryFindStyle(object styleKey, out Style style)
        {
            TryFindResource(styleKey, out style);
            return style != null;
        }

        public static bool TryFindDefaultStyle<TType>(out Style style) where TType : FrameworkElement
        {
            TryFindStyle(typeof(TType), out style);
            return style != null;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T t)
                        yield return t;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
    }
}