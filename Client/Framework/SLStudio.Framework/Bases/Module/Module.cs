using System;
using System.Threading.Tasks;
using SLStudio.Logging;

namespace SLStudio
{
    public abstract class Module : IModule
    {
        private readonly ILogger logger;
        private bool registered;
        private bool scheduled;
        private bool ran;

        protected Module()
        {
            logger = LogManager.GetLogger(GetType().Name);
            registered = false;
            scheduled = false;
            ran = false;

            Name = GetType().Name;
            Author = GetType().Namespace;
            Priority = 0;
            CanBeLoaded = true;
            Version = GetType().Assembly.GetName().Version;
        }

        public virtual string Name { get; }

        public virtual string Author { get; }

        public virtual int Priority { get; }

        public virtual bool CanBeLoaded { get; }

        public Version Version { get; }

        public void OnRegister(IModuleRegister register)
        {
            if (registered || !CanBeLoaded)
                return;

            try
            {
                logger.Debug($"Registering module {Name}...");

                Register(register);
                registered = true;

                logger.Debug($"Module {Name} registered successfully");
            }
            catch (Exception ex)
            {
                logger.Debug($"Falied to register module {Name}");
                logger.Error(ex);
            }
        }

        public void OnSchedule(IModuleScheduler scheduler)
        {
            if (scheduled || !CanBeLoaded)
                return;

            try
            {
                logger.Debug($"Scheduling module {Name}...");

                Schedule(scheduler);
                scheduled = true;

                logger.Debug($"Module {Name} schedule successfully");
            }
            catch (Exception ex)
            {
                logger.Debug($"Falied to schedule module {Name}");
                logger.Error(ex);
            }
        }

        public async Task OnRun(IObjectFactory objectFactory)
        {
            if (ran || !CanBeLoaded)
                return;

            try
            {
                logger.Debug($"Running module {Name}...");

                await Run(objectFactory);
                ran = true;

                logger.Debug($"Module {Name} ran successfully");
            }
            catch (Exception ex)
            {
                logger.Debug($"Falied to run module {Name}");
                logger.Error(ex);
            }
        }

        protected virtual void Register(IModuleRegister register)
        {
        }

        protected virtual void Schedule(IModuleScheduler scheduler)
        {
        }

        protected virtual Task Run(IObjectFactory objectFactory)
        {
            return Task.CompletedTask;
        }
    }
}