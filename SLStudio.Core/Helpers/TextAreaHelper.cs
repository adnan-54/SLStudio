using ICSharpCode.AvalonEdit.Editing;
using System.Windows;
using System.Windows.Media;

namespace SLStudio.Core
{
    public static class TextAreaHelper
    {
        public static DependencyProperty CaretBrushProperty = DependencyProperty.RegisterAttached("CaretBrush", typeof(Brush), typeof(TextAreaHelper), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(15, 15, 15)), OnCaretBrushChanged));

        public static void SetCaretBrush(DependencyObject d, Brush value)
        {
            d.SetValue(CaretBrushProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(TextArea))]
        public static Brush GetCaretBrush(DependencyObject d)
        {
            return (Brush)d.GetValue(CaretBrushProperty);
        }

        private static void OnCaretBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextArea textArea) || !(e.NewValue is Brush brush))
                return;
            textArea.Caret.CaretBrush = brush;
        }
    }
}