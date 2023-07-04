using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ametrin.UnityUtils.EditorTools{
    [CustomPropertyDrawer(typeof(InlineEditorAttribute), true)]
    public sealed class InlineEditorAttributeDrawer : PropertyDrawer{
        private const int FOLDOUT_WIDTH = 16;
        private Editor cachedEditor;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            EditorGUI.BeginProperty(position, label, property);
            if(property.objectReferenceValue is null || !property.HasVisibleSubProperties()){
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            var propertyRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(propertyRect, property);       
            property.isExpanded = EditorGUI.Foldout(propertyRect, property.isExpanded, GUIContent.none);

            if(!property.isExpanded){
                EditorGUI.EndProperty();
                return;
            }

            // GUI.Box(new Rect(0, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, Screen.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");

            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();

            var y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            using var serializedData = new SerializedObject(property.objectReferenceValue);
            using var iterator = serializedData.GetIterator();
            iterator.NextVisible(true);
            do{
                if(iterator.IsOnBlacklist()) continue;
                var height = EditorGUI.GetPropertyHeight(iterator, new GUIContent(iterator.displayName), true);
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, height), iterator, true);
                y += height + EditorGUIUtility.standardVerticalSpacing;
            }while (iterator.NextVisible(false));

            y += EditorGUIUtility.standardVerticalSpacing; // more spacing at the bottom 

            if(EditorGUI.EndChangeCheck()){
                serializedData.ApplyModifiedProperties();
            }
            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            float totalHeight = EditorGUIUtility.singleLineHeight;
            if (property.objectReferenceValue == null || !property.HasVisibleSubProperties()){
                return totalHeight;
            }

            if (property.isExpanded){
                using var serializedObject = new SerializedObject(property.objectReferenceValue);
                using var iterator = serializedObject.GetIterator();
                iterator.NextVisible(true);
                do{
                    if(iterator.IsOnBlacklist()) continue;
                    float height = EditorGUI.GetPropertyHeight(iterator, label, true) + EditorGUIUtility.standardVerticalSpacing;
                    totalHeight += height;
                }
                while (iterator.NextVisible(false));
                totalHeight += EditorGUIUtility.standardVerticalSpacing*2;
            }
            return totalHeight;
        }
    }
}