using System;

namespace SLStudio.Core
{
    internal class ActiveWorkspaceChangedEvent : EventArgs
    {
        public ActiveWorkspaceChangedEvent(IWorkspaceItem oldItem, IWorkspaceItem newItem)
        {
            OldItem = oldItem;
            NewItem = newItem;
        }

        public IWorkspaceItem OldItem { get; }
        public IWorkspaceItem NewItem { get; }
    }
}