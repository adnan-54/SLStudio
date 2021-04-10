using SLStudio.Logging;
using SLStudio.Resources;
using System;
using System.Threading.Tasks;

namespace SLStudio
{
    public abstract class StudioModule : IStudioModule
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<StudioModule>();

        private bool isRegistered;

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

        public bool IsLoaded => isRegistered;

        public void RegisterModule(IModuleRegister register)
        {
            if (isRegistered)
                return;

            logger.Debug($"Registering module {Name}");

            try
            {
                Register(register);
                isRegistered = true;

                logger.Debug($"Module {Name} registered successfully");
            }
            catch (Exception ex)
            {
                logger.Debug($"Failed to register module {Name}");
                logger.Error(ex);

                throw;
            }
        }

        protected abstract void Register(IModuleRegister register);
    }
}