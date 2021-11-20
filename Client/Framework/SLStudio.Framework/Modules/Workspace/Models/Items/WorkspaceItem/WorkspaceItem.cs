using System;

namespace SLStudio
{
    public abstract class WorkspaceItem : ViewModel, IWorkspaceItem
    {
        protected WorkspaceItem()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public abstract IWorkspaceHeader Header { get; }

        public abstract WorkspacePlacement Placement { get; }

        public bool Active
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool Visible
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void Activate()
        {
            Active = true;
        }

        public virtual void Show()
        {
            Visible = true;
        }

        public virtual void Close()
        {
            Visible = false;
        }
    }
}
