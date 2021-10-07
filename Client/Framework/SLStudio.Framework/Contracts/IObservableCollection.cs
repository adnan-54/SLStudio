using System.Collections.Generic;
using System.Collections.Specialized;

namespace SLStudio
{
    public interface IObservableCollection<T> : IList<T>, INotifyPropertyChangedEx, INotifyCollectionChanged
    {
        void AddRange(IEnumerable<T> items);

        void RemoveRange(IEnumerable<T> items);
    }
}