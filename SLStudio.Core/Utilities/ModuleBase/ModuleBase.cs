using SLStudio.Core.Utilities.ModuleBase;

namespace SLStudio.Core
{
    public abstract class ModuleBase : IModule
    {
        public virtual ModulePriority ModulePriority => ModulePriority.Normal;
        public abstract string ModuleName { get; }
        public abstract string ModuleDescrition { get; }
        public virtual bool ShouldBeLoaded => true;

        public abstract void Register(IContainer container);
    }
}