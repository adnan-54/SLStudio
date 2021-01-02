using System.Threading.Tasks;

namespace SLStudio.Data.Rdf
{
    public interface IRdfFilePersister
    {
        IRdfFile New();

        Task<IRdfFile> Read(string path);

        Task Save(IRdfFile rdf);

        Task<IRdfFile> SaveAs(string path, IRdfFile rdf);
    }
}