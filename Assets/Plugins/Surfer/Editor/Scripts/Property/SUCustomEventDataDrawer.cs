using UnityEditor;
using UnityEngine;

namespace Surfer
{
    [CustomPropertyDrawer(typeof(SUCustomEventData))]
    public class SUCustomEventDataDrawer : PropertyDrawer
    {

        string[] _eventNames = default;
        int _idsIndex = default;
        

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
            if (property.propertyType == SerializedPropertyType.Generic)
            {

                position.height = SurferHelper.lineHeight;

                _eventNames = SurferHelper.SO.GetEventsList();

                _idsIndex = Mathf.Clamp(System.Array.IndexOf(_eventNames, SurferHelper.SO.GetEvent(property.FindPropertyRelative("_guid").stringValue)),0, _eventNames.Length);

                _idsIndex = EditorGUI.Popup(position,!string.IsNullOrEmpty(label.text) ? "Event" : label.text,_idsIndex, _eventNames);

                property.FindPropertyRelative("_guid").stringValue = SurferHelper.SO.GetEventKey(_eventNames[_idsIndex]);
                
            }
        }
    }
}