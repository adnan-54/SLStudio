using System;
using System.ComponentModel;

namespace SLStudio
{
    public abstract class WorkspaceDocument : WorkspaceItem, IWorkspaceDocument
    {
        public event CancelEventHandler Closing;

        public event EventHandler Closed;

        public bool IsLastFocusedDocument { get; set; }

        public override WorkspacePlacement Placement => WorkspacePlacement.Document;
    }
}
