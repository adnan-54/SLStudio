using SLStudio.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SLStudio
{
    public abstract class MenuButtonHandler : MenuItemHandler<IMenuButton>, IMenuButtonHandler
    {
        private static readonly ILogger logger = LogManager.GetLogger<MenuButtonHandler>();

        private event EventHandler IsExecutingChanged;

        event EventHandler ICommand.CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        event EventHandler IMenuButtonHandler.IsExecutingChanged
        {
            add => IsExecutingChanged += value;
            remove => IsExecutingChanged -= value;
        }

        public bool IsExecuting
        {
            get => GetValue<bool>();
            private set => SetValue(value, RaiseIsExecutingChanged);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return !IsExecuting && CanExecute();
        }

        async void ICommand.Execute(object parameter)
        {
            if (!CanExecute() || IsExecuting)
                return;

            try
            {
                IsExecuting = true;
                await Execute();
            }
            catch (Exception ex)
            {
                logger.Warn(ex);
            }
            finally
            {
                IsExecuting = false;
            }
        }

        protected virtual bool CanExecute()
        {
            return true;
        }

        protected abstract Task Execute();

        private void RaiseIsExecutingChanged()
        {
            IsExecutingChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
