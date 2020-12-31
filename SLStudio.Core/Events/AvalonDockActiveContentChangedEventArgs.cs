using System;

namespace SLStudio.Core
{
    class AvalonDockActiveContentChangedEventArgs : EventArgs
    {
        public AvalonDockActiveContentChangedEventArgs(IPanelItem item)
        {
            NewItem = item;
        }

        public IPanelItem NewItem { get; }
    }
}
