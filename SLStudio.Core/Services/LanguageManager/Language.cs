using System.Globalization;
using Humanizer;

namespace SLStudio.Core.LanguageManager
{
    public class Language
    {
        public Language(string code)
        {
            if (!string.IsNullOrEmpty(code))
                Culture = CultureInfo.CreateSpecificCulture(code);
        }

        public CultureInfo Culture { get; }

        public string DisplayName => Culture?.NativeName.ApplyCase(LetterCasing.Title) ?? "Auto";

        public string Code => Culture?.Name;
    }
}