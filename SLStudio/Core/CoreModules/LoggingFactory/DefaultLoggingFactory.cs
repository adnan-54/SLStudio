using Caliburn.Micro;
using SLStudio.Core.CoreModules.Logging;

namespace SLStudio.Core.CoreModules.LoggingFactory
{
    internal class DefaultLoggingFactory : ILoggingFactory
    {
        private readonly IEventAggregator eventAggregator;

        public DefaultLoggingFactory(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public ILogger GetLoggerFor<Type>() where Type : class
        {
            return new Logger(typeof(Type), eventAggregator);
        }
    }
}