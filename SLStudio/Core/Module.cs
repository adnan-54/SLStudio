using Caliburn.Micro;

namespace SLStudio.Core
{
    internal class Module : ModuleBase
    {
        public override ModulePriority ModulePriority => ModulePriority.Core;

        public override string ModuleName => "SLStudio Core";
        public override string ModuleDescrition => "Core module.";

        public override void Register(SimpleContainer container)
        {
        }
    }
}