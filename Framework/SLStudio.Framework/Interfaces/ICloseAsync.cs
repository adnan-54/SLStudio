using System.Threading.Tasks;

namespace SLStudio
{
    public interface ICloseAsync : IClose
    {
        Task CloseAsync();
    }
}