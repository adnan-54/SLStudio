﻿using AvalonDock.Themes;
using HandyControl.Data;
using System;

namespace SLStudio.Core
{
    public class Theme
    {
        public Theme(string id, string displayName, SkinType skinType, AvalonDock.Themes.Theme dockTheme, Uri path, Uri icons)
        {
            Id = id;
            DisplayName = displayName;
            SkinType = skinType;
            DockTheme = dockTheme;
            Path = path;
            Icons = icons;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public SkinType SkinType { get; }
        public AvalonDock.Themes.Theme DockTheme { get; }
        public Uri Path { get; }
        public Uri Icons { get; }
    }
}