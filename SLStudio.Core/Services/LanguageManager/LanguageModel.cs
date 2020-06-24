namespace SLStudio.Core
{
    public class LanguageModel
    {
        internal LanguageModel(string displayName, string code)
        {
            DisplayName = displayName;
            Code = code;
        }

        public string DisplayName { get; }
        public string Code { get; }
    }
}