﻿using HandyControl.Data;
using System;

namespace SLStudio.Core
{
    public class Theme
    {
        public Theme(string id, string displayName, SkinType skinType, AvalonDock.Themes.Theme dockTheme, Uri path)
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
        public AvalonDock.Themes.Theme DockTheme { get; }
        public Uri Path { get; }
    }
}