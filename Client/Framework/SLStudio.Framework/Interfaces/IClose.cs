using System;
using System.ComponentModel;

namespace SLStudio
{
    public interface IClose
    {
        event EventHandler<CancelEventArgs> Closing;

        event EventHandler Closed;

        bool IsClosed { get; }

        void Close();
    }
}