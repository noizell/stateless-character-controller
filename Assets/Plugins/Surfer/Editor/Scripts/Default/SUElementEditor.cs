using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer
{

    [CustomEditor(typeof(SUElement),true)]
    public class SUElementEditor : Editor
    {

        SerializedProperty _elementData , _behaviours , _selectedBehaviour, _event, _raycast2D3D,
            _customConds, _fastConds , _fastReactions, _animations, _actions, _reactions,
            _fastFailReactions, _failAnimations, _failActions , _failReactions = default;

        bool _foldConditions = default, _foldReactions = default , _foldEvent = default, _foldSuccess = true;

        int _eventIdx = 0;

        SUElement _element = default;

        GUIStyle _style, _styleGreen, _styleRed = default;

        string _textWhen = "WHEN!", _textIf = "IF!", _textDo = "DO!";

        float _btnHeight = EditorGUIUtility.singleLineHeight * 1.25f;

        int CountSuccessFastR
        {
            get
            {
                return Mathf.Clamp(_fastReactions.FindPropertyRelative("_reactions").arraySize - 1,0,1_000);
            }
        }

        int CountFailFastR
        {
            get
            {
                return Mathf.Clamp(_fastFailReactions.FindPropertyRelative("_reactions").arraySize -1,0,1_000) ;
            }
        }

        int CountSuccessCustomR
        {
            get
            {
                return Mathf.Clamp( _reactions.FindPropertyRelative("_reactions").arraySize-1,0,1_000) ;
            }
        }

        int CountFailCustomR
        {
            get
            {
                return Mathf.Clamp(_failReactions.FindPropertyRelative("_reactions").arraySize-1,0,1_000) ;
            }
        }


        void OnEnable()
        {
            if(serializedObject == null)
                return;

            _elementData = serializedObject.FindProperty("_elementData");
            _behaviours = serializedObject.FindProperty("_behaviours").FindPropertyRelative("_behaviours");
            _raycast2D3D = serializedObject.FindProperty("_raycast2D3D");

            _element = (SUElement)target;

            CreateStyle(ref _style,"Rect");
            CreateStyle(ref _styleRed, "RectRed");
            CreateStyle(ref _styleGreen, "RectGreen");

            if(_foldSuccess)
                ChangeStyle(ref _styleRed, "RectWhite", Color.gray);
            else
                ChangeStyle(ref _styleGreen, "RectWhite", Color.gray);
        }


        private void OnDisable() 
        {
            if(target == null)
                return;
            if(EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            serializedObject.ApplyModifiedProperties();
            UpdateDictionary();

        }

        private void OnValidate() {

            serializedObject.ApplyModifiedProperties();
            UpdateDictionary();
        }

        void CreateStyle(ref GUIStyle style,string imageName)
        {

            style = new GUIStyle();
            style.normal.background = EditorGUIUtility.Load("Assets/Surfer/Editor/Images/"+imageName+".png") as Texture2D;
            style.stretchWidth = true;
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;

        }

        void ChangeStyle(ref GUIStyle style,string imageName, Color textColor)
        {
            style.normal.background = EditorGUIUtility.Load("Assets/Surfer/Editor/Images/" + imageName + ".png") as Texture2D;
            style.normal.textColor = textColor;
        }


        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                GUI.enabled = false;
                //EditorGUILayout.LabelField("Not editable during Play Mode!");
                //return;
            }

            serializedObject.Update();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_raycast2D3D, new GUIContent("Is sprite or mesh?"));
            EditorGUILayout.PropertyField(_elementData);

            SurferHelper.DrawLine();
            EditorGUILayout.Space();
            SurferHelper.DrawLine();
            EditorGUILayout.Space();

            if(_behaviours.arraySize <= 0)
                EditorGUILayout.LabelField("No events created! Add one!");
            else
                EditorGUILayout.LabelField("Choose the event behaviour to edit!");

            using (var group = new GUILayout.HorizontalScope())
            {
                if(_behaviours.arraySize > 0)
                    _eventIdx = EditorGUILayout.Popup(_eventIdx, GetEventsList());


                if (GUILayout.Button("+"))
                {
                    AddNewEvent();
                    serializedObject.ApplyModifiedProperties();

                    _element.Behaviours.Behaviours[_element.Behaviours.Behaviours.Count - 1] = new SUBehaviourData();

                    UpdateDictionary();
                    return;
                }

                if (_behaviours.arraySize > 0)
                {
                    if (GUILayout.Button("x"))
                    {
                        RemoveEvent();
                        serializedObject.ApplyModifiedProperties();
                        UpdateDictionary();
                        return;
                    }
                }


            }



            EditorGUILayout.Space();
            SurferHelper.DrawLine();


            if (_behaviours.arraySize <= 0)
            {

                serializedObject.ApplyModifiedProperties();
                return;
            }
            //show selected behaviour

            _selectedBehaviour = _behaviours.GetArrayElementAtIndex(_eventIdx);
            _event = _selectedBehaviour.FindPropertyRelative("_event");
            EditorGUILayout.Space();




            if(GUILayout.Button(_textWhen, _style,GUILayout.Height(_btnHeight)))
            {
                _foldEvent = !_foldEvent;
            }


            if (_foldEvent)
            {
                SurferHelper.DrawLine();
                EditorGUILayout.PropertyField(_event);
            }




            //conditions
            EditorGUI.indentLevel = 0;
            EditorGUILayout.Space();


            Conditions();

            //reactions
            EditorGUILayout.Space();

            Reactions();

            EditorGUILayout.Space();

            if(GUI.changed)
            {

                serializedObject.ApplyModifiedProperties();
                UpdateDictionary();

            }


        }



        void Reactions()
        {

            _reactions = _selectedBehaviour.FindPropertyRelative("_reactions");
            _failReactions = _selectedBehaviour.FindPropertyRelative("_failReactions");

            if(!IsNewComponent())
            {
                _fastReactions = _selectedBehaviour.FindPropertyRelative("_fastReactions");
                _animations = _selectedBehaviour.FindPropertyRelative("_animations");
                _actions = _selectedBehaviour.FindPropertyRelative("_actions");

                _fastFailReactions = _selectedBehaviour.FindPropertyRelative("_fastFailReactions");
                _failAnimations = _selectedBehaviour.FindPropertyRelative("_failAnimations");
                _failActions = _selectedBehaviour.FindPropertyRelative("_failActions");
            }

            


            if (GUILayout.Button(_textDo, _style, GUILayout.Height(_btnHeight)))
            {
                _foldReactions = !_foldReactions;
            }


            //total reactions calculations
            if (!_foldReactions)
            {
                //calculate total reactions
                int totTrue = IsNewComponent() ? CountSuccessCustomR : CountSuccessFastR
                    + _actions.arraySize + CountSuccessCustomR + GetCountAnimations(_animations );

                int totFalse = IsNewComponent() ? CountFailCustomR : CountFailFastR
                    + _failActions.arraySize + CountFailCustomR + GetCountAnimations(_failAnimations );

                _textDo = totTrue == 0 && totFalse == 0 ? "DO!" : "DO ... ( True:" + totTrue + " , False:"+ totFalse + " )";
            }
            else
            {
                _textDo = "DO!";

                using (var group = new GUILayout.HorizontalScope())
                {

                    if (GUILayout.Button("If Conditions True...", _styleGreen, GUILayout.Height(_btnHeight)))
                    {
                        _foldSuccess = true;
                        ChangeStyle(ref _styleGreen, "RectGreen", Color.white);
                        ChangeStyle(ref _styleRed, "RectWhite", Color.gray);

                    }
                    if (GUILayout.Button("If Conditions False...", _styleRed, GUILayout.Height(_btnHeight)))
                    {
                        _foldSuccess = false;
                        ChangeStyle(ref _styleGreen, "RectWhite", Color.gray);
                        ChangeStyle(ref _styleRed, "RectRed", Color.white);
                    }
                }


                if(_foldSuccess)
                {
                    if(IsNewComponent())
                    {
                        EditorGUILayout.PropertyField(_reactions, new GUIContent(SurferHelper.kNoFoldout));
                    }
                    else
                    {
                        SurferHelper.DrawLine();
                        EditorGUILayout.PropertyField(_fastReactions, new GUIContent(CountSuccessFastR <=0 ? "Fast!" : "Fast! ("+CountSuccessFastR+")"));
                        EditorGUILayout.PropertyField(_reactions, new GUIContent(CountSuccessCustomR <=0 ? "Custom!" : "Custom! ("+CountSuccessCustomR+")"));
                        EditorGUILayout.PropertyField(_animations, new GUIContent(GetCountAnimations(_animations) <= 0 ? "Animated!" :  "Animated! ("+GetCountAnimations(_animations)+")"));
                        EditorGUILayout.PropertyField(_actions, new GUIContent(_actions.arraySize <=0 ? "Surfer's!" : "Surfer's! ("+_actions.arraySize+")"));
                    }
                    
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(_selectedBehaviour.FindPropertyRelative("OnSuccess"));
                    
                }
                else
                {

                    if(IsNewComponent())
                    {
                        EditorGUILayout.PropertyField(_failReactions, new GUIContent(SurferHelper.kNoFoldout));
                    }
                    else
                    {
                        SurferHelper.DrawLine();
                        EditorGUILayout.PropertyField(_fastFailReactions,new GUIContent(CountFailFastR <=0 ? "Fast!" : "Fast! ("+CountFailFastR+")"));
                        EditorGUILayout.PropertyField(_failReactions,  new GUIContent(CountFailCustomR <=0 ? "Custom!" : "Custom! ("+CountFailCustomR+")"));
                        EditorGUILayout.PropertyField(_failAnimations, new GUIContent(GetCountAnimations(_failAnimations) <= 0 ? "Animated!" :  "Animated! ("+GetCountAnimations(_failAnimations)+")"));
                        EditorGUILayout.PropertyField(_failActions, new GUIContent(_failActions.arraySize <=0 ? "Surfer's!" : "Surfer's! ("+_failActions.arraySize+")"));
                    }


                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(_selectedBehaviour.FindPropertyRelative("OnFail"));
                    
                }
            }

        }


        int GetCountAnimations(SerializedProperty property)
        {
            var counter = 0;

            if (property.FindPropertyRelative("_position").FindPropertyRelative("_startMode").enumValueIndex != 0)
                counter += 1;
            if (property.FindPropertyRelative("_anchoredPosition").FindPropertyRelative("_startMode").enumValueIndex != 0)
                counter += 1;
            if (property.FindPropertyRelative("_rotation").FindPropertyRelative("_startMode").enumValueIndex != 0)
                counter += 1;
            if (property.FindPropertyRelative("_scale").FindPropertyRelative("_startMode").enumValueIndex != 0)
                counter += 1;
            if (property.FindPropertyRelative("_color").FindPropertyRelative("_startMode").enumValueIndex != 0)
                counter += 1;
            if (property.FindPropertyRelative("_canvasGroup").FindPropertyRelative("_startMode").enumValueIndex != 0)
                counter += 1;
            if (property.FindPropertyRelative("_jump").FindPropertyRelative("_startMode").enumValueIndex != 0)
                counter += 1;


            if (property.FindPropertyRelative("_punch").FindPropertyRelative("_mode").enumValueIndex != 0)
                counter += 1;
            if (property.FindPropertyRelative("_rectSize").FindPropertyRelative("_mode").enumValueIndex != 0)
                counter += 1;
            if (property.FindPropertyRelative("_shake").FindPropertyRelative("_mode").enumValueIndex != 0)
                counter += 1;
            if (property.FindPropertyRelative("_charTweener").FindPropertyRelative("_mode").enumValueIndex != 0)
                counter += 1;

            return counter;

        }


        void Conditions()
        {

            _customConds = _selectedBehaviour.FindPropertyRelative("_customConds");

            if(!IsNewComponent())
                _fastConds = _selectedBehaviour.FindPropertyRelative("_fastConds");


            if (GUILayout.Button(_textIf, _style, GUILayout.Height(_btnHeight)))
            {
                _foldConditions = !_foldConditions;
            }


            int customCTotal = _customConds.FindPropertyRelative("_conditions").arraySize - 1;
            int fastCTotal = IsNewComponent() ? 0 : _fastConds.FindPropertyRelative("_conditions").arraySize - 1;

            if (!_foldConditions)
            {
                int tot = IsNewComponent() ? customCTotal : customCTotal + fastCTotal;
                tot = Mathf.Clamp(tot , 0, tot);

                _textIf = tot == 0 ? "IF!" : "IF ... ( " + tot + " )";
            }
            else
            {
                _textIf = "IF!";

                if(IsNewComponent())
                {
                    EditorGUILayout.PropertyField(_customConds, new GUIContent(SurferHelper.kNoFoldout));
                }
                else
                {
                    SurferHelper.DrawLine();
                    EditorGUILayout.PropertyField(_customConds, new GUIContent(customCTotal <= 0 ? "Custom!" : "Custom! ("+customCTotal+")"));
                    EditorGUILayout.PropertyField(_fastConds, new GUIContent(fastCTotal <= 0 ? "Fast!" : "Fast! (" + fastCTotal + ")"));
                }

                
            }

        }



        void RemoveEvent()
        {

            _behaviours.DeleteArrayElementAtIndex(_eventIdx);

            _eventIdx = Mathf.Clamp(_eventIdx - 1, 0, _behaviours.arraySize - 1);


        }

        void AddNewEvent()
        {

            _behaviours.InsertArrayElementAtIndex(Mathf.Clamp(_behaviours.arraySize - 1, 0, _behaviours.arraySize - 1));

            _eventIdx = _behaviours.arraySize - 1;


        }


        void UpdateDictionary()
        {

            _element.Events.Clear();

            SUBehavioursData _copy = JsonUtility.FromJson<SUBehavioursData>(JsonUtility.ToJson(_element.Behaviours));

            for (int i = 0; i < _copy.Behaviours.Count; i++)
            {
                var item = _copy.Behaviours[i];


                if (_element.Events.TryGetValue(item.EventType, out SUBehavioursData value))
                {
                    value.Behaviours.Add(item);
                }
                else
                {
                    var all = new SUBehavioursData();
                    all.Behaviours.Add(item);
                    _element.Events.Add(item.EventType, all);
                }
            }


            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                EditorSceneManager.MarkSceneDirty(_element.gameObject.scene);

            }
        }


        string[] GetEventsList()
        {

            List<string> eventNames = new List<string>();
            

            int counter = 1;


            for (int f = 0; f < _behaviours.arraySize; f++)
            {
                var eventData = _behaviours.GetArrayElementAtIndex(f).FindPropertyRelative("_event");
                var eventType = (SUEvent.Type_ID)eventData.FindPropertyRelative("_type").enumValueIndex;

                if(eventType == SUEvent.Type_ID.MyCustomEvent)
                {
                    var customEvents = eventData.FindPropertyRelative("_cEventsData");

                    if(customEvents == null)
                    continue;

                    customEvents = customEvents.FindPropertyRelative("_list");

                    if(customEvents.arraySize > 1)
                    {
                        var allEventNames = "";
                        
                        for(int i=0;i<customEvents.arraySize;i++)
                        {
                            var item = customEvents.GetArrayElementAtIndex(i);
                            var eventName = SurferHelper.SO.GetEvent(item.FindPropertyRelative("_guid").stringValue);

                            if(string.IsNullOrEmpty(eventName))
                            continue;

                            allEventNames += eventName;

                            if(i < customEvents.arraySize-2)
                            {
                                allEventNames += ",";
                            }
                        }

                        eventNames.Add(counter + ") " +allEventNames);

                    }
                    else
                    {
                        eventNames.Add(counter + ") CUSTOM EVENT NOT SELECTED! ");
                    }

                }
                else
                {
                    eventNames.Add(counter + ") " + (eventType).ToString().Replace("_", " -> "));
                }

                counter++;
            }
            return eventNames.ToArray();

        }

        /// <summary>
        /// If true the component in use is SUElement, otherwise is SUElementCanvas
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsNewComponent()
        {
            return false;
        }



    }


}







