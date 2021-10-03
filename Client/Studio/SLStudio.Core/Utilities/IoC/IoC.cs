using SimpleInjector;
using System;
using System.Collections.Generic;

namespace SLStudio.Core
{
    public static class IoC
    {
        private static bool isInitialized = false;

        private static Func<Type, object> GetInstance = @object => throw new InvalidOperationException("IoC is not initialized yet");

        private static Func<Type, IEnumerable<object>> GetAllInstances = @object => throw new InvalidOperationException("IoC is not initialized yet");

        public static object Get(Type service)
        {
            return GetInstance(service);
        }

        public static TService Get<TService>() where TService : class
        {
            return (TService)GetInstance(typeof(TService));
        }

        public static IEnumerable<object> GetAll(Type service)
        {
            return GetAllInstances(service);
        }

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