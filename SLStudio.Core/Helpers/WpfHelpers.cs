using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace SLStudio.Core
{
    public static class WpfHelpers
    {
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