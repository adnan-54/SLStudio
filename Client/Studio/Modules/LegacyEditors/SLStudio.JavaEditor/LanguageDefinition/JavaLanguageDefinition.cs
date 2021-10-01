using SLStudio.Core;

namespace SLStudio.JavaEditor
{
    [LanguageDefinition]
    internal class JavaLanguageDefinition : LanguageDefinition
    {
        public JavaLanguageDefinition()
        {
            Name = "Java";
            Extension = ".java";
            FoldingStrategy = new JavaFoldingStrategy();
            LightThemeXshd = "SLStudio.JavaEditor.LanguageDefinition.LightHighlighting.xshd";
            DarkThemeXshd = "SLStudio.JavaEditor.LanguageDefinition.DarkHighlighting.xshd";
        }

        public override string Name { get; }

        public override string Extension { get; }

        public override IFoldingStrategy FoldingStrategy { get; }

        protected override string LightThemeXshd { get; }

        protected override string DarkThemeXshd { get; }
    }
}