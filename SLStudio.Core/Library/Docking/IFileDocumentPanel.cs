using System.IO;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public abstract class FileDocumentPanelBase : DocumentPanelBase, IFileDocumentPanel
    {
        public string FileName
        {
            get => GetProperty(() => FileName);
            private set => SetProperty(() => FileName, value);
        }

        public bool IsNew
        {
            get => GetProperty(() => IsNew);
            protected set => SetProperty(() => IsNew, value);
        }

        public bool IsDirty
        {
            get => GetProperty(() => IsDirty);
            protected set => SetProperty(() => IsDirty, value);
        }

        public virtual bool ShouldBackup => IsDirty;

        public Task New(string displayName, string content)
        {
            IsNew = true;
            DisplayName = displayName;

            return DoNew(content);
        }

        protected abstract Task DoNew(string content);

        public Task Load(string fileName)
        {
            IsNew = false;
            FileName = fileName;
            DisplayName = Path.GetFileName(FileName);

            return DoLoad();
        }

        protected abstract Task DoLoad();

        public async Task Save(string fileName)
        {
            IsNew = false;
            FileName = fileName;
            DisplayName = Path.GetFileName(fileName);

            await DoSave();
            IsDirty = false;
        }

        protected abstract Task DoSave();
    }

    public interface IFileDocumentPanel : IDocumentPanel
    {
        string FileName { get; }

        bool IsNew { get; }
        bool IsDirty { get; }
        bool ShouldBackup { get; }

        Task New(string displayName, string content);

        Task Load(string fileName);

        Task Save(string fileName);
    }
}