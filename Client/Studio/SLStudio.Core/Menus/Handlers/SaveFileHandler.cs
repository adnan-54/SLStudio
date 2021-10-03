using DevExpress.Mvvm;
using SLStudio.Core.Menus.Resources;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class SaveFileHandler : SaveFileBaseHandler
    {
        private readonly IShell shell;

        public SaveFileHandler(IShell shell, IFileService fileService, IMessenger messenger) : base(fileService, messenger)
        {
            this.shell = shell;
        }

        public override string DisplayName => MenuResources.file_save;

        public override string DisplayNameFormat => MenuResources.file_save_format;

        public override async Task Execute(IMenuItem menu, object parameter)
        {
            if (shell.ActiveWorkspace is IFileDocumentItem file)
                await FileService.Save(file);
        }
    }
}