using System;
using System.ComponentModel;

namespace SLStudio
{
    public abstract class WindowViewModel : ViewModel, IWindowViewModel, IHaveDisplayName, ILoadable, IClosable
    {
        private IWindowService WindowService => GetService<IWindowService>();

        public event CancelEventHandler Closing;

        public event EventHandler Closed;

        public virtual string DisplayName
        {
            get => GetValue<string>();
            protected set => SetValue(value);
        }

        public virtual object Result
        {
            get => GetValue<object>();
            protected set
            {
                SetValue(value);
                RaisePropertyChanged(nameof(HasResult));
            }
        }

        public virtual bool HasResult => Result is not null;

        public void Activate()
        {
            WindowService?.Activate();
        }

        public void Maximize()
        {
            WindowService?.Maximize();
        }

        public void Restore()
        {
            WindowService?.Restore();
        }

        public void Minimize()
        {
            WindowService?.Minimize();
        }

        public void Close()
        {
            WindowService?.Close();
        }

        public object GetResult()
        {
            return Result;
        }

        TResult IWindowViewModel.GetResult<TResult>()
        {
            if (HasResult && Result is TResult result)
                return result;

            return default;
        }

        bool IWindowViewModel.TryGetResult<TResult>(out TResult result)
        {
            result = (this as IWindowViewModel).GetResult<TResult>();

            return result is not null;
        }

        void IClosable.OnClosing(CancelEventArgs args)
        {
            Closing?.Invoke(this, args);
        }

        void IClosable.OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}