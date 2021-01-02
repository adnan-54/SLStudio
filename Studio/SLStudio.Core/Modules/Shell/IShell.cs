using System.Threading.Tasks;

namespace SLStudio.Core
{
    public interface IShell : IWindow
    {
        BindableCollection<IDocumentPanel> Documents { get; }

        BindableCollection<IToolPanel> Tools { get; }

        IPanelItem SelectedItem { get; }

        Task OpenPanel(IPanelItem item);

        Task ClosePanel(IPanelItem item);
    }
}