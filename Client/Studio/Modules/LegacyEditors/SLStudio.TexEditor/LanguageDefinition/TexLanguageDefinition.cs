using SLStudio.Core;

namespace SLStudio.TexEditor
{
    [LanguageDefinition]
    internal class TexLanguageDefinition : LanguageDefinition
    {
        public TexLanguageDefinition()
        {
            Name = "Tex";
            Extension = ".tex";
            LightThemeXshd = "SLStudio.TexEditor.LanguageDefinition.LightHighlighting.xshd";
            DarkThemeXshd = "SLStudio.TexEditor.LanguageDefinition.DarkHighlighting.xshd";
        }

        public override string Name { get; }

        public override string Extension { get; }

        protected override string LightThemeXshd { get; }

        protected override string DarkThemeXshd { get; }
    }
}