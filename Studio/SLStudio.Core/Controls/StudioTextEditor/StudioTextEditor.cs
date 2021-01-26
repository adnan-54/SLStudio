using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using SLStudio.Core.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SLStudio.Core
{
    public class StudioTextEditor : TextEditor
    {
        #region DependencyProperties

        static StudioTextEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StudioTextEditor), new FrameworkPropertyMetadata(typeof(StudioTextEditor)));
        }

        public static readonly DependencyProperty CurrentLineProperty = DependencyProperty.RegisterAttached("CurrentLine", typeof(int), typeof(TextEditor), new PropertyMetadata(1));

        public static readonly DependencyProperty CurrentColumnProperty = DependencyProperty.RegisterAttached("CurrentColumn", typeof(int), typeof(TextEditor), new PropertyMetadata(1));

        public static readonly DependencyProperty CurrentZoomProperty = DependencyProperty.RegisterAttached("CurrentZoom", typeof(double), typeof(TextEditor), new PropertyMetadata(1.0, OnCurrentZoomChanged));

        public static void SetCurrentLine(DependencyObject d, int value)
        {
            d.SetValue(CurrentLineProperty, value);
        }

        public static int GetCurrentLine(DependencyObject d)
        {
            return (int)d.GetValue(CurrentLineProperty);
        }

        public static void SetCurrentColumn(DependencyObject d, int value)
        {
            d.SetValue(CurrentColumnProperty, value);
        }

        public static int GetCurrentColumn(DependencyObject d)
        {
            return (int)d.GetValue(CurrentColumnProperty);
        }

        public static void SetCurrentZoom(DependencyObject d, double value)
        {
            d.SetValue(CurrentZoomProperty, value);
        }

        public static double GetCurrentZoom(DependencyObject d)
        {
            return (double)d.GetValue(CurrentZoomProperty);
        }

        private static void OnCurrentZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not StudioTextEditor textEditor || e.NewValue is not double newValue)
                return;

            var textArea = textEditor.TextArea;
            if (textArea != null)
                textArea.FontSize = 14f * newValue;
        }

        #endregion DependencyProperties

        private ComboBox zoomCombo;
        private string lastValidZoomComboValue;

        public StudioTextEditor()
        {
            TextArea.Caret.PositionChanged += CaretPositionChanged;
            PreviewMouseWheel += OnMouseWheel;
            PreviewKeyDown += OnPreviewKeyDown;
            Loaded += OnLoaded;
        }

        private void CaretPositionChanged(object sender, EventArgs e)
        {
            SetCurrentColumn(this, TextArea.Caret.Column);
            SetCurrentLine(this, TextArea.Caret.Line);
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                return;

            if (e.Delta > 0)
                IncreaseZoom();
            else
                DecreseZoom();

            e.Handled = true;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                if (e.Key == Key.Add)
                {
                    IncreaseZoom();
                    e.Handled = true;
                }
                else
                if (e.Key == Key.Subtract)
                {
                    DecreseZoom();
                    e.Handled = true;
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;

            SetupZoomComboBox();
            SetupCurrentLineHighlighter();
            SetupLeftMargins();
        }

        private void SetupZoomComboBox()
        {
            var scrollViewer = Template.FindName("PART_ScrollViewer", this) as ScrollViewer;
            zoomCombo = scrollViewer.Template.FindName("PART_ZoomComboBox", scrollViewer) as ComboBox;
            zoomCombo.PreviewKeyDown += PART_ZoomComboBox_OnPreviewKeyDown;
            zoomCombo.PreviewLostKeyboardFocus += PART_ZoomComboBox_OnPreviewLostKeyboardFocus;
            zoomCombo.DropDownClosed += PART_ZoomComboBox_OnDropDownClosed;
            lastValidZoomComboValue = zoomCombo.Text;
        }

        private void SetupCurrentLineHighlighter()
        {
            TextArea.Options.HighlightCurrentLine = true;

            WpfHelpers.TryFindResource("Border", out Brush borderBrush);
            var backgorund = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            var border = new Pen(borderBrush, 2.5);

            backgorund.Freeze();
            border.Freeze();

            TextArea.TextView.CurrentLineBackground = backgorund;
            TextArea.TextView.CurrentLineBorder = border;
        }

        private void SetupLeftMargins()
        {
            foreach (var item in TextArea.LeftMargins.ToList())
            {
                if (item is LineNumberMargin || item is Line)
                {
                    TextArea.LeftMargins.Remove(item);
                    continue;
                }
            }

            var leftMargin = new StudioTextEditorLeftMargin(CreateLineNumberMargin());
            TextArea.LeftMargins.Insert(0, leftMargin);
        }

        private LineNumberMargin CreateLineNumberMargin()
        {
            LineNumberMargin lineNumbers = new LineNumberMargin();
            WpfHelpers.TryFindResource("Focused", out SolidColorBrush foregroundColor);
            lineNumbers.SetValue(ForegroundProperty, foregroundColor);
            lineNumbers.SetValue(AbstractMargin.TextViewProperty, TextArea.TextView);
            lineNumbers.SetValue(AbstractMargin.TextViewProperty, TextArea.TextView);
            return lineNumbers;
        }

        private void PART_ZoomComboBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                TextArea.Focus();
                e.Handled = true;
            }
        }

        private void PART_ZoomComboBox_OnPreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (int.TryParse(zoomCombo.Text.TrimEnd('%'), out var _))
                lastValidZoomComboValue = zoomCombo.Text;
            else
                zoomCombo.Text = lastValidZoomComboValue;
        }

        private void PART_ZoomComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            TextArea.Focus();
        }

        private void IncreaseZoom()
        {
            var targetZoom = GetCurrentZoom(this) + 0.1;
            if (targetZoom >= 4.0)
                targetZoom = 4.0;

            SetCurrentZoom(this, targetZoom);
        }

        private void DecreseZoom()
        {
            var targetZoom = GetCurrentZoom(this) - 0.1;
            if (targetZoom <= 0.2)
                targetZoom = 0.2;

            SetCurrentZoom(this, targetZoom);
        }
    }
}