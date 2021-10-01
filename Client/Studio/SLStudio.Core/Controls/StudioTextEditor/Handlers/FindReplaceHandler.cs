using SLStudio.Core.Controls.StudioTextEditor.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SLStudio.Core
{
    internal class FindReplaceHandler
    {
        private readonly StudioTextEditor textEditor;
        private readonly IFindReplaceService findReplace;
        private readonly FindAndReplaceViewModel viewModel;
        private readonly FindAndReplaceView view;
        private ContentControl contentControl;

        public FindReplaceHandler(StudioTextEditor textEditor, IFindReplaceService findReplace)
        {
            this.textEditor = textEditor;
            this.findReplace = findReplace;
            viewModel = new(textEditor, findReplace);
            view = new FindAndReplaceView(viewModel);

            textEditor.PreviewKeyDown += OnPreviewKeyDown;
            textEditor.Loaded += OnLoaded;
            textEditor.Unloaded += OnUnloaded;
            textEditor.TextChanged += OnTextChanged;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                if (e.Key == Key.F)
                {
                    viewModel.ShowFind();
                    e.Handled = true;
                }
                else
                if (e.Key == Key.H)
                {
                    viewModel.ShowFindReplace();
                    e.Handled = true;
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            contentControl = textEditor.Template.FindName("PART_FindAndReplace", textEditor) as ContentControl;

            if (contentControl is null)
                return;

            contentControl.Content = view;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (contentControl is null)
                return;

            contentControl.Content = null;
            contentControl = null;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (viewModel.IsOpen)
                findReplace.ForceCache();
        }
    }
}