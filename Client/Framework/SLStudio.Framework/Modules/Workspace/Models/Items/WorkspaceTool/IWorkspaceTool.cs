using System;
using System.ComponentModel;

namespace SLStudio
{
    public interface IWorkspaceTool : IWorkspaceItem
    {
        event EventHandler<CancelEventArgs> Hiding;

        internal void OnHiding(CancelEventArgs e);
    }
}
