using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Indentation;
using SLStudio.Logging;
using System;
using System.Xml;

namespace SLStudio.Core
{
    public abstract class LanguageDefinition : ILanguageDefinition
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<LanguageDefinition>();

        private readonly IThemeManager themeManager;
        private IHighlightingDefinition syntaxHighlighting;

        protected LanguageDefinition(IThemeManager themeManager)
        {
            this.themeManager = themeManager;
        }

        public abstract string Name { get; }

        public abstract string Extension { get; }

        public virtual IIndentationStrategy IndentationStrategy { get; }

        public virtual IFoldingStrategy FoldingStrategy { get; }

        public IHighlightingDefinition SyntaxHighlighting => GetSyntaxHighlighting();

        protected virtual string LightThemeXshd { get; }

        protected virtual string DarkThemeXshd { get; }

        private IHighlightingDefinition GetSyntaxHighlighting()
        {
            if (syntaxHighlighting == null)
                syntaxHighlighting = CreateSyntaxHighlighting(themeManager.CurrentTheme.IsDark ? DarkThemeXshd : LightThemeXshd);
            return syntaxHighlighting;
        }

        private IHighlightingDefinition CreateSyntaxHighlighting(string path)
        {
            try
            {
                using var stream = GetType().Assembly.GetManifestResourceStream(path);
                using var reader = new XmlTextReader(stream);

                var xshd = HighlightingLoader.LoadXshd(reader);
                var manager = HighlightingManager.Instance;

                var definition = HighlightingLoader.Load(xshd, manager);
                manager.RegisterHighlighting(Name, new string[] { Extension }, definition);

                return manager.GetDefinition(Name);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }
    }

    public interface ILanguageDefinition
    {
        string Name { get; }

        string Extension { get; }

        IIndentationStrategy IndentationStrategy { get; }

        IFoldingStrategy FoldingStrategy { get; }

        IHighlightingDefinition SyntaxHighlighting { get; }
    }
}