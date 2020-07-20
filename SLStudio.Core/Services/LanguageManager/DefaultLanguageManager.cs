using HandyControl.Tools;
using Humanizer.Configuration;
using SLStudio.Core.Humanizer;
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
        internal DefaultLanguageManager()
        {
            AvaliableLanguages = new List<Language>
            {
                new Language("en-US"),
                new Language("pt-BR"),
                new Language(Thread.CurrentThread.CurrentUICulture.Name, $"Auto ({Thread.CurrentThread.CurrentUICulture.DisplayName})", true)
            };

            Initalize();
        }

        public IEnumerable<Language> AvaliableLanguages { get; }

        public Language CurrentLanguage { get; private set; }

        public void SetLanguage(Language language)
        {
            var code = language.IsAuto ? string.Empty : language.Code;
            LanguageManagerSettings.Default.UserLanguage = code;
            LanguageManagerSettings.Default.Save();
            CurrentLanguage = language;
        }

        public void Reset()
        {
            LanguageManagerSettings.Default.Reset();
            LanguageManagerSettings.Default.Save();
            CurrentLanguage = AvaliableLanguages.First(l => l.Code == Thread.CurrentThread.CurrentUICulture.Name);
        }

        private void Initalize()
        {
            if (string.IsNullOrEmpty(LanguageManagerSettings.Default.UserLanguage))
                CurrentLanguage = AvaliableLanguages.First(l => l.IsAuto);
            else
                CurrentLanguage = AvaliableLanguages.First(l => l.Code == LanguageManagerSettings.Default.UserLanguage);

            Thread.CurrentThread.CurrentCulture = CurrentLanguage.Culture;
            Thread.CurrentThread.CurrentUICulture = CurrentLanguage.Culture;
            CultureInfo.DefaultThreadCurrentCulture = CurrentLanguage.Culture;
            CultureInfo.DefaultThreadCurrentUICulture = CurrentLanguage.Culture;
            ConfigHelper.Instance.SetLang(CurrentLanguage.Code);
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CurrentLanguage.Code)));
            SetupHumanizer();
        }

        private static void SetupHumanizer()
        {
            Configurator.DateTimeHumanizeStrategy = new DaysOnlyHumanizeStrategy();
        }
    }
}