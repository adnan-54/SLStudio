using System;
using System.ComponentModel;

namespace SLStudio
{
    public interface IWorkspaceDocument : IWorkspaceItem
    {
        event CancelEventHandler Closing;

        event EventHandler Closed;

        bool LastFocusedDocument { get; }

        internal void OnClosing(CancelEventArgs e);

        internal void OnClosed();
    }
}
