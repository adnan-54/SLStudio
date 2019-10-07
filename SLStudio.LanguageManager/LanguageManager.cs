using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace SLStudio.LanguageManager
{
    public static class LanguageManager
    {
        private static readonly ILog Log = LogManager.GetLogger(nameof(LanguageManager));

        private static AvaliableLanguages CurrentLanguage { get; set; }

        private static readonly List<IMultiLanguageObject> multiLanguageObjects = new List<IMultiLanguageObject>();

        public static void Initialize()
        {
            CurrentLanguage = (AvaliableLanguages)SettingsManager.LanguageManager.Default.CurrentLanguage;
            SetCurrentLanguage(CurrentLanguage);
        }

        public static void SetCurrentLanguage(AvaliableLanguages language)
        {
            try
            {
                CultureInfo newCulture = new CultureInfo(language.ToString(), true);
                newCulture.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = newCulture;
                Thread.CurrentThread.CurrentUICulture = newCulture;

                SettingsManager.LanguageManager.Default.CurrentLanguage = (int)language;
                SettingsManager.LanguageManager.Default.Save();

                UpdateLanguage();
            }
            catch(Exception ex)
            {
                Log.Error(ex);
            }
        }

        public static AvaliableLanguages GetCurrentLanguage()
        {
            return CurrentLanguage;
        }

        public static void Register(IMultiLanguageObject multiLanguageObject)
        {
            if (multiLanguageObjects.IndexOf(multiLanguageObject) == -1)
                multiLanguageObjects.Add(multiLanguageObject);
        }

        public static void UpdateLanguage()
        {
            foreach(IMultiLanguageObject @object in multiLanguageObjects)
            {
                @object.UpdateLanguage();
            }
        }
    }
}
