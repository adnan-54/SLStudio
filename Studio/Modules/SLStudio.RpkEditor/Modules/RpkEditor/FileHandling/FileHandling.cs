using System.Threading.Tasks;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    partial class RpkEditorViewModel
    {
        private string lastCheckpoint;

        protected override Task DoLoad()
        {
            return Task.CompletedTask;
        }

        protected override Task DoNew(string content)
        {
            Content = content;
            SetCheckpoint(Content);

            return Task.CompletedTask;
        }

        protected override Task DoSave()
        {
            SetCheckpoint(Content);

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