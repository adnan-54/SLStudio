using Caliburn.Micro;
using MahApps.Metro;
using SLStudio.Core.Modules.Options.Resources;
using SLStudio.Properties;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace SLStudio.Core.Modules.Options.ViewModels
{
    internal class OptionsViewModel : Screen, IOptions
    {
        public OptionsViewModel()
        {
            AvaliableLanguages = new BindableCollection<StudioOptionViewModel>
            {
                new StudioOptionViewModel(OptionsResx.English, "en"),
                new StudioOptionViewModel(OptionsResx.Portuguese, "pt-br")
            };
            SelectedLanguage = AvaliableLanguages.Where(l => l.Value == Settings.Default.LanguageCode).FirstOrDefault();

            AvaliableThemes = new BindableCollection<StudioOptionViewModel>
            {
                new StudioOptionViewModel(OptionsResx.Dark, "BaseDark"),
                new StudioOptionViewModel(OptionsResx.Light, "BaseLight")
            };
            SelectedTheme = AvaliableThemes.Where(t => t.Value == Settings.Default.ThemeBase).FirstOrDefault();

            AvaliableAccents = new BindableCollection<StudioOptionViewModel>();
            var metroAccents = ThemeManager.Accents;
            metroAccents.ToList().OrderBy(a => a.Name).ToList().ForEach(a => AvaliableAccents.Add(new StudioOptionViewModel(a.Name, a.Name)));
            SelectedAccent = AvaliableAccents.Where(a => a.Value == Settings.Default.ThemeAccent).FirstOrDefault();

            DisplayName = OptionsResx.OptionsName;
        }

        public BindableCollection<StudioOptionViewModel> AvaliableLanguages { get; }

        private StudioOptionViewModel selectedLanguage;

        public StudioOptionViewModel SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                selectedLanguage = value;
                NotifyOfPropertyChange(() => SelectedLanguage);
                NotifyOfPropertyChange(() => HasChanges);
                NotifyOfPropertyChange(() => NeedsRestart);
            }
        }

        private bool previewThemes = true;

        public bool PreviewThemes
        {
            get => previewThemes;
            set
            {
                previewThemes = value;
                NotifyOfPropertyChange(() => PreviewThemes);

                if (PreviewThemes)
                    ApplyTheme(SelectedAccent?.Value, SelectedTheme?.Value);
            }
        }

        public BindableCollection<StudioOptionViewModel> AvaliableThemes { get; }

        private StudioOptionViewModel selectedTheme;

        public StudioOptionViewModel SelectedTheme
        {
            get => selectedTheme;
            set
            {
                selectedTheme = value;
                NotifyOfPropertyChange(() => SelectedTheme);
                NotifyOfPropertyChange(() => HasChanges);

                if (PreviewThemes)
                    ApplyTheme(SelectedAccent?.Value, SelectedTheme?.Value);
            }
        }

        public BindableCollection<StudioOptionViewModel> AvaliableAccents { get; }

        private StudioOptionViewModel selectedAccents;

        public StudioOptionViewModel SelectedAccent
        {
            get => selectedAccents;
            set
            {
                selectedAccents = value;
                NotifyOfPropertyChange(() => SelectedAccent);
                NotifyOfPropertyChange(() => HasChanges);

                if (PreviewThemes)
                    ApplyTheme(SelectedAccent?.Value, SelectedTheme?.Value);
            }
        }

        private bool showInitialScreen = Settings.Default.ShowInitialScreen;

        public bool ShowInitialScreen
        {
            get => showInitialScreen;
            set
            {
                showInitialScreen = value;
                NotifyOfPropertyChange(() => ShowInitialScreen);
                NotifyOfPropertyChange(() => HasChanges);
            }
        }

        private bool fastSplashScreen = Settings.Default.FastSplashScreen;

        public bool FastSplashScreen
        {
            get => fastSplashScreen;
            set
            {
                fastSplashScreen = value;

                if (FastSplashScreen)
                {
                    SplashScreenSleepTime = Settings.Default.SplashScreenSleepTime;
                    NotifyOfPropertyChange(() => SplashScreenSleepTime);
                }

                NotifyOfPropertyChange(() => FastSplashScreen);
                NotifyOfPropertyChange(() => HasChanges);
            }
        }

        private int splashScreenSleepTime = Settings.Default.SplashScreenSleepTime;

        public int SplashScreenSleepTime
        {
            get => splashScreenSleepTime;
            set
            {
                splashScreenSleepTime = value;
                NotifyOfPropertyChange(() => SplashScreenSleepTime);
                NotifyOfPropertyChange(() => HasChanges);
            }
        }

        private bool showConsoleAtStartup = Settings.Default.ShowConsoleAtStartup;

        public bool ShowConsoleAtStartup
        {
            get => showConsoleAtStartup;
            set
            {
                showConsoleAtStartup = value;
                NotifyOfPropertyChange(() => ShowConsoleAtStartup);
                NotifyOfPropertyChange(() => HasChanges);
            }
        }

        public bool HasChanges => SelectedLanguage.Value != Settings.Default.LanguageCode ||
                                  SelectedTheme.Value != Settings.Default.ThemeBase ||
                                  SelectedAccent.Value != Settings.Default.ThemeAccent ||
                                  ShowInitialScreen != Settings.Default.ShowInitialScreen ||
                                  FastSplashScreen != Settings.Default.FastSplashScreen ||
                                  SplashScreenSleepTime != Settings.Default.SplashScreenSleepTime ||
                                  ShowConsoleAtStartup != Settings.Default.ShowConsoleAtStartup;

        public bool NeedsRestart => SelectedLanguage.Value != Settings.Default.LanguageCode;

        private void ApplyTheme(string accentKey, string themeKey)
        {
            if (string.IsNullOrEmpty(accentKey))
                accentKey = Settings.Default.ThemeAccent;
            if (string.IsNullOrEmpty(themeKey))
                themeKey = Settings.Default.ThemeBase;

            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(accentKey), ThemeManager.GetAppTheme(themeKey));
        }

        public void Reset()
        {
            var result = MessageBox.Show(OptionsResx.ResertToDefaultDialog, OptionsResx.ResetToDefault, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Settings.Default.Reset();
                Settings.Default.Save();

                SelectedLanguage = AvaliableLanguages.First(l => l.Value == Settings.Default.LanguageCode);
                SelectedTheme = AvaliableThemes.First(t => t.Value == Settings.Default.ThemeBase);
                SelectedAccent = AvaliableAccents.First(a => a.Value == Settings.Default.ThemeAccent);
                ShowInitialScreen = Settings.Default.ShowInitialScreen;
                FastSplashScreen = Settings.Default.FastSplashScreen;
                SplashScreenSleepTime = Settings.Default.SplashScreenSleepTime;
                ShowConsoleAtStartup = Settings.Default.ShowConsoleAtStartup;

                TryCloseAsync();
            }
        }

        public void Save()
        {
            var shouldRestart = NeedsRestart;

            Settings.Default.LanguageCode = SelectedLanguage.Value;
            Settings.Default.ThemeBase = SelectedTheme.Value;
            Settings.Default.ThemeAccent = SelectedAccent.Value;
            Settings.Default.ShowInitialScreen = ShowInitialScreen;
            Settings.Default.FastSplashScreen = FastSplashScreen;
            Settings.Default.SplashScreenSleepTime = SplashScreenSleepTime;
            Settings.Default.ShowConsoleAtStartup = ShowConsoleAtStartup;
            Settings.Default.Save();

            if (shouldRestart)
            {
                var result = MessageBox.Show(OptionsResx.NeedsRestartDialog, OptionsResx.NeedsRestartTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }
            }

            TryCloseAsync();
        }

        public void OnClose(CancelEventArgs e)
        {
            if (HasChanges)
            {
                var result = MessageBox.Show(OptionsResx.DiscardChangesDialog, OptionsResx.DiscardChanges, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            ApplyTheme(Settings.Default.ThemeAccent, Settings.Default.ThemeBase);
        }
    }

    internal class StudioOptionViewModel
    {
        public StudioOptionViewModel(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
}