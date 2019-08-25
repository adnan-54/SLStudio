using SLStudio.Properties;
using System.Drawing;

namespace SLStudio.ViewsExtensions.Themes
{
    public class Theme
    {
        public string themeName;
        public string themeAuthor;

        private Color borders;
        private Color bordersDark;
        private Color bordersLight;
        private Color error;
        private Color font;
        private Color fontDark;
        private Color fontLight;
        private Color info;
        private Color link;
        private Color selection;
        private Color selectionDark;
        private Color selectionLight;
        private Color style;
        private Color theme;
        private Color themeDark;
        private Color themeLight;
        private Color warning;
        private Color workspace;
        private Color workspaceDark;
        private Color workspaceLight;

        public Color Borders
        {
            get
            {
                return borders;
            }
            set
            {
                borders = value;
                DefaultTheme.Default.borders = borders;
            }
        }

        public Theme()
        {
            /*this.Borders = Settings.Default.borders;
            this.BordersDark = Settings.Default.bordersDark;
            this.BordersLight = Settings.Default.bordersLight;
            this.Error = Settings.Default.error;
            this.Font = Settings.Default.font;
            this.FontDark = Settings.Default.fontDark;
            this.FontLight = Settings.Default.fontLight;
            this.Info = Settings.Default.info;
            this.Link = Settings.Default.link;
            this.Selection = Settings.Default.selection;
            this.SelectionDark = Settings.Default.selectionDark;
            this.SelectionLight = Settings.Default.selectionLight;
            this.Style = Settings.Default.style;
            this.Theme = Settings.Default.theme;
            this.ThemeDark = Settings.Default.themeDark;
            this.ThemeLight = Settings.Default.themeLight;
            this.Warning = Settings.Default.warning;
            this.Workspace = Settings.Default.workspace;
            this.WorkspaceDark = Settings.Default.workspaceDark;
            this.WorkspaceLight = Settings.Default.workspaceLight;*/
        }
    }
}
