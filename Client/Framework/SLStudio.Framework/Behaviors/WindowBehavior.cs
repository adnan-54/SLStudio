using DevExpress.Mvvm.UI;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace SLStudio
{
    internal class WindowBehavior : ServiceBaseGeneric<Window>, IWindowService
    {
        private readonly IUiSynchronization uiSynchronization;

        public WindowBehavior(IUiSynchronization uiSynchronization)
        {
            this.uiSynchronization = uiSynchronization;
        }

        public object ViewModel => AssociatedObject?.DataContext;

        protected override void OnAttached()
        {
            base.OnAttached();

            BindTitle();

            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
            AssociatedObject.Closing += OnClosing;
            AssociatedObject.Closed += OnClosed;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnloaded;
            AssociatedObject.Closing -= OnClosing;
            AssociatedObject.Closed -= OnClosed;

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

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (ViewModel is not IClosable closable)
                return;

            closable.OnClosing(e);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            if (ViewModel is not IClosable closable)
                return;

            closable.OnClosed();
        }

        private void BindTitle()
        {
            if (ViewModel is not IHaveDisplayName)
                return;

            var binding = new Binding()
            {
                Path = new PropertyPath(nameof(IHaveDisplayName.DisplayName)),
                Mode = BindingMode.OneWay
            };

            AssociatedObject.SetBinding(Window.TitleProperty, binding);
        }

        public void Activate()
        {
            uiSynchronization.Execute(() =>
            {
                if (AssociatedObject.WindowState == WindowState.Minimized)
                    Restore();

                AssociatedObject.Focus();
            });
        }

        public void Maximize()
        {
            SystemCommands.MaximizeWindow(AssociatedObject);
        }

        public void Restore()
        {
            SystemCommands.RestoreWindow(AssociatedObject);
        }

        public void Minimize()
        {
            SystemCommands.MinimizeWindow(AssociatedObject);
        }

        public void Close()
        {
            SystemCommands.CloseWindow(AssociatedObject);
        }
    }
}