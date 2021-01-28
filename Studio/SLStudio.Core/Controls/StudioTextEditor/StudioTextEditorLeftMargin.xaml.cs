using System.Windows;
using System.Windows.Controls;

namespace SLStudio.Core.Controls
{
    public partial class StudioTextEditorLeftMargin : UserControl
    {
        public static readonly DependencyProperty MarginContentProperty = DependencyProperty.Register("MarginContent", typeof(UIElement), typeof(StudioTextEditorLeftMargin), new PropertyMetadata(null));

        public StudioTextEditorLeftMargin(UIElement marginContent)
        {
            InitializeComponent();
            MarginContent = marginContent;
        }

        public UIElement MarginContent
        {
            get => (UIElement)GetValue(MarginContentProperty);
            set => SetValue(MarginContentProperty, value);
        }
    }
}