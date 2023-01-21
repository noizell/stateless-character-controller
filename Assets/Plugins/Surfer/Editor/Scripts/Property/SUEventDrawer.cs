using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace Surfer
{
    [CustomPropertyDrawer(typeof(SUEvent))]
    public class SUEventDrawer : PropertyDrawer
    {
        SerializedProperty _statesData, _type, _scenesData, _keyCodeVal, _intVal, _stringVal, _updateLoop, _eventTypeRew,
            _pInput, _floatVal, _cEventsData, _intVal2 = default;

        SUEvent.Type_ID _selEvent
        {
            get
            {
                return (SUEvent.Type_ID)_type.enumValueIndex;
            }
        }


        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            //EditorGUI.indentLevel += EditorGUI.indentLevel == 0 ? 0 : 1;

            position.height = SurferHelper.lineHeight;


            _type.enumValueIndex = EditorGUI.Popup(position, _type.enumValueIndex, GetEventList(property));

            CheckEvent(position);


        }



        void CheckEvent(Rect position)
        {

            if(IsState)
            {

                EditorGUI.indentLevel = 1;
                position.y += SurferHelper.lineHeight;
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, _intVal, new GUIContent("Version"));


                if (IsCustomState)
                {
                    position.y += SurferHelper.lineHeight;
                    EditorGUI.PropertyField(position, _intVal2, new GUIContent("PlayerID"));

                    position.y += SurferHelper.lineHeight;
                    EditorGUI.PropertyField(position, _statesData);

                }


            }
            else if(IsFloat)
            {

                EditorGUI.indentLevel = 1;
                position.y += SurferHelper.lineHeight;

                EditorGUI.PropertyField(position, _floatVal, new GUIContent("Value"));
            }
            else if(IsCustomScene)
            {

                EditorGUI.indentLevel = 1;
                position.y += SurferHelper.lineHeight;

                EditorGUI.PropertyField(position, _scenesData);

            }
            else if(IsKeyCode)
            {

                if (!SurferHelper.HasIntegration(SurferHelper.OldInputSymbol))
                {

                    position.y += SurferHelper.lineHeight;
                    position.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.LabelField(position, "Activate Old Input System Integration to use it!");
                    return;
                }

                EditorGUI.indentLevel = 1;
                position.y += SurferHelper.lineHeight;

                EditorGUI.PropertyField(position, _keyCodeVal, new GUIContent("Key Code"));

            }
            else if (IsCustomEvent)
            {

                EditorGUI.indentLevel = 1;
                position.y += SurferHelper.lineHeight;

                EditorGUI.PropertyField(position, _cEventsData, new GUIContent("Custom Events"));

            }
            else if (IsNewInputField)
            {
                if (!SurferHelper.HasIntegration(SurferHelper.NewInputSymbol))
                {

                    position.y += SurferHelper.lineHeight;
                    position.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.LabelField(position, "Activate New Input System Integration to use it!");
                    return;
                }

                EditorGUI.indentLevel = 1;
                position.y += SurferHelper.lineHeight;
                position.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(position, _stringVal, new GUIContent("Action name"));

                position.y += SurferHelper.lineHeight;
                position.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(position, _pInput, new GUIContent("Input Asset"));

            }
            else if(IsStringField)
            {

                if (!SurferHelper.HasIntegration(SurferHelper.OldInputSymbol))
                {

                    position.y += SurferHelper.lineHeight;
                    position.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.LabelField(position, "Activate Old Input System Integration to use it!");
                    return;
                }

                EditorGUI.indentLevel = 1;
                position.y += SurferHelper.lineHeight;
                position.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(position, _stringVal, new GUIContent("Action name"));

            }
            else if(IsRewired)
            {

                if (!SurferHelper.HasIntegration(SurferHelper.RewiredInputSymbol))
                {

                    position.y += SurferHelper.lineHeight;
                    position.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.LabelField(position, "Activate Rewired Integration to use it!");
                    return;
                }


                EditorGUI.indentLevel = 1;
                position.y += SurferHelper.lineHeight;
                position.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(position, _stringVal, new GUIContent("Action name"));
                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position, _intVal, new GUIContent("PlayerID"));
                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position, _updateLoop, new GUIContent("Update Loop"));
                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position, _eventTypeRew, new GUIContent("Event Type"));
            }

        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            float height = EditorGUIUtility.singleLineHeight;


            _type = property.FindPropertyRelative("_type");
            _stringVal = property.FindPropertyRelative("_stringVal");
            _intVal = property.FindPropertyRelative("_intVal");
            _intVal2 = property.FindPropertyRelative("_intVal2");
            _statesData = property.FindPropertyRelative("_statesData");
            _scenesData = property.FindPropertyRelative("_scenesData");
            _keyCodeVal = property.FindPropertyRelative("_keyCodeVal");
            _eventTypeRew = property.FindPropertyRelative("_eventType");
            _updateLoop = property.FindPropertyRelative("_updateLoop");
            _pInput = property.FindPropertyRelative("_pInput");
            _floatVal = property.FindPropertyRelative("_floatVal");
            _cEventsData = property.FindPropertyRelative("_cEventsData");


            if (IsState)
            {
                height += SurferHelper.lineHeight;

                if(IsCustomState)
                {
                    height += EditorGUI.GetPropertyHeight(_statesData, label);
                    height += SurferHelper.lineHeight;
                }
            }
            else if (IsCustomScene)
            {
                height += EditorGUI.GetPropertyHeight(_scenesData, label);
            }
            else if (IsFloat)
            {
                height += EditorGUI.GetPropertyHeight(_floatVal, label);
            }
            else if(IsKeyCode)
            {
                height += EditorGUI.GetPropertyHeight(_keyCodeVal, label);
            }
            else if (IsCustomEvent)
            {
                height += EditorGUI.GetPropertyHeight(_cEventsData, label);
            }
            else if(IsStringField)
            {
                height += EditorGUI.GetPropertyHeight(_stringVal, label);


                if (IsNewInputField)
                {

                    if (_pInput != null)
                    {
                        height += EditorGUI.GetPropertyHeight(_pInput, label);
                    }
                    else
                    {
                        height += SurferHelper.lineHeight;
                    }

                }

            }
            else if (IsRewired)
            {
                if(_eventTypeRew != null)
                {
                    height += EditorGUI.GetPropertyHeight(_stringVal, label);
                    height += EditorGUI.GetPropertyHeight(_intVal, label);
                    height += EditorGUI.GetPropertyHeight(_updateLoop, label);
                    height += EditorGUI.GetPropertyHeight(_eventTypeRew, label);
                }
                else
                {
                    height += SurferHelper.lineHeight;
                }


            }


            return height;
        }


        string[] GetEventList(SerializedProperty property)
        {

            string[] list = System.Enum.GetNames(typeof(SUEvent.Type_ID));

            for (int i = 0; i < list.Length; i++)
            {

                if (!IsEventAvailable((SUEvent.Type_ID)i,property))
                {
                    list[i] = null;
                    continue;
                }

                list[i] = list[i].Replace("_", @"/");
            }


            return list;

        }




        bool IsEventAvailable(SUEvent.Type_ID eventID,SerializedProperty property)
        {

            if ( (eventID == SUEvent.Type_ID.Toggle_OnFalse
                || eventID == SUEvent.Type_ID.Toggle_OnTrue)
                && (property.serializedObject.targetObject as Component).GetComponent<Toggle>() == null)
                return false;

            if ((eventID == SUEvent.Type_ID.InputField_OnEndEdit
                || eventID == SUEvent.Type_ID.InputField_OnValueChanged)
                && (property.serializedObject.targetObject as Component).GetComponent<TMP_InputField>() == null)
                return false;

            if ((eventID == SUEvent.Type_ID.ScrollRect_OnNotReachedAnySide
                || eventID == SUEvent.Type_ID.ScrollRect_OnReachedBottom
                || eventID == SUEvent.Type_ID.ScrollRect_OnReachedLeft
                || eventID == SUEvent.Type_ID.ScrollRect_OnReachedRight
                || eventID == SUEvent.Type_ID.ScrollRect_OnReachedTop)
                && (property.serializedObject.targetObject as Component).GetComponent<ScrollRect>() == null)
                return false;

            if ((eventID == SUEvent.Type_ID.Dropdown_OnFirstOptionSelected
                || eventID == SUEvent.Type_ID.Dropdown_OnOptionSelected)
                && (property.serializedObject.targetObject as Component).GetComponent<TMP_Dropdown>() == null)
                return false;

            if ((eventID == SUEvent.Type_ID.Slider_OnGreaterThan
                || eventID == SUEvent.Type_ID.Slider_OnLowerThan
                || eventID == SUEvent.Type_ID.Slider_OnMax
                || eventID == SUEvent.Type_ID.Slider_OnMin)
                && (property.serializedObject.targetObject as Component).GetComponent<Slider>() == null)
                return false;

            return true;

        }


        bool IsCustomScene
        {
            get
            {
                return _selEvent == SUEvent.Type_ID.Scene_Activated || _selEvent == SUEvent.Type_ID.Scene_Deactivated
                || _selEvent == SUEvent.Type_ID.Scene_Loaded || _selEvent == SUEvent.Type_ID.Scene_Loading
                || _selEvent == SUEvent.Type_ID.Scene_Unloaded || _selEvent == SUEvent.Type_ID.Scene_Unloading;
            }
            
        }

        bool IsCustomEvent
        {
            get
            {
                return _selEvent == SUEvent.Type_ID.MyCustomEvent;
            }
        }

        bool IsFloat
        {
            get
            {
                return _selEvent == SUEvent.Type_ID.Slider_OnGreaterThan || _selEvent == SUEvent.Type_ID.Slider_OnLowerThan;
            }
        }

        bool IsCustomState
        {
            get
            {
                return _selEvent == SUEvent.Type_ID.State_Enter || _selEvent == SUEvent.Type_ID.State_Exit;
            }
        }

        bool IsState
        {
            get
            {
                return IsCustomState || _selEvent == SUEvent.Type_ID.State_MyStateEnter || _selEvent == SUEvent.Type_ID.State_MyStateExit;
            }
        }

        bool IsKeyCode
        {
            get
            {
                return _selEvent == SUEvent.Type_ID.Input_OldInput_OnKeyDown;
            }
        }


        bool IsStringField
        {
            get
            {
                return _selEvent == SUEvent.Type_ID.Input_NewInput_OnAction || _selEvent == SUEvent.Type_ID.Input_OldInput_OnButtonDown;
            }
        }

        bool IsNewInputField
        {
            get
            {
                return _selEvent == SUEvent.Type_ID.Input_NewInput_OnAction;
            }
        }


        bool IsRewired
        {
            get
            {
                return _selEvent == SUEvent.Type_ID.Input_Rewired_OnAction;
            }
        }


    }






}


