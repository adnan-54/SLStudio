using System.Globalization;

namespace SLStudio.Core.LanguageManager
{
    public class Language
    {
        internal Language(string code)
        {
            if (!string.IsNullOrEmpty(code))
                Culture = CultureInfo.CreateSpecificCulture(code);
        }

        public CultureInfo Culture { get; }

        public string DisplayName => Culture?.NativeName ?? "Auto";

        public string Code => Culture?.Name;
    }
}