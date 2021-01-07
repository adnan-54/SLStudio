using DevExpress.Mvvm;

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

        public abstract string DisplayName { get; }

        public abstract string DisplayNameFormat { get; }

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
                return string.Format(DisplayNameFormat, activeFileDocument.DisplayName);
            return DisplayName;
        }
    }
}