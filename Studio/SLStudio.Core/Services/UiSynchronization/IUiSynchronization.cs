using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SLStudio.Core
{
    public interface IUiSynchronization
    {
        bool IsShuttingDown { get; }
        bool CanAccess { get; }

        Thread DispatcherThread { get; }

        void EnsureExecuteOnUi(Action action);

        Task EnsureExecuteOnUiAsync(Action action);

        Task EnsureExecuteOnUiAsync(Func<Task> action);

        void InvokeOnUi(Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Normal);

        Task InvokeOnUiAsync(Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Normal);

        Task InvokeOnUiAsync(Func<Task> action, DispatcherPriority dispatcherPriority = DispatcherPriority.Normal);
    }
}