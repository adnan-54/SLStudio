using AvalonDock.Themes;
using ControlzEx.Theming;
using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;
using MahApps.Metro.Theming;
using SLStudio.Core.Services.ThemeManager.Resources;
using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SLStudio.Core
{
    internal class DefaultThemeManager : IThemeManager
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultThemeManager>();

        private readonly IList<ThemeModel> avaliableThemes;

        public DefaultThemeManager()
        {
            var dockDarkTheme = new Vs2013DarkTheme();
            var dockLightTheme = new Vs2013LightTheme();
            avaliableThemes = new List<ThemeModel>()
            {
                new ThemeModel("SLStudio.Dark", ThemeManagerResources.Dark, SkinType.Dark, dockDarkTheme, new Uri("pack://application:,,,/SLStudio.Core;component/Resources/Themes/Dark.xaml")),
                new ThemeModel("SLStudio.Light", ThemeManagerResources.Light, SkinType.Default, dockLightTheme, new Uri("pack://application:,,,/SLStudio.Core;component/Resources/Themes/Light.xaml"))
            };
            Initialize();
        }

        public IEnumerable<ThemeModel> AvaliableThemes => avaliableThemes;

        public ThemeModel CurrentTheme => AvaliableThemes.FirstOrDefault(t => t.Id == ThemeManagerSettings.Default.UserTheme);

        public event EventHandler ThemeChanged;

        private void Initialize()
        {
            CreateMahappsTheme();
            SetThemeInternal(CurrentTheme);
        }

        public void SetTheme(ThemeModel theme)
        {
            if (theme == CurrentTheme)
                return;
            SetThemeInternal(theme);
        }

        public void Refresh()
        {
            SetTheme(CurrentTheme);
        }

        public void Reset()
        {
            ThemeManagerSettings.Default.Reset();
            ThemeManagerSettings.Default.Save();
            Initialize();
        }

        private void SetThemeInternal(ThemeModel theme)
        {
            UpdateHandyControlTheme(theme);
            UpdateMahappsTheme(theme);

            ThemeManagerSettings.Default.UserTheme = theme.Id;
            ThemeManagerSettings.Default.Save();

            OnThemeChanged();
        }

        private void CreateMahappsTheme()
        {
            ThemeManager.Current.ClearThemes();
            foreach (var theme in AvaliableThemes)
                ThemeManager.Current.AddLibraryTheme(new LibraryTheme(theme.Path, MahAppsLibraryThemeProvider.DefaultInstance));
        }

        private void UpdateHandyControlTheme(ThemeModel theme)
        {
            SharedResourceDictionary.SharedDictionaries.Clear();
            ResourceHelper.GetTheme("HandyTheme", Application.Current.Resources).Skin = theme.SkinType;

            foreach (Window window in Application.Current.Windows)
            {
                try
                {
                    if (window.GetType().Namespace.Contains("avalon", StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    window.OnApplyTemplate();
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }

        private void UpdateMahappsTheme(ThemeModel theme)
        {
            var mahAppsTheme = ThemeManager.Current.GetTheme(theme.Id);
            ThemeManager.Current.ChangeTheme(Application.Current, mahAppsTheme);
        }

        private void OnThemeChanged()
        {
            ThemeChanged?.Invoke(this, new EventArgs());
        }
    }
}