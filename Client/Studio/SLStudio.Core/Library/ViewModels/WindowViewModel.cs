using DevExpress.Mvvm;
using SLStudio.Core.Behaviors;
using System;
using System.ComponentModel;

namespace SLStudio.Core
{
    public abstract class WindowViewModel : ViewModelBase, IWindow
    {
        private bool eventsInitialized = false;

        private ICurrentWindowBehavior CurrentWindow => GetService<ICurrentWindowBehavior>() ?? throw new InvalidOperationException($"{nameof(ICurrentWindowBehavior)} was not applied in the view");

        public event EventHandler Loaded;

        public event EventHandler Activated;

        public event EventHandler<CancelEventArgs> Closing;

        public event EventHandler Closed;

        public string DisplayName
        {
            get => GetProperty(() => DisplayName);
            set => SetProperty(() => DisplayName, value);
        }

        public void Activate()
        {
            CurrentWindow.Activate();
        }

        public void Minimize()
        {
            CurrentWindow.Minimize();
        }

        public void Restore()
        {
            CurrentWindow.Restore();
        }

        public void Maximize()
        {
            CurrentWindow.Maximize();
        }

        public virtual void TryClose()
        {
            TryClose(null);
        }

        public virtual void TryClose(bool? dialogResult)
        {
            CurrentWindow.TryClose(dialogResult);
        }

        protected virtual void OnLoaded()
        {
        }

        protected virtual void OnActivated()
        {
        }

        protected virtual void OnClosing(CancelEventArgs args)
        {
        }

        protected virtual void OnClosed()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void SetupEvents(CurrentWindowBehavior currentWindow)
        {
            if (eventsInitialized || currentWindow != CurrentWindow)
                return;
            eventsInitialized = true;

            currentWindow.SetLoadedAction(RaiseLoaded);
            currentWindow.SetActivatedAction(RaiseActivated);
            currentWindow.SetClosingAction(RaiseClosing);
            currentWindow.SetClosedAction(RaiseClosed);
        }

        private void RaiseLoaded(object sender, EventArgs e)
        {
            Loaded?.Invoke(sender, e);
            OnLoaded();
        }

        private void RaiseActivated(object sender, EventArgs e)
        {
            Activated?.Invoke(sender, e);
            OnActivated();
        }

        private void RaiseClosing(object sender, CancelEventArgs e)
        {
            Closing?.Invoke(sender, e);
            OnClosing(e);
        }

        private void RaiseClosed(object sender, EventArgs e)
        {
            Closed?.Invoke(sender, e);
            OnClosed();
        }
    }
}