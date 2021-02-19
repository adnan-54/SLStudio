using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using SLStudio.Core.Controls;
using SLStudio.Core.Controls.StudioTextEditor;
using System;
using System.Collections.Generic;
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
        static StudioTextEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StudioTextEditor), new FrameworkPropertyMetadata(typeof(StudioTextEditor)));
        }

        public static readonly DependencyProperty CurrentLineProperty = DependencyProperty.RegisterAttached("CurrentLine", typeof(int), typeof(TextEditor), new PropertyMetadata(1));

        public static int GetCurrentLine(DependencyObject d)
        {
            return (int)d.GetValue(CurrentLineProperty);
        }

        public static void SetCurrentLine(DependencyObject d, int value)
        {
            d.SetValue(CurrentLineProperty, value);
        }

        public static readonly DependencyProperty CurrentColumnProperty = DependencyProperty.RegisterAttached("CurrentColumn", typeof(int), typeof(TextEditor), new PropertyMetadata(1));

        public static int GetCurrentColumn(DependencyObject d)
        {
            return (int)d.GetValue(CurrentColumnProperty);
        }

        public static void SetCurrentColumn(DependencyObject d, int value)
        {
            d.SetValue(CurrentColumnProperty, value);
        }

        public static readonly DependencyProperty CurrentZoomProperty = DependencyProperty.RegisterAttached("CurrentZoom", typeof(double), typeof(TextEditor), new PropertyMetadata(1.0, OnCurrentZoomChanged));

        public static double GetCurrentZoom(DependencyObject d)
        {
            return (double)d.GetValue(CurrentZoomProperty);
        }

        public static void SetCurrentZoom(DependencyObject d, double value)
        {
            d.SetValue(CurrentZoomProperty, value);
        }

        private static void OnCurrentZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not StudioTextEditor textEditor || e.NewValue is not double newValue || e.OldValue is not double oldValue)
                return;

            textEditor.OnCurrentZoomChanged(newValue, oldValue);
        }

        private readonly IEnumerable<ITextEditorHandler> handlers;

        public StudioTextEditor()
        {
            handlers = new List<ITextEditorHandler>()
            {
                new ZoomHandler(this),
                new CaretPositionHandler(this),
            };
            PreviewKeyDown += OnPreviewKeyDown;
            Loaded += OnLoaded;
        }

        public event EventHandler<ValueChangedEventArgs<double>> CurrentZoomChanged;

        public int CurrentLine
        {
            get => GetCurrentLine(this);
            set
            {
                if (CurrentLine == value)
                    return;

                SetCurrentLine(this, value);
                TextArea.Caret.Line = value;
            }
        }

        public int CurrentColumn
        {
            get => GetCurrentColumn(this);
            set
            {
                if (CurrentColumn == value)
                    return;

                SetCurrentColumn(this, value);
                TextArea.Caret.Column = value;
            }
        }

        public double CurrentZoom
        {
            get => GetCurrentZoom(this);
            set => SetCurrentZoom(this, value);
        }

        private void OnCurrentZoomChanged(double newValue, double oldValue)
        {
            CurrentZoomChanged?.Invoke(this, new ValueChangedEventArgs<double>(newValue, oldValue));
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                if (e.Key == Key.G)
                {
                    ShowGoToLine();
                    e.Handled = true;
                }
                else
                if (e.Key == Key.F)
                {
                    ShowFind();
                    e.Handled = true;
                }
                else
                if (e.Key == Key.H)
                {
                    ShowFindAndReplace();
                    e.Handled = true;
                }
            }
        }

        private void ShowGoToLine()
        {
            var maxLine = Document.LineCount;
            var dialog = new GoToLineWindow(maxLine, TextArea.Caret.Line) { Owner = Application.Current.MainWindow };
            if (dialog.ShowDialog().GetValueOrDefault() && dialog.TryGetResult(out var targetLine))
            {
                TextArea.Caret.Column = 0;
                TextArea.Caret.Line = targetLine;
                ScrollToLine(targetLine);
            }
        }

        private void ShowFind()
        {
        }

        private void ShowFindAndReplace()
        {
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;

            SetupCurrentLineHighlighter();
            SetupLeftMargins();
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
    }
}