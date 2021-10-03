using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    public abstract class ServiceCollection : IServiceCollection
    {
        private readonly IDictionary<Type, object> services;
        private bool locked;
        private bool registered;

        protected ServiceCollection()
        {
            services = new Dictionary<Type, object>();
            locked = false;
            registered = false;
        }

        IReadOnlyDictionary<Type, object> IServiceCollection.GetAll()
        {
            CheckLocked();
            CheckRegistered();

            return services.ToDictionary(d => d.Key, d => d.Value);
        }

        TService IServiceCollection.Get<TService>()
        {
            CheckLocked();
            CheckRegistered();

            var serviceType = typeof(TService);

            if (!services.TryGetValue(serviceType, out var service))
                throw new Exception($"Could not find any registration for '{typeof(TService).Name}'.");

            return service as TService;
        }

        public void Lock()
        {
            CheckLocked();

            services.Clear();
            locked = true;
        }

        protected abstract void RegisterServices();

        protected void RegisterService<TService>(TService service) where TService : class, IService
        {
            CheckLocked();

            var serviceType = typeof(TService);

            if (!services.TryAdd(serviceType, service))
                throw new Exception($"Service '{serviceType.Name}' is already registered.");
        }

        private void CheckLocked()
        {
            if (locked)
                throw new InvalidOperationException($"This service container is already locked. Use '{nameof(IObjectFactory)}' or '{nameof(IContainer)}' instead.");
        }

        private void CheckRegistered()
        {
            if (registered)
                return;

            RegisterServices();

            registered = true;
        }
    }
}