using SLStudio.Core.Resources.Strings;
using System.Collections.Generic;

namespace SLStudio.Core.Utilities.ThemeManager
{
    internal static class ThemesHelper
    {
        public static readonly List<string> AvaliableKeys = new List<string>
        {
            "Backstage.Button.Background",
            "Backstage.Delimiter",
            "Backstage.Editor.Background",
            "Backstage.Focused",
            "Backstage.Foreground",
            "Backstage.HoverBackground",
            "Backstage.SelectionBackground",
            "Backstage.Window.Background",
            "Border",
            "Button.Background",
            "Control.Background",
            "Custom.Blue",
            "Custom.Green",
            "Custom.Red",
            "Delimiter",
            "Editor.Background",
            "Focused",
            "Foreground",
            "HoverBackground",
            "HoverBorder",
            "HoverForeground",
            "SelectionBackground",
            "SelectionBorder",
            "SelectionForeground",
            "Window.Background"
        };

        public static Theme DefaultLightTheme()
        {
            return new Theme()
            {
                IsDefault = true,
                Name = StudioResources.ThemeLight,
                Description = "",
                Author = "SLStudio",
                BaseColor = "Light",
                ColorScheme = "Blue",
                Palette = new Dictionary<string, string>()
                {
                    {"Backstage.Button.Background", "#FF118EDA"},
                    {"Backstage.Delimiter", "#FF118EDA"},
                    {"Backstage.Editor.Background", "#FF006EB7"},
                    {"Backstage.Focused", "#FFFFFFFF"},
                    {"Backstage.Foreground", "#FFFFFFFF"},
                    {"Backstage.HoverBackground", "#FF2E98E0"},
                    {"Backstage.SelectionBackground", "#FF005E9F"},
                    {"Backstage.Window.Background", "#FF007ACC"},
                    {"Border", "#FFCCCEDB"},
                    {"Button.Background", "#FFF5F5F7"},
                    {"Control.Background", "#FFF5F5F7"},
                    {"Custom.Blue", "#FF0026C1"},
                    {"Custom.Green", "#FF00C126"},
                    {"Custom.Red", "#FFC12600"},
                    {"Delimiter", "#FFCCCEDB"},
                    {"Editor.Background", "#FFFFFFFF"},
                    {"Focused", "#FF007ACC"},
                    {"Foreground", "#FF1E1E1E"},
                    {"HoverBackground", "#FFC9DEF5"},
                    {"HoverBorder", "#FFC9DEF5"},
                    {"HoverForeground", "#FF1E1E1E"},
                    {"SelectionBackground", "#FF007ACC"},
                    {"SelectionBorder", "#FF007ACC"},
                    {"SelectionForeground", "#FFFFFFFF"},
                    {"Window.Background", "#FFEEEEF2"}
                }
            };
        }

        public static Theme DefaultDarkTheme()
        {
            return new Theme()
            {
                IsDefault = true,
                Name = StudioResources.ThemeDark,
                Description = "",
                Author = "SLStudio",
                BaseColor = "Dark",
                ColorScheme = "Blue",
                Palette = new Dictionary<string, string>()
                {
                    {"Backstage.Button.Background", "#FF118EDA"},
                    {"Backstage.Delimiter", "#FF118EDA"},
                    {"Backstage.Editor.Background", "#FF006EB7"},
                    {"Backstage.Focused", "#FFFFFFFF"},
                    {"Backstage.Foreground", "#FFFFFFFF"},
                    {"Backstage.HoverBackground", "#FF1C97EA"},
                    {"Backstage.SelectionBackground", "#FF005E9F"},
                    {"Backstage.Window.Background", "#FF007ACC"},
                    {"Border", "#FF434346"},
                    {"Button.Background", "#FF333337"},
                    {"Control.Background", "#FF2D2D30"},
                    {"Custom.Blue", "#FF0026C1"},
                    {"Custom.Green", "#FF00C126"},
                    {"Custom.Red", "#FFC12600"},
                    {"Delimiter", "#FF3F3F46"},
                    {"Editor.Background", "#FF333337"},
                    {"Focused", "#FF007ACC"},
                    {"Foreground", "#FFF1F1F1"},
                    {"HoverBackground", "#FF3E3E40"},
                    {"HoverBorder", "#FF3E3E40"},
                    {"HoverForeground", "#FFFFFFFF"},
                    {"SelectionBackground", "#FF007ACC"},
                    {"SelectionBorder", "#FF007ACC"},
                    {"SelectionForeground", "#FFFFFFFF"},
                    {"Window.Background", "#FF2D2D30"}
                }
            };
        }

        public static Theme DefaultBlueTheme()
        {
            return new Theme()
            {
                IsDefault = true,
                Name = StudioResources.ThemeBlue,
                Description = "",
                Author = "SLStudio",
                BaseColor = "Light",
                ColorScheme = "Blue",
                Palette = new Dictionary<string, string>()
                {
                    {"Backstage.Button.Background", "#FF4D6082"},
                    {"Backstage.Delimiter", "#FF5B7199"},
                    {"Backstage.Editor.Background", "#FF35496A"},
                    {"Backstage.Focused", "#FFFFFFFF"},
                    {"Backstage.Foreground", "#FFFFFFFF"},
                    {"Backstage.HoverBackground", "#FF5B7199"},
                    {"Backstage.SelectionBackground", "#FF758BB4"},
                    {"Backstage.Window.Background", "#FF35496A"},
                    {"Border", "#FF8E9BBC"},
                    {"Button.Background", "#FFDCE0EC"},
                    {"Control.Background", "#FFCFD6E5"},
                    {"Custom.Blue", "#FF0026C1"},
                    {"Custom.Green", "#FF00C126"},
                    {"Custom.Red", "#FFC12600"},
                    {"Delimiter", "#FFBDC5D8"},
                    {"Editor.Background", "#FFFFFFFF"},
                    {"Focused", "#FFE5C365"},
                    {"Foreground", "#FF1E1E1E"},
                    {"HoverBackground", "#FFFDF4BF"},
                    {"HoverBorder", "#FFE5C365"},
                    {"HoverForeground", "#FF1E1E1E"},
                    {"SelectionBackground", "#FFFFF29D"},
                    {"SelectionBorder", "#FFE5C365"},
                    {"SelectionForeground", "#FF1E1E1E"},
                    {"Window.Background", "#FFD6DBE9"}
                }
            };
        }
    }
}
