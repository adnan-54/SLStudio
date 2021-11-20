using System.Collections.Generic;

namespace SLStudio
{
    public interface IWorkspace : IViewModel
    {
        IEnumerable<IWorkspaceItem> Workspaces { get; }

        IEnumerable<IWorkspaceTool> Tools { get; }

        IEnumerable<IWorkspaceDocument> Documents { get; }

        IWorkspaceDocument LastFocusedDocument { get; }

        IWorkspaceItem SelectedWorkspace { get; }

        TWorkspace Show<TWorkspace>()
            where TWorkspace : class, IWorkspaceItem;

        void Show(params IWorkspaceItem[] workspaces);

        void Close(IWorkspaceItem workspace);

        void Close(params IWorkspaceItem[] workspaces);
    }
}
