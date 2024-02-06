using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Ametrin.Utils.Optional;

namespace Ametrin.Utils.Registry{
    public class Registry<TKey, TValue> : IRegistry<TKey, TValue> where TKey : notnull {
        private readonly IReadOnlyDictionary<TKey, TValue> Entries;
        public int Count => Entries.Count;
        public IEnumerable<TKey> Keys => Entries.Keys;
        public TValue this[TKey key] => Entries[key];

        public Registry(IReadOnlyDictionary<TKey, TValue> entries) {
            Entries = entries;
        }
        public Registry(IEnumerable<KeyValuePair<TKey, TValue>> entries) : this(entries.ToDictionary()) { }
        public Registry(IEnumerable<TValue> values, Func<TValue, TKey> keyProvider) : this(values.ToDictionary(keyProvider)){}

        public Option<TValue> TryGet(TKey key){
            if (Entries.TryGetValue(key, out var value)){
                return value;
            }
            return Option<TValue>.None();
        }
        public bool ContainsKey(TKey key) => Entries.ContainsKey(key);

        public IEnumerator<TValue> GetEnumerator() => Entries.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}