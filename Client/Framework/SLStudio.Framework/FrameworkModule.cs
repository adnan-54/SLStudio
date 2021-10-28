using System;
using System.Windows;

namespace SLStudio
{
    public class FrameworkModule : Module
    {
        private static IServiceContainer serviceContainer;

        public static IServiceContainer GetServiceContainer(Application application)
        {
            if (serviceContainer is null)
                serviceContainer = new FrameworkServices(application);
            return serviceContainer;
        }

        public override int Priority => int.MaxValue;

        protected override void Register(IModuleRegister register)
        {
            register.ServiceContainer(serviceContainer);
            register.Resource(new Uri("pack://application:,,,/SLStudio.Framework;component/Resources/MenuResources.xaml"));

            //Workspace
            register.ViewModel<IWorkspace, WorkspaceViewModel>(LifeStyle.Singleton);
            register.View<WorkspaceView, IWorkspace>();
        }
    }
}