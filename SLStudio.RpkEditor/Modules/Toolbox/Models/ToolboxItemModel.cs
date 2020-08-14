using SLStudio.RpkEditor.Rpk;

namespace SLStudio.RpkEditor.Modules.Toolbox.Models
{
    internal class ToolboxItemModel
    {
        public ToolboxItemModel(ResourceMetadata metadata)
        {
            Metadata = metadata;
        }

        public ResourceMetadata Metadata { get; }

        public string DisplayName => Metadata?.DisplayName;

        public string IconSource => Metadata?.IconSource;

        public string Category => Metadata?.Category;
    }
}