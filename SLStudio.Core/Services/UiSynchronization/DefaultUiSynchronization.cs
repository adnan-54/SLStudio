using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SLStudio.Core
{
    internal class DefaultUiSynchronization : IUiSynchronization
    {
        private readonly Dispatcher dispatcher;

        public DefaultUiSynchronization(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public bool IsShuttingDown
            => dispatcher.HasShutdownStarted || dispatcher.HasShutdownFinished;

        public bool CanAccess
            => dispatcher.CheckAccess();

        public void EnsureExecuteOnUi(Action action)
        {
            if (IsShuttingDown)
                return;

            if (CanAccess)
                action();
            else
                dispatcher.Invoke(action);
        }

        public Task EnsureExecuteOnUiAsync(Action action)
        {
            if (IsShuttingDown)
                return Task.FromResult(true);

            if (CanAccess)
            {
                action();
                return Task.FromResult(true);
            }
            else
                return dispatcher.InvokeAsync(action).Task;
        }

        public async Task EnsureExecuteOnUiAsync(Func<Task> action)
        {
            if (IsShuttingDown)
                return;

            if (CanAccess)
                await action();
            else
                await dispatcher.InvokeAsync(action).Task.Unwrap();
        }

        public void InvokeOnUi(Action action, DispatcherPriority dispatcherPriority)
        {
            if (IsShuttingDown)
                return;

            dispatcher.Invoke(action, dispatcherPriority);
        }

        public Task InvokeOnUiAsync(Action action, DispatcherPriority dispatcherPriority)
        {
            if (IsShuttingDown)
                return Task.FromResult(true);

            return dispatcher.InvokeAsync(action, dispatcherPriority).Task;
        }

        public async Task InvokeOnUiAsync(Func<Task> action, DispatcherPriority dispatcherPriority)
        {
            if (IsShuttingDown)
                return;

            await dispatcher.InvokeAsync(action, dispatcherPriority).Task.Unwrap();
        }
    }
}