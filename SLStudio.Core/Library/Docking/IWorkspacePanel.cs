using AvalonDock;
using DevExpress.Mvvm;
using System;

namespace SLStudio.Core
{
    public abstract class WorkspacePanel : ViewModelBase, IWorkspacePanel
    {
        protected WorkspacePanel()
        {
            Id = $"{Guid.NewGuid()}";
        }

        public string Id
        {
            get => GetProperty(() => Id);
            set => SetProperty(() => Id, value);
        }

        public bool IsSelected
        {
            get => GetProperty(() => IsSelected);
            set => SetProperty(() => IsSelected, value);
        }

        public string DisplayName
        {
            get => GetProperty(() => DisplayName);
            set => SetProperty(() => DisplayName, value);
        }

        public bool IsActive { get; set; }

        public bool IsClosed
        {
            get => GetProperty(() => IsClosed);
            set => SetProperty(() => IsClosed, value);
        }

        public virtual void Show()
        {
            IsClosed = false;
            IsSelected = true;
        }

        public virtual void Activate()
        {
            Show();
            IsActive = true;
            RaisePropertyChanged(() => IsActive);
        }

        public virtual void OnActivated()
        {
        }

        public virtual void OnDeactivated()
        {
        }

        public virtual void Close()
        {
            IsClosed = true;
        }

        public virtual void OnClosing(DocumentClosingEventArgs e)
        {
        }

        public abstract void OnClosed(DocumentClosedEventArgs e);

        public void Dispose()
            => OnDispose();

        protected virtual void OnDispose()
        {
        }

        public override string ToString() => $"{GetType().Name}: Name='{DisplayName}'; Id='{Id}'";
    }

    public interface IWorkspacePanel : IHaveDisplayName, IDisposable, ISupportServices
    {
        string Id { get; set; }
        bool IsSelected { get; }
        bool IsActive { get; }
        bool IsClosed { get; }

        void Show();

        void Activate();

        void OnActivated();

        void OnDeactivated();

        void Close();

        void OnClosing(DocumentClosingEventArgs e);

        void OnClosed(DocumentClosedEventArgs e);
    }
}