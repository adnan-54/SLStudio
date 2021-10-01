using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio
{
    public interface IModuleScheduler
    {
        IEnumerable<IScheduledAction> ScheduledActions { get; }

        IEnumerable<IScheduledTask> ScheduledTasks { get; }

        void RegisterAction(Action<IObjectFactory> action);

        void RegisterTask(Func<IObjectFactory, Task> task);
    }
}