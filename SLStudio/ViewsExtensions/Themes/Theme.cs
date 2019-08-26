using SLStudio.Enums;
using SLStudio.Properties;
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

        public Theme()
        {
            this.themeName = Resources.themes.themeLight;
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

        public Theme(DefaultThemes defaultThemeName)
        {
            if (defaultThemeName == DefaultThemes.Light)
            {
                this.themeName = Resources.themes.themeLight;
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
                this.themeName = Resources.themes.themeDark;
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

            this.themeAuthor = "Adnan";
        }
    }
}
