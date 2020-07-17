using System;
using System.Collections.Generic;

namespace SLStudio.Core
{
    public interface IThemeManager
    {
        IEnumerable<ThemeModel> AvaliableThemes { get; }
        ThemeModel CurrentTheme { get; }

        event EventHandler ThemeChanged;

        void SetTheme(ThemeModel theme);

        void Refresh();

        void Reset();
    }
}