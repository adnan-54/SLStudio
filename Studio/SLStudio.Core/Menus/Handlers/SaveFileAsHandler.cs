using DevExpress.Mvvm;
using SLStudio.Core.Menus.Resources;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class SaveFileAsHandler : SaveFileBaseHandler
    {
        private readonly IShell shell;

        public SaveFileAsHandler(IShell shell, IFileService fileService, IMessenger messenger) : base(fileService, messenger)
        {
            this.shell = shell;
        }

        public override string DisplayName => MenuResources.file_saveAs;

        public override string DisplayNameFormat => MenuResources.file_saveAs_format;

        public override async Task Execute(IMenuItem menu, object parameter)
        {
            if (shell.ActiveWorkspace is IFileDocumentItem file)
                await FileService.SaveAs(file);
        }
    }
}