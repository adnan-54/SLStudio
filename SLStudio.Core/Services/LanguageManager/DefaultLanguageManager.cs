using HandyControl.Tools;
using Humanizer.Configuration;
using SLStudio.Core.Humanizer;
using SLStudio.Core.Services.LanguageManager.Resources;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace SLStudio.Core
{
    internal class DefaultLanguageManager : ILanguageManager
    {
        private readonly CultureInfo localCulture;

        internal DefaultLanguageManager()
        {
            localCulture = Thread.CurrentThread.CurrentCulture;

            AvaliableLanguages = new List<LanguageModel>
            {
                new LanguageModel("English", "en"),
                new LanguageModel("Português", "pt-br"),
                new LanguageModel("Auto", string.Empty)
            };

            Initalize();
        }

        public IEnumerable<LanguageModel> AvaliableLanguages { get; }

        public LanguageModel CurrentLanguage => AvaliableLanguages.FirstOrDefault(l => l.Code == LanguageManagerSettings.Default.LanguageCode);

        private void Initalize()
        {
            SetupHumanizer();
            SetLanguage(CurrentLanguage);
        }

        public void SetLanguage(LanguageModel language)
        {
            if (language == CurrentLanguage)
                return;

            if (!string.IsNullOrEmpty(language.Code))
            {
                var culture = new CultureInfo(language.Code);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                ConfigHelper.Instance.SetLang(language.Code);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = localCulture;
                Thread.CurrentThread.CurrentUICulture = localCulture;
                ConfigHelper.Instance.SetLang("en");
            }

            LanguageManagerSettings.Default.LanguageCode = language.Code;
            LanguageManagerSettings.Default.Save();
        }

        private static void SetupHumanizer()
        {
            Configurator.DateTimeHumanizeStrategy = new DaysOnlyHumanizeStrategy();
        }

        public void Reset()
        {
            LanguageManagerSettings.Default.Reset();
            LanguageManagerSettings.Default.Save();
            SetLanguage(CurrentLanguage);
        }
    }
}