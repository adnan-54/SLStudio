﻿using Caliburn.Micro;
using SLStudio.Core.Modules.MainMenu.ViewModels;
using SLStudio.Core.Modules.StatusBar.ViewModels;
using SLStudio.Core.Modules.ToolBar.ViewModels;
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
            container.Singleton<IToolBar, ToolBarViewModel>();
            container.Singleton<IStatusBar, StatusBarViewModel>();
        }
    }
}