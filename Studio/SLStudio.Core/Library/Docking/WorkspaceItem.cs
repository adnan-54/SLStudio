using DevExpress.Mvvm;
using System;
using System.ComponentModel;

namespace SLStudio.Core
{
    public abstract class WorkspaceItem : ViewModelBase, IWorkspaceItem
    {
        protected WorkspaceItem()
        {
            Token = Guid.NewGuid();
            Id = $"_{Token.ToString().Replace("-", null).Substring(0, 10)}";
        }

        ~WorkspaceItem()
        {
            Dispose(false);
        }

        public object Token { get; }

        public string Id
        {
            get => GetProperty(() => Id);
            set => SetProperty(() => Id, value);
        }

        public string DisplayName
        {
            get => GetProperty(() => DisplayName);
            set => SetProperty(() => DisplayName, value);
        }

        public string ToolTip
        {
            get => GetProperty(() => ToolTip);
            set => SetProperty(() => ToolTip, value);
        }

        public Uri IconSource
        {
            get => GetProperty(() => IconSource);
            set => SetProperty(() => IconSource, value);
        }

        public bool IsSelected
        {
            get => GetProperty(() => IsSelected);
            set => SetProperty(() => IsSelected, value);
        }

        public bool IsActive
        {
            get => GetProperty(() => IsActive);
            set => SetProperty(() => IsActive, value);
        }

        public virtual bool IsClosed
        {
            get => GetProperty(() => IsClosed);
            set => SetProperty(() => IsClosed, value);
        }

        public abstract WorkspaceItemPlacement Placement { get; }

        public virtual ClosingBehavior ClosingBehavior => ClosingBehavior.Default;

        protected bool Disposed { get; private set; }

        public void Activate()
        {
            Show();
            IsActive = true;
        }

        public void Open()
        {
            IsClosed = false;
        }

        public void Show()
        {
            Open();
            IsSelected = true;
        }

        public void Close()
        {
            IsClosed = true;
        }

        public virtual void OnActivated()
        {
        }

        public virtual void OnDeactivated()
        {
        }

        public virtual void OnClosing(CancelEventArgs e)
        {
        }

        public virtual void OnClosed()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Disposed = true;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: Name='{DisplayName}'; Id='{Id}'";
        }
    }

    public interface IWorkspaceItem : IHaveDisplayName, IClosable, ISupportServices, IDisposable
    {
        string Id { get; set; }

        object Token { get; }

        string ToolTip { get; }

        WorkspaceItemPlacement Placement { get; }

        bool IsClosed { get; }

        bool IsSelected { get; }

        bool IsActive { get; }

        void Open();

        void Show();

        void Activate();

        void OnActivated();

        void OnDeactivated();

        void OnClosing(CancelEventArgs e);

        void OnClosed();
    }
}