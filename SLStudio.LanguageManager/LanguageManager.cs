using SLStudio.Logging;
using System.Reflection;

namespace SLStudio.Language
{
    public static class LanguageManager
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

        private static string DefaultLanguage { get; set; }

        public static void Initialize()
        {

        }

        public static void SetCurrentLanguage(AvaliableLanguages language)
        {

        }
    }
}
