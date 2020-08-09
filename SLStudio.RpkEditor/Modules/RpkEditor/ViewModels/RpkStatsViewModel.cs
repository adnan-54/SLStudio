using SLStudio.RpkEditor.Modules.RpkEditor.Resources;

namespace SLStudio.RpkEditor.Modules.RpkEditor.ViewModels
{
    internal class RpkStatsViewModel : RpkEditorBase
    {
        public RpkStatsViewModel()
        {
            DisplayName = RpkEditorResources.Stats;
            IconSource = "ExplodedPieChart";
        }
    }
}