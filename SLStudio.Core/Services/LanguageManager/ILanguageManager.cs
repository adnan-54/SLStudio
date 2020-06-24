using System.Collections.Generic;

namespace SLStudio.Core
{
    public interface ILanguageManager
    {
        IEnumerable<LanguageModel> AvaliableLanguages { get; }
        LanguageModel CurrentLanguage { get; }

        void SetLanguage(LanguageModel language);

        void Reset();
    }
}