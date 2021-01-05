using ICSharpCode.AvalonEdit.Document;
using SlrrLib.Model;
using SLStudio.Core;
using SLStudio.RpkEditor.Resources;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    [FileEditor(".rpk", "editorName", "editorDescription", "editorCategory", typeof(RpkEditorResources), "pack://application:,,,/SLStudio.RpkEditor;component/Resources/Icons/rpkFileIcon.png")]
    internal partial class RpkEditorViewModel : FileDocumentBase
    {
        public RpkEditorViewModel()
        {
            TextDocument = new TextDocument();
            TextDocument.TextChanged += OnTextChanged;
        }

        public TextDocument TextDocument { get; }

        public string Content
        {
            get => TextDocument.Text;
            set => TextDocument.Text = value;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            UpdateIsDirty();
        }
    }
}