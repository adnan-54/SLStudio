namespace SLStudio;

public interface IModuleLoader
{
    IEnumerable<IModule> LoadedModules { get; }

    void LoadModules();
}
