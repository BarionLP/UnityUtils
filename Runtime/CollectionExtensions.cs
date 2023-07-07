using System.Collections.Generic;
using System.Linq;
using System;

namespace Ametrin.Utils{
    public static class CollectionExtensions{
        private static readonly Random _Random = new(DateTime.UtcNow.Millisecond);
        public static T GetRandomElement<T>(this ICollection<T> enumerable){
            return enumerable.ElementAt(_Random.Next(0, enumerable.Count));
        }

        public static void Move<T>(this IList<T> from, int idx, ICollection<T> to){
            if (idx < 0 || idx >= from.Count) throw new IndexOutOfRangeException(nameof(idx));

            var item = from[idx];
            to.Add(item);
            from.RemoveAt(idx);
        }
    }
}