using System;

namespace SLStudio.Core
{
    internal class WorkspaceClosedEvent : EventArgs
    {
        public WorkspaceClosedEvent(IWorkspaceItem item)
        {
            Item = item;
        }

        public IWorkspaceItem Item { get; }
    }
}