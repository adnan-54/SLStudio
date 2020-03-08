using DevExpress.Mvvm.UI.Interactivity;
using ICSharpCode.AvalonEdit;
using System.Windows;
using System.Windows.Controls;

namespace SLStudio.Core.Behaviors
{
    public class TextBoxScrollToEndBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is TextEditor textEditor)
                textEditor.TextChanged += OnTextChanged;
            else
            if (AssociatedObject is TextBox textBox)
                textBox.TextChanged += OnTextChanged;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject is TextEditor textEditor)
                textEditor.TextChanged -= OnTextChanged;
            else
            if (AssociatedObject is TextBox textBox)
                textBox.TextChanged -= OnTextChanged;

            base.OnDetaching();
        }

        private void OnTextChanged(object sender, object e)
        {
            if (sender is TextEditor textEditor)
                textEditor.ScrollToEnd();
            else
            if (sender is TextBox textBox)
                textBox.ScrollToEnd();
        }
    }
}