using SLStudio.Language.Rdf;

namespace SLStudio.RdfEditor.Modules.Toolbox.Models
{
    internal class ToolboxItemModel
    {
        public ToolboxItemModel(RdfAttributes metadata)
        {
            Metadata = metadata;
        }

        public RdfAttributes Metadata { get; }
    }
}