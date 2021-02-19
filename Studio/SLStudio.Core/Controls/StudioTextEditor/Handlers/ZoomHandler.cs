using DevExpress.Mvvm;
using ICSharpCode.AvalonEdit.Folding;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SLStudio.Core
{
    internal class ZoomHandler : BindableBase, ITextEditorHandler
    {
        private readonly StudioTextEditor textEditor;
        private double initialFontSize;
        private ComboBox combo;
        private double lastValidZoomComboValue;

        public ZoomHandler(StudioTextEditor textEditor)
        {
            this.textEditor = textEditor;

            textEditor.Loaded += OnLoaded;
            textEditor.Unloaded += OnUnloaded;
            textEditor.PreviewMouseWheel += OnMouseWheel;
            textEditor.PreviewKeyDown += OnKeyDown;
            textEditor.CurrentZoomChanged += OnCurrentZoomChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            initialFontSize = textEditor.TextArea.FontSize;

            var scrollViewer = textEditor.Template.FindName("PART_ScrollViewer", textEditor) as ScrollViewer;
            combo = scrollViewer.Template.FindName("PART_ZoomComboBox", scrollViewer) as ComboBox;
            combo.PreviewKeyDown += Combo_OnPreviewKeyDown;
            combo.PreviewLostKeyboardFocus += Combo_OnPreviewLostKeyboardFocus;
            combo.DropDownClosed += Combo_OnDropDownClosed;

            PopulateCombo();

            lastValidZoomComboValue = 1.0;

            UpdateComboValue();
        }

        private void PopulateCombo()
        {
            var values = new string[] { "20 %", "50 %", "70 %", "100 %", "150 %", "200 %", "400 %" };
            combo.Items.Clear();

            foreach (var value in values)
                combo.Items.Add(value);

            combo.SelectedItem = "100 %";
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            combo.PreviewKeyDown -= Combo_OnPreviewKeyDown;
            combo.PreviewLostKeyboardFocus -= Combo_OnPreviewLostKeyboardFocus;
            combo.DropDownClosed -= Combo_OnDropDownClosed;
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && UpdateZoom(e.Delta > 0);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && (e.Key == Key.Add || e.Key == Key.Subtract) && UpdateZoom(e.Key == Key.Add);
        }

        private void OnCurrentZoomChanged(object sender, ValueChangedEventArgs<double> e)
        {
            var textArea = textEditor.TextArea;
            if (textArea != null)
                textArea.FontSize = initialFontSize * e.NewValue;

            textEditor.TextArea.LeftMargins.OfType<FoldingMargin>().First().Measure(new Size(100, 100));
            textEditor.TextArea.LeftMargins.OfType<FoldingMargin>().First().InvalidateMeasure();
            textEditor.TextArea.LeftMargins.OfType<FoldingMargin>().First().InvalidateVisual();

            UpdateComboValue(e.NewValue);
        }

        private void UpdateComboValue(double value = -1)
        {
            if (value == -1)
                value = lastValidZoomComboValue;

            var stringValue = $"{Math.Round(value * 100)} %";
            if (combo.Text != stringValue)
            {
                combo.SelectedItem = null;
                combo.Text = null;

                combo.SelectedItem = stringValue;
                combo.Text = stringValue;
            }
        }

        private void Combo_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                e.Handled = textEditor.TextArea.Focus();
        }

        private void Combo_OnPreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (double.TryParse(combo.Text.TrimEnd('%'), out var value))
            {
                value /= 100;

                if (SetZoom(value))
                    lastValidZoomComboValue = value;
            }

            UpdateComboValue();
        }

        private void Combo_OnDropDownClosed(object sender, EventArgs e)
        {
            textEditor.TextArea.Focus();
            UpdateComboValue();
        }

        private bool SetZoom(double value)
        {
            if (value < 0.2 || value > 4)
                return false;

            textEditor.CurrentZoom = value;

            return true;
        }

        private bool UpdateZoom(bool increase)
        {
            if (increase)
                return IncreaseZoom();
            return DecreseZoom();
        }

        private bool IncreaseZoom()
        {
            var targetZoom = Math.Round(textEditor.CurrentZoom + 0.1, 2);

            if (targetZoom > 4.0)
                return false;

            textEditor.CurrentZoom = targetZoom;

            return true;
        }

        private bool DecreseZoom()
        {
            var targetZoom = Math.Round(textEditor.CurrentZoom - 0.1, 2);

            if (targetZoom < 0.2)
                return false;

            textEditor.CurrentZoom = targetZoom;

            return true;
        }
    }
}