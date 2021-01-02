using HandyControl.Tools;
using Humanizer.Configuration;
using SLStudio.Core.LanguageManager;
using SLStudio.Core.Services.LanguageManager;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace SLStudio.Core
{
    internal class DefaultLanguageManager : ILanguageManager
    {
        private readonly LanguageManagerSettings settings = LanguageManagerSettings.Default;

        private readonly IList<Language> avaliableLanguages = new List<Language>()
        {
            new Language("en"),
            new Language("pt-BR"),
            new Language(null)
        };

        internal DefaultLanguageManager()
        {
            DefaultLanguage = new Language(Thread.CurrentThread.CurrentCulture.Name);

            Initalize();
        }

        public IEnumerable<Language> AvaliableLanguages => avaliableLanguages;

        public Language DefaultLanguage { get; }

        public Language CurrentLanguage { get; private set; }

        public void SetLanguage(Language language)
        {
            CurrentLanguage = language;
            Save();
        }

        public void Save()
        {
            settings.UserLanguage = CurrentLanguage?.Code;
            settings.Save();
        }

        public void Reset()
        {
            settings.Reset();
            settings.Save();
        }

        private void Initalize()
        {
            var targetCode = string.IsNullOrEmpty(settings.UserLanguage) ? null : settings.UserLanguage;
            CurrentLanguage = AvaliableLanguages.First(l => l.Code == targetCode);
            var targetCulture = targetCode switch
            {
                null => DefaultLanguage.Culture,
                _ => CurrentLanguage.Culture
            };

            Thread.CurrentThread.CurrentCulture = targetCulture;
            Thread.CurrentThread.CurrentUICulture = targetCulture;
            CultureInfo.DefaultThreadCurrentCulture = targetCulture;
            CultureInfo.DefaultThreadCurrentUICulture = targetCulture;
            ConfigHelper.Instance.SetLang(targetCulture.Name);
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(targetCulture.Name)));
            Configurator.DateTimeHumanizeStrategy = new DaysOnlyHumanizeStrategy();
        }
    }
}