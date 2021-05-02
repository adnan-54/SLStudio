using System;
using System.Threading.Tasks;

namespace SLStudio
{
    internal class ScheduledTask : IScheduledTask
    {
        private readonly Func<IObjectFactory, Task> task;

        public ScheduledTask(Func<IObjectFactory, Task> task)
        {
            this.task = task;
        }

        public bool Completed { get; private set; }

        public async Task Run(IObjectFactory objectFactory)
        {
            if (Completed)
                return;

            await task(objectFactory);

            Completed = true;
        }
    }
}