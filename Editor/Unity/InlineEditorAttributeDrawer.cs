using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ametrin.Utils.Unity.EditorTools
{
    [CustomPropertyDrawer(typeof(InlineEditorAttribute), true)]
    public sealed class InlineEditorAttributeDrawer : PropertyDrawer
    {
        private readonly static StyleColor BackgroundColor = new Color(0, 0, 0, 0.1f);
        private readonly static StyleColor CollapsedBackgroundColor = new Color(0, 0, 0, 0);

        private VisualElement Root;
        private PropertyField PropertyField;
        private Foldout EditorFoldout;
        private VisualElement InlinedEditor;
        private SerializedProperty Property;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            Property = property;
            Root = new VisualElement();
            Root.style.BorderRadius(5);
            Root.style.paddingBottom = EditorGUIUtility.standardVerticalSpacing;

            CreateFoldout();
            CreatePropertyField();

            RefreshUI();

            return Root;
        }

        private void CreateFoldout()
        {
            EditorFoldout = new()
            {
                value = false
            };
            EditorFoldout.RegisterValueChangedCallback(OnFoldoutChanged);
            EditorFoldout.style.paddingBottom = EditorGUIUtility.standardVerticalSpacing;
            UpdateRootColor();
            Root.Add(EditorFoldout);
        }

        private void CreatePropertyField()
        {
            PropertyField = new(Property);
            PropertyField.style.alignContent = Align.Center;
            PropertyField.style.top = 0;
            PropertyField.style.right = 0;
            PropertyField.RegisterValueChangeCallback(evt =>
            {
                evt.StopImmediatePropagation();
                RefreshUI();
            });
            Root.Add(PropertyField);
        }

        private void RefreshUI()
        {
            var obj = Property.objectReferenceValue;
            if (obj != null)
            {
                PropertyField.style.position = Position.Absolute;
                PropertyField.style.left = 15;
                EditorFoldout.SetEnabled(true);
                EditorFoldout.style.display = DisplayStyle.Flex;
            }
            else
            {
                EditorFoldout.Clear();
                EditorFoldout.style.display = DisplayStyle.None;
                EditorFoldout.SetEnabled(false);
                PropertyField.style.position = Position.Relative;
                PropertyField.style.left = 0;
                Root.style.backgroundColor = CollapsedBackgroundColor;
            }
        }

        private void BuildEditor()
        {
            var editor = Editor.CreateEditor(Property.objectReferenceValue);
            var rootier = Root.GetFirstAncestorOfType<Foldout>()?.GetFirstAncestorOfType<Foldout>();
            if (rootier == null)
            {
                InlinedEditor = editor.CreateInspectorGUI();
                InlinedEditor.Bind(new(Property.objectReferenceValue));
            }
            else
            {
                var imGUIContainer = new IMGUIContainer(editor.OnInspectorGUI);
                InlinedEditor = imGUIContainer;
            }

            EditorFoldout.Add(InlinedEditor);
        }

        private void OnFoldoutChanged(ChangeEvent<bool> evt)
        {
            evt.StopImmediatePropagation();
            if (InlinedEditor == null && Root.parent != null)
            {
                BuildEditor();
            }

            UpdateRootColor();
        }

        private void UpdateRootColor()
        {
            Root.style.backgroundColor = EditorFoldout.value ? BackgroundColor : CollapsedBackgroundColor;
        }

        //IMGUI compat, used when nesting to deep
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
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