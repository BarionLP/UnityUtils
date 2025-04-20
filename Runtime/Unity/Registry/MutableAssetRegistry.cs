using System.Collections.Generic;
using System.Linq;

namespace Ametrin.Utils.Registry.Unity
{
    public sealed class MutableAssetRegistry<T> : MutableRegistry<string, T> where T : UnityEngine.Object
    {
        public MutableAssetRegistry() : base() { }
        public MutableAssetRegistry(IEnumerable<T> assets) : base(assets.ToDictionary(asset => asset.name)) { }
    }
}
