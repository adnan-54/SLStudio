using System.Collections.Concurrent;

namespace SLStudio.Logging
{
    internal class DefaultLoggerFactory : ILoggerFactory
    {
        private readonly IObjectFactory objectFactory;
        private readonly ConcurrentDictionary<string, ILogger> cache;

        public DefaultLoggerFactory(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
            cache = new ConcurrentDictionary<string, ILogger>();
        }

        public ILogger Create(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = "<unnamed>";

            if (!cache.TryGetValue(name, out var logger))
            {
                logger = objectFactory.Create<DefaultLogger>();
                (logger as DefaultLogger).Name = name;
                cache.TryAdd(name, logger);
            }

            return logger;
        }
    }

    internal interface ILoggerFactory
    {
        ILogger Create(string name);
    }
}