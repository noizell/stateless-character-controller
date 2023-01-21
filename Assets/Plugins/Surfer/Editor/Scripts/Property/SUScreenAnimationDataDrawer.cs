using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Surfer
{

    [CustomPropertyDrawer(typeof(SUScreenAnimationData),true)]
    public class SUScreenAnimationDataDrawer : SUAnimationDataDrawer
    {

        protected override bool OnCanUseDefaultModes(ref Rect position,SerializedProperty property)
        {   
            
            position.y += SurferHelper.lineHeight;
            EditorGUI.PropertyField(position,_startMode);
            
            if(!IsNone())
            {

                if(IsCustomStart())
                {
                    position.y += SurferHelper.lineHeight;
                    position.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(position,property.FindPropertyRelative("_fromParent"), new GUIContent("From Parent %"));
                    position.y += SurferHelper.lineHeight;
                    position.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(position,property.FindPropertyRelative("_fromSize"), new GUIContent("From Size %"));
                }

                position.y += SurferHelper.lineHeight;
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position,property.FindPropertyRelative("_endMode"));

                if(IsCustomEnd())
                {
                    position.y += SurferHelper.lineHeight;
                    position.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(position,property.FindPropertyRelative("_toParent"), new GUIContent("To Parent %"));
                    position.y += SurferHelper.lineHeight;
                    position.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(position,property.FindPropertyRelative("_toSize"), new GUIContent("To Size %"));
                }

                OnSupplementaryFields(ref position,property);
                
                
            }

            return false;
        }


        protected virtual void OnSupplementaryFields(ref Rect position,SerializedProperty property)
        {
            
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            base.GetPropertyHeight(property,label);

            if(!property.isExpanded)
            return SurferHelper.lineHeight;

            int total = 3;

            if(IsNone())
            return SurferHelper.lineHeight * total;

            if(IsCustomStart())
                total += 2;
            if(IsCustomEnd())
                total += 2;

            return SurferHelper.lineHeight * (total+GetHeightCommonFields());
        }

        protected override bool IsNone()
        {
            return _startMode.enumValueIndex == 0; 
        }

        bool IsCustomStart()
        {
            return _startMode.enumValueIndex == (int)SUJumpData.StartMode_ID.From; 
        }  

        bool IsCustomEnd()
        {
            return _endMode.enumValueIndex == (int)SUJumpData.EndMode_ID.To; 
        } 
    }



}
