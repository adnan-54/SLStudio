using DevExpress.Mvvm;
using SLStudio.Core;
using SLStudio.RpkEditor.Data;
using SLStudio.RpkEditor.Modules.Toolbox.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.RpkEditor.Modules.ToolBox.ViewModels
{
    internal class RpkToolBoxViewModel : ViewModelBase, IToolboxContent
    {
        private readonly BindableCollection<ToolboxItemModel> resources;

        public RpkToolBoxViewModel()
        {
            resources = new BindableCollection<ToolboxItemModel>();
            FetchItems().FireAndForget();
        }

        public IReadOnlyCollection<ToolboxItemModel> Resources => resources;

        private Task FetchItems()
        {
            resources.Clear();
            resources.Add(new ToolboxItemModel(new MeshDefinitionMetadata()));
            return Task.CompletedTask;
        }
    }
}