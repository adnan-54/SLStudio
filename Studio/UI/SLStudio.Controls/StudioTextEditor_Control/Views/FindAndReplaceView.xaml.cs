using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SLStudio.Core.Controls.StudioTextEditor.Views
{
    /// <summary>
    /// Interaction logic for FindAndReplaceView.xaml
    /// </summary>
    public partial class FindAndReplaceView : UserControl
    {
        public FindAndReplaceView()
        {
            DataContext = new FindAndReplaceViewModel();
            InitializeComponent();
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var targetWidth = ActualWidth - e.HorizontalChange;
            if (targetWidth >= MinWidth && targetWidth <= MaxWidth)
                Width = targetWidth;
        }
    }
}