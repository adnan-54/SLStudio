using SLStudio.Logging;
using System.IO;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public abstract class FileDocumentBase : DocumentBase, IFileDocumentItem
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<FileDocumentBase>();

        public string FileName
        {
            get => GetProperty(() => FileName);
            set
            {
                SetProperty(() => FileName, value);
                if (!string.IsNullOrEmpty(value))
                    DisplayName = Path.GetFileName(value);
            }
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

        public virtual bool CanSave => true;

        public Task New(string displayName, string content)
        {
            IsNew = true;
            IsDirty = true;
            DisplayName = displayName;

            return DoNew(content);
        }

        public Task LoadFrom(Stream stream)
        {
            IsNew = false;

            return DoLoadFrom(stream);
        }

        public Task SaveTo(Stream stream)
        {
            IsNew = false;
            IsDirty = false;

            return DoSaveTo(stream);
        }

        protected abstract Task DoNew(string content);

        protected abstract Task DoLoadFrom(Stream stream);

        protected abstract Task DoSaveTo(Stream stream);
    }

    public interface IFileDocumentItem : IDocumentItem
    {
        string FileName { get; set; }

        bool IsNew { get; }

        bool IsDirty { get; }

        bool CanSave { get; }

        Task New(string displayName, string content);

        Task LoadFrom(Stream stream);

        Task SaveTo(Stream stream);
    }
}