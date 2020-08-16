using ICSharpCode.AvalonEdit.Document;
using SLStudio.Core;
using SLStudio.RpkEditor.Modules.RpkEditor.Resources;
using System.Threading.Tasks;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    internal class RpkCodeViewModel : RpkEditorBase
    {
        private readonly IUiSynchronization uiSynchronization;

        public RpkCodeViewModel(IUiSynchronization uiSynchronization)
        {
            this.uiSynchronization = uiSynchronization;

            TextDocument = new TextDocument();

            DisplayName = RpkEditorResources.Code;
            IconSource = "CodeDefinition";
        }

        public TextDocument TextDocument { get; }

        public async Task UpdateContent(string content)
        {
            await uiSynchronization.EnsureExecuteOnUiAsync(() => TextDocument.Text = content);
        }
    }
}