using System;
using System.Collections.Generic;

namespace Ametrin.Utils{
    #nullable enable
    public static class DictionaryExtensions{
        public static bool TryGetValue<T>(this Dictionary<string, T> dictionary, ReadOnlySpan<char> spanKey, out T result){
            foreach(var key in dictionary.Keys){
                if(spanKey.SequenceEqual(key)){
                    result = dictionary[key];
                    return true;
                }
            }

            result = default!;
            return false;
        }
    }
}