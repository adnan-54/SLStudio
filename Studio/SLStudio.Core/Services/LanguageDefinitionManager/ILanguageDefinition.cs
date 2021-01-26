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

        private Lazy<IHighlightingDefinition> syntaxHighlighting;

        protected LanguageDefinition()
        {
            syntaxHighlighting = new Lazy<IHighlightingDefinition>(GetSyntaxHighlighting);
        }

        public abstract string Name { get; }

        public abstract string Extension { get; }

        public virtual IIndentationStrategy IndentationStrategy { get; }

        public virtual IFoldingStrategy FoldingStrategy { get; }

        public IHighlightingDefinition SyntaxHighlighting => syntaxHighlighting.Value;

        protected virtual string LightThemeXshd { get; }

        protected virtual string DarkThemeXshd { get; }

        protected virtual IHighlightingDefinition GetSyntaxHighlighting()
        {
            var themeManager = IoC.Get<IThemeManager>();
            return CreateSyntaxHighlighting(themeManager.CurrentTheme.IsDark ? DarkThemeXshd : LightThemeXshd);
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