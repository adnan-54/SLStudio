using DevExpress.Mvvm.UI;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Search;
using System.Windows;
using System.Windows.Media;

namespace SLStudio.Core.Behaviors
{
    public class AvalonEditSearchBehavior : ServiceBaseGeneric<TextEditor>, IAvalonEditSearch
    {
        private SearchPanel searchPanel;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            searchPanel = SearchPanel.Install(AssociatedObject.TextArea);
            if (WpfHelpers.TryFindResource("HoverBackground", out Brush brush))
                searchPanel.MarkerBrush = brush;
        }

        public void Find()
        {
            if (searchPanel.IsClosed)
                searchPanel.Open();

            searchPanel.Reactivate();
        }

        public void FindNext()
        {
            searchPanel.FindNext();
        }

        public void FindPrevious()
        {
            searchPanel.FindPrevious();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            base.OnDetaching();
        }
    }

    public interface IAvalonEditSearch
    {
        void Find();

        void FindNext();

        void FindPrevious();
    }
}