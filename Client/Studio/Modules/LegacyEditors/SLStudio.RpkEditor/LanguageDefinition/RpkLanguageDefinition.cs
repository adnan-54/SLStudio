using ICSharpCode.AvalonEdit.Highlighting;
using SLStudio.Core;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SLStudio.RpkEditor
{
    [LanguageDefinition]
    internal class RpkLanguageDefinition : LanguageDefinition
    {
        public RpkLanguageDefinition()
        {
            Name = "Rpk";
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

        protected override IHighlightingDefinition GetSyntaxHighlighting()
        {
            var syntaxHighlighting = base.GetSyntaxHighlighting();

            var specialWords = syntaxHighlighting.MainRuleSet.Rules.FirstOrDefault(r => r.Regex.ToString().Contains("TypeID|", StringComparison.OrdinalIgnoreCase)).Regex.ToString();
            specialWords = specialWords.Replace(@"\b(?>", "");
            specialWords = specialWords.Replace(@")\b", "");
            var regex = $"(?<=({specialWords}) ).*";
            var valuesRegex = new Regex(regex);
            var valuesColor = syntaxHighlighting.GetNamedColor("Values");
            var valuesRule = new HighlightingRule()
            {
                Regex = valuesRegex,
                Color = valuesColor
            };
            syntaxHighlighting.MainRuleSet.Rules.Add(valuesRule);

            return syntaxHighlighting;
        }
    }
}