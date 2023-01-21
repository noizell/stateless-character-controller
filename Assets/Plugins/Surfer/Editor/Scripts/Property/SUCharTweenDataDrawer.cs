using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


 namespace Surfer
 {

    [CustomPropertyDrawer(typeof(SUCharTweenData))]
    public class SUCharTweenDataDrawer : SUAnimationDataDrawer
    {

        SerializedProperty  _mode = default;

        protected override bool OnCanUseDefaultModes(ref Rect position,SerializedProperty property)
        {   
            
            position.y += SurferHelper.lineHeight;
            EditorGUI.PropertyField(position,_mode);
          

            return false;
        }



        protected override bool OnCanBeLooped()
        {
            return false;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            _mode = property.FindPropertyRelative("_mode");
            base.GetPropertyHeight(property,label);

            if(!property.isExpanded)
            return SurferHelper.lineHeight;

            return (IsNone() ? SurferHelper.lineHeight*3 : SurferHelper.lineHeight * (1 + GetHeightCommonFields()));
        }

        protected override bool IsNone()
        {
            return _mode.enumValueIndex == 0; 
        } 
    }

 }
 