using System.Threading.Tasks;

namespace SLStudio
{
    internal class Scheduler : IScheduler
    {
        public Task Run()
        {
            return Task.CompletedTask;
        }
    }

    internal interface IScheduler
    {
        Task Run();
    }

    internal class Register : IRegister
    {
    }

    internal interface IRegister
    {
    }
}