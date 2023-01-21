using UnityEditor;
using UnityEngine;

namespace Surfer
{
    [CustomPropertyDrawer(typeof(SUStateData))]
    public class SUStateDataDrawer : PropertyDrawer
    {

        string[] _stateNames = default;
        int _idsIndex = default;


        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(property.serializedObject.isEditingMultipleObjects)
            {
                EditorGUI.LabelField(position,"Multiple Editing of State name not allowed");
                return;
            }

            // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
            if (property.propertyType == SerializedPropertyType.Generic)
            {
                position.height = SurferHelper.lineHeight;

                _stateNames = SurferHelper.SO.GetStatesList();

                if(string.IsNullOrEmpty(property.FindPropertyRelative("_guid").stringValue))
                {
                    _idsIndex = 0;
                }
                else
                {
                    _idsIndex = Mathf.Clamp(System.Array.IndexOf(_stateNames,SurferHelper.SO.GetState(property.FindPropertyRelative("_guid").stringValue)),0,_stateNames.Length);
                }
                
                
                _idsIndex = EditorGUI.Popup(position,label.text.Contains("Data") ? "State" : label.text,_idsIndex,_stateNames);

                property.FindPropertyRelative("_guid").stringValue = SurferHelper.SO.GetStateKey(_stateNames[_idsIndex]);
                
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return SurferHelper.lineHeight ;
        }
    }


}