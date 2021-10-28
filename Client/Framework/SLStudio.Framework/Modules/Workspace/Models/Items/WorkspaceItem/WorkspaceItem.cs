using System;

namespace SLStudio
{
    public abstract class WorkspaceItem : ViewModel, IWorkspaceItem
    {
        protected WorkspaceItem()
        {
            Id = Guid.NewGuid();
            Header = new WorkspaceHeader()
            {
                Title = "Untitled"
            };
        }

        public Guid Id { get; }

        public IWorkspaceHeader Header
        {
            get => GetValue<IWorkspaceHeader>();
            set => SetValue(value);
        }

        public abstract WorkspacePlacement Placement { get; }

        public bool IsActive
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Show()
        {

        }

        public void Close()
        {
        }
    }
}
