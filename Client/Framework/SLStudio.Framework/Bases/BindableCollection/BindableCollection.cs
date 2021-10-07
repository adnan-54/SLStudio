using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Threading;

namespace SLStudio
{
    public class BindableCollection<T> : ObservableCollection<T>, IObservableCollection<T>
    {
        public BindableCollection()
        {
            IsNotifying = true;
        }

        public BindableCollection(IEnumerable<T> collection) : base(collection)
        {
            IsNotifying = true;
        }

        public bool IsNotifying { get; set; }

        public void NotifyOfPropertyChange(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public void Refresh()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (!IsNotifying)
                return;

            OnUIThread(() => base.OnPropertyChanged(e));
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!IsNotifying)
                return;

            OnUIThread(() => base.OnCollectionChanged(e));
        }

        protected override void InsertItem(int index, T item)
        {
            OnUIThread(() => base.InsertItem(index, item));
        }

        protected override void SetItem(int index, T item)
        {
            OnUIThread(() => base.SetItem(index, item));
        }

        protected override void RemoveItem(int index)
        {
            OnUIThread(() => base.RemoveItem(index));
        }

        protected override void ClearItems()
        {
            OnUIThread(() => base.ClearItems());
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            OnUIThread(() =>
            {
                var previousNotificationSetting = IsNotifying;
                IsNotifying = false;
                var index = Count;
                foreach (var item in items)
                {
                    InsertItem(index, item);
                    index++;
                }
                IsNotifying = previousNotificationSetting;

                Refresh();
            });
        }

        public virtual void RemoveRange(IEnumerable<T> items)
        {
            OnUIThread(() =>
            {
                var previousNotificationSetting = IsNotifying;
                IsNotifying = false;
                foreach (var item in items)
                {
                    var index = IndexOf(item);
                    if (index >= 0)
                        RemoveItem(index);
                }
                IsNotifying = previousNotificationSetting;

                Refresh();
            });
        }

        protected virtual void OnUIThread(Action action)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;

            if (dispatcher != null && dispatcher.CheckAccess())
            {
                action();
                return;
            }

            Exception exception = null;
            dispatcher.Invoke(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });

            if (exception != null)
                throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
        }
    }
}