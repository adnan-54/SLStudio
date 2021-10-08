using System;
using System.ComponentModel;
using System.Windows;

namespace SLStudio
{
    public class FrameworkModule : Module
    {
        private static IServiceCollection services;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IServiceCollection CreateServiceCollection(Application application)
        {
            if (services != null)
                throw new InvalidOperationException("Service collection already created.");

            services = new FrameworkServices(application);
            return services;
        }

        public override int Priority => int.MaxValue;

        protected override void Register(IModuleRegister register)
        {
            register.ServiceCollection(services);
            register.RegisterResource(new Uri("pack://application:,,,/SLStudio.Framework;component/Resources/MenuResources.xaml"));
        }
    }
}