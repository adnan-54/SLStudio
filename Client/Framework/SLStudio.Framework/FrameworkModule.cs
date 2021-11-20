using System.Windows;

namespace SLStudio
{
    public class FrameworkModule : Module
    {
        private static IServiceContainer serviceContainer;

        public static IServiceContainer GetServiceContainer(Application application)
        {
            return serviceContainer ??= new FrameworkServices(application);
        }

        public override int Priority => int.MaxValue;

        protected override void Register(IModuleRegister register)
        {
            register.ServiceContainer(serviceContainer);
            register.Resource(new("pack://application:,,,/SLStudio.Framework;component/Resources/MenuResources.xaml"));
            register.Resource(new("pack://application:,,,/SLStudio.Framework;component/Modules/Workspace/Resources/WorkspaceResources.xaml"));

            //Workspace
            register.ViewModel<IWorkspace, WorkspaceViewModel>(LifeStyle.Singleton);
            register.View<WorkspaceView, IWorkspace>();
        }
    }
}
