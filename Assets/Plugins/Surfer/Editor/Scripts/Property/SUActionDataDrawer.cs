using UnityEditor;
using UnityEngine;


namespace Surfer
{

    [CustomPropertyDrawer(typeof(SUActionData))]
    public class SUActionDataDrawer : PropertyDrawer
    {

        SerializedProperty  _mode, _stateData, _conds = default;

        float _subHeight = default;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            
            EditorGUI.indentLevel += EditorGUI.indentLevel == 0 ? 0 : 1;
            EditorGUI.BeginProperty(position, GUIContent.none, property);


            position.height = SurferHelper.lineHeight;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded,string.IsNullOrEmpty(label.text) ? "Surfer Action" : label.text,true);

            GetProperties(property);

            if (property.isExpanded)
            {

                _stateData = property.FindPropertyRelative("_stateData");

                position.y += SurferHelper.lineHeight;


                EditorGUI.PropertyField(position,_mode);

                if(IsNone())
                return;


                position.height = EditorGUIUtility.singleLineHeight;
                position.y += SurferHelper.lineHeight;
                
                EditorGUI.PropertyField(position,_conds);


                if (IsStateMode())
                {
                    position.y += _subHeight;

                    EditorGUI.PropertyField(position, _stateData);

                    position.y += SurferHelper.lineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("_version"));


                    position.y += SurferHelper.lineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("_playerID"));

                }
                else if(IsPrefabStateMode())
                {

                    position.y += _subHeight;
                    EditorGUI.PropertyField(position,property.FindPropertyRelative("_prefab"));

                    position.y += SurferHelper.lineHeight;
                    EditorGUI.PropertyField(position,property.FindPropertyRelative("_parentMode"));
                    position.y += SurferHelper.lineHeight;
                    EditorGUI.PropertyField(position,property.FindPropertyRelative("_version"));
                    
                }
                else if (IsCloseMyState())
                {

                    position.y += _subHeight;
                    position.y -= SurferHelper.lineHeight;
                }
                else if (IsCustomEvent())
                {

                    position.y += _subHeight;

                    EditorGUI.PropertyField(position, property.FindPropertyRelative("_cEventData"));
                }
                else
                {
                    position.y += _subHeight;

                    EditorGUI.PropertyField(position,property.FindPropertyRelative("_sceneData"));
                    position.y += SurferHelper.lineHeight;
                    EditorGUI.PropertyField(position,_stateData,new GUIContent("Overlay State"));
                    position.y += SurferHelper.lineHeight;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("_version"));

                    if (!IsCloseScene())
                    {

                        if(!IsSceneActivation())
                        {
                            position.y += SurferHelper.lineHeight;
                            EditorGUI.PropertyField(position,property.FindPropertyRelative("_additive"));
                        }
                        

                        if(IsSceneAsync())
                        {
                            position.y += SurferHelper.lineHeight;
                            EditorGUI.PropertyField(position,property.FindPropertyRelative("_autoActivation"));
                        }
                        

                    }
                    
                    

                }
                


                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position,property.FindPropertyRelative("_delay"));
                position.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(position,property.FindPropertyRelative("_audioClip"));


            }

            
            EditorGUI.EndProperty();
        }



        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {


            int totShown = 0;

            GetProperties(property);

            if (property.isExpanded)
            {

                totShown+=1;

                if(!IsNone())
                {
                    totShown += 2;

                    if(IsStateMode() || IsCloseMyState())
                    {
                        totShown+=4;
                    }
                    else if(IsCustomEvent())
                    {
                        totShown += 2;
                    }
                    else 
                    {

                        totShown+=4;
                        
                        if(!IsCloseScene())
                        {

                            if(!IsSceneActivation())
                                totShown+=1;

                            if(IsSceneAsync())
                                totShown+=1;
                            
                        }
                    }
                }


                return (_subHeight + (totShown * SurferHelper.lineHeight));

            }
            else
            {
                
                return SurferHelper.lineHeight;
            }

            
        }

        void GetProperties(SerializedProperty property)
        {

            _mode = property.FindPropertyRelative("_mode");
            _conds = property.FindPropertyRelative("_conds");
            _subHeight = EditorGUI.GetPropertyHeight(_conds);
        }

        bool IsStateMode()
        {
            if(_mode.enumValueIndex == (int)SUActionData.SUAction_ID.OpenState
            || _mode.enumValueIndex == (int)SUActionData.SUAction_ID.CloseState
            || _mode.enumValueIndex == (int)SUActionData.SUAction_ID.ToggleState)
            return true;

            return false;
        }

        bool IsPrefabStateMode()
        {

            return _mode.enumValueIndex == (int)SUActionData.SUAction_ID.OpenPrefabState;
        }


        bool IsSceneAsync()
        {
            return _mode.enumValueIndex == (int)SUActionData.SUAction_ID.LoadSceneAsync;
        }

        bool IsNone()
        {
            return _mode.enumValueIndex == (int)SUActionData.SUAction_ID.None;
        }

        bool IsCloseMyState()
        {
            return _mode.enumValueIndex == (int)SUActionData.SUAction_ID.CloseMyState;
        }

        bool IsCustomEvent()
        {
            return _mode.enumValueIndex == (int)SUActionData.SUAction_ID.SendCustomEvent;
        }

        bool IsCloseScene()
        {
            return _mode.enumValueIndex == (int)SUActionData.SUAction_ID.UnloadSceneAsync;
        }

        bool IsSceneActivation()
        {
            return _mode.enumValueIndex == (int)SUActionData.SUAction_ID.SetActiveScene;
        }

    }

 }
 
