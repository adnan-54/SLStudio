using DevExpress.Mvvm.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SLStudio.Core.Behaviors
{
    public class ListBoxSelectionBehavior : ServiceBaseGeneric<ListBox>, IListBoxSelection
    {
        public static readonly DependencyProperty ShouldFocusProperty = DependencyProperty.Register(nameof(AutoFocus), typeof(bool), typeof(ListBoxSelectionBehavior));

        public bool AutoFocus
        {
            get { return (bool)GetValue(ShouldFocusProperty); }
            set { SetValue(ShouldFocusProperty, value); }
        }

        private IDisposable dispatcherOperation;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnAssociatedObjectLoaded;
            AssociatedObject.SelectionChanged += OnSelectionChanged;
            AssociatedObject.ItemContainerGenerator.ItemsChanged += OnItemsChanged;
        }

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            Focus();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems.OfType<ISelectable>())
                item.IsSelected = true;
            foreach (var item in e.RemovedItems.OfType<ISelectable>())
                item.IsSelected = false;

            if (AssociatedObject?.SelectedItem != null && AssociatedObject?.SelectedItems.Count <= 1)
            {
                if (!AutoFocus)
                    ScrollIntoView(AssociatedObject.SelectedItem);
                else
                    FocusInternal(AssociatedObject.SelectedItem);
            }
        }

        private void OnItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (AssociatedObject.IsFocused || AssociatedObject.IsKeyboardFocusWithin)
                Focus();
        }

        private void ScrollIntoView(object target)
        {
            AssociatedObject.ScrollIntoView(target);
            if (AssociatedObject.ItemContainerGenerator.ContainerFromItem(target) == null)
                AssociatedObject.ScrollIntoView(target);
        }

        public void Focus()
        {
            if (AssociatedObject?.SelectedItem != null)
                FocusInternal(AssociatedObject.SelectedItem);
        }

        private void FocusInternal(object target)
        {
            dispatcherOperation?.Dispose();
            dispatcherOperation = Dispatcher.InvokeDisposableAsync(() =>
            {
                ScrollIntoView(target);

                var previousSelected = AssociatedObject.SelectedItems.Cast<object>().ToList();
                if (previousSelected.Contains(target))
                    previousSelected.Remove(target);

                if (AssociatedObject.ItemContainerGenerator.ContainerFromItem(target) is ListBoxItem item)
                    item.Focus();

                foreach (var selected in previousSelected)
                    AssociatedObject.SelectedItems.Add(selected);
            });
        }

        IEnumerable IListBoxSelection.GetSelectedItems() => AssociatedObject?.SelectedItems;

        protected override void OnDetaching()
        {
            dispatcherOperation?.Dispose();
            AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
            AssociatedObject.ItemContainerGenerator.ItemsChanged -= OnItemsChanged;
            base.OnDetaching();
        }
    }

    public interface IListBoxSelection
    {
        void Focus();

        IEnumerable GetSelectedItems();
    }

    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
}