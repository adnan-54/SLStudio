using Caliburn.Micro;

namespace SLStudio.Core
{
    public interface IModule
    {
        bool ShouldBeLoaded { get; }
        string ModuleName { get; }
        string ModuleDescrition { get; }
        void Register(SimpleContainer container);
    }
}
