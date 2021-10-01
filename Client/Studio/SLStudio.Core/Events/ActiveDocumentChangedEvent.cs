using System;

namespace SLStudio.Core
{
    internal class ActiveDocumentChangedEvent : EventArgs
    {
        public ActiveDocumentChangedEvent(IDocumentItem oldItem, IDocumentItem newItem)
        {
            OldItem = oldItem;
            NewItem = newItem;
        }

        public IDocumentItem OldItem { get; }
        public IDocumentItem NewItem { get; }
    }
}