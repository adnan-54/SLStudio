using System.Globalization;

namespace SLStudio.Core.LanguageManager
{
    public class Language
    {
        internal Language(string code, string displayName = null, bool isAuto = false)
        {
            Culture = CultureInfo.CreateSpecificCulture(code);

            if (string.IsNullOrEmpty(displayName) && Culture != null)
                DisplayName = char.ToUpper(Culture.NativeName[0]) + Culture.NativeName.Substring(1);
            else
                DisplayName = displayName;

            IsAuto = isAuto;
        }

        public CultureInfo Culture { get; }

        public string DisplayName { get; }

        public string Code => Culture.Name;

        public bool IsAuto { get; }
    }
}