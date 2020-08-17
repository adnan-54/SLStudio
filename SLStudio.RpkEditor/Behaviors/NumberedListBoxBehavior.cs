using DevExpress.Mvvm.UI.Interactivity;
using MahApps.Metro.Controls;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace SLStudio.RpkEditor.Behaviors
{
    internal class NumberedListBoxBehavior : Behavior<Grid>
    {
        private ItemsControl itemsControl;
        private ListBox listBox;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= OnLoaded;

            itemsControl = AssociatedObject.FindChild<ItemsControl>();
            listBox = AssociatedObject.FindChild<ListBox>();

            if (listBox != null)
                ((INotifyCollectionChanged)listBox.Items).CollectionChanged += OnItemsChanged;

            UpdateLines();
        }

        private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateLines();
        }

        private void UpdateLines()
        {
            if (listBox == null || itemsControl == null)
                return;

            if (listBox.Items.Count > itemsControl.Items.Count)
            {
                while (itemsControl.Items.Count < listBox.Items.Count)
                    itemsControl.Items.Add(new LineNumberModel(itemsControl.Items.Count + 1));
            }
            else
            if (listBox.Items.Count < itemsControl.Items.Count)
            {
                while (itemsControl.Items.Count > listBox.Items.Count)
                    itemsControl.Items.RemoveAt(itemsControl.Items.Count - 1);
            }
        }

        protected override void OnDetaching()
        {
            itemsControl = null;
            listBox = null;

            base.OnDetaching();
        }
    }

    internal class LineNumberModel
    {
        public LineNumberModel(int lineNumber)
        {
            LineNumber = lineNumber;
        }

        public int LineNumber { get; }
    }
}