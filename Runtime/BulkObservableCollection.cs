using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Ametrin.Utils
{
    public sealed class BulkObservableCollection<T> : ObservableCollection<T>
    {
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection is null || !collection.Any()) return;

            CheckReentrancy();

            foreach (var item in collection)
            {
                Items.Add(item);
            }

            OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
        }
    }
}
