using MvvmDialogs;
using SimpleInjector;
using SLStudio.Core.Menus;
using SLStudio.Core.Modules.NewFile.ViewModels;
using SLStudio.Core.Modules.Output.ViewModels;
using SLStudio.Core.Modules.Shell.ViewModels;
using SLStudio.Core.Modules.StatusBar.ViewModels;
using SLStudio.Core.Modules.ToolBox.ViewModels;
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
            container.RegisterInstance<IDialogService>(new DialogService());
            container.RegisterSingleton<IFileService, DefaultFileService>();

            container.RegisterSingleton<IRecentFilesRepository, DefaultRecentFilesRespository>();

            container.RegisterSingleton<IErrorHandler, DefaultErrorHandler>();

            container.RegisterSingleton<IMenuItemFactory, DefaultMenuItemFactory>();
            container.RegisterSingleton<IMenuLookup, DefaultMenuLookup>();

            container.RegisterSingleton<IShell, ShellViewModel>();
            container.RegisterService<IToolbox, ToolBoxViewModel>();
            container.RegisterService<IOutput, OutputViewModel>();
            container.RegisterService<IStatusBar, StatusBarViewModel>();

            container.RegisterDisposable<INewFileDialog, NewFileViewModel>();
        }
    }
}