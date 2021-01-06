using DevExpress.Mvvm;
using SLStudio.Core.Menus.Resources;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class SaveFileAsHandler : MenuCommandHandler
    {
        private readonly IShell shell;
        private readonly IFileService fileService;
        private string displayName;

        public SaveFileAsHandler(IShell shell, IFileService fileService, IMessenger messenger)
        {
            this.shell = shell;
            this.fileService = fileService;

            messenger.Register<ActiveDocumentChangedEvent>(this, UpdateDisplayName);
        }

        public override bool CanExecute(IMenuItem menu, object parameter)
        {
            if (menu.DisplayName != displayName)
                menu.DisplayName = displayName;

            return shell.ActiveWorkspace is IFileDocumentItem file && !fileService.GetDescription(file).ReadOnly;
        }

        public override async Task Execute(IMenuItem menu, object parameter)
        {
            if (shell.ActiveWorkspace is IFileDocumentItem file)
                await fileService.SaveAs(file);
        }

        private void UpdateDisplayName(ActiveDocumentChangedEvent e)
        {
            if (e.NewItem != null && e.NewItem is IFileDocumentItem)
                displayName = string.Format(MenuResources.file_saveAs_format, e.NewItem.DisplayName);
            else
                displayName = MenuResources.file_saveAs;
        }
    }
}