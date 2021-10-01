using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio
{
    internal class ModuleScheduler : IModuleScheduler
    {
        private readonly List<IScheduledAction> scheduledActions;
        private readonly List<IScheduledTask> scheduledTasks;

        public ModuleScheduler()
        {
            scheduledActions = new List<IScheduledAction>();
            scheduledTasks = new List<IScheduledTask>();
        }

        public IEnumerable<IScheduledAction> ScheduledActions => scheduledActions;

        public IEnumerable<IScheduledTask> ScheduledTasks => scheduledTasks;

        public void RegisterAction(Action<IObjectFactory> action)
        {
            scheduledActions.Add(new ScheduledAction(action));
        }

        public void RegisterTask(Func<IObjectFactory, Task> task)
        {
            scheduledTasks.Add(new ScheduledTask(task));
        }
    }
}