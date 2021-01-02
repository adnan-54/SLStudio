using SimpleInjector;
using System;
using System.Collections.Generic;

namespace SLStudio.Core
{
    public static class IoC
    {
        private static bool isInitialized = false;

        private static Func<Type, object> GetInstance = (service)
            => throw new InvalidOperationException("IoC is not initialized");

        private static Func<Type, IEnumerable<object>> GetAllInstances = service
            => throw new InvalidOperationException("IoC is not initialized");

        public static object Get(Type service)
            => GetInstance(service);

        public static TService Get<TService>() where TService : class
            => (TService)GetInstance(typeof(TService));

        public static IEnumerable<object> GetAll(Type service)
            => GetAllInstances(service);

        internal static void Initialize(Container container)
        {
            if (isInitialized)
                return;
            isInitialized = true;

            GetAllInstances = container.GetAllInstances;
            GetInstance = container.GetInstance;
        }
    }
}