using AvalonDock.Themes;
using HandyControl.Data;
using System;

namespace SLStudio.Core
{
    public class ThemeModel
    {
        public ThemeModel(string id, string displayName, SkinType skinType, Theme dockTheme, Uri path)
        {
            Id = id;
            DisplayName = displayName;
            SkinType = skinType;
            DockTheme = dockTheme;
            Path = path;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public SkinType SkinType { get; }
        public Theme DockTheme { get; }
        public Uri Path { get; }
    }
}