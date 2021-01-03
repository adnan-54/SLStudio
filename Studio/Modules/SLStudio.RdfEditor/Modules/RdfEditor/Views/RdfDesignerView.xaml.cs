using SLStudio.Core.Controls;
using System.Windows.Controls;

namespace SLStudio.RdfEditor.Modules.RdfEditor.Views
{
    public partial class RdfDesignerView : UserControl
    {
        public RdfDesignerView()
        {
            InitializeComponent();

            ItemsControl.Items.Add(new ListboxLineNumberMargin());
            foreach (IMetadataAware aware in ItemsControl.Items)
                aware.Attach(ListBox);
        }
    }
}