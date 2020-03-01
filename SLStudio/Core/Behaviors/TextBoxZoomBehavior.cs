using ICSharpCode.AvalonEdit;
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SLStudio.Core.Behaviors
{
    public class TextBoxZoomBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty FontMinSizeProperty = DependencyProperty.Register("FontMinSize", typeof(double), typeof(TextBoxZoomBehavior), new FrameworkPropertyMetadata(5d));
        public static readonly DependencyProperty FontMaxSizeProperty = DependencyProperty.Register("FontMaxSize", typeof(double), typeof(TextBoxZoomBehavior), new FrameworkPropertyMetadata(64d));
        public static readonly DependencyProperty StepsProperty = DependencyProperty.Register("Steps", typeof(double), typeof(TextBoxZoomBehavior), new FrameworkPropertyMetadata(3d));

        public double FontMinSize
        {
            get => (double)GetValue(FontMinSizeProperty);
            set => SetValue(FontMinSizeProperty, value);
        }

        public double FontMaxSize
        {
            get => (double)GetValue(FontMaxSizeProperty);
            set => SetValue(FontMaxSizeProperty, value);
        }

        public double Steps
        {
            get => (double)GetValue(StepsProperty);
            set => SetValue(StepsProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is TextEditor textEditor)
                textEditor.PreviewMouseWheel += TextEditorOnPreviewMouseWheel;
            else
            if (AssociatedObject is TextBox textBox)
                textBox.PreviewMouseWheel += TextBoxOnPreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject is TextEditor textEditor)
                textEditor.PreviewMouseWheel -= TextEditorOnPreviewMouseWheel;
            else
            if (AssociatedObject is TextBox textBox)
                textBox.PreviewMouseWheel -= TextBoxOnPreviewMouseWheel;

            base.OnDetaching();
        }

        private void TextEditorOnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                var textEditor = sender as TextEditor;
                double currentSize = textEditor.FontSize;

                if (e.Delta > 0)
                {
                    if (currentSize < FontMaxSize)
                    {
                        double newSize = Math.Min(FontMaxSize, currentSize + Steps);
                        textEditor.FontSize = newSize;
                    }
                }
                else
                {
                    if (currentSize > FontMinSize)
                    {
                        double newSize = Math.Max(FontMinSize, currentSize - Steps);
                        textEditor.FontSize = newSize;
                    }
                }

                e.Handled = true;
            }
        }

        private void TextBoxOnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                var textBox = sender as TextBox;
                double currentSize = textBox.FontSize;

                if (e.Delta > 0)
                {
                    if (currentSize < FontMaxSize)
                    {
                        double newSize = Math.Min(FontMaxSize, currentSize + Steps);
                        textBox.FontSize = newSize;
                    }
                }
                else
                {
                    if (currentSize > FontMinSize)
                    {
                        double newSize = Math.Max(FontMinSize, currentSize - Steps);
                        textBox.FontSize = newSize;
                    }
                }

                e.Handled = true;
            }
        }
    }
}