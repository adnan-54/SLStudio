using System.Reflection;

namespace SLStudio;

internal abstract class StudioModule : ModuleBase
{
    public override string Name => GetType().Name;

    public override string Description => "Default SLStudio module";

    public override string License => "MIT";

    public override string Author => "Adnan54";

    public override Version Version => Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(1, 0, 0);

    public override int Priority => 0;
}
