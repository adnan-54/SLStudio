using DevExpress.Mvvm;
using SLStudio.Core;
using SLStudio.RpkEditor.Rpk;
using SLStudio.RpkEditor.Rpk.Definitions;
using System.Collections.Generic;

namespace SLStudio.RpkEditor.Modules.ToolBox.ViewModels
{
    internal class RpkToolBoxViewModel : ViewModelBase, IToolboxContent
    {
        private readonly BindableCollection<ResourceBaseModel> resources;

        public RpkToolBoxViewModel()
        {
            resources = new BindableCollection<ResourceBaseModel>
            {
                new MeshDefinitionModel()
            };
        }

        public IReadOnlyCollection<ResourceBaseModel> Resources => resources;
    }
}