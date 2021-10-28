using System;
using System.Collections.Generic;

namespace SLStudio
{
    public static class IoC
    {
        private static IContainer container;
        private static bool IsInitialized => container != null;

        public static object Get(Type service)
        {
            if (IsInitialized)
                return container.GetInstance(service);
            return default;
        }

        public static TService Get<TService>() where TService : class
        {
            if (IsInitialized)
                return container.GetInstance(typeof(TService)) as TService;
            return default;
        }

        public static IEnumerable<object> GetAll(Type service)
        {
            if (IsInitialized)
                return container.GetAllInstances(service);
            return default;

        }

        internal static void Initialize(IContainer container)
        {
            if (!IsInitialized)
                IoC.container = container;
        }
    }
}