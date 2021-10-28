using System.Collections.Generic;

namespace SLStudio
{
    public interface IWorkspace : IViewModel
    {
        IEnumerable<IWorkspaceItem> Workspaces { get; }

        IWorkspaceDocument LastFocusedDocument { get; }

        IWorkspaceItem SelectedWorkspace { get; }

        TWorkspace Show<TWorkspace>()
            where TWorkspace : class, IWorkspaceItem;

        IEnumerable<IWorkspaceItem> Show(params IWorkspaceItem[] workspaces);

        void Close(params IWorkspaceItem[] workspaces);
    }
}
