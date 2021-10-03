using System;

namespace SLStudio
{
    internal class ObjectFactory : StudioService, IObjectFactory
    {
        private readonly IContainer container;

        public ObjectFactory(IContainer container)
        {
            this.container = container;
        }

        public TService Create<TService>() where TService : class
        {
            return container.GetInstance<TService>();
        }

        public object Create(Type serviceType)
        {
            return container.GetInstance(serviceType);
        }

        public TService Create<TService>(Type serviceType) where TService : class
        {
            return (TService)container.GetInstance(serviceType);
        }
    }
}