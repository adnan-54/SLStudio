using System;
using System.ComponentModel;

namespace SLStudio
{
    public interface IWindowViewModel : IViewModel
    {
        event CancelEventHandler Closing;

        event EventHandler Closed;

        string Title { get; }

        bool HasResult { get; }

        object Result { get; }

        void Activate();

        void Maximize();

        void Restore();

        void Minimize();

        void Close();

        object GetResult();

        TResult GetResult<TResult>()
            where TResult : class;

        bool TryGetResult<TResult>(out TResult result)
            where TResult : class;

        internal void OnClosing(CancelEventArgs args);

        internal void OnClosed();
    }
}