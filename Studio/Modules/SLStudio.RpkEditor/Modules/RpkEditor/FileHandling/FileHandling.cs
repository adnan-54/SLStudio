using System.IO;
using System.Threading.Tasks;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    partial class RpkEditorViewModel
    {
        private string lastCheckpoint;

        protected override Task DoNew(string content)
        {
            content ??= string.Empty;

            SetCheckpoint(content);

            return Task.CompletedTask;
        }

        protected override Task DoLoadFrom(Stream stream)
        {
            return Task.CompletedTask;
        }

        protected override Task DoSaveTo(Stream stream)
        {
            return Task.CompletedTask;
        }

        private void SetCheckpoint(string content)
        {
            lastCheckpoint = content;
            UpdateIsDirty();
        }

        private void UpdateIsDirty()
        {
            if (string.IsNullOrEmpty(lastCheckpoint))
                lastCheckpoint = string.Empty;

            IsDirty = !lastCheckpoint.Equals(Content);
        }
    }
}