using SLStudio.Core;
using SLStudio.Language.Rdf;
using SLStudio.RdfEditor.Modules.Toolbox.ViewModels;

namespace SLStudio.RdfEditor.Services
{
    internal class DefaultToolContentProvider : ToolContentProvider
    {
        private readonly IRdfDefinitionLookup lookup;

        public DefaultToolContentProvider(IRdfDefinitionLookup lookup)
        {
            this.lookup = lookup;
        }

        public override void Register()
        {
            Register<IToolbox, IRdfToolbox>(new RdfToolboxViewModel(lookup));
        }
    }
}