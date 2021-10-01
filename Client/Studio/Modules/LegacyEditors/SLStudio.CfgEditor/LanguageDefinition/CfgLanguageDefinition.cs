using SLStudio.Core;

namespace SLStudio.CfgEditor
{
    [LanguageDefinition]
    internal class CfgLanguageDefinition : LanguageDefinition
    {
        public CfgLanguageDefinition()
        {
            Name = "Cfg";
            Extension = ".cfg";
            LightThemeXshd = "SLStudio.CfgEditor.LanguageDefinition.LightHighlighting.xshd";
            DarkThemeXshd = "SLStudio.CfgEditor.LanguageDefinition.DarkHighlighting.xshd";
        }

        public override string Name { get; }

        public override string Extension { get; }

        protected override string LightThemeXshd { get; }

        protected override string DarkThemeXshd { get; }
    }
}