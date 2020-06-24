using SimpleInjector;
using SimpleInjector.Diagnostics;

namespace SLStudio.Core
{
    public static class ContainerExtensions
    {
        public static void RegisterDisposable<TService, TImplementation>(this Container container) where TService : class where TImplementation : class, TService
        {
            container.Register<TService, TImplementation>();
            var registration = container.GetRegistration(typeof(TService)).Registration;
            registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "We should dispose ourselves");
        }

        public static void RegisterServiceAndImplementationAsSingleton<TService, TImplementation>(this Container container) where TService : class where TImplementation : class, TService
        {
            container.RegisterSingleton<TImplementation>();
            container.RegisterSingleton<TService, TImplementation>();
        }
    }
}