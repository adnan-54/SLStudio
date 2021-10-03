using System;
using System.Collections.Generic;

namespace SLStudio
{
    public static class IoC
    {
        private static bool IsInitialized => Container != null;

        private static IContainer Container;

        public static object Get(Type service)
        {
            return Container?.GetInstance(service);
        }

        public static TService Get<TService>() where TService : class
        {
            return Container?.GetInstance(typeof(TService)) as TService;
        }

        public static IEnumerable<object> GetAll(Type service)
        {
            return Container?.GetAllInstances(service);
        }

        internal static void Initialize(IContainer container)
        {
            if (IsInitialized)
                return;

            Container = container;
        }
    }
}