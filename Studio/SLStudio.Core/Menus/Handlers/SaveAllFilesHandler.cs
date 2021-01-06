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

        public override Task Execute(IMenuItem menu, object parameter)
        {
            foreach (var file in shell.Workspaces.OfType<IFileDocumentItem>().Where(i => i.IsDirty))
                fileService.Save(file);

            return Task.CompletedTask;
        }
    }
}