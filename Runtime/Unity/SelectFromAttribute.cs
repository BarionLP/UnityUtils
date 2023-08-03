using System;
using UnityEngine;

namespace Ametrin.Utils.Unity{

    // TODO: make generic when Unity updates C#
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class SelectFromAttribute : PropertyAttribute{
        public readonly object[] Values;
        public readonly Type Type;
        public readonly bool AllowRawInput;
        
        public SelectFromAttribute(Type type, params object[] values) : this(type, false, values){}
        
        public SelectFromAttribute(Type type, bool allowRawInput, params object[] values){
            Type = type;
            AllowRawInput = allowRawInput;
            Values = values;
        }
    }
}