using DevExpress.Mvvm;
using System;

namespace SLStudio
{
    public abstract class ViewModel : ViewModelBase, IViewModel, ILoadable
    {
        public event EventHandler Loaded;

        public event EventHandler Unloaded;

        void ILoadable.OnLoaded()
        {
            Loaded?.Invoke(this, EventArgs.Empty);
        }

        void ILoadable.OnUnloaded()
        {
            Unloaded?.Invoke(this, EventArgs.Empty);
        }
    }
}