using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SLStudio
{
    public interface IUiSynchronization : IService
    {
        void Execute(Action action);

        Task ExecuteAsync(Action action);

        Task ExecuteAsync(Func<Task> action);

        void Invoke(Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Normal);

        Task InvokeAsync(Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Normal);

        Task InvokeAsync(Func<Task> action, DispatcherPriority dispatcherPriority = DispatcherPriority.Normal);
    }
}
