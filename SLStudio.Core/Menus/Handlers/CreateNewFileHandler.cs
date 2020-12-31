using SLStudio.Core.Modules.NewFile.ViewModels;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class CreateNewFileHandler : MenuCommandHandler
    {
        private readonly IFileService fileService;
        private readonly IWindowManager windowManager;

        public CreateNewFileHandler(IFileService fileService, IWindowManager windowManager)
        {
            this.fileService = fileService;
            this.windowManager = windowManager;
        }

        public override Task Execute(IMenuItem menu, object parameter)
        {
            var newFileDialog = new NewFileViewModel(fileService);
            windowManager.ShowDialog(newFileDialog);
            return Task.CompletedTask;
        }
    }
}