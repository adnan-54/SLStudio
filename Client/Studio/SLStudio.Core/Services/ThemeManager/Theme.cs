using HandyControl.Data;
using System;

namespace SLStudio.Core
{
    public class Theme
    {
        public Theme(string id, string displayName, bool isDark, SkinType skinType, AvalonDock.Themes.Theme dockTheme, Uri path)
        {
            Id = id;
            DisplayName = displayName;
            IsDark = isDark;
            SkinType = skinType;
            DockTheme = dockTheme;
            Path = path;
        }

        public string Id { get; }

        public string DisplayName { get; }

        public bool IsDark { get; }

        public bool IsLight => !IsDark;

        public SkinType SkinType { get; }

        public AvalonDock.Themes.Theme DockTheme { get; }

        public Uri Path { get; }
    }
}