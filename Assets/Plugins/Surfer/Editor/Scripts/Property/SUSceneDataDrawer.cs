using UnityEditor;
using UnityEngine;

namespace Surfer
{
    [CustomPropertyDrawer(typeof(SUSceneData))]
    public class SUSceneDataDrawer : PropertyDrawer
    {

        string[] _sceneNames = default;
        int _idsIndex = default;
        

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
            if (property.propertyType == SerializedPropertyType.Generic)
            {

                position.height = SurferHelper.lineHeight;

                _sceneNames = SurferHelper.SO.GetScenesNameStrings();

                _idsIndex = Mathf.Clamp(System.Array.IndexOf(_sceneNames,SurferHelper.SO.GetSceneName(property.FindPropertyRelative("_guid").stringValue)),0,_sceneNames.Length);

                _idsIndex = EditorGUI.Popup(position,!string.IsNullOrEmpty(label.text) ? "Scene" : label.text,_idsIndex,_sceneNames);

                property.FindPropertyRelative("_guid").stringValue = SurferHelper.SO.GetSceneGUID(_sceneNames[_idsIndex]);
                
            }
        }
    }
}