using SLStudio.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SLStudio.Core.Utilities.ThemeManager
{
    internal class DefaultThemeManager : IThemeManager
    {
        private static readonly ILogger logger = LogManager.GetLogger(typeof(DefaultThemeManager));

        private readonly List<Theme> avaliableThemes;

        public DefaultThemeManager()
        {
            avaliableThemes = new List<Theme>()
            {
                ThemesHelper.LightTheme,
                ThemesHelper.DarkTheme,
                ThemesHelper.BlueTheme
            };
        }

        public IEnumerable<Theme> AvaliableThemes => avaliableThemes;

        public Theme CurrentTheme { get; private set; }

        public void SetTheme(Theme theme)
        {
            try
            {
                if (theme == null)
                    theme = AvaliableThemes.FirstOrDefault(t => t.IsDefault);

                var themeResource = ControlzEx.Theming.ThemeManager.Current.Themes.FirstOrDefault(t => t.BaseColorScheme == theme.BaseColor && t.ColorScheme == theme.ColorScheme);
                ControlzEx.Theming.ThemeManager.Current.ChangeTheme(Application.Current, themeResource);

                foreach (var palette in theme.Palette)
                {
                    var color = (Color)ColorConverter.ConvertFromString(palette.Value);
                    var brush = new SolidColorBrush(color);

                    Application.Current.Resources[palette.Key + ".Color"] = color;
                    Application.Current.Resources[palette.Key] = brush;
                }

                CurrentTheme = theme;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}