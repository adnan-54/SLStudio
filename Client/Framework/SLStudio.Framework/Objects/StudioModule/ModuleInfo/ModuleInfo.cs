using System;
using System.Collections.Generic;

namespace SLStudio
{
    internal class ModuleInfo : IModuleInfo
    {
        private readonly IModuleContainer moduleContainer;

        public ModuleInfo(IModuleContainer moduleContainer)
        {
            this.moduleContainer = moduleContainer;
        }

        public IStudioModule Module => moduleContainer.Module;
        public IReadOnlyDictionary<Type, Type> Services => moduleContainer.Services;
        public IReadOnlyDictionary<Type, Type> Workers => moduleContainer.Workers;
        public IReadOnlyDictionary<Type, Type> ViewModels => moduleContainer.ViewModels;
        public IReadOnlyDictionary<Type, Type> Views => moduleContainer.Views;
        public IReadOnlyDictionary<Type, Type> Windows => moduleContainer.Windows;
        public IReadOnlyDictionary<Type, Type> Dialogs => moduleContainer.Dialogs;
        public IReadOnlyCollection<IStudioTheme> Themes => moduleContainer.Themes;
        public IReadOnlyCollection<IStudioLanguage> Languages => moduleContainer.Languages;
    }
}