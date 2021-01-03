using SimpleInjector;
using SLStudio.Core;

namespace SLStudio.FileTypes
{
    internal class Module : ModuleBase
    {
        public override string Name => "SLStudio.RpkEditor";
        public override string Author => "Adnan54";

        protected override void Register(Container container)
        {
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Rpk, RpkMetadata>();
            //});
        }
    }
}