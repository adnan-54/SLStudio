using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Threading;

namespace SLStudio.Core
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

        public virtual void NotifyOfPropertyChange(string propertyName)
        {
            if (IsNotifying)
                OnUIThread(() => OnPropertyChanged(new PropertyChangedEventArgs(propertyName)));
        }

        public void Refresh()
        {
            OnUIThread(() =>
            {
                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        protected override sealed void InsertItem(int index, T item)
        {
            OnUIThread(() => InsertItemBase(index, item));
        }

        protected virtual void InsertItemBase(int index, T item)
        {
            base.InsertItem(index, item);
        }

        protected override sealed void SetItem(int index, T item)
        {
            OnUIThread(() => SetItemBase(index, item));
        }

        protected virtual void SetItemBase(int index, T item)
        {
            base.SetItem(index, item);
        }

        protected override sealed void RemoveItem(int index)
        {
            OnUIThread(() => RemoveItemBase(index));
        }

        protected virtual void RemoveItemBase(int index)
        {
            base.RemoveItem(index);
        }

        protected override sealed void ClearItems()
        {
            OnUIThread(ClearItemsBase);
        }

        protected virtual void ClearItemsBase()
        {
            base.ClearItems();
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (IsNotifying)
                base.OnCollectionChanged(e);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (IsNotifying)
                base.OnPropertyChanged(e);
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            void AddRange()
            {
                var previousNotificationSetting = IsNotifying;
                IsNotifying = false;
                var index = Count;
                foreach (var item in items)
                {
                    InsertItemBase(index, item);
                    index++;
                }
                IsNotifying = previousNotificationSetting;

                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            OnUIThread(AddRange);
        }

        public virtual void RemoveRange(IEnumerable<T> items)
        {
            void RemoveRange()
            {
                var previousNotificationSetting = IsNotifying;
                IsNotifying = false;
                foreach (var item in items)
                {
                    var index = IndexOf(item);
                    if (index >= 0)
                        RemoveItemBase(index);
                }
                IsNotifying = previousNotificationSetting;

                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            OnUIThread(RemoveRange);
        }

        protected virtual void OnUIThread(Action action)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            OnUIThreadCore();

            void OnUIThreadCore()
            {
                if (!CheckAccess())
                {
                    Exception exception = null;
                    void method()
                    {
                        try
                        {
                            action();
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                        }
                    }
                    dispatcher.Invoke(method);
                    if (exception != null)
                        throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
                }
                else
                    action();
            }

            bool CheckAccess()
            {
                return dispatcher == null || dispatcher.CheckAccess();
            }
        }
    }
}