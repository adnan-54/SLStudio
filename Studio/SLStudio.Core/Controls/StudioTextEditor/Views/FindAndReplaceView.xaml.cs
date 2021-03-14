using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SLStudio.Core.Controls.StudioTextEditor.Views
{
    public partial class FindAndReplaceView : UserControl
    {
        private bool focusRequested;
        private readonly FindAndReplaceViewModel viewModel;

        public FindAndReplaceView(FindAndReplaceViewModel viewModel)
        {
            this.viewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.FocusRequested += OnFocusRequested;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            viewModel.FocusRequested -= OnFocusRequested;
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var targetWidth = ActualWidth - e.HorizontalChange;
            
            if (targetWidth < MinWidth)
                targetWidth = MinWidth;
            else
            if (targetWidth > MaxWidth)
                targetWidth = MaxWidth;

            Width = targetWidth;
        }

        private void OnFocusRequested(object sender, EventArgs e)
        {
            if (!searchBox.IsLoaded)
            {
                searchBox.Loaded += SearchBox_OnLoaded;
                focusRequested = true;
            }
            else
                FocusTextBox();
        }

        private void SearchBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (focusRequested)
            {
                searchBox.Loaded -= SearchBox_OnLoaded;
                FocusTextBox();
                focusRequested = false;
            }
        }

        private void FocusTextBox()
        {
            Keyboard.Focus(searchBox);
            searchBox.Focus();
            searchBox.SelectAll();
        }
    }
}