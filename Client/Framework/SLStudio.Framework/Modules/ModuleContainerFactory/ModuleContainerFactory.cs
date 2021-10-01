using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    internal class ModuleContainerFactory : StudioService, IModuleContainerFactory
    {
        private readonly IContainer container;

        public ModuleContainerFactory(IContainer container)
        {
            this.container = container;
        }

        public IModuleContainer CreateContainerFor(IStudioModule module)
        {
            var register = new ModuleRegister(container);
            var scheduler = new ModuleScheduler();
            var moduleContainer = new ModuleContainer(module, register, scheduler);

            return moduleContainer;
        }
    }
}