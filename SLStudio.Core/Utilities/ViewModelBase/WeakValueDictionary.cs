using Caliburn.Micro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Core.Utilities.ViewModelBase
{
    internal class WeakValueDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TValue : class
    {
        private readonly Dictionary<TKey, WeakReference> inner;
        private readonly WeakReference gcSentinel = new WeakReference(new object());

        private bool IsCleanupNeeded()
        {
            if (gcSentinel.Target == null)
            {
                gcSentinel.Target = new object();
                return true;
            }

            return false;
        }

        private void CleanAbandonedItems()
        {
            var keysToRemove = inner.Where(pair => !pair.Value.IsAlive)
                .Select(pair => pair.Key)
                .ToList();

            keysToRemove.Apply(key => inner.Remove(key));
        }

        private void CleanIfNeeded()
        {
            if (IsCleanupNeeded())
                CleanAbandonedItems();
        }

        public WeakValueDictionary()
        {
            inner = new Dictionary<TKey, WeakReference>();
        }

        public WeakValueDictionary(IDictionary<TKey, TValue> dictionary)
        {
            inner = new Dictionary<TKey, WeakReference>();
            dictionary.Apply(item => inner.Add(item.Key, new WeakReference(item.Value)));
        }

        public WeakValueDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            inner = new Dictionary<TKey, WeakReference>(comparer);
            dictionary.Apply(item => inner.Add(item.Key, new WeakReference(item.Value)));
        }

        public WeakValueDictionary(IEqualityComparer<TKey> comparer)
        {
            inner = new Dictionary<TKey, WeakReference>(comparer);
        }

        public WeakValueDictionary(int capacity)
        {
            inner = new Dictionary<TKey, WeakReference>(capacity);
        }

        public WeakValueDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            inner = new Dictionary<TKey, WeakReference>(capacity, comparer);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            CleanIfNeeded();
            var enumerable = inner.Select(pair => new KeyValuePair<TKey, TValue>(pair.Key, (TValue)pair.Value.Target))
                .Where(pair => pair.Value != null);
            return enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            inner.Clear();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (!TryGetValue(item.Key, out TValue value))
                return false;

            return value == item.Value;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }

            if ((arrayIndex + Count) > array.Length)
            {
                throw new ArgumentException(
                    "The number of elements in the source collection is greater than the available space from arrayIndex to the end of the destination array.");
            }

            this.ToArray().CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!TryGetValue(item.Key, out TValue value))
                return false;

            if (value != item.Value)
                return false;

            return inner.Remove(item.Key);
        }

        public int Count
        {
            get
            {
                CleanIfNeeded();
                return inner.Count;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        public void Add(TKey key, TValue value)
        {
            CleanIfNeeded();
            inner.Add(key, new WeakReference(value));
        }

        public bool ContainsKey(TKey key)
        {
            return TryGetValue(key, out _);
        }

        public bool Remove(TKey key)
        {
            CleanIfNeeded();
            return inner.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            CleanIfNeeded();

            if (!inner.TryGetValue(key, out WeakReference wr))
            {
                value = null;
                return false;
            }

            var result = (TValue)wr.Target;
            if (result == null)
            {
                inner.Remove(key);
                value = null;
                return false;
            }

            value = result;
            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (!TryGetValue(key, out TValue result))
                    throw new KeyNotFoundException();

                return result;
            }
            set
            {
                CleanIfNeeded();
                inner[key] = new WeakReference(value);
            }
        }

        public ICollection<TKey> Keys
        {
            get { return inner.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return new ValueCollection(this); }
        }

        private sealed class ValueCollection : ICollection<TValue>
        {
            private readonly WeakValueDictionary<TKey, TValue> inner;

            public ValueCollection(WeakValueDictionary<TKey, TValue> dictionary)
            {
                inner = dictionary;
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return inner.Select(pair => pair.Value).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            void ICollection<TValue>.Add(TValue item)
            {
                throw new NotSupportedException();
            }

            void ICollection<TValue>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<TValue>.Contains(TValue item)
            {
                return inner.Any(pair => pair.Value == item);
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }

                if (arrayIndex < 0 || arrayIndex >= array.Length)
                {
                    throw new ArgumentOutOfRangeException("arrayIndex");
                }

                if ((arrayIndex + Count) > array.Length)
                {
                    throw new ArgumentException(
                        "The number of elements in the source collection is greater than the available space from arrayIndex to the end of the destination array.");
                }

                this.ToArray().CopyTo(array, arrayIndex);
            }

            bool ICollection<TValue>.Remove(TValue item)
            {
                throw new NotSupportedException();
            }

            public int Count
            {
                get { return inner.Count; }
            }

            bool ICollection<TValue>.IsReadOnly
            {
                get { return true; }
            }
        }
    }
}