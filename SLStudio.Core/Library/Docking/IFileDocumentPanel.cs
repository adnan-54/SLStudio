namespace SLStudio.Core
{
    public class FileDocumentPanelBase : DocumentPanelBase, IFileDocumentPanel
    {
        public bool IsDirty
        {
            get => GetProperty(() => IsDirty);
            set => SetProperty(() => IsDirty, value);
        }
    }

    public interface IFileDocumentPanel : IDocumentPanel
    {
        bool IsDirty { get; }
    }
}