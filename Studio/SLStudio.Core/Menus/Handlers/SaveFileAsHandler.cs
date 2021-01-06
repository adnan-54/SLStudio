using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class SaveFileAsHandler : MenuCommandHandler
    {
        private readonly IShell shell;
        private readonly IFileService fileService;

        public SaveFileAsHandler(IShell shell, IFileService fileService)
        {
            this.shell = shell;
            this.fileService = fileService;
        }

        public override bool CanExecute(IMenuItem menu, object parameter)
        {
            return shell.ActiveWorkspace is IFileDocumentItem file && file.IsDirty;
        }

        public override Task Execute(IMenuItem menu, object parameter)
        {
            if (shell.ActiveWorkspace is IFileDocumentItem file)
                fileService.SaveAs(file);

            return Task.CompletedTask;
        }
    }
}