using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ametrin.UnityUtils.EditorTools{

    [CustomPropertyDrawer(typeof(SelectFromAttribute))]
    public sealed class SelectFromAttributeDrawer : PropertyDrawer{
        private const int INFO_WIDTH = 64;
        private GUIContent[] _strValues;
        private GUIContent[] DisplayValues {
            get{
                if(_strValues is null){
                    _strValues = Values.Select((value) => new GUIContent(value.ToString())).ToArray();
                }
                return _strValues;
            }
        }
        private object[] Values => Attribute.Values;
        private SelectFromAttribute Attribute => ((SelectFromAttribute)attribute);

        [SerializeField] private int Selected = 0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            var popupRect = new Rect(position.x, position.y, position.width-INFO_WIDTH, position.height);
            var infoRect = new Rect(position.x+position.width-INFO_WIDTH-20, position.y, INFO_WIDTH+20, position.height);
            if(Values.Contains(property.boxedValue)){
                Selected = Array.IndexOf(Values, property.boxedValue);
            }
            EditorGUI.BeginChangeCheck();
            Selected = EditorGUI.Popup(popupRect, label, Selected, DisplayValues);
            if(EditorGUI.EndChangeCheck()){
                property.boxedValue = Values[Selected];
            }

            if(Attribute.AllowRawInput) {
                if(Attribute.Type == typeof(float)){
                    property.floatValue = EditorGUI.FloatField(infoRect, property.floatValue);
                }else if(Attribute.Type == typeof(int)){
                    property.intValue = EditorGUI.IntField(infoRect, property.intValue);
                }else if(Attribute.Type == typeof(double)){
                    property.doubleValue = EditorGUI.DoubleField(infoRect, property.doubleValue);
                }else if(Attribute.Type == typeof(long)){
                    property.longValue = EditorGUI.LongField(infoRect, property.longValue);
                }else{
                    EditorGUI.LabelField(infoRect, property.boxedValue.ToString());
                }
            } else{
                EditorGUI.LabelField(infoRect, property.boxedValue.ToString());
            }
        }
    }
}