using DevExpress.Mvvm;
using SLStudio.Core.Behaviors;
using System;
using System.ComponentModel;

namespace SLStudio.Core
{
    public abstract class WindowViewModel : ViewModelBase, IWindow
    {
        private ICurrentWindowBehavior CurrentWindow => GetService<ICurrentWindowBehavior>() ?? throw new InvalidOperationException($"{nameof(ICurrentWindowBehavior)} was not applied in the view");

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

        public virtual void OnLoaded()
        {
        }

        public virtual void OnActivated()
        {
        }

        public virtual void OnClosing(CancelEventArgs args)
        {
        }

        public virtual void OnClosed()
        {
        }
    }
}