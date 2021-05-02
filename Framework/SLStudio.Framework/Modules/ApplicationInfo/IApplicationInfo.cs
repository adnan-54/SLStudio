using System.Collections.Generic;
using System.Windows.Threading;

namespace SLStudio
{
    public interface IApplicationInfo : IStudioService
    {
        IEnumerable<string> StartupArguments { get; }

        Dispatcher Dispatcher { get; }
    }
}