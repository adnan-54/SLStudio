using System;

namespace SLStudio.Core
{
    class ActiveWorkspaceChangedEvent : EventArgs
    {
        public ActiveWorkspaceChangedEvent(IWorkspaceItem item)
        {
            NewItem = item;
        }

        public IWorkspaceItem NewItem { get; }
    }
}
