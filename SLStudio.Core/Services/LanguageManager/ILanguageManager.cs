using SLStudio.Core.LanguageManager;
using System.Collections.Generic;

namespace SLStudio.Core
{
    public interface ILanguageManager
    {
        IEnumerable<Language> AvaliableLanguages { get; }

        Language CurrentLanguage { get; }

        void SetLanguage(Language language);

        void Reset();
    }
}