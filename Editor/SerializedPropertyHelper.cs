using System.Linq;
using UnityEditor;

namespace Ametrin.UnityUtils.EditorTools{

    public static class SerializedPropertyHelper{
        private static readonly string[] IgnorableSubProperties = new string[] { "m_Script" };

        public static bool HasVisibleSubProperties(this SerializedProperty property){
            var data = property.objectReferenceValue;
            using var serializedData = new SerializedObject(data);
            using var subPropertyIterator = serializedData.GetIterator();
            while (subPropertyIterator.NextVisible(true)){
                if(IgnorableSubProperties.Contains(subPropertyIterator.name)) continue;
                return true;
            }
            return false;
        }

        public static bool IsOnBlacklist(this SerializedProperty property){
            return IgnorableSubProperties.Contains(property.name);
        }
    }
}