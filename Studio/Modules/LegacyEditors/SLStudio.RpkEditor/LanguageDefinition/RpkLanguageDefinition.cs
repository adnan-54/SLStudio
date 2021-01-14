using SLStudio.Core;

namespace SLStudio.RpkEditor
{
    [LanguageDefinition]
    internal class RpkLanguageDefinition : LanguageDefinition
    {
        public RpkLanguageDefinition(IThemeManager themeManager) : base(themeManager)
        {
            Name = "Rpk File";
            Extension = ".rpk";
            FoldingStrategy = new RpkFoldingStrategy();
            LightThemeXshd = "SLStudio.RpkEditor.LanguageDefinition.LightHighlighting.xshd";
            DarkThemeXshd = "SLStudio.RpkEditor.LanguageDefinition.DarkHighlighting.xshd";
        }

        public override string Name { get; }

        public override string Extension { get; }

        public override IFoldingStrategy FoldingStrategy { get; }

        protected override string LightThemeXshd { get; }

        protected override string DarkThemeXshd { get; }
    }
}