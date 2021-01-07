using DevExpress.Mvvm;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class CloseFileHandler : MenuCommandHandler
    {
        private readonly IShell shell;
        private IDocumentItem lastActiveDocument;

        public CloseFileHandler(IShell shell, IMessenger messenger)
        {
            this.shell = shell;

            messenger.Register<ActiveDocumentChangedEvent>(this, OnActiveDocumentChanged);
            messenger.Register<WorkspaceClosedEvent>(this, OnWorkspaceClosed);
        }

        public override bool CanExecute(IMenuItem menu, object parameter)
        {
            return lastActiveDocument != null;
        }

        public override async Task Execute(IMenuItem menu, object parameter)
        {
            await shell.CloseWorkspaces(lastActiveDocument);
        }

        private void OnActiveDocumentChanged(ActiveDocumentChangedEvent e)
        {
            lastActiveDocument = e.NewItem;
        }

        private void OnWorkspaceClosed(WorkspaceClosedEvent e)
        {
            if (e.Item == lastActiveDocument)
                lastActiveDocument = null;
        }
    }
}