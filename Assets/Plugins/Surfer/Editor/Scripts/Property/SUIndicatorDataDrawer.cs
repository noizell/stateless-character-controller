using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Surfer
{
    [CustomPropertyDrawer(typeof(SUIndicatorData))]
    public class SUIndicatorDataDrawer : PropertyDrawer
    {

        SerializedProperty _type, _prefab, _radius, _customTag, _renderMode, _cam , _playerObj, _onScreenOffset = default;

        bool IsOnScreenMode
        {
            get
            {
                return _type.enumValueIndex == (int)SUIndicatorData.Type_ID.Both
                    || _type.enumValueIndex == (int)SUIndicatorData.Type_ID.OnScreen;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.indentLevel += EditorGUI.indentLevel == 0 ? 0 : 1;

            position.height = EditorGUIUtility.singleLineHeight;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, string.IsNullOrEmpty(label.text) ? "Data" : label.text, true);


            if(property.isExpanded)
            {
                GetPropertyRelatives(property);

                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position, _customTag);

                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position, _prefab);

                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position, _cam);

                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position, _renderMode);

                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position, _type);


                if (IsOnScreenMode)
                {
                    position.y += SurferHelper.lineHeight;
                    EditorGUI.PropertyField(position, _onScreenOffset, new GUIContent("OnScreen Offset"));
                }

                position.y += SurferHelper.lineHeight;
                EditorGUI.LabelField(position,new GUIContent("-----Distance Check-----"));

                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position, _playerObj);

                if(_playerObj.objectReferenceValue != null)
                {

                    position.y += SurferHelper.lineHeight;
                    EditorGUI.PropertyField(position, _radius);

                }



            }

        }



        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            float height = default;
            GetPropertyRelatives(property);

            if (IsOnScreenMode)
            {
                height += SurferHelper.lineHeight;
            }

            if (_playerObj.objectReferenceValue != null)
                height += SurferHelper.lineHeight;

            height += SurferHelper.lineHeight * 9;
            return height;
        }


        void GetPropertyRelatives(SerializedProperty property)
        {
            _type = property.FindPropertyRelative("_type");
            _prefab = property.FindPropertyRelative("_prefab");
            _renderMode = property.FindPropertyRelative("_renderMode");
            _cam = property.FindPropertyRelative("_cam");
            _radius = property.FindPropertyRelative("_radius");
            _playerObj = property.FindPropertyRelative("_playerObj");
            _customTag = property.FindPropertyRelative("_customTag");
            _onScreenOffset = property.FindPropertyRelative("_onScreenOffset");
        }




    }
}


