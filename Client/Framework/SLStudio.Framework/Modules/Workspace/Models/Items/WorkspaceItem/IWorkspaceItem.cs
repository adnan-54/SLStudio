using System;

namespace SLStudio
{
    public interface IWorkspaceItem : IViewModel
    {
        Guid Id { get; }

        IWorkspaceHeader Header { get; }

        WorkspacePlacement Placement { get; }

        bool IsActive { get; }

        void Activate();

        void Show();

        void Close();
    }
}
