using System;
using System.ComponentModel;

namespace SLStudio
{
    public abstract class WorkspaceDocument : WorkspaceItem, IWorkspaceDocument
    {
        public event CancelEventHandler Closing;

        public event EventHandler Closed;

        public bool LastFocusedDocument { get; set; }

        public override WorkspacePlacement Placement => WorkspacePlacement.Document;

        void IWorkspaceDocument.OnClosing(CancelEventArgs e)
        {
            Closing?.Invoke(this, e);
        }

        void IWorkspaceDocument.OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
