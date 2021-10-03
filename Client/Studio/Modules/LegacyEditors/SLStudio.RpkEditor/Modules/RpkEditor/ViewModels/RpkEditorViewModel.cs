using ICSharpCode.AvalonEdit.Document;
using SlrrLib.Model;
using SLStudio.Core;
using SLStudio.RpkEditor.Resources;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SLStudio.RpkEditor
{
    [FileEditor("editorName", "editorDescription", "editorCategory", typeof(RpkEditorResources), "RpkIcon", false, ".rpk")]
    internal class RpkEditorViewModel : FileDocumentBase, IRpkEditor
    {
        private string lastCheckpoint;
        private int busyOperations = 0;

        public RpkEditorViewModel()
        {
            TextDocument = new TextDocument();
            TextDocument.TextChanged += OnTextChanged;
        }

        public bool IsBusy => busyOperations > 0;

        public TextDocument TextDocument { get; }

        public string Content
        {
            get => TextDocument.Text;
            set => TextDocument.Text = value;
        }

        protected override Task DoNew(string content)
        {
            Content = content;

            return Task.CompletedTask;
        }

        protected async override Task DoLoadFrom(Stream stream)
        {
            Busy();

            var rpkex = new RpkExchange();
            Content = await rpkex.GetRdb2FromRpkStream(stream);
            SetCheckpoint(Content);

            Idle();
        }

        protected override async Task DoSaveTo(Stream stream)
        {
            Busy();

            var rpkex = new RpkExchange();
            await rpkex.ConvertStringToRpkStream(stream, Content);
            SetCheckpoint(Content);

            Idle();
        }

        private void Busy()
        {
            ++busyOperations;
            RaisePropertyChanged(() => IsBusy);
        }

        private void Idle()
        {
            --busyOperations;
            RaisePropertyChanged(() => IsBusy);
        }

        private void SetCheckpoint(string content)
        {
            lastCheckpoint = content;
            UpdateIsDirty();
        }

        private void UpdateIsDirty()
        {
            if (IsNew || string.IsNullOrEmpty(FileName))
            {
                IsDirty = true;
                return;
            }

            if (string.IsNullOrEmpty(lastCheckpoint))
                lastCheckpoint = string.Empty;

            IsDirty = !lastCheckpoint.Equals(Content);
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            UpdateIsDirty();
        }
    }

    public interface IRpkEditor : IFileDocumentItem
    {
    }
}