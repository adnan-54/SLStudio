using DevExpress.Mvvm.UI;
using SLStudio.Core;

namespace SLStudio.RpkEditor.Behaviors
{
    internal class RpkTextEditorBehavior : ServiceBaseGeneric<StudioTextEditor>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            var cuzito = IoC.Get<ILanguageDefinitionManager>();
            var tabaquito = cuzito.GetByExtension(".rpk");
            AssociatedObject.SyntaxHighlighting = tabaquito.SyntaxHighlighting;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}