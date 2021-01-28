using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class SaveAllFilesHandler : MenuCommandHandler
    {
        private readonly IShell shell;
        private readonly IFileService fileService;

        public SaveAllFilesHandler(IShell shell, IFileService fileService)
        {
            this.shell = shell;
            this.fileService = fileService;
        }

        public override bool CanExecute(IMenuItem menu, object parameter)
        {
            return shell.Workspaces.OfType<IFileDocumentItem>().Any(f => !fileService.GetDescription(f).ReadOnly);
        }

        public override async Task Execute(IMenuItem menu, object parameter)
        {
            foreach (var file in shell.Workspaces.OfType<IFileDocumentItem>().Where(f => f.IsDirty))
                await fileService.Save(file);
        }
    }
}