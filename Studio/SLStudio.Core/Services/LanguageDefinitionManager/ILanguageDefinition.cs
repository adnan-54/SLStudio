using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Indentation;
using SLStudio.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace SLStudio.Core
{
    public abstract class LanguageDefinition : ILanguageDefinition
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<LanguageDefinition>();

        private readonly IThemeManager themeManager;
        private IHighlightingDefinition syntaxHighlighting;

        protected LanguageDefinition()
        {
            themeManager = IoC.Get<IThemeManager>();
        }

        public abstract string Name { get; }

        public abstract string Extension { get; }

        public virtual IIndentationStrategy IndentationStrategy { get; }

        public virtual IFoldingStrategy FoldingStrategy { get; }

        public IHighlightingDefinition SyntaxHighlighting => GetSyntaxHighlighting();

        protected virtual string LightThemeSyntaxHighlightingPath { get; }

        protected virtual string DarkThemeSyntaxHighlightingPath { get; }

        private IHighlightingDefinition GetSyntaxHighlighting()
        {
            if (syntaxHighlighting == null)
                syntaxHighlighting = CreateSyntaxHighlighting(themeManager.CurrentTheme.IsDark ? DarkThemeSyntaxHighlightingPath : LightThemeSyntaxHighlightingPath);
            return syntaxHighlighting;
        }

        private IHighlightingDefinition CreateSyntaxHighlighting(string path)
        {
            try
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                using var reader = new XmlTextReader(stream);

                var xshd = HighlightingLoader.LoadXshd(reader);
                var manager = HighlightingManager.Instance;

                var definition = HighlightingLoader.Load(xshd, manager);
                manager.RegisterHighlighting(xshd.Name, xshd.Extensions.ToArray(), definition);

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