﻿
using System;
using System.Collections.Generic;
using Ametrin.Utils.Optional;

namespace Ametrin.Utils.Registry
{
    public sealed class TypeRegistry<TKey> : Registry<TKey, Type> where TKey : notnull
    {
        public TypeRegistry(IReadOnlyDictionary<TKey, Type> entries) : base(entries) { }
        public TypeRegistry(IEnumerable<KeyValuePair<TKey, Type>> entries) : base(entries) { }
        public TypeRegistry(IEnumerable<Type> values, Func<Type, TKey> keyProvider) : base(values, keyProvider) { }
    }

    public sealed class MutableTypeRegistry<TKey> : MutableRegistry<TKey, Type> where TKey : notnull
    {
        public MutableTypeRegistry() { }
        public MutableTypeRegistry(IEnumerable<KeyValuePair<TKey, Type>> entries) : base(entries) { }
        public MutableTypeRegistry(IDictionary<TKey, Type> entries) : base(entries) { }

        public ResultFlag TryRegister<TType>(TKey key) => TryRegister(key, typeof(TType));
    }
}
