using SLStudio.Core.Menus;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SLStudio.Core.Library.Menus
{
    internal class MenuCommandHandlerWrapper : ICommand
    {
        private readonly IMenuItem menuItem;
        private readonly IMenuCommandHandler commandHandler;

        public MenuCommandHandlerWrapper(IMenuItem menuItem, IMenuCommandHandler commandHandler)
        {
            this.menuItem = menuItem;
            this.commandHandler = commandHandler;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return commandHandler.CanExecute(menuItem, parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteAsync(parameter).FireAndForget();
        }

        private async Task ExecuteAsync(object parameter)
        {
            if (CanExecute(parameter))
                await commandHandler.Execute(menuItem, parameter);
        }
    }
}