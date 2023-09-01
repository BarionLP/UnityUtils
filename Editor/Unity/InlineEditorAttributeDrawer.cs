using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ametrin.Utils.Unity.EditorTools{
    [CustomPropertyDrawer(typeof(InlineEditorAttribute), true)]
    public sealed class InlineEditorAttributeDrawer : PropertyDrawer{
        private readonly static Dictionary<string, bool> IsFoldoutOpen = new();
        private readonly static StyleColor BackgroundColor = new Color(0, 0, 0, 0.1f);
        private readonly static StyleColor CollapsedBackgroundColor = new Color(0, 0, 0, 0);
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property){
            var root = new VisualElement();
            root.style.BorderRadius(5);
            root.style.paddingBottom = EditorGUIUtility.standardVerticalSpacing;

            root.Add(CreateFoldout(property.propertyPath, root));            
            root.Add(CreatePropertyField(property, root));

            RefreshUI(root, property);

            return root;
        }

        private static Foldout CreateFoldout(string key, VisualElement root){
            var foldout = new Foldout{
                value = IsFoldoutOpen.GetOrCreate(key, false)
            };
            foldout.RegisterValueChangedCallback(@event =>{
                IsFoldoutOpen[key] = @event.newValue;
                UpdateRootColor(root, @event.newValue);
            });
            var imGUIContainer = new IMGUIContainer();
            imGUIContainer.style.marginBottom = EditorGUIUtility.standardVerticalSpacing;
            foldout.Add(imGUIContainer);
            UpdateRootColor(root, foldout.value);
            return foldout;
        }

        private static PropertyField CreatePropertyField(SerializedProperty property, VisualElement root){
            var propertyField = new PropertyField(property);
            propertyField.style.alignContent = Align.Center;
            propertyField.style.top = 0;
            propertyField.style.right = 0;
            propertyField.RegisterValueChangeCallback(evt =>{
                RefreshUI(root, property);
            });
            return propertyField;
        }

        private static void RefreshUI(VisualElement root, SerializedProperty property){
            var propertyField = root.Q<PropertyField>();
            var foldout = root.Q<Foldout>();
            var imGUIContainer = foldout.Q<IMGUIContainer>();

            var obj = property.objectReferenceValue;
            if (obj != null){
                var editor = Editor.CreateEditor(obj);
                imGUIContainer.onGUIHandler = editor.OnInspectorGUI;
                propertyField.style.position = Position.Absolute;
                propertyField.style.left = 15;
                foldout.SetEnabled(true);
                foldout.style.display = DisplayStyle.Flex;
            }else{
                foldout.style.display = DisplayStyle.None;
                foldout.SetEnabled(false);
                propertyField.style.position = Position.Relative;
                propertyField.style.left = 0;
                root.style.backgroundColor = CollapsedBackgroundColor;
            }
        }

        private static void UpdateRootColor(VisualElement root, bool foldoutExpanded){
            root.style.backgroundColor = foldoutExpanded ? BackgroundColor : CollapsedBackgroundColor;
        }

        //for nested editors
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;

        // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
        //     EditorGUI.BeginProperty(position, label, property);
        //     if(property.objectReferenceValue is null || !property.HasVisibleSubProperties()){
        //         EditorGUI.PropertyField(position, property, label);
        //         EditorGUI.EndProperty();
        //         return;
        //     }

        //     var propertyRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        //     EditorGUI.PropertyField(propertyRect, property);       
        //     property.isExpanded = EditorGUI.Foldout(propertyRect, property.isExpanded, GUIContent.none);

        //     if(!property.isExpanded){
        //         EditorGUI.EndProperty();
        //         return;
        //     }

        //     EditorGUI.indentLevel++;
        //     EditorGUI.BeginChangeCheck();

        //     var y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        //     using var serializedData = new SerializedObject(property.objectReferenceValue);
        //     using var iterator = serializedData.GetIterator();
        //     iterator.NextVisible(true);
        //     do{
        //         if(iterator.CanBeIgnored()) continue;
        //         var height = EditorGUI.GetPropertyHeight(iterator, new GUIContent(iterator.displayName), true);
        //         EditorGUI.PropertyField(new Rect(position.x, y, position.width, height), iterator, true);
        //         y += height + EditorGUIUtility.standardVerticalSpacing;
        //     }while (iterator.NextVisible(false));

        //     y += EditorGUIUtility.standardVerticalSpacing; // more spacing at the bottom 

        //     if(EditorGUI.EndChangeCheck()){
        //         serializedData.ApplyModifiedProperties();
        //     }
        //     EditorGUI.indentLevel--;

        //     EditorGUI.EndProperty();
        // }

        // public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
        //     float totalHeight = EditorGUIUtility.singleLineHeight;
        //     if (property.objectReferenceValue == null || !property.HasVisibleSubProperties()){
        //         return totalHeight;
        //     }

        //     if (property.isExpanded){
        //         using var serializedObject = new SerializedObject(property.objectReferenceValue);
        //         using var iterator = serializedObject.GetIterator();
        //         iterator.NextVisible(true);
        //         do{
        //             if(iterator.CanBeIgnored()) continue;
        //             float height = EditorGUI.GetPropertyHeight(iterator, label, true) + EditorGUIUtility.standardVerticalSpacing;
        //             totalHeight += height;
        //         }
        //         while (iterator.NextVisible(false));
        //         totalHeight += EditorGUIUtility.standardVerticalSpacing*2;
        //     }
        //     return totalHeight;
        // }
    }
}