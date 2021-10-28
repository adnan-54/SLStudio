using System;
using System.ComponentModel;

namespace SLStudio
{
    public abstract class WorkspaceTool : WorkspaceItem, IWorkspaceTool
    {
        public event EventHandler<CancelEventArgs> Hiding;

        public bool IsVisible
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }
    }
}
