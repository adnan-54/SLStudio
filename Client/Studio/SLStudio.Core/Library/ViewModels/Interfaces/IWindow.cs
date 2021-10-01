using DevExpress.Mvvm;
using System;
using System.ComponentModel;

namespace SLStudio.Core
{
    public interface IWindow : IHaveDisplayName, IClose, ISupportServices
    {
        event EventHandler Loaded;

        event EventHandler Activated;

        event EventHandler<CancelEventArgs> Closing;

        event EventHandler Closed;

        void Activate();

        void Minimize();

        void Restore();

        void Maximize();
    }
}