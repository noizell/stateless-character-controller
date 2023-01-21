using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Surfer
{

    [CustomPropertyDrawer(typeof(SUReactionData))]
    public class SUReactionDataDrawer : PropertyDrawer
    {

        string[] _stateNames = default;
        int _idsIndex = default;
        SerializedProperty _name,_key,_vals = default;

        List<PathField> _fields = new List<PathField>();

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
            if (property.propertyType == SerializedPropertyType.Generic)
            {
                GetProperties(property);

                _stateNames = DefaultCustomReactions.UnifiedNames;


                if(_name.stringValue.Equals(SurferHelper.Unset))
                {
                    _idsIndex = 0;
                }
                else
                {
                    _idsIndex = Mathf.Clamp(System.Array.IndexOf(_stateNames, CustomReactionsExtensions.GetNameFromUnion(_key.stringValue)),0,_stateNames.Length);
                }


                position.height = SurferHelper.lineHeight;
                _idsIndex = EditorGUI.Popup(position,!string.IsNullOrEmpty(label.text) ? "Reaction" : label.text,_idsIndex,_stateNames);

                _name.stringValue =  _stateNames[_idsIndex];
                _key.stringValue = CustomReactionsExtensions.GetKeyFromUnion(_stateNames[_idsIndex]);



                if (_fields == null || _vals == null)
                    return;

                property.AddCustomUserFields(ref position,_fields,ref _vals);


            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            GetProperties(property);

            if (_fields != null)
            {
                var height = SurferHelper.lineHeight;
                
                foreach (var item in _fields)
                {
                    if(item.Field_ID == PathFieldType_ID.SerializedField)
                    {
                        var propField = property.FindPropertyRelative(item.SerializedFieldName);
                        
                        if(propField != null)
                            height += EditorGUI.GetPropertyHeight(propField);
                    }
                    else
                        height += SurferHelper.lineHeight;
                }

                return height;

            }
            


            return SurferHelper.lineHeight;

     
        }


        void GetProperties(SerializedProperty property)
        {
            _name = property.FindPropertyRelative("_name");
            _key = property.FindPropertyRelative("_key");
            _vals = property.FindPropertyRelative("_fieldsValues");
            _fields = CustomReactionsExtensions.GetFieldsList(_key.stringValue);
        }


    }


}