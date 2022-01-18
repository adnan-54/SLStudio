using System.Reflection;

namespace SLStudio;

internal class AssemblyLoader : IAssemblyLoader
{
    private static readonly ILogger logger = LogManager.GetLogger();

    private readonly List<Assembly> loadedAssemblies;
    private bool isLoaded;

    public AssemblyLoader()
    {
        loadedAssemblies = new();
    }

    public IEnumerable<Assembly> LoadedAssemblies => loadedAssemblies;

    public void LoadAssemblies()
    {
        if (isLoaded)
            throw new InvalidOperationException("Assemblies already loaded");
        isLoaded = true;

        try
        {
            LoadAssembliesFrom("./", "SLStudio.*.dll");
            LoadAssembliesFrom("./Modules", "*.dll");
        }
        catch (Exception ex)
        {
            logger.Exception(ex);
            throw;
        }
    }

    private void LoadAssembliesFrom(string path, string filter)
    {
        if (!Directory.Exists(path))
        {
            logger.Warning("Could not load assemblies from '{0}' because the directory does not exist");
            return;
        }

        var files = Directory.GetFiles(path, filter);
        foreach (var file in files)
            LoadAssembly(file);
    }

    private void LoadAssembly(string file)
    {
        logger.Information("Loading assembly '{0}'", file);

        var assemblyName = AssemblyName.GetAssemblyName(file);
        var loaded = AppDomain.CurrentDomain.Load(assemblyName);
        loadedAssemblies.Add(loaded);
    }
}