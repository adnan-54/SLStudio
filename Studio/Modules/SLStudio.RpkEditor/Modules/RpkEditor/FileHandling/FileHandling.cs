using System.IO;
using System.Threading.Tasks;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    partial class RpkEditorViewModel
    {
        private string lastCheckpoint;

        protected override Task DoNew(string content)
        {
            return Task.CompletedTask;
        }

        protected override Task DoLoadFrom(Stream stream)
        {
            return Task.CompletedTask;
        }

        protected override async Task DoSaveTo(Stream stream)
        {
            var streamWriter = new StreamWriter(stream);
            await streamWriter.WriteAsync(Content);
            await streamWriter.FlushAsync();

            SetCheckpoint(Content);
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
    }
}