﻿using Caliburn.Micro;
using SLStudio.Core.Framework;
using SLStudio.Core.Modules;
using SLStudio.Core.Modules.Shell.ViewModels;

namespace SLStudio.Core
{
    class Module : ModuleBase
    {
        public override ModulePriority ModulePriority => ModulePriority.Core;

        public override string ModuleName => "SLStudio Core";
        public override string ModuleDescrition => "Core module.";

        public override void Register(SimpleContainer container)
        {
            //default services
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();

            //core services
            container.Singleton<IShell, ShellViewModel>();
        }
    }
}
