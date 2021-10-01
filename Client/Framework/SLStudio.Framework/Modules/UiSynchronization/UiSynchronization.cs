using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SLStudio
{
    internal class UiSynchronization : StudioService, IUiSynchronization
    {
        private readonly Dispatcher dispatcher;

        public UiSynchronization(IApplicationInfo appInfo)
        {
            dispatcher = appInfo.Dispatcher;
        }

        public bool IsShuttingDown => dispatcher is null || dispatcher.HasShutdownStarted || dispatcher.HasShutdownFinished;
        public bool CanAccess => dispatcher is not null && dispatcher.CheckAccess();
        public Thread Dispatcher => dispatcher?.Thread;

        public void Execute(Action action)
        {
            if (IsShuttingDown)
                return;

            if (CanAccess)
                action();
            else
                dispatcher.Invoke(action);
        }

        public Task ExecuteAsync(Action action)
        {
            if (IsShuttingDown)
                return Task.CompletedTask;

            if (CanAccess)
            {
                action();
                return Task.CompletedTask;
            }
            else
                return dispatcher.InvokeAsync(action).Task;
        }

        public Task ExecuteAsync(Func<Task> action)
        {
            if (IsShuttingDown)
                return Task.CompletedTask;

            if (CanAccess)
                return action();
            else
                return dispatcher.InvokeAsync(action).Task.Unwrap();
        }

        public void Invoke(Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Normal)
        {
            if (IsShuttingDown)
                return;

            dispatcher.Invoke(action, dispatcherPriority);
        }

        public Task InvokeAsync(Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Normal)
        {
            if (IsShuttingDown)
                return Task.CompletedTask;

            return dispatcher.InvokeAsync(action, dispatcherPriority).Task;
        }

        public Task InvokeAsync(Func<Task> action, DispatcherPriority dispatcherPriority = DispatcherPriority.Normal)
        {
            if (IsShuttingDown)
                return Task.CompletedTask;

            return dispatcher.InvokeAsync(action, dispatcherPriority).Task.Unwrap();
        }
    }
}