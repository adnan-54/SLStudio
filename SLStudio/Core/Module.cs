using Caliburn.Micro;
using SLStudio.Core.Modules.MainWindow.ViewModels;
using SLStudio.Core.Modules.Shell.ViewModels;
using SLStudio.Core.Modules.StatusBar.Resources.ViewModels;
using SLStudio.Core.Modules.Toolbar.ViewModels;

namespace SLStudio.Core
{
    internal class Module : ModuleBase
    {
        public override ModulePriority ModulePriority => ModulePriority.Core;

        public override string ModuleName => "SLStudio Core";
        public override string ModuleDescrition => "Core module.";

        public override void Register(SimpleContainer container)
        {
            //default services
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();

            //core
            container.Singleton<IMainWindow, MainWindowViewModel>();
            container.Singleton<IToolbar, ToolbarViewModel>();
            container.Singleton<IShell, ShellViewModel>();
            container.Singleton<IStatusBar, StatusBarViewModel>();
        }
    }
}