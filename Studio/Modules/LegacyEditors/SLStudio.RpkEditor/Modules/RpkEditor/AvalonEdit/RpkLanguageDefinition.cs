using SLStudio.Core;

namespace SLStudio.RpkEditor
{
    [LanguageDefinition]
    internal class RpkLanguageDefinition : LanguageDefinition
    {
        public override string Name => "Rpk File";

        public override string Extension => ".rpk";

        protected override string LightThemeXshd => "SLStudio.RpkEditor.Modules.RpkEditor.AvalonEdit.RpkLight.xshd";

        protected override string DarkThemeXshd => "SLStudio.RpkEditor.Modules.RpkEditor.AvalonEdit.RpkLight.xshd";
    }
}