namespace SLStudio.Core.Utilities.ModuleBase
{
    public interface IModule
    {
        ModulePriority ModulePriority { get; }
        string ModuleName { get; }
        string ModuleDescrition { get; }
        bool ShouldBeLoaded { get; }

        void Register(IContainer container);
    }
}