using System;
using System.Collections.Generic;

namespace SLStudio
{
    public static class IoC
    {
        private static IContainer container;

        private static bool IsInitialized => container != null;

        public static object GetInstance(Type service)
        {
            return container?.GetInstance(service);
        }

        public static TService GetInstance<TService>() where TService : class
        {
            return container?.GetInstance(typeof(TService)) as TService;
        }

        public static IEnumerable<object> GetAllInstances(Type service)
        {
            return container?.GetAllInstances(service);
        }

        internal static void Initialize(IContainer container)
        {
            if (IsInitialized)
                return;
            IoC.container = container;
        }
    }
}