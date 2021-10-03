using DevExpress.Mvvm.UI;
using System;
using System.ComponentModel;
using System.Threading;
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

            if (AssociatedObject.DataContext is not WindowViewModel viewModel)
                throw new InvalidOperationException($"The Window DataContext does not implement {nameof(WindowViewModel)}");
            this.viewModel = viewModel;

            viewModel.SetupEvents(this);

            if (AssociatedObject.IsLoaded)
                LoadedAction?.Invoke(AssociatedObject, EventArgs.Empty);

            AssociatedObject.Loaded += WindowLoaded;
            AssociatedObject.Activated += WindowActivated;
            AssociatedObject.Closing += WindowClosing;
            AssociatedObject.Closed += WindowClosed;
        }

        public Action<object, EventArgs> LoadedAction { get; private set; }

        public Action<object, EventArgs> ActivatedAction { get; private set; }

        public Action<object, CancelEventArgs> ClosingAction { get; private set; }

        public Action<object, EventArgs> ClosedAction { get; private set; }

        internal void SetLoadedAction(Action<object, EventArgs> action)
        {
            if (LoadedAction == null)
                LoadedAction = action;
        }

        internal void SetActivatedAction(Action<object, EventArgs> action)
        {
            if (ActivatedAction == null)
                ActivatedAction = action;
        }

        internal void SetClosingAction(Action<object, CancelEventArgs> action)
        {
            if (ClosingAction == null)
                ClosingAction = action;
        }

        internal void SetClosedAction(Action<object, EventArgs> action)
        {
            if (ClosedAction == null)
                ClosedAction = action;
        }

        private void WindowLoaded(object sender, EventArgs e)
        {
            LoadedAction?.Invoke(sender, e);
        }

        private void WindowActivated(object sender, EventArgs e)
        {
            ActivatedAction?.Invoke(sender, e);
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            ClosingAction?.Invoke(sender, e);
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            ClosedAction?.Invoke(sender, e);
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
            AssociatedObject.Loaded -= WindowLoaded;
            AssociatedObject.Activated -= WindowActivated;
            AssociatedObject.Closing -= WindowClosing;
            AssociatedObject.Closed -= WindowClosed;

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