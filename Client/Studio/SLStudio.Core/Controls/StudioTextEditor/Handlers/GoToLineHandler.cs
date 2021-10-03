using System.Windows.Input;

namespace SLStudio.Core
{
    internal class GoToLineHandler
    {
        private readonly GoToLineViewModel dialog;

        public GoToLineHandler(StudioTextEditor textEditor)
        {
            dialog = new GoToLineViewModel(textEditor);

            textEditor.PreviewKeyDown += OnPreviewKeyDown;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.G)
            {
                dialog.Show();
                e.Handled = true;
            }
        }
    }
}