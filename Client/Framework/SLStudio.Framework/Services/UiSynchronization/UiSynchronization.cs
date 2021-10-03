using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SLStudio
{
    internal class UiSynchronization : Service, IUiSynchronization
    {
        private readonly Dispatcher dispatcher;

        public UiSynchronization(IApplicationInfo applicationInfo)
        {
            dispatcher = applicationInfo.Dispatcher;
        }

        public void Execute(Action action)
        {
            dispatcher.Invoke(action);
        }

        public Task ExecuteAsync(Action action)
        {
            return dispatcher.InvokeAsync(action).Task;
        }

        public Task ExecuteAsync(Func<Task> action)
        {
            return dispatcher.InvokeAsync(action).Task.Unwrap();
        }

        public void Invoke(Action action, DispatcherPriority dispatcherPriority)
        {
            dispatcher.Invoke(action, dispatcherPriority);
        }

        public Task InvokeAsync(Action action, DispatcherPriority dispatcherPriority)
        {
            return dispatcher.InvokeAsync(action, dispatcherPriority).Task;
        }

        public Task InvokeAsync(Func<Task> action, DispatcherPriority dispatcherPriority)
        {
            return dispatcher.InvokeAsync(action, dispatcherPriority).Task.Unwrap();
        }
    }
}
