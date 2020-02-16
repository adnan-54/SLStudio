using Caliburn.Micro;

namespace SLStudio.Core.Utilities.ModuleBase
{
    public interface IModule
    {
        bool ShouldBeLoaded { get; }
        ModulePriority ModulePriority { get; }
        string ModuleName { get; }
        string ModuleDescrition { get; }

        void Register(SimpleContainer container);
    }
}