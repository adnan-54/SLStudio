using System.Threading.Tasks;

namespace SLStudio
{
    public interface IScheduledTask
    {
        bool Completed { get; }

        Task Run(IObjectFactory objectFactory);
    }
}