using ICSharpCode.AvalonEdit.Document;
using SlrrLib.Model;
using SLStudio.Core;
using SLStudio.RpkEditor.Resources;
using System;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    [FileEditor(".rpk", "editorName", "editorDescription", "editorCategory", typeof(RpkEditorResources), "pack://application:,,,/SLStudio.RpkEditor;component/Resources/Icons/rpkFileIcon.png")]
    internal partial class RpkEditorViewModel : FileDocumentPanelBase
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