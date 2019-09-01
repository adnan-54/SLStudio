using SLStudio.Extensions.Enums;
using SLStudio.Extensions.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SLStudio.ViewsExtensions.Themes
{
    public static class ThemeManager
    {
        private static string themesPath = Path.Combine(Application.StartupPath, "themes");
        private static List<IThemedControl> ThemedControls = new List<IThemedControl>();

        public static bool AddControl(IThemedControl control)
        {
            try
            {
                ThemedControls.Add(control);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
            }

            return true;
        }

        public static bool RemoveControl(IThemedControl control)
        {
            try
            {
                ThemedControls.Remove(control);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

            return true;
        }

        public static bool UpdateControls()
        {
            try
            {
                UpdateThemedControlsList();

                foreach (IThemedControl control in ThemedControls)
                {
                    control.UpdateTheme();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }

            return true;
        }

        public static bool ApplyTheme(Theme theme)
        {
            try
            {
                UpdateThemedControlsList();

                foreach (IThemedControl control in ThemedControls)
                {
                    control.Theme = theme;
                }

                UpdateControls();
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }

            return true;
        }

        public static bool ApplyTheme(IThemedControl control, Theme theme)
        {
            try
            {
                UpdateThemedControlsList();
                control.Theme = theme;
                control.UpdateTheme();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }

            return true;
        }

        public static List<Theme> GetAvaliableThemes()
        {
            List<Theme> AvaliableThemes = new List<Theme>();

            AvaliableThemes.Add(new Theme(DefaultThemes.Light));
            AvaliableThemes.Add(new Theme(DefaultThemes.Dark));

            //Todo: serialize themes from themes directory

            return AvaliableThemes;
        }
         
        private static bool UpdateThemedControlsList()
        {
            try
            {
                foreach (IThemedControl control in ThemedControls)
                {
                    if (control == null)
                        ThemedControls.Remove(control);
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }

            return true;
        }
    }
}