using SimpleInjector;
using SLStudio.Core;

namespace SLStudio.Language.Rdf
{
    internal class Module : ModuleBase
    {
        public override string Name => "SLStudio.Language.Rdf";
        public override string Author => "Adnan54";

        protected override void Register(Container container)
        {
            container.RegisterService<IRdfDefinitionLookup, DefaultRdfDefinitionLookup>();
        }
    }
}