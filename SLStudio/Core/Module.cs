﻿using Caliburn.Micro;
using SLStudio.Core.Modules.Console.ViewModels;
using SLStudio.Core.Modules.Logs.ViewModels;
using SLStudio.Core.Modules.MainMenu.ViewModels;
using SLStudio.Core.Modules.Options.ViewModels;
using SLStudio.Core.Modules.Shell.ViewModels;
using SLStudio.Core.Modules.StatusBar.Resources.ViewModels;
using SLStudio.Core.Modules.Toolbar.ViewModels;
using SLStudio.Core.Services.LoggingService;
using SLStudio.Core.Utilities.ErrorHandler;
using SLStudio.Core.Utilities.ObjectFactory;

namespace SLStudio.Core
{
    internal class Module : ModuleBase
    {
        public override ModulePriority ModulePriority => ModulePriority.Core;

        public override string ModuleName => "SLStudio Core";

        public override string ModuleDescrition => "Core module.";

        public override void Register(SimpleContainer container)
        {
            //CoreModules
            container.Singleton<ILoggingFactory, DefaultLoggingFactory>();
            container.Singleton<IObjectFactory, DefaultObjectFactory>();
            container.Singleton<IErrorHandler, DefaultErrorHandler>();

            //Services
            container.Singleton<ILoggingService, DefaultLoggingService>();

            //Modules
            container.Singleton<IMainMenu, MainMenuViewModel>();
            container.Singleton<IToolbar, ToolbarViewModel>();
            container.Singleton<IShell, ShellViewModel>();
            container.Singleton<IStatusBar, StatusBarViewModel>();
            container.PerRequest<IConsole, ConsoleViewModel>();
            container.PerRequest<IOptions, OptionsViewModel>();
            container.PerRequest<LogsViewModel>();
        }
    }
}