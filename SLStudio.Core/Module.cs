using SimpleInjector;
using SLStudio.Core.Menus;
using SLStudio.Core.Modules.Output.ViewModels;
using SLStudio.Core.Modules.Shell.ViewModels;
using SLStudio.Core.Modules.StatusBar.ViewModels;
using SLStudio.Core.Services;

namespace SLStudio.Core
{
    internal class Module : ModuleBase
    {
        public override string Name => "SLStudio.Core";
        public override string Author => "Adnan54";
        public override int Priority => int.MaxValue;

        protected override void Register(Container container)
        {
            container.RegisterServiceAndImplementationAsSingleton<IErrorHandler, DefaultErrorHandler>();
            container.RegisterServiceAndImplementationAsSingleton<IMenuItemFactory, DefaultMenuItemFactory>();
            container.RegisterServiceAndImplementationAsSingleton<IMenuLookup, DefaultMenuLookup>();
            container.RegisterServiceAndImplementationAsSingleton<IRecentFilesRepository, DefaultRecentFilesRespository>();

            container.RegisterSingleton<IShell, ShellViewModel>();
            container.RegisterSingleton<IStatusBar, StatusBarViewModel>();
            container.RegisterSingleton<IOutput, OutputViewModel>();
        }
    }
}