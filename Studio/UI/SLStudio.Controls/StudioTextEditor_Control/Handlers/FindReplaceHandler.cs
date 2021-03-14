using SLStudio.Core.Controls.StudioTextEditor.Views;
using System.Windows;
using System.Windows.Input;

namespace SLStudio.Controls.StudioTextEditor_Control
{
    internal class FindReplaceHandler
    {
        private readonly StudioTextEditor textEditor;
        private readonly FindAndReplaceViewModel viewModel;
        private FindAndReplaceView view;

        public FindReplaceHandler(StudioTextEditor textEditor, IFindReplaceService findReplace)
        {
            this.textEditor = textEditor;
            viewModel = new(findReplace);

            textEditor.PreviewKeyDown += OnPreviewKeyDown;
            textEditor.Loaded += OnLoaded;
            textEditor.Unloaded += OnUnloaded;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.F)
            {
                viewModel.IsOpen = true;
                e.Handled = true;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            view = textEditor.Template.FindName("PART_FindAndReplace", textEditor) as FindAndReplaceView;
            if (view is not null)
                view.DataContext = viewModel;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (view is not null)
            {
                view.DataContext = null;
                view = null;
            }
        }
    }
}