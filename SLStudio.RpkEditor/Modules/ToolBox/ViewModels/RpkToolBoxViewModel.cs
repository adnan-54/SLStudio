using DevExpress.Mvvm;
using SLStudio.Core;
using SLStudio.RpkEditor.Modules.Toolbox.Models;
using SLStudio.RpkEditor.Rpk.Definitions;
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
            resources.Add(new ToolboxItemModel(new MeshDefinition()));
            return Task.CompletedTask;
        }
    }
}