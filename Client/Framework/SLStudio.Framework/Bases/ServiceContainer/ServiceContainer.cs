using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    public abstract class ServiceContainer : IServiceContainer
    {
        private record ServiceDescriptor(Type ServiceType, object Instance);
        private readonly List<ServiceDescriptor> descriptions;
        private bool isInitialized;

        protected ServiceContainer()
        {
            descriptions = new();
        }

        TService IServiceContainer.GetService<TService>()
        {
            EnsureInitialized();
            var service = descriptions.FirstOrDefault(s => s.ServiceType == typeof(TService))?.Instance as TService;
            if (service is not null)
                return service;
            throw new InvalidOperationException($"Cannot find service '{typeof(TService)}' on this container");
        }

        bool IServiceContainer.TryGetService<TService>(out TService service)
        {
            EnsureInitialized();
            service = descriptions.FirstOrDefault(s => s.ServiceType == typeof(TService))?.Instance as TService;
            return service != null;
        }

        void IServiceContainer.RegisterToContainer(IContainer container)
        {
            EnsureInitialized();
            foreach (var service in descriptions)
                container.RegisterSingleton(service.ServiceType, service.Instance);
        }

        private void EnsureInitialized()
        {
            if (isInitialized)
                return;
            isInitialized = true;
            Initialize();
        }

        protected abstract void Initialize();

        protected void RegisterService<TService>(TService service)
            where TService : class
        {
            var serviceType = typeof(TService);

            if (descriptions.Any(desc => desc.ServiceType == serviceType))
                throw new InvalidOperationException($"There is already a registration for service '{serviceType}'");
            descriptions.Add(new(serviceType, service));
        }
    }
}
