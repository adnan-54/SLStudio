using SimpleInjector;
using System;

namespace SLStudio.Core
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

        TService IObjectFactory.Create<TService>(Type serviceType)
        {
            return (TService)container.GetInstance(serviceType);
        }
    }
}