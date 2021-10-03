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
                new Theme("SLStudio.Dark", ThemeManagerResources.Dark, true, SkinType.Dark, new Vs2013DarkTheme(), new Uri("pack://application:,,,/SLStudio.Core;component/Resources/Themes/Dark.xaml")),
                new Theme("SLStudio.Light", ThemeManagerResources.Light, false, SkinType.Default, new Vs2013LightTheme(), new Uri("pack://application:,,,/SLStudio.Core;component/Resources/Themes/Light.xaml"))
            };

            Initialize();
        }

        public IEnumerable<Theme> AvaliableThemes => avaliableThemes;

        public Theme CurrentTheme { get; private set; }

        public void SetTheme(Theme theme)
        {
            ThemeManagerSettings.Default.UserTheme = theme.Id;
            ThemeManagerSettings.Default.Save();
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
    }

    public interface IThemeManager
    {
        IEnumerable<Theme> AvaliableThemes { get; }

        Theme CurrentTheme { get; }

        void SetTheme(Theme theme);

        void Reset();
    }
}