using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer
{

    [CustomPropertyDrawer(typeof(SUAnimationData),true)]
    public class SUAnimationDataDrawer : PropertyDrawer
    {


        protected SerializedProperty  _startMode , _endMode, _ease, _looped, _obj, _useUnscaledTime = default;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);

            if (property.type.Contains("Char") && !SurferHelper.HasIntegration(SurferHelper.CharTweenerSymbol))
            {
                position.height = SurferHelper.lineHeight;
                EditorGUI.LabelField(position, "---Activate CharTweener Integration to use it!---");
                EditorGUI.EndProperty();
                return;
            }
            //if(property.type.Contains("Color") && (property.serializedObject.targetObject as Component).GetComponent<Renderer>() == null 
            //&& (property.serializedObject.targetObject as Component).GetComponent<Graphic>() == null )
            //{
            //    position.height = SurferHelper.lineHeight;
            //    EditorGUI.LabelField(position,"---Add Renderer or Graphic to animate color!---");
            //    EditorGUI.EndProperty();
            //    return;
            //}


            GetProperties(property);
            position.height = EditorGUIUtility.singleLineHeight;
            property.isExpanded = EditorGUI.Foldout(position,property.isExpanded,IsNone() ? property.displayName : property.displayName+" (In Use) ",true);

            if(property.isExpanded)
            {

                if(OnCanUseDefaultModes(ref position,property))
                {

                    position.y += SurferHelper.lineHeight;

                    _startMode.enumValueIndex = (int)(SUAnimationData.StartMode_ID)EditorGUI.EnumPopup(position,new GUIContent("Start Mode"),(SUAnimationData.StartMode_ID)_startMode.intValue,(Enum value)=>
                    {

                        return CanUseStartMode((int)(SUAnimationData.StartMode_ID)value);

                    });

                    if(IsNone())
                    return;




                    if(_startMode.intValue == (int)SUAnimationData.StartMode_ID.From)
                    {
                        position.y += SurferHelper.lineHeight;
                        EditorGUI.PropertyField(position,property.FindPropertyRelative("_from"));
                    }


                    position.y += SurferHelper.lineHeight;

                    _endMode.enumValueIndex = (int)(SUAnimationData.EndMode_ID)EditorGUI.EnumPopup(position,new GUIContent("End Mode"),(SUAnimationData.EndMode_ID)_endMode.intValue,(Enum value)=>
                    {

                        return CanUseEndMode((int)(SUAnimationData.EndMode_ID)value);

                    });


                    if(_endMode.intValue == (int)SUAnimationData.EndMode_ID.To)
                    {
                        position.y += SurferHelper.lineHeight;
                        EditorGUI.PropertyField(position,property.FindPropertyRelative("_to"));
                    }

                }


                if(IsNone())
                    return;

                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position,property.FindPropertyRelative("_duration"));
                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position,property.FindPropertyRelative("_delay"));
                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position,property.FindPropertyRelative("_delayMode"));
                position.y += SurferHelper.lineHeight;

                _ease.enumValueIndex = (int)(Ease)EditorGUI.EnumPopup(position,new GUIContent("Ease"),(Ease)_ease.enumValueIndex,(Enum value)=>
                {

                    return CanUseEase((int)(Ease)value);

                });


                if(OnCanBeLooped())
                {
                    position.y += SurferHelper.lineHeight;
                    EditorGUI.PropertyField(position, _looped);

                    if (_looped.boolValue)
                    {
                        position.y += SurferHelper.lineHeight;
                        EditorGUI.PropertyField(position, property.FindPropertyRelative("_loop"));

                    }
                }


                position.y += SurferHelper.lineHeight;

                if (_obj.objectReferenceValue == null)
                    _obj.objectReferenceValue = (property.serializedObject.targetObject as Component).gameObject;

                EditorGUI.PropertyField(position, _obj, new GUIContent("Who?"));                         
                position.y += SurferHelper.lineHeight;       
                EditorGUI.PropertyField(position, _useUnscaledTime, new GUIContent("Ignore Time Scale"));

            }

            EditorGUI.EndProperty();

        }

        bool CanUseStartMode(int startModeID)
        {
            return startModeID != (int)SUAnimationData.StartMode_ID.FromLeft 
            && startModeID != (int)SUAnimationData.StartMode_ID.FromBottom 
            && startModeID != (int)SUAnimationData.StartMode_ID.FromTop 
            && startModeID != (int)SUAnimationData.StartMode_ID.FromRight
            && startModeID != (int)SUAnimationData.StartMode_ID.FromCenter;
        }

        bool CanUseEndMode(int endModeID)
        {
            return endModeID != (int)SUAnimationData.EndMode_ID.ToLeft 
            && endModeID != (int)SUAnimationData.EndMode_ID.ToBottom 
            && endModeID != (int)SUAnimationData.EndMode_ID.ToTop 
            && endModeID != (int)SUAnimationData.EndMode_ID.ToRight
            && endModeID != (int)SUAnimationData.EndMode_ID.ToCenter;
        }

        bool CanUseEase(int easeId)
        {
            return easeId != (int)Ease.INTERNAL_Custom && easeId != (int)Ease.INTERNAL_Zero;
        }

        protected virtual bool OnCanUseDefaultModes(ref Rect position,SerializedProperty property)
        {

            return true;

        }


        protected virtual bool OnCanBeLooped()
        {
            return true;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            GetProperties(property);

            int totShown = 1;
            
            if(property.isExpanded)
            {
                totShown +=2;

                if(!IsNone())
                {

                    if(_startMode.intValue == (int)SUCGroupData.StartMode_ID.From)
                        totShown +=1;
                    if(_endMode.intValue == (int)SUCGroupData.EndMode_ID.To)
                        totShown +=1;
                
                    
                    totShown+=GetHeightCommonFields();
                }
                    

                return SurferHelper.lineHeight * totShown;
            }
            else
            {
                return SurferHelper.lineHeight * totShown;
            }

        }

        void GetProperties(SerializedProperty property)
        {

            _startMode = property.FindPropertyRelative("_startMode");
            _endMode = property.FindPropertyRelative("_endMode");
            _ease = property.FindPropertyRelative("_ease");
            _looped = property.FindPropertyRelative("_looped");
            _obj = property.FindPropertyRelative("_obj");
            _useUnscaledTime = property.FindPropertyRelative("_useUnscaledTime");
        }

        protected virtual int GetHeightCommonFields()
        {
            int defaultTotal = 7;

            if(_looped.boolValue)
                return defaultTotal+=1;

            return defaultTotal;
        }

        protected virtual bool IsNone()
        {
            return _startMode.intValue == (int)SUColorData.StartMode_ID.None;
        }

    }


}

