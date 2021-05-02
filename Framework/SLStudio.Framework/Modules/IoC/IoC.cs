using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    public static class IoC
    {
        private static IContainer container;

        public static bool IsInitialized => container is not null;

        public static object Get(Type service)
        {
            if (!IsInitialized)
                return null;

            return container.GetInstance(service);
        }

        public static TService Get<TService>() where TService : class
        {
            if (!IsInitialized)
                return null;

            return container.GetInstance<TService>();
        }

        public static IEnumerable<object> GetAll(Type service)
        {
            if (!IsInitialized)
                return Enumerable.Empty<object>();

            return container.GetAllInstances(service);
        }

        public static IEnumerable<TService> GetAll<TService>() where TService : class
        {
            if (!IsInitialized)
                return Enumerable.Empty<TService>();

            return container.GetAllInstances<TService>();
        }

        internal static void Initialize(IContainer container)
        {
            if (IsInitialized)
                return;

            IoC.container = container;
        }
    }
}