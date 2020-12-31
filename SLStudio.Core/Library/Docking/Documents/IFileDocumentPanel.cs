using System.Threading.Tasks;

namespace SLStudio.Core
{
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