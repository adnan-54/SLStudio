using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public interface IShell : IWindow
    {
        IReadOnlyCollection<IWorkspaceItem> Workspaces { get; }

        IWorkspaceItem ActiveWorkspace { get; }

        Task<T> AddWorkspace<T>() where T : class, IWorkspaceItem;

        Task<T> OpenWorkspace<T>() where T : class, IWorkspaceItem;

        Task AddWorkspaces(params IWorkspaceItem[] workspaces);

        Task OpenWorkspaces(params IWorkspaceItem[] workspaces);

        Task CloseWorkspaces(params IWorkspaceItem[] workspaces);
    }
}