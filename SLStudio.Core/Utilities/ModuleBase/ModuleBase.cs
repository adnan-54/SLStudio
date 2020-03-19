using SLStudio.Core.Utilities.ModuleBase;
using System;
using System.Collections.Generic;

namespace SLStudio.Core
{
    public abstract class ModuleBase : IModule
    {
        public virtual ModulePriority ModulePriority => ModulePriority.Normal;
        public abstract string ModuleName { get; }
        public abstract string ModuleDescrition { get; }
        public virtual bool ShouldRegister => true;

        public virtual IEnumerable<Uri> GetResources()
        {
            yield return null;
        }

        public abstract void Register(IContainer container);
    }
}