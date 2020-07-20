using System.Collections.Generic;

namespace SLStudio.Core
{
    public interface IThemeManager
    {
        IEnumerable<Theme> AvaliableThemes { get; }

        Theme CurrentTheme { get; }

        void SetTheme(Theme theme);

        void Reset();
    }
}