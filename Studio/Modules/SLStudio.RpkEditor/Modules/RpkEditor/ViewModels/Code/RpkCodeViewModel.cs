using ICSharpCode.AvalonEdit.Document;
using SLStudio.RpkEditor.Modules.RpkEditor.Resources;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    internal class RpkCodeViewModel : RpkEditorBase
    {
        public RpkCodeViewModel()
        {
            TextDocument = new TextDocument();

            DisplayName = RpkEditorResources.Code;
            IconSource = "CodeDefinition";
        }

        public TextDocument TextDocument { get; }

        public void UpdateContent(string content)
        {
            TextDocument.Text = content;
        }
    }
}