using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Windows;

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

        private readonly ZoomHandler zoomHandler;
        private readonly GoToLineHandler goToLineHandler;
        private readonly FindReplaceHandler findReplaceHandler;

        private static void OnCurrentZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not StudioTextEditor textEditor || e.NewValue is not double newValue || e.OldValue is not double oldValue)
                return;

            textEditor.OnCurrentZoomChanged(newValue, oldValue);
        }

        public StudioTextEditor()
        {
            _ = new CaretPositionHandler(this);
            _ = new RendersHandler(this);
            _ = new LeftMarginsHandler(this);

            zoomHandler = new ZoomHandler(this);
            goToLineHandler = new GoToLineHandler(this);

            FindReplaceService = new FindReplaceService(this);
            findReplaceHandler = new FindReplaceHandler(this, FindReplaceService);
        }

        public event EventHandler<ValueChangedEventArgs<double>> CurrentZoomChanged;

        public IFindReplaceService FindReplaceService { get; }

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
    }
}