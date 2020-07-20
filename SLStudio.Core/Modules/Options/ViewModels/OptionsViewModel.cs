using SLStudio.Core.Modules.Options.Resources;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Core.Modules.Options.ViewModels
{
    internal class OptionsViewModel : WindowViewModel
    {
        private readonly IThemeManager themeManager;
        private readonly ILanguageManager languageManager;

        public OptionsViewModel(IThemeManager themeManager, ILanguageManager languageManager)
        {
            this.themeManager = themeManager;
            this.languageManager = languageManager;

            AvaliableThemes = themeManager.AvaliableThemes.ToList();
            AvaliableLanguages = languageManager.AvaliableLanguages.ToList();

            SelectedTheme = themeManager.CurrentTheme;
            SelectedLanguage = languageManager.CurrentLanguage;

            DisplayName = OptionsResources.Options;
        }

        public IReadOnlyCollection<ThemeModel> AvaliableThemes { get; }

        public IReadOnlyCollection<LanguageModel> AvaliableLanguages { get; }

        public ThemeModel SelectedTheme
        {
            get => GetProperty(() => SelectedTheme);
            set => SetProperty(() => SelectedTheme, value);
        }

        public LanguageModel SelectedLanguage
        {
            get => GetProperty(() => SelectedLanguage);
            set => SetProperty(() => SelectedLanguage, value);
        }

        public void Save()
        {
            themeManager.SetTheme(SelectedTheme);
            languageManager.SetLanguage(SelectedLanguage);
            TryClose();
        }

        public void Reset()
        {
            themeManager.Reset();
            languageManager.Reset();
            TryClose();
        }
    }
}