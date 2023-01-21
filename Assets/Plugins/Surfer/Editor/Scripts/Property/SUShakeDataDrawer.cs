using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


 namespace Surfer
 {

    [CustomPropertyDrawer(typeof(SUShakeData))]
    public class SUShakeDataDrawer : SUAnimationDataDrawer
    {

        SerializedProperty  _mode = default;

        protected override bool OnCanUseDefaultModes(ref Rect position,SerializedProperty property)
        {   
            
            position.y += SurferHelper.lineHeight;
            EditorGUI.PropertyField(position,_mode);
            
            if(!IsNone())
            {

                position.y += SurferHelper.lineHeight;
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position,property.FindPropertyRelative("_shake"));
                position.y += SurferHelper.lineHeight;
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position,property.FindPropertyRelative("_randomness"));
                position.y += SurferHelper.lineHeight;
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position,property.FindPropertyRelative("_vibrato"));
                

            }

            return false;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            _mode = property.FindPropertyRelative("_mode");
            base.GetPropertyHeight(property,label);

            if(!property.isExpanded)
            return SurferHelper.lineHeight;

            return (IsNone() ? SurferHelper.lineHeight*3 : SurferHelper.lineHeight * (5+GetHeightCommonFields()));
        }

        protected override bool IsNone()
        {
            return _mode.enumValueIndex == 0; 
        } 
    }

 }
 