using SLStudio.Extensions.Enums;
using SLStudio.ViewsExtensions.Themes.Presets;
using System.Drawing;

namespace SLStudio.ViewsExtensions.Themes
{
    public class Theme
    {
        public string themeName;
        public string themeAuthor;

        public Color borders;
        public Color bordersDark;
        public Color bordersLight;
        public Color error;
        public Color font;
        public Color fontDark;
        public Color fontLight;
        public Color info;
        public Color link;
        public Color selection;
        public Color selectionDark;
        public Color selectionLight;
        public Color style;
        public Color theme;
        public Color themeDark;
        public Color themeLight;
        public Color warning;
        public Color workspace;
        public Color workspaceDark;
        public Color workspaceLight;

        public Theme(DefaultThemes defaultThemeName = DefaultThemes.UserDefault)
        {
            if (defaultThemeName == DefaultThemes.UserDefault)
            {
                this.themeName = UserCurrent.Default.name;
                this.themeAuthor = UserCurrent.Default.author;
                this.borders = UserCurrent.Default.borders;
                this.bordersDark = UserCurrent.Default.bordersDark;
                this.bordersLight = UserCurrent.Default.bordersLight;
                this.error = UserCurrent.Default.error;
                this.font = UserCurrent.Default.font;
                this.fontDark = UserCurrent.Default.fontDark;
                this.fontLight = UserCurrent.Default.fontLight;
                this.info = UserCurrent.Default.info;
                this.link = UserCurrent.Default.link;
                this.selection = UserCurrent.Default.selection;
                this.selectionDark = UserCurrent.Default.selectionDark;
                this.selectionLight = UserCurrent.Default.selectionLight;
                this.style = UserCurrent.Default.style;
                this.theme = UserCurrent.Default.theme;
                this.themeDark = UserCurrent.Default.themeDark;
                this.themeLight = UserCurrent.Default.themeLight;
                this.warning = UserCurrent.Default.warning;
                this.workspace = UserCurrent.Default.workspace;
                this.workspaceDark = UserCurrent.Default.workspaceDark;
                this.workspaceLight = UserCurrent.Default.workspaceLight;
            }
            else
            if (defaultThemeName == DefaultThemes.Light)
            {
                this.themeName = Resources.Messages.ThemeManager.themeLight;
                this.themeAuthor = "Adnan";
                this.borders = Light.Default.borders;
                this.bordersDark = Light.Default.bordersDark;
                this.bordersLight = Light.Default.bordersLight;
                this.error = Light.Default.error;
                this.font = Light.Default.font;
                this.fontDark = Light.Default.fontDark;
                this.fontLight = Light.Default.fontLight;
                this.info = Light.Default.info;
                this.link = Light.Default.link;
                this.selection = Light.Default.selection;
                this.selectionDark = Light.Default.selectionDark;
                this.selectionLight = Light.Default.selectionLight;
                this.style = Light.Default.style;
                this.theme = Light.Default.theme;
                this.themeDark = Light.Default.themeDark;
                this.themeLight = Light.Default.themeLight;
                this.warning = Light.Default.warning;
                this.workspace = Light.Default.workspace;
                this.workspaceDark = Light.Default.workspaceDark;
                this.workspaceLight = Light.Default.workspaceLight;
            }
            else
            if(defaultThemeName == DefaultThemes.Dark)
            {
                this.themeName = Resources.Messages.ThemeManager.themeDark;
                this.themeAuthor = "Adnan";
                this.borders = Dark.Default.borders;
                this.bordersDark = Dark.Default.bordersDark;
                this.bordersLight = Dark.Default.bordersLight;
                this.error = Dark.Default.error;
                this.font = Dark.Default.font;
                this.fontDark = Dark.Default.fontDark;
                this.fontLight = Dark.Default.fontLight;
                this.info = Dark.Default.info;
                this.link = Dark.Default.link;
                this.selection = Dark.Default.selection;
                this.selectionDark = Dark.Default.selectionDark;
                this.selectionLight = Dark.Default.selectionLight;
                this.style = Dark.Default.style;
                this.theme = Dark.Default.theme;
                this.themeDark = Dark.Default.themeDark;
                this.themeLight = Dark.Default.themeLight;
                this.warning = Dark.Default.warning;
                this.workspace = Dark.Default.workspace;
                this.workspaceDark = Dark.Default.workspaceDark;
                this.workspaceLight = Dark.Default.workspaceLight;
            }
        }

        public Theme SerializeFromFile(string filePath)
        {
            return new Theme();
        }
    }
}
