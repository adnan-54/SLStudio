using SLStudio.Core.CoreModules.Logging;

namespace SLStudio.Core.CoreModules.LoggingFactory
{
    internal class DefaultLoggingFactory : ILoggingFactory
    {
        public ILogger GetLoggerFor<Type>() where Type : class
        {
            return new Logger(typeof(Type));
        }
    }
}