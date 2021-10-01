using SLStudio.Logging;

namespace SLStudio
{
    public abstract class StudioModule : IStudioModule
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<StudioModule>();

        private bool isRegistered;
        private bool isScheduled;

        protected StudioModule()
        {
            Name = GetType().FullName;
            Author = GetType().Assembly.Location;
            Priority = 0;
            ShouldBeLoaded = true;
        }

        public virtual string Name { get; }

        public virtual string Author { get; }

        public virtual int Priority { get; }

        public virtual bool ShouldBeLoaded { get; }

        public bool IsLoaded => isRegistered && isScheduled;

        public void Load(IModuleContainer container)
        {
            Register(container);
            Schedule(container);
        }

        private void Register(IModuleContainer container)
        {
            if (isRegistered)
                return;

            logger.Debug($"Registering module {Name}");

            Register(container.Register);

            logger.Debug($"Module {Name} registered successfully");

            isRegistered = true;
        }

        private void Schedule(IModuleContainer container)
        {
            if (isScheduled)
                return;

            logger.Debug($"Scheduling module {Name}");

            Schedule(container.Scheduler);

            logger.Debug($"Module {Name} scheduled successfully");

            isScheduled = true;
        }

        protected virtual void Register(IModuleRegister register)
        {
        }

        protected virtual void Schedule(IModuleScheduler scheduler)
        {
        }
    }
}