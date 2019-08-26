using SLStudio.Properties;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace SLStudio.ViewsExtensions.Themes
{
    public static class ThemeManager
    {
        public static void Update(Theme newTheme, Form parent)
        {
            try
            {
                Refresh(parent);

                Settings.Default.borders = newTheme.borders;
                Settings.Default.bordersDark = newTheme.bordersDark;
                Settings.Default.bordersLight = newTheme.bordersLight;
                Settings.Default.error = newTheme.error;
                Settings.Default.font = newTheme.font;
                Settings.Default.fontDark = newTheme.fontDark;
                Settings.Default.fontLight = newTheme.fontLight;
                Settings.Default.info = newTheme.info;
                Settings.Default.link = newTheme.link;
                Settings.Default.selection = newTheme.selection;
                Settings.Default.selectionDark = newTheme.selectionDark;
                Settings.Default.selectionLight = newTheme.selectionLight;
                Settings.Default.style = newTheme.style;
                Settings.Default.theme = newTheme.theme;
                Settings.Default.themeDark = newTheme.themeDark;
                Settings.Default.themeLight = newTheme.themeLight;
                Settings.Default.warning = newTheme.warning;
                Settings.Default.workspace = newTheme.workspace;
                Settings.Default.workspaceDark = newTheme.workspaceDark;
                Settings.Default.workspaceLight = newTheme.workspaceLight;
                Settings.Default.themeName = newTheme.themeName;
                Settings.Default.themeAutor = newTheme.themeAuthor;
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public static void Refresh(Form parent)
        {
            try
            {
                var properties = parent.GetType().GetProperties();

                foreach(PropertyInfo property in properties)
                {
                    MessageBox.Show(property.ToString() + ": " + property.GetValue(parent).ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public static void Refresh(Control control)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}