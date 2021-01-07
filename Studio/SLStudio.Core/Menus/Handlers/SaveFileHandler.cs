using DevExpress.Mvvm;
using SLStudio.Core.Menus.Resources;
using System;
using System.Linq;
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

        public override async Task Execute(IMenuItem menu, object parameter)
        {
            if (shell.ActiveWorkspace is IFileDocumentItem file)
                await FileService.Save(file);
        }
    }
}