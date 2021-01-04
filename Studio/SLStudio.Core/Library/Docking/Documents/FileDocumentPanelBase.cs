using System.IO;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public abstract class FileDocumentPanelBase : DocumentPanelBase, IFileDocumentPanel
    {
        public string FileName
        {
            get => GetProperty(() => FileName);
            set => SetProperty(() => FileName, value);
        }

        public bool IsNew
        {
            get => GetProperty(() => IsNew);
            set => SetProperty(() => IsNew, value);
        }

        public bool IsDirty
        {
            get => GetProperty(() => IsDirty);
            set => SetProperty(() => IsDirty, value);
        }

        public virtual bool ShouldBackup => IsDirty;

        public Task New(string displayName, string content)
        {
            IsNew = true;
            DisplayName = displayName;

            return DoNew(content);
        }

        public Task Load(string fileName)
        {
            IsNew = false;
            FileName = fileName;
            DisplayName = Path.GetFileName(FileName);

            return DoLoad();
        }

        public async Task Save(string fileName)
        {
            IsNew = false;
            FileName = fileName;
            DisplayName = Path.GetFileName(fileName);

            await DoSave();
            IsDirty = false;
        }

        protected abstract Task DoNew(string content);

        protected abstract Task DoLoad();

        protected abstract Task DoSave();
    }
}