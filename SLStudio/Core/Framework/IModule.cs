using Caliburn.Micro;

namespace SLStudio.Core
{
    public abstract class ModuleBase : IModule
    {
        public bool ShouldBeLoaded => true;
        public virtual ModulePriority ModulePriority => ModulePriority.Normal;
        
        public abstract string ModuleName { get; }
        public abstract string ModuleDescrition { get; }

        public abstract void Register(SimpleContainer container);
    }

    public interface IModule
    {
        bool ShouldBeLoaded { get; }
        ModulePriority ModulePriority { get; }
        string ModuleName { get; }
        string ModuleDescrition { get; }

        void Register(SimpleContainer container);
    }

    public enum ModulePriority
    {
        Low,
        Normal,
        High,
        Core
    }
}
