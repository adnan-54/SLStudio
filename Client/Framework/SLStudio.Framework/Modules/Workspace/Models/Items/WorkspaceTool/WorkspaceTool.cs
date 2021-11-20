using System;
using System.ComponentModel;

namespace SLStudio
{
    public abstract class WorkspaceTool : WorkspaceItem, IWorkspaceTool
    {
        public event EventHandler<CancelEventArgs> Hiding;

        void IWorkspaceTool.OnHiding(CancelEventArgs e)
        {
            Hiding?.Invoke(this, e);
        }
    }
}
