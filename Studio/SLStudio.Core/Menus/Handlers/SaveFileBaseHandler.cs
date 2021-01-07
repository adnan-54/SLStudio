using DevExpress.Mvvm;
using SLStudio.Core.Menus.Resources;

namespace SLStudio.Core.Menus.Handlers
{
    internal abstract class SaveFileBaseHandler : MenuCommandHandler
    {
        private IFileDocumentItem activeFileDocument;

        public SaveFileBaseHandler(IFileService fileService, IMessenger messenger)
        {
            FileService = fileService;

            messenger.Register<ActiveDocumentChangedEvent>(this, OnActiveDocumentChanged);
            messenger.Register<WorkspaceClosedEvent>(this, OnWorkspaceClosed);
        }

        public IFileService FileService { get; }

        public override bool CanExecute(IMenuItem menu, object parameter)
        {
            var displayName = GetDisplayName();
            if (menu.DisplayName != displayName)
                menu.DisplayName = displayName;

            return activeFileDocument != null && !FileService.GetDescription(activeFileDocument).ReadOnly;
        }

        private void OnActiveDocumentChanged(ActiveDocumentChangedEvent e)
        {
            activeFileDocument = e.NewItem as IFileDocumentItem;
        }

        private void OnWorkspaceClosed(WorkspaceClosedEvent e)
        {
            if (e.Item == activeFileDocument)
                activeFileDocument = null;
        }

        private string GetDisplayName()
        {
            if (activeFileDocument != null)
                return string.Format(MenuResources.file_saveAs_format, activeFileDocument.DisplayName);
            return MenuResources.file_saveAs;
        }
    }
}