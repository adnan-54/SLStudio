using DevExpress.Mvvm;
using System;

namespace SLStudio
{
    public abstract class ViewModel : ViewModelBase, IViewModel
    {
        public event EventHandler Loaded;

        public event EventHandler Unloaded;

        public bool IsLoaded
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        void IViewModel.OnLoaded()
        {
            if (IsLoaded)
                return;
            IsLoaded = true;
            Loaded?.Invoke(this, EventArgs.Empty);
        }

        void IViewModel.OnUnloaded()
        {
            if (!IsLoaded)
                return;
            IsLoaded = false;
            Unloaded?.Invoke(this, EventArgs.Empty);
        }
    }
}