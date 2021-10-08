using System.Windows;

namespace SLStudio
{
    public static class VisibilityHelper
    {
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached("IsVisible", typeof(bool?), typeof(VisibilityHelper), new FrameworkPropertyMetadata(default(bool?), FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, IsVisibleChangedCallback));

        public static void SetIsVisible(DependencyObject element, bool? value)
        {
            element.SetValue(IsVisibleProperty, value);
        }

        public static bool? GetIsVisible(DependencyObject element)
        {
            return (bool?)element.GetValue(IsVisibleProperty);
        }

        private static void IsVisibleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement fe)
                return;

            fe.Visibility = ((bool?)e.NewValue) == true ? Visibility.Visible : Visibility.Collapsed;
        }

        public static readonly DependencyProperty IsCollapsedProperty = DependencyProperty.RegisterAttached("IsCollapsed", typeof(bool?), typeof(VisibilityHelper), new FrameworkPropertyMetadata(default(bool?), FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, IsCollapsedChangedCallback));

        public static void SetIsCollapsed(DependencyObject element, bool? value)
        {
            element.SetValue(IsCollapsedProperty, value);
        }

        public static bool? GetIsCollapsed(DependencyObject element)
        {
            return (bool?)element.GetValue(IsCollapsedProperty);
        }

        private static void IsCollapsedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement fe)
                return;

            fe.Visibility = ((bool?)e.NewValue) == true ? Visibility.Collapsed : Visibility.Visible;
        }

        public static readonly DependencyProperty IsHiddenProperty = DependencyProperty.RegisterAttached("IsHidden", typeof(bool?), typeof(VisibilityHelper), new FrameworkPropertyMetadata(default(bool?), FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, IsHiddenChangedCallback));

        public static void SetIsHidden(DependencyObject element, bool? value)
        {
            element.SetValue(IsHiddenProperty, value);
        }

        public static bool? GetIsHidden(DependencyObject element)
        {
            return (bool?)element.GetValue(IsHiddenProperty);
        }

        private static void IsHiddenChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement fe)
                return;

            fe.Visibility = ((bool?)e.NewValue) == true ? Visibility.Hidden : Visibility.Visible;
        }
    }
}
