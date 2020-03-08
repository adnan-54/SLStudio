using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SLStudio.Core.Utilities.ThemeManager
{
    internal class DefaultThemeManager : IThemeManager
    {
        private readonly ILogger logger;
        private readonly List<Theme> avaliableThemes;

        public DefaultThemeManager(ILoggingFactory loggingFactory)
        {
            logger = loggingFactory.GetLoggerFor<DefaultThemeManager>();
            avaliableThemes = new List<Theme>();
            FetchThemes().FireAndForget();
        }

        public IEnumerable<Theme> AvaliableThemes => avaliableThemes;

        public Theme CurrentTheme { get; private set; }

        public void SetTheme(Theme theme)
        {
            try
            {
                var themeResource = MahApps.Metro.ThemeManager.Themes.FirstOrDefault(t => t.BaseColorScheme == theme.BaseColor && t.ColorScheme == theme.ColorScheme);
                MahApps.Metro.ThemeManager.ChangeTheme(Application.Current, themeResource);

                if (theme == null)
                    theme = AvaliableThemes.FirstOrDefault(t => t.IsDefault);

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

        private Task FetchThemes()
        {
            avaliableThemes.Clear();
            avaliableThemes.AddRange(GetDefaultThemes());

            return Task.FromResult(true);
        }

        private IEnumerable<Theme> GetDefaultThemes()
        {
            yield return ThemesHelper.DefaultLightTheme();
            yield return ThemesHelper.DefaultDarkTheme();
            yield return ThemesHelper.DefaultBlueTheme();
        }
    }
}
