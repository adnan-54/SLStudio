using DevExpress.Mvvm;
using SLStudio.Core;
using SLStudio.Language.Rdf;
using SLStudio.RdfEditor.Modules.Toolbox.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.RdfEditor.Modules.Toolbox.ViewModels
{
    internal class RdfToolboxViewModel : ViewModelBase, IRdfToolbox
    {
        private readonly IRdfDefinitionLookup lookup;

        public RdfToolboxViewModel(IRdfDefinitionLookup lookup)
        {
            this.lookup = lookup;

            Items = new BindableCollection<ToolboxItemModel>();

            FetchItems().FireAndForget();
        }

        public BindableCollection<ToolboxItemModel> Items { get; }

        private Task FetchItems()
        {
            Items.Clear();
            Items.AddRange(lookup.Lookup().Select(m => new ToolboxItemModel(m)));
            return Task.CompletedTask;
        }
    }
}