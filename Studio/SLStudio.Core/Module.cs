using MvvmDialogs;
using SimpleInjector;
using SLStudio.Core.Menus;
using SLStudio.Core.Modules.Output.ViewModels;
using SLStudio.Core.Modules.Shell.ViewModels;
using SLStudio.Core.Modules.StatusBar.ViewModels;
using SLStudio.Core.Modules.ToolBox.ViewModels;
using SLStudio.Core.Services;
using SLStudio.Core.Services.ToolManager;

namespace SLStudio.Core
{
    internal class Module : ModuleBase
    {
        public override string Name => "SLStudio.Core";
        public override string Author => "Adnan54";
        public override int Priority => int.MaxValue;

        protected override void Register(Container container)
        {
            container.RegisterInstance<IDialogService>(new DialogService());
            container.RegisterService<IFileService, DefaultFileService>();
            container.RegisterService<IRecentFilesRepository, DefaultRecentFilesRespository>();
            container.RegisterService<IErrorHandler, DefaultErrorHandler>();
            container.RegisterService<IMenuItemFactory, DefaultMenuItemFactory>();
            container.RegisterService<IMenuLookup, DefaultMenuLookup>();
            container.RegisterService<IToolManager, DefaultToolManager>();

            container.RegisterSingleton<IShell, ShellViewModel>();
            container.RegisterSingleton<IToolbox, ToolBoxViewModel>();
            container.RegisterSingleton<IOutput, OutputViewModel>();
            container.RegisterSingleton<IStatusBar, StatusBarViewModel>();
        }
    }
}