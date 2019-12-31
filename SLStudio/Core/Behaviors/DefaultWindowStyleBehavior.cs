using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Interactivity;

namespace SLStudio.Core.Behaviors
{
    public class DefaultWindowStyleBehavior : Behavior<MetroWindow>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject is MetroWindow window)
            {
                if (Application.Current.TryFindResource("DefaultWindowStyle") is Style style)
                    window.Style = style;
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            base.OnDetaching();
        }
    }
}