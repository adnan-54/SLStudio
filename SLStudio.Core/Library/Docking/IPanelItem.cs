using System;

namespace SLStudio.Core
{
    public interface IPanelItem : IHaveDisplayName, IDisposable
    {
        string Id { get; }

        bool IsSelected { get; }

        bool IsActive { get; }

        void OnActivated();

        void OnDeactivated();

        void OnDisposed();

        void Activate();
    }
}