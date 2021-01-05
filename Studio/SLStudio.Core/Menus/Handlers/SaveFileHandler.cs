using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class SaveFileHandler : MenuCommandHandler
    {
        private readonly IShell shell;
        private readonly IFileService fileService;

        public SaveFileHandler(IShell shell, IFileService fileService)
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
                fileService.Save(file);

            return Task.CompletedTask;
        }
    }

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

    internal class SaveAllFilesHandler : MenuCommandHandler
    {
        private readonly IShell shell;
        private readonly IFileService fileService;

        public SaveAllFilesHandler(IShell shell, IFileService fileService)
        {
            this.shell = shell;
            this.fileService = fileService;
        }

        public override Task Execute(IMenuItem menu, object parameter)
        {
            foreach (var file in shell.Workspaces.OfType<IFileDocumentItem>().Where(i => i.IsDirty))
                fileService.Save(file);

            return Task.CompletedTask;
        }
    }
}