using System;
using System.Threading.Tasks;

namespace SLStudio
{
    public interface IModule
    {
        string Name { get; }

        string Author { get; }

        int Priority { get; }

        bool CanBeLoaded { get; }

        Version Version { get; }

        void OnRegister(IModuleRegister register);

        void OnSchedule(IModuleScheduler schedule);

        Task OnRun(IObjectFactory objectFactory);
    }
}