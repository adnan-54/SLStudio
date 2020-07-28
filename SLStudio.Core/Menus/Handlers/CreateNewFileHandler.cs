using DevExpress.Mvvm.POCO;
using SLStudio.Core.Modules.NewFile.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace SLStudio.Core.Menus.Handlers
{
    internal class CreateNewFileHandler : MenuCommandHandler
    {
        private readonly IWindowManager windowManager;
        private readonly IShell shell;
        private readonly IUiSynchronization uiSynchronization;
        private readonly IFileService fileService;

        public CreateNewFileHandler(IWindowManager windowManager, IShell shell, IUiSynchronization uiSynchronization)
        {
            this.windowManager = windowManager;
            this.shell = shell;
            this.uiSynchronization = uiSynchronization;
            this.fileService = shell.GetService<IFileService>();
        }

        public override Task Execute(IMenuItem menu, object parameter)
        {
            var newFileDialog = new NewFileViewModel(shell, uiSynchronization);
            var result = windowManager.ShowDialog(newFileDialog);
            if (result == true)
            {
            }
            return Task.CompletedTask;
        }
    }
}