using SLStudio.Language.Rdf;
using SLStudio.RdfEditor.Modules.RdfEditor.Resources;

namespace SLStudio.RdfEditor.Modules.RdfEditor.ViewModels
{
    internal class RdfDesignerViewModel : RdfEditorViewModelBase
    {
        public RdfDesignerViewModel()
        {
            Definitions = new RdfCollection();

            DisplayName = RdfEditorResources.tab_designer_title;
            IconSource = "FrameworkDesignStudio";

            Definitions.Add(new RdfMetadata(new RdfAttributes(typeof(RdfDefinition)), new RdfDefinition()));
        }

        public RdfCollection Definitions { get; }

        public RdfMetadata SelectedDefinition
        {
            get => GetProperty(() => SelectedDefinition);
            set => SetProperty(() => SelectedDefinition, value);
        }
    }
}