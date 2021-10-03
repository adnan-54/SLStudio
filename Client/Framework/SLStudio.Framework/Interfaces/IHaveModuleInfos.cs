using System;
using System.Collections.Generic;

namespace SLStudio
{
    public interface IHaveModuleInfos
    {
        IReadOnlyDictionary<Type, Type> Services { get; }
        IReadOnlyDictionary<Type, Type> Workers { get; }
        IReadOnlyDictionary<Type, Type> ViewModels { get; }
        IReadOnlyDictionary<Type, Type> Views { get; }
        IReadOnlyDictionary<Type, Type> Windows { get; }
        IReadOnlyDictionary<Type, Type> Dialogs { get; }
        IReadOnlyCollection<IStudioTheme> Themes { get; }
        IReadOnlyCollection<IStudioLanguage> Languages { get; }
    }
}