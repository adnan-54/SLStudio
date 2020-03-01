using SLStudio.Core.Utilities.Logger;

namespace SLStudio.Core
{
    internal class DefaultLoggingFactory : ILoggingFactory
    {
        public ILogger GetLoggerFor<Type>() where Type : class
        {
            return new DefaultLogger(typeof(Type));
        }
    }
}