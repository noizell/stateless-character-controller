using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Surfer
{
    [CustomPropertyDrawer(typeof(SUFastConditionData))]
    public class SUFastConditionDataDrawer : PropertyDrawer
    {
        SerializedProperty _conditionID, _stateData, _sceneData, _stringVal, _stringVal2,
            _cGroup, _toggle, _tmp,_tmpInput, _floatVal, _objVal, _intVal,
            _platform , _language, _slider, _dropdown = default;

        Rect _pos = default;
        SerializedProperty _mainProp = default;

        SUFastConditionData.Type_ID _selected
        {
            get
            {
                return (SUFastConditionData.Type_ID)_conditionID.enumValueIndex;
            }
        }



        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            if (property.propertyType == SerializedPropertyType.Generic)
            {

                //SurferHelper.DrawLine(ref position);
                position.height = EditorGUIUtility.singleLineHeight;
                GetPropertyRelatives(property);

                _conditionID.enumValueIndex = EditorGUI.Popup(position, !string.IsNullOrEmpty(label.text) ? "Condition" : label.text, _conditionID.enumValueIndex, GetReactionsList(property));


                _pos = position;
                _mainProp = property;
                AddFields();


            }
        }



        void GetPropertyRelatives(SerializedProperty property)
        {
            _conditionID = property.FindPropertyRelative("_conditionID");

            _stateData = property.FindPropertyRelative("_stateData");
            _sceneData = property.FindPropertyRelative("_sceneData");
            _stringVal = property.FindPropertyRelative("_stringVal");
            _stringVal2 = property.FindPropertyRelative("_stringVal2");
            _floatVal = property.FindPropertyRelative("_floatVal");
            _objVal = property.FindPropertyRelative("_objVal");
            _intVal = property.FindPropertyRelative("_intVal");

            _cGroup = property.FindPropertyRelative("_cGroup");
            _toggle = property.FindPropertyRelative("_toggle");
            _tmp = property.FindPropertyRelative("_tmp");
            _tmpInput = property.FindPropertyRelative("_tmpInput");

            _language = property.FindPropertyRelative("_language");
            _platform = property.FindPropertyRelative("_platform");

            _slider = property.FindPropertyRelative("_slider");
            _dropdown = property.FindPropertyRelative("_dropdown");
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            return SurferHelper.lineHeight * 2;

        }

        string[] GetReactionsList(SerializedProperty property)
        {

            string[] list = System.Enum.GetNames(typeof(SUFastConditionData.Type_ID));

            for (int i = 0; i < list.Length; i++)
            {
                list[i] = list[i].Replace("_", @"/");
            }

            return list;

        }


        #region Checks


        void AddFields()
        {

            switch ((SUFastConditionData.Type_ID)_conditionID.enumValueIndex)
            {

                case SUFastConditionData.Type_ID.CanvasGroup_IsAlpha:
                case SUFastConditionData.Type_ID.CanvasGroup_IsAlphaGreaterThan:
                case SUFastConditionData.Type_ID.CanvasGroup_IsNotAlpha:
                case SUFastConditionData.Type_ID.CanvasGroup_IsAlphaLowerThan:

                    _mainProp.AddField<CanvasGroup>(ref _pos,_cGroup);
                    _floatVal.AddValueField(ref _pos);

                    break;


                case SUFastConditionData.Type_ID.State_IsOpen:

                    _stateData.AddValueField(ref _pos, 1);
                    _intVal.AddValueField(ref _pos);

                    break;

                case SUFastConditionData.Type_ID.State_IsMyStateOpen:

                    _intVal.AddValueField(ref _pos,0, new GUIContent("Version"));

                    break;

                case SUFastConditionData.Type_ID.Scene_IsLastClosed:

                    _sceneData.AddValueField(ref _pos, 1);

                    break;

                case SUFastConditionData.Type_ID.Toggle_IsOff:
                case SUFastConditionData.Type_ID.Toggle_IsOn:

                    _mainProp.AddField<Toggle>(ref _pos, _toggle);

                    break;


                case SUFastConditionData.Type_ID.Text_IsEqual:
                case SUFastConditionData.Type_ID.Text_IsNotEqual:
                case SUFastConditionData.Type_ID.Text_IsNotNullOrEmpty:
                case SUFastConditionData.Type_ID.Text_IsNotValidEmailAddress:
                case SUFastConditionData.Type_ID.Text_IsNullOrEmpty:
                case SUFastConditionData.Type_ID.Text_IsValidEmailAddress:

                    _mainProp.AddField<TextMeshProUGUI>(ref _pos, _tmp);

                    break;

                case SUFastConditionData.Type_ID.InputField_IsEqual:
                case SUFastConditionData.Type_ID.InputField_IsNotEqual:
                case SUFastConditionData.Type_ID.InputField_IsNotNullOrEmpty:
                case SUFastConditionData.Type_ID.InputField_IsNotValidEmailAddress:
                case SUFastConditionData.Type_ID.InputField_IsNullOrEmpty:
                case SUFastConditionData.Type_ID.InputField_IsValidEmailAddress:

                    _mainProp.AddField<TMP_InputField>(ref _pos, _tmpInput);

                    break;

                case SUFastConditionData.Type_ID.GameObject_IsLayer:
                case SUFastConditionData.Type_ID.GameObject_IsNotLayer:

                    _mainProp.AddGOField(ref _pos, _objVal);
                    _intVal.AddIntList(ref _pos, SurferHelper.SO.Layers);

                    break;

                case SUFastConditionData.Type_ID.GameObject_IsNotTag:
                case SUFastConditionData.Type_ID.GameObject_IsTag:

                    _mainProp.AddGOField(ref _pos, _objVal);
                    _intVal.AddIntList(ref _pos, SurferHelper.SO.Tags);

                    break;


                case SUFastConditionData.Type_ID.GameObject_IsName:
                case SUFastConditionData.Type_ID.GameObject_IsNotName:

                    _mainProp.AddGOField(ref _pos, _objVal);
                    _stringVal.AddValueField(ref _pos);

                    break;


                case SUFastConditionData.Type_ID.Application_IsLanguage:
                case SUFastConditionData.Type_ID.Application_IsNotLanguage:

                    _language.AddValueField(ref _pos, 0, new GUIContent("Language"));

                    break;


                case SUFastConditionData.Type_ID.Application_IsPlatform:
                case SUFastConditionData.Type_ID.Application_IsNotPlatform:

                    _platform.AddValueField(ref _pos, 0, new GUIContent("Platform"));

                    break;


                case SUFastConditionData.Type_ID.Slider_IsMax:
                case SUFastConditionData.Type_ID.Slider_IsMin:

                    _mainProp.AddField<Slider>(ref _pos, _slider);

                    break;

                case SUFastConditionData.Type_ID.Slider_IsGreaterThan:
                case SUFastConditionData.Type_ID.Slider_IsLowerThan:

                    _mainProp.AddField<Slider>(ref _pos, _slider);
                    _floatVal.AddValueField(ref _pos);

                    break;


                case SUFastConditionData.Type_ID.Dropdown_IsSelected:

                    _mainProp.AddField<TMP_Dropdown>(ref _pos, _dropdown);
                    _intVal.AddValueField(ref _pos);

                    break;


                case SUFastConditionData.Type_ID.DateTime_IsDay:

                    _intVal.AddValueField(ref _pos,0,new GUIContent("Day"));

                    break;


                case SUFastConditionData.Type_ID.DateTime_IsMonth:

                    _intVal.AddValueField(ref _pos, 0, new GUIContent("Month"));

                    break;

                case SUFastConditionData.Type_ID.DateTime_IsYear:

                    _intVal.AddValueField(ref _pos, 0, new GUIContent("Year"));

                    break;



                case SUFastConditionData.Type_ID.PlayerPrefs_IsIntEqualTo:
                case SUFastConditionData.Type_ID.PlayerPrefs_IsIntGreaterThan:
                case SUFastConditionData.Type_ID.PlayerPrefs_IsIntLowerThan:

                    //if (string.IsNullOrEmpty(_stringVal.stringValue))
                    //    _stringVal.stringValue = "_myKey";

                    _stringVal.AddValueField(ref _pos,1);
                    _intVal.AddValueField(ref _pos,2);

                    break;

                case SUFastConditionData.Type_ID.PlayerPrefs_IsFloatEqualTo:
                case SUFastConditionData.Type_ID.PlayerPrefs_IsFloatGreaterThan:
                case SUFastConditionData.Type_ID.PlayerPrefs_IsFloatLowerThan:

                    _stringVal.AddValueField(ref _pos, 1);
                    _floatVal.AddValueField(ref _pos, 2);

                    break;


                case SUFastConditionData.Type_ID.PlayerPrefs_IsStringEqualTo:
                case SUFastConditionData.Type_ID.PlayerPrefs_StringContains:

                    _stringVal.AddValueField(ref _pos, 1);
                    _stringVal2.AddValueField(ref _pos, 2);

                    break;

                case SUFastConditionData.Type_ID.PlayerPrefs_HasKey:

                    _stringVal.AddValueField(ref _pos, 1);

                    break;


            }




        }



        #endregion

    }
}