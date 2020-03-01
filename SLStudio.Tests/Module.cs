using SLStudio.Core;

namespace SLStudio.Tests
{
    public class Module : ModuleBase
    {
        public override string ModuleName => "Tests";

        public override string ModuleDescrition => "Module used for tests";

        public override void Register(IContainer container)
        {

        }
    }
}
