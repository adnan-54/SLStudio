using SimpleInjector;
using System;

namespace SLStudio.Logging
{
    internal class DefaultObjectFactory : IObjectFactory
    {
        private readonly Container container;

        public DefaultObjectFactory(Container container)
        {
            this.container = container;
        }

        TService IObjectFactory.Create<TService>()
        {
            return container.GetInstance<TService>();
        }

        object IObjectFactory.Create(Type serviceType)
        {
            return container.GetInstance(serviceType);
        }
    }

    internal interface IObjectFactory
    {
        TService Create<TService>() where TService : class;

        object Create(Type serviceType);
    }
}