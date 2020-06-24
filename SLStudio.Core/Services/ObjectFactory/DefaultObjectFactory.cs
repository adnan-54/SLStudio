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

        TService IObjectFactory.Create<TService>() => container.GetInstance<TService>();

        object IObjectFactory.Create(Type serviceType) => container.GetInstance(serviceType);
    }
}