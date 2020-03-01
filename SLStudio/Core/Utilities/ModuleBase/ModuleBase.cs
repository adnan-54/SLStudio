using SLStudio.Core.Utilities.ModuleBase;

namespace SLStudio.Core
{
    internal abstract class ModuleBase : IModule
    {
        public bool ShouldBeLoaded => true;

        public virtual ModulePriority ModulePriority => ModulePriority.Normal;

        public abstract string ModuleName { get; }

        public abstract string ModuleDescrition { get; }

        public abstract void Register(IContainer container);
    }
}