using System.Collections.Generic;

namespace Ametrin.Utils.Registry.Unity{
    public sealed class AssetRegistry<TValue> : Registry<string, TValue> where TValue : UnityEngine.Object{
        public AssetRegistry(IEnumerable<TValue> values) : base(values, val=> val.name){}
    }
}