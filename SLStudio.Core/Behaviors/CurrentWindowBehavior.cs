using DevExpress.Mvvm.UI;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;

namespace SLStudio.Core.Behaviors
{
    internal class CurrentWindowBehavior : ServiceBaseGeneric<Window>, ICurrentWindowBehavior
    {
        private WindowViewModel viewModel;

        protected override void OnAttached()
        {
            base.OnAttached();
            if (!(AssociatedObject.DataContext is WindowViewModel windowScreen))
                throw new InvalidOperationException($"The Window DataContext does not implement {nameof(WindowViewModel)}");

            viewModel = windowScreen;
            if (AssociatedObject.IsLoaded)
                windowScreen.OnLoaded();

            AssociatedObject.ContentRendered += WindowLoaded;
            AssociatedObject.Closing += WindowClosing;
            AssociatedObject.Closed += WindowClosed;
            AssociatedObject.Activated += WindowActivated;
        }

        private void WindowLoaded(object sender, EventArgs e)
        {
            viewModel.OnLoaded();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            viewModel.OnClosing(e);
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            viewModel.OnClosed();
        }

        private void WindowActivated(object sender, EventArgs e)
        {
            viewModel.OnActivated();
        }

        public void Activate()
        {
            if (AssociatedObject.WindowState == WindowState.Minimized)
                Restore();
            AssociatedObject.Focus();
        }

        public void Minimize()
        {
            SystemCommands.MinimizeWindow(AssociatedObject);
        }

        public void Restore()
        {
            SystemCommands.RestoreWindow(AssociatedObject);
        }

        public void Maximize()
        {
            SystemCommands.MaximizeWindow(AssociatedObject);
        }

        public void TryClose(bool? dialogResult)
        {
            if (ComponentDispatcher.IsThreadModal && dialogResult.HasValue)
                AssociatedObject.DialogResult = dialogResult;
            AssociatedObject.Close();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ContentRendered -= WindowLoaded;
            AssociatedObject.Closing -= WindowClosing;
            AssociatedObject.Closed -= WindowClosed;
            AssociatedObject.Activated -= WindowActivated;

            base.OnDetaching();
        }
    }

    internal interface ICurrentWindowBehavior
    {
        void Activate();

        void Minimize();

        void Restore();

        void Maximize();

        void TryClose(bool? dialogResult);
    }
}