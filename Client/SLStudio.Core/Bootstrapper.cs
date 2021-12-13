using System.Diagnostics;
using SLStudio.Logger;

namespace SLStudio;

public class Bootstrapper
{
    private static readonly ILogger logger = LogManager.GetLogger<Bootstrapper>();
    private static readonly Bootstrapper bootstrapper = new();

    public static int Run()
    {
        return 0;
    }
}