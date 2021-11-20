using System;
using System.ComponentModel;

namespace SLStudio
{
    public abstract class WindowViewModel : ViewModel, IWindowViewModel
    {
        private IWindowService WindowService => GetService<IWindowService>();

        protected WindowViewModel()
        {
            Title = GetType().Name;
        }

        public event CancelEventHandler Closing;

        public event EventHandler Closed;

        public virtual string Title
        {
            get => GetValue<string>();
            protected set => SetValue(value);
        }

        public virtual bool HasResult => Result is not null;

        public virtual object Result
        {
            get => GetValue<object>();
            protected set
            {
                if (SetValue(value))
                    RaisePropertyChanged(nameof(HasResult));
            }
        }

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
            return Result as TResult;
        }

        bool IWindowViewModel.TryGetResult<TResult>(out TResult result)
        {
            result = Result as TResult;
            return result is not null;
        }

        void IWindowViewModel.OnClosing(CancelEventArgs args)
        {
            Closing?.Invoke(this, args);
        }

        void IWindowViewModel.OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
