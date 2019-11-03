using SLStudio.Studio.Core.Framework.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;

namespace SLStudio.Studio.Core.Framework.Themes
{
    [Export(typeof(IThemeManager))]
    public class ThemeManager : IThemeManager
    {
        public event EventHandler CurrentThemeChanged;

        private readonly SettingsPropertyChangedEventManager<Properties.Settings> _settingsEventManager =
            new SettingsPropertyChangedEventManager<Properties.Settings>(Properties.Settings.Default);

        private ResourceDictionary _applicationResourceDictionary;

        public List<ITheme> Themes
        {
            get; private set;
        }

        public ITheme CurrentTheme { get; private set; }

        [ImportingConstructor]
        public ThemeManager([ImportMany] ITheme[] themes)
        {
            Themes = new List<ITheme>(themes);
            _settingsEventManager.AddListener(s => s.ThemeName, value => SetCurrentTheme(value));
        }

        public bool SetCurrentTheme(string name)
        {
            var theme = Themes.FirstOrDefault(x => x.GetType().Name == name);
            if (theme == null)
                return false;

            var mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
                return false;

            CurrentTheme = theme;

            if (_applicationResourceDictionary == null)
            {
                _applicationResourceDictionary = new ResourceDictionary();
                Application.Current.Resources.MergedDictionaries.Add(_applicationResourceDictionary);
            }
            _applicationResourceDictionary.BeginInit();
            _applicationResourceDictionary.MergedDictionaries.Clear();

            var windowResourceDictionary = mainWindow.Resources.MergedDictionaries[0];
            windowResourceDictionary.BeginInit();
            windowResourceDictionary.MergedDictionaries.Clear();

            foreach (var uri in theme.ApplicationResources)
                _applicationResourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = uri
                });

            foreach (var uri in theme.MainWindowResources)
                windowResourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = uri
                });

            _applicationResourceDictionary.EndInit();
            windowResourceDictionary.EndInit();

            RaiseCurrentThemeChanged(EventArgs.Empty);

            return true;
        }

        private void RaiseCurrentThemeChanged(EventArgs args)
        {
            CurrentThemeChanged?.Invoke(this, args);
        }
    }
}