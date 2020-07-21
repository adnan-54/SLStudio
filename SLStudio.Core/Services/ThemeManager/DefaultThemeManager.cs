using AvalonDock.Themes;
using ControlzEx.Theming;
using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;
using MahApps.Metro.Theming;
using SLStudio.Core.Services.ThemeManager.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SLStudio.Core
{
    internal class DefaultThemeManager : IThemeManager
    {
        private readonly IList<Theme> avaliableThemes;

        public DefaultThemeManager()
        {
            avaliableThemes = new List<Theme>()
            {
                new Theme("SLStudio.Dark", ThemeManagerResources.Dark, SkinType.Dark, new Vs2013DarkTheme(), new Uri("pack://application:,,,/SLStudio.Core;component/Resources/Themes/Dark.xaml"), new Uri("pack://application:,,,/SLStudio.Core;component/Resources/Icons/IconsDark.xaml")),
                new Theme("SLStudio.Light", ThemeManagerResources.Light, SkinType.Default, new Vs2013LightTheme(), new Uri("pack://application:,,,/SLStudio.Core;component/Resources/Themes/Light.xaml"), new Uri("pack://application:,,,/SLStudio.Core;component/Resources/Icons/IconsLight.xaml"))
            };

            Initialize();
        }

        public IEnumerable<Theme> AvaliableThemes => avaliableThemes;

        public Theme CurrentTheme { get; private set; }

        public void SetTheme(Theme theme)
        {
            ThemeManagerSettings.Default.UserTheme = theme.Id;
            ThemeManagerSettings.Default.Save();
            CurrentTheme = theme;
        }

        public void Reset()
        {
            ThemeManagerSettings.Default.Reset();
            ThemeManagerSettings.Default.Save();
        }

        private void Initialize()
        {
            CurrentTheme = AvaliableThemes.First(t => t.Id == ThemeManagerSettings.Default.UserTheme);
            CreateMahappsTheme();
            UpdateHandyControlTheme();
            UpdateMahappsTheme();
            LoadIcons();
        }

        private void CreateMahappsTheme()
        {
            ThemeManager.Current.ClearThemes();
            foreach (var theme in AvaliableThemes)
                ThemeManager.Current.AddLibraryTheme(new LibraryTheme(theme.Path, MahAppsLibraryThemeProvider.DefaultInstance));
        }

        private void UpdateHandyControlTheme()
        {
            SharedResourceDictionary.SharedDictionaries.Clear();
            ResourceHelper.GetTheme("HandyTheme", Application.Current.Resources).Skin = CurrentTheme.SkinType;
        }

        private void UpdateMahappsTheme()
        {
            var mahAppsTheme = ThemeManager.Current.GetTheme(CurrentTheme.Id);
            ThemeManager.Current.ChangeTheme(Application.Current, mahAppsTheme);
        }

        private void LoadIcons()
        {
            var targetUri = AvaliableThemes.FirstOrDefault(t => t.Id == "SLStudio.Light").Icons.OriginalString;
            var targetResource = Application.Current.Resources.MergedDictionaries.FirstOrDefault(d => d.Source?.OriginalString == targetUri);
            Application.Current.Resources.MergedDictionaries.Remove(targetResource);
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = CurrentTheme.Icons });
        }
    }
}