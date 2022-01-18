using System.Reflection;

namespace SLStudio;

internal class ModuleLoader : IModuleLoader
{
    private static readonly ILogger logger = LogManager.GetLogger();

    private readonly IConfigurationContext configurationContext;
    private readonly List<IModule> loadedModules;
    private bool isLoaded;


    public ModuleLoader(IConfigurationContext configurationContext)
    {
        this.configurationContext = configurationContext;
        loadedModules = new();
    }

    public IEnumerable<IModule> LoadedModules => loadedModules;

    public void LoadModules()
    {
        if (isLoaded)
            throw new InvalidOperationException("Modules already loaded");
        isLoaded = true;

        try
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(ModuleBase)));
            var modules = types.Select(t => Activator.CreateInstance(t) as IModule ?? throw new InvalidOperationException("A invalid module has been detected"))
                               .OrderByDescending(m => m.Priority);

            foreach (var module in modules)
                LoadModule(module);
        }
        catch (Exception ex)
        {
            logger.Exception(ex);
            throw;
        }
    }

    private void LoadModule(IModule module)
    {
        logger.Information("Loading module '{0}'", module.Name);

        module.Configure(configurationContext);
        loadedModules.Add(module);
    }
}
