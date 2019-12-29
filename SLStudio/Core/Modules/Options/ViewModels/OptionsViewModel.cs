using Caliburn.Micro;
using MahApps.Metro;
using SLStudio.Core.Modules.Options.Resources;
using SLStudio.Properties;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace SLStudio.Core.Modules.Options.ViewModels
{
    class OptionsViewModel : Screen
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
                TryApplyTheme();
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
                TryApplyTheme();
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
                TryApplyTheme();
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

        public bool HasChanges => SelectedLanguage.Value != Settings.Default.LanguageCode ||
                                  SelectedTheme.Value != Settings.Default.ThemeBase ||
                                  SelectedAccent.Value != Settings.Default.ThemeAccent ||
                                  ShowInitialScreen != Settings.Default.ShowInitialScreen ||
                                  FastSplashScreen != Settings.Default.FastSplashScreen ||
                                  SplashScreenSleepTime != Settings.Default.SplashScreenSleepTime;

        public bool NeedsRestart => SelectedLanguage.Value != Settings.Default.LanguageCode;

        private void TryApplyTheme()
        {
            try
            {
                if(PreviewThemes)
                    ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(SelectedAccent.Value), ThemeManager.GetAppTheme(SelectedTheme.Value));
                else
                    ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(Settings.Default.ThemeAccent), ThemeManager.GetAppTheme(Settings.Default.ThemeBase));
            }
            catch { }
        }

        public void Reset()
        {
            var result = MessageBox.Show(OptionsResx.ResertToDefaultDialog, OptionsResx.ResetToDefault, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Settings.Default.Reset();
                Settings.Default.Save();

                PreviewThemes = true;

                SelectedTheme = AvaliableThemes.First(t => t.Value == Settings.Default.ThemeBase);
                SelectedAccent = AvaliableAccents.First(a => a.Value == Settings.Default.ThemeAccent);
                ShowInitialScreen = Settings.Default.ShowInitialScreen;
                FastSplashScreen = Settings.Default.FastSplashScreen;
                SplashScreenSleepTime = Settings.Default.SplashScreenSleepTime;

                if (NeedsRestart)
                {
                    var result2 = MessageBox.Show(OptionsResx.NeedsRestartDialog, OptionsResx.NeedsRestartTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result2 == MessageBoxResult.Yes)
                    {
                        SelectedLanguage = AvaliableLanguages.First(l => l.Value == Settings.Default.LanguageCode);

                        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }
                    else
                        SelectedLanguage = AvaliableLanguages.First(l => l.Value == Settings.Default.LanguageCode);
                }
                else
                    SelectedLanguage = AvaliableLanguages.First(l => l.Value == Settings.Default.LanguageCode);

                TryClose();
            }
        }

        public void Cancel()
        {
            if(HasChanges)
            {
                var result = MessageBox.Show(OptionsResx.DiscardChangesDialog, OptionsResx.DiscardChanges, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    PreviewThemes = true;
                    SelectedTheme = AvaliableThemes.First(t => t.Value == Settings.Default.ThemeBase);
                    SelectedAccent = AvaliableAccents.First(a => a.Value == Settings.Default.ThemeAccent);

                    TryClose();
                }
            }
        }

        public void Save()
        {
            Settings.Default.ThemeBase = SelectedTheme.Value;
            Settings.Default.ThemeAccent = SelectedAccent.Value;
            Settings.Default.ShowInitialScreen = ShowInitialScreen;
            Settings.Default.FastSplashScreen = FastSplashScreen;
            Settings.Default.SplashScreenSleepTime = SplashScreenSleepTime;
            Settings.Default.Save();

            PreviewThemes = true;
            TryApplyTheme();

            if (NeedsRestart)
            {
                var result = MessageBox.Show(OptionsResx.NeedsRestartDialog, OptionsResx.NeedsRestartTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    Settings.Default.LanguageCode = SelectedLanguage.Value;
                    Settings.Default.Save();

                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }
                else
                {
                    Settings.Default.LanguageCode = SelectedLanguage.Value;
                    Settings.Default.Save();
                }
            }
            else
            {
                Settings.Default.LanguageCode = SelectedLanguage.Value;
                Settings.Default.Save();
            }

            TryClose();
        }

        public void OnClose(CancelEventArgs e)
        {
            if (HasChanges)
            {
                var result = MessageBox.Show(OptionsResx.DiscardChangesDialog, OptionsResx.DiscardChanges, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    PreviewThemes = true;

                    SelectedTheme = AvaliableThemes.First(t => t.Value == Settings.Default.ThemeBase);
                    SelectedAccent = AvaliableAccents.First(a => a.Value == Settings.Default.ThemeAccent);
                }
                else
                    e.Cancel = true;
            }
        }
    }

    class StudioOptionViewModel
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
