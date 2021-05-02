namespace SLStudio.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var initializer = Initializer.Initialize();
            var assemblyLoader = initializer.GetInstance<IAssemblyLoader>();
            assemblyLoader.LoadAssemblies();
            var moduleLoader = initializer.GetInstance<IModuleLoader>();
            moduleLoader.LoadModules();
        }
    }
}