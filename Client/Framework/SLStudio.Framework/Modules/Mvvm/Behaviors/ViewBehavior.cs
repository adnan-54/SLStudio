using DevExpress.Mvvm.UI.Interactivity;
using System.Windows;
using System.Windows.Controls;

namespace SLStudio
{
    internal class ViewBehavior : Behavior<Control>
    {
        private IViewModel viewModel;

        protected override void OnAttached()
        {
            base.OnAttached();

            viewModel = AssociatedObject.DataContext as IViewModel;

            if (AssociatedObject.IsLoaded)
                viewModel?.OnLoaded();

            AssociatedObject.DataContextChanged += OnDataContextChanged;
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            viewModel = null;

            AssociatedObject.DataContextChanged -= OnDataContextChanged;
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnloaded;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel?.OnUnloaded();
            viewModel = e.NewValue as IViewModel;
            if (AssociatedObject.IsLoaded)
                viewModel?.OnLoaded();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel?.OnLoaded();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            viewModel?.OnUnloaded();
        }
    }
}
