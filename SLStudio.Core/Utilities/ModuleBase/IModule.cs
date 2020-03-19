using System;
using System.Collections.Generic;

namespace SLStudio.Core.Utilities.ModuleBase
{
    public interface IModule
    {
        ModulePriority ModulePriority { get; }
        string ModuleName { get; }
        string ModuleDescrition { get; }
        bool ShouldRegister { get; }

        IEnumerable<Uri> GetResources();

        void Register(IContainer container);
    }
}