using System;

namespace SLStudio.Core
{
    internal class ActiveToolChangedEvent : EventArgs
    {
        public ActiveToolChangedEvent(IToolItem oldItem, IToolItem newItem)
        {
            OldItem = oldItem;
            NewItem = newItem;
        }

        public IToolItem OldItem { get; }
        public IToolItem NewItem { get; }
    }
}