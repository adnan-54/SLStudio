using DevExpress.Mvvm.UI.Interactivity;
using System;
using System.ComponentModel;
using System.Windows;

namespace SLStudio
{
    internal class WindowBehavior : Behavior<Window>
    {
        private IWindowViewModel viewModel;

        protected override void OnAttached()
        {
            base.OnAttached();

            viewModel = AssociatedObject.DataContext as IWindowViewModel;

            AssociatedObject.DataContextChanged += OnDataContextChanged;
            AssociatedObject.Closing += OnClosing;
            AssociatedObject.Closed += OnClosed;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            viewModel = null;

            AssociatedObject.DataContextChanged -= OnDataContextChanged;
            AssociatedObject.Closing -= OnClosing;
            AssociatedObject.Closed -= OnClosed;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel = e.NewValue as IWindowViewModel;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            viewModel?.OnClosing(e);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            viewModel?.OnClosed();
        }
    }
}
