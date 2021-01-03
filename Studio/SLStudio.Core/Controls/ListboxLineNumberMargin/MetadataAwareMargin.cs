using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace SLStudio.Core.Controls
{
    public abstract class MetadataAwareMargin : FrameworkElement, IMetadataAware
    {
        private ScrollBar bar;
        private ScrollViewer viewer;

        public MetadataAwareMargin()
        {
        }

        protected ItemsControl Control { get; private set; }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (sizeInfo.HeightChanged)
            {
                ListenToScrollChanges();
                InvalidateVisual();
            }
        }

        protected virtual int IndexOf(FrameworkElement element)
        {
            if (element == null)
                return -1;

            if (Control.ItemsSource is IList itemsSource)
            {
                var boundItem = Control.ItemContainerGenerator.ItemFromContainer(element);
                if (boundItem == null)
                    return -1;

                return itemsSource.IndexOf(boundItem);
            }
            return Control.ItemContainerGenerator.IndexFromContainer(element);
        }

        protected virtual int LineOf(FrameworkElement element) => IndexOf(element) + 1;

        protected virtual IEnumerable<FrameworkElement> GetVisibleElements()
        {
            return Control.Items.OfType<object>()
                .Select(i => (FrameworkElement)Control.ItemContainerGenerator.ContainerFromItem(i))
                .Where(e => e != null)
                .Where(e => IsElementVisible(e));
        }

        private bool IsElementVisible(FrameworkElement element)
        {
            element.SizeChanged -= ElementSizeChanged;
            element.SizeChanged += ElementSizeChanged;

            if (element.RenderSize == Size.Empty || !element.IsVisible)
                return false;

            element.IsVisibleChanged -= ElementIsVisibleChanged;
            element.IsVisibleChanged += ElementIsVisibleChanged;

            var listBoxBounds = new Rect(new Point(0, 0), Control.RenderSize);
            var elementBounds = element.TransformToAncestor(Control).TransformBounds(new Rect(new Point(0, 0), element.RenderSize));
            return elementBounds.Top > listBoxBounds.Top && elementBounds.Top < listBoxBounds.Bottom;
        }

        private void ElementSizeChanged(object sender, SizeChangedEventArgs e) => InvalidateVisual();

        private void ElementIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => InvalidateVisual();

        #region IMetadataAware

        public void Attach(ItemsControl control)
        {
            Control = control;
            if (Control.ItemsSource != null)
            {
                UnlistenToCollectionChange();
                ListenToCollectionChange();
            }
            else
                ListenToItemsSourceChange();
        }

        public void Detach()
        {
            UnlistenToItemsSourceChange();
            UnlistenToCollectionChange();

            if (bar != null)
                bar.ValueChanged -= ScrollBarValueChanged;

            Control = null;
            viewer = null;
            bar = null;

            InvalidateVisual();
        }

        #endregion IMetadataAware

        private void ListenToItemsSourceChange()
        {
            var descriptor = TypeDescriptor.GetProperties(Control)["ItemsSource"];
            descriptor.AddValueChanged(Control, OnItemsSourceChanged);
        }

        private void UnlistenToItemsSourceChange()
        {
            var descriptor = TypeDescriptor.GetProperties(Control)["ItemsSource"];
            descriptor.RemoveValueChanged(Control, OnItemsSourceChanged);
        }

        protected virtual void OnItemsSourceChanged(object sender, EventArgs e) => ListenToCollectionChange();

        private void ListenToCollectionChange()
        {
            if (Control.HasItems)
                CollectionChanged(this, null);
            if (Control.ItemsSource is INotifyCollectionChanged)
                CollectionViewSource.GetDefaultView(Control.ItemsSource).CollectionChanged += CollectionChanged;
        }

        private void UnlistenToCollectionChange()
        {
            if (Control.ItemsSource is INotifyCollectionChanged)
                CollectionViewSource.GetDefaultView(Control.ItemsSource).CollectionChanged -= CollectionChanged;
        }

        protected virtual void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ListenToScrollChanges();
            InvalidateVisual();
        }

        private void ListenToScrollChanges()
        {
            if (viewer != null && bar != null)
                return;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (Control == null)
                    return;

                viewer = Control.GetVisualChild<ScrollViewer>();
                if (viewer != null)
                {
                    viewer.Unloaded -= ViewerUnloaded;
                    viewer.Unloaded += ViewerUnloaded;

                    bar = viewer.Template.FindName("PART_VerticalScrollBar", viewer) as ScrollBar;
                    if (bar != null)
                    {
                        bar.Unloaded += BarUnloaded;
                        bar.ValueChanged += ScrollBarValueChanged;
                    }
                }
            }), System.Windows.Threading.DispatcherPriority.Render);
        }

        private void ScrollBarValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => InvalidateVisual();

        private void ViewerUnloaded(object sender, RoutedEventArgs e)
        {
            viewer = null;
            ListenToScrollChanges();
        }

        private void BarUnloaded(object sender, RoutedEventArgs e)
        {
            if (bar != null)
            {
                bar.Unloaded -= BarUnloaded;
                bar.ValueChanged -= ScrollBarValueChanged;
                bar = null;
            }
        }
    }
}