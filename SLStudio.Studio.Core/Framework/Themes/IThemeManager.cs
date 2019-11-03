using System;
using System.Collections.Generic;

namespace SLStudio.Studio.Core.Framework.Themes
{
    public interface IThemeManager
    {
        event EventHandler CurrentThemeChanged;

        List<ITheme> Themes { get; }
        ITheme CurrentTheme { get; }

        bool SetCurrentTheme(string name);
    }
}