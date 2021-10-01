using System;
using System.Collections.Generic;

namespace SLStudio
{
    internal class ModuleContainer : IModuleContainer
    {
        public ModuleContainer(IStudioModule module, IModuleRegister register, IModuleScheduler scheduler)
        {
            Module = module;
            Register = register;
            Scheduler = scheduler;
        }

        public IStudioModule Module { get; }
        public IModuleRegister Register { get; }
        public IModuleScheduler Scheduler { get; }

        public IReadOnlyDictionary<Type, Type> Services => Register.Services;
        public IReadOnlyDictionary<Type, Type> Workers => Register.Workers;
        public IReadOnlyDictionary<Type, Type> ViewModels => Register.ViewModels;
        public IReadOnlyDictionary<Type, Type> Views => Register.Views;
        public IReadOnlyDictionary<Type, Type> Windows => Register.Windows;
        public IReadOnlyDictionary<Type, Type> Dialogs => Register.Dialogs;
        public IReadOnlyCollection<IStudioTheme> Themes => Register.Themes;
        public IReadOnlyCollection<IStudioLanguage> Languages => Register.Languages;
    }
}