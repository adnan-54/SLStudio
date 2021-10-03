using DevExpress.Mvvm.UI;
using System.Windows;
using System.Windows.Controls;

namespace SLStudio
{
    internal class ViewBehavior : ServiceBaseGeneric<UserControl>
    {
        public object ViewModel => AssociatedObject?.DataContext;

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnloaded;

            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel is not ILoadable loadable)
                return;

            loadable.OnLoaded();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel is not ILoadable loadable)
                return;

            loadable.OnUnloaded();
        }
    }
}