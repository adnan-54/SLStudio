using DevExpress.Mvvm;
using System;

namespace SLStudio.Core
{
    public class PanelItemBase : ViewModelBase, IPanelItem
    {
        public string Id => Guid.NewGuid().ToString();

        public string DisplayName
        {
            get => GetProperty(() => DisplayName);
            set => SetProperty(() => DisplayName, value);
        }

        public bool IsSelected
        {
            get => GetProperty(() => IsSelected);
            set => SetProperty(() => IsSelected, value);
        }

        public bool IsActive
        {
            get => GetProperty(() => IsActive);
            set => SetProperty(() => IsActive, value);
        }

        public virtual void OnActivated()
        {
        }

        public virtual void OnDeactivated()
        {
        }

        public void Dispose()
        {
            OnDisposed();
        }

        public virtual void OnDisposed()
        {
        }

        public virtual void Activate()
        {
            IsSelected = true;
            IsActive = true;
        }
    }
}