using SLStudio.Core.Modules.NewFile.ViewModels;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class CreateNewFileHandler : MenuCommandHandler
    {
        private readonly IWindowManager windowManager;
        private readonly IShell shell;
        private readonly IUiSynchronization uiSynchronization;

        public CreateNewFileHandler(IWindowManager windowManager, IShell shell, IUiSynchronization uiSynchronization)
        {
            this.windowManager = windowManager;
            this.shell = shell;
            this.uiSynchronization = uiSynchronization;
        }

        public override Task Execute(IMenuItem menu, object parameter)
        {
            var newFileDialog = new NewFileViewModel(shell, uiSynchronization);
            windowManager.ShowDialog(newFileDialog);
            return Task.CompletedTask;
        }
    }
}