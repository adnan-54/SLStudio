using Caliburn.Micro;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    class BootstrapperService : IBootstrapperService
    {
        private readonly SimpleContainer container;
        private readonly ISplashScreenService splashScreen;

        public BootstrapperService(SimpleContainer container, ISplashScreenService splashScreen)
        {
            this.container = container;
            this.splashScreen = splashScreen;
        }

        public void Initialize()
        {
            splashScreen.Show();

            var types = GetType().Assembly.GetTypes();
            var modules = types.Where(type => type.IsClass &&
                                      type.Name.Equals("Module") &&
                                      type.GetInterface(nameof(IModule)) != null).ToList();

            foreach (var module in modules)
            {
                var instance = Activator.CreateInstance(module) as IModule;
                LoadModule(instance);
            }
        }

        private void LoadModule(IModule module)
        {
            if(module != null && module.ShouldBeLoaded)
            {
                splashScreen.UpdateStatus(module.ModuleName);
                module.Register(container);
            }
        }
    }

    public interface IBootstrapperService
    {
        void Initialize();
    }
}
