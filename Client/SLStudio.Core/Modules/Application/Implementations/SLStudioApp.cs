using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio;

internal class SLStudioApp : Application, IApplication
{
}

public interface IApplication
{
    IWindowViewModel MainWindow { get; }

    IWindowViewModel CurrentWindow { get; }

    IEnumerable<IWindowViewModel> Windows { get; }

    ShutdownMode ShutdownMode { get; }

    int Run();

    void Shutdown(int exitCode);

    bool TryFindResource(out object resourceKey);

}