using System;

namespace SLStudio
{
    internal class ScheduledAction : IScheduledAction
    {
        private readonly Action<IObjectFactory> action;

        public ScheduledAction(Action<IObjectFactory> action)
        {
            this.action = action;
        }

        public bool Completed { get; private set; }

        public void Run(IObjectFactory objectFactory)
        {
            if (Completed)
                return;

            action(objectFactory);

            Completed = true;
        }
    }
}