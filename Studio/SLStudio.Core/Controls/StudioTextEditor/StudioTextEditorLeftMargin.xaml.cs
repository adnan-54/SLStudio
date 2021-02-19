using System.Windows;
using System.Windows.Controls;

namespace SLStudio.Core
{
    public partial class StudioTextEditorLeftMargin : UserControl
    {
        public StudioTextEditorLeftMargin(UIElement marginContent)
        {
            MarginContent = marginContent;

            InitializeComponent();
        }

        public UIElement MarginContent { get; set; }
    }
}