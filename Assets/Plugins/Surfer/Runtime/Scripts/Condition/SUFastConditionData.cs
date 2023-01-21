using System.Collections;
using System.Collections.Generic;
using Surfer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

namespace Surfer
{

    [System.Serializable]
    public class SUFastConditionData
    {

        public enum Type_ID
        {
            None,
            CanvasGroup_IsAlpha,
            CanvasGroup_IsNotAlpha,
            CanvasGroup_IsAlphaLowerThan,
            CanvasGroup_IsAlphaGreaterThan,
            State_IsMyStateOpen,
            State_IsOpen,
            Scene_IsLastClosed,
            Toggle_IsOn,
            Toggle_IsOff,
            Text_IsNullOrEmpty,
            Text_IsNotNullOrEmpty,
            Text_IsValidEmailAddress,
            Text_IsNotValidEmailAddress,
            Text_IsEqual,
            Text_IsNotEqual,
            InputField_IsNullOrEmpty,
            InputField_IsNotNullOrEmpty,
            InputField_IsValidEmailAddress,
            InputField_IsNotValidEmailAddress,
            InputField_IsEqual,
            InputField_IsNotEqual,
            GameObject_IsName,
            GameObject_IsNotName,
            GameObject_IsLayer,
            GameObject_IsNotLayer,
            GameObject_IsTag,
            GameObject_IsNotTag,
            //2.1
            Application_IsLanguage,
            Application_IsNotLanguage,
            Application_IsPlatform,
            Application_IsNotPlatform,
            Application_IsMobileDevice,
            Application_IsNotMobileDevice,
            Application_IsConsolePlatform,
            Application_IsNotConsolePlatform,
            Slider_IsMin,
            Slider_IsMax,
            Slider_IsGreaterThan,
            Slider_IsLowerThan,
            Dropdown_IsSelected,
            //2.2
            DateTime_IsDay,
            DateTime_IsMonth,
            DateTime_IsYear,
            DateTime_IsChristmas,
            DateTime_IsHalloween,
            //2.3
            PlayerPrefs_IsIntEqualTo,
            PlayerPrefs_IsIntLowerThan,
            PlayerPrefs_IsIntGreaterThan,
            PlayerPrefs_IsFloatEqualTo,
            PlayerPrefs_IsFloatLowerThan,
            PlayerPrefs_IsFloatGreaterThan,
            PlayerPrefs_IsStringEqualTo,
            PlayerPrefs_StringContains,
            PlayerPrefs_HasKey,


        }


        [SerializeField]
        Type_ID _conditionID = default;


        [SerializeField]
        float _floatVal = default;
        [SerializeField]
        SUStateData _stateData = default;
        [SerializeField]
        SUSceneData _sceneData = default;
        [SerializeField]
        string _stringVal = default;
        [SerializeField]
        string _stringVal2 = default;
        [SerializeField]
        GameObject _objVal = default;
        [SerializeField]
        int _intVal = default;
        [SerializeField]
        SystemLanguage _language = default;
        [SerializeField]
        RuntimePlatform _platform = default;



        [SerializeField]
        TMP_Dropdown _dropdown = default;
        [SerializeField]
        CanvasGroup _cGroup = default;
        [SerializeField]
        Toggle _toggle = default;
        [SerializeField]
        TextMeshProUGUI _tmp = default;
        [SerializeField]
        TMP_InputField _tmpInput = default;
        [SerializeField]
        Slider _slider = default;


        public bool IsSatisfied(GameObject caller)
        {

            switch(_conditionID)
            {

                case Type_ID.None:
                    return true;

                //canvas group

                case Type_ID.CanvasGroup_IsAlpha:

                    return _cGroup != null && _cGroup.alpha == _floatVal;


                case Type_ID.CanvasGroup_IsNotAlpha:

                    return _cGroup != null && _cGroup.alpha != _floatVal;

                case Type_ID.CanvasGroup_IsAlphaGreaterThan:

                    return _cGroup != null && _cGroup.alpha > _floatVal;


                case Type_ID.CanvasGroup_IsAlphaLowerThan:

                    return _cGroup != null && _cGroup.alpha < _floatVal;

                //state

                case Type_ID.State_IsMyStateOpen:

                    return SurferManager.I.IsMyStateOpen(caller,_intVal);

                case Type_ID.State_IsOpen:

                    return SurferManager.I.IsOpen(_stateData.Name,_intVal);

                //scene


                case Type_ID.Scene_IsLastClosed:

                    return SurferManager.I.LastUnloaded == _sceneData.Name;


                //toggle

                case Type_ID.Toggle_IsOff:

                    return _toggle != null && _toggle.isOn == false;


                case Type_ID.Toggle_IsOn:

                    return _toggle != null && _toggle.isOn == true;

                //text


                case Type_ID.Text_IsNullOrEmpty:

                    return _tmp != null && string.IsNullOrEmpty(_tmp.text);


                case Type_ID.Text_IsNotNullOrEmpty:

                    return _tmp != null && !string.IsNullOrEmpty(_tmp.text);

                case Type_ID.Text_IsNotValidEmailAddress:

                    return _tmp != null && !IsValidEmail(_tmp.text);


                case Type_ID.Text_IsValidEmailAddress:

                    return _tmp != null && IsValidEmail(_tmp.text);


                case Type_ID.Text_IsEqual:

                    return _tmp != null && _tmp.text == _stringVal;


                case Type_ID.Text_IsNotEqual:

                    return _tmp != null && _tmp.text != _stringVal;


                //input field

                case Type_ID.InputField_IsNullOrEmpty:

                    return _tmpInput != null && string.IsNullOrEmpty(_tmpInput.text);


                case Type_ID.InputField_IsNotNullOrEmpty:

                    return _tmpInput != null && !string.IsNullOrEmpty(_tmpInput.text);

                case Type_ID.InputField_IsNotValidEmailAddress:

                    return _tmpInput != null && !IsValidEmail(_tmpInput.text);


                case Type_ID.InputField_IsValidEmailAddress:

                    return _tmpInput != null && IsValidEmail(_tmpInput.text);


                case Type_ID.InputField_IsEqual:

                    return _tmpInput != null && _tmpInput.text == _stringVal;


                case Type_ID.InputField_IsNotEqual:

                    return _tmpInput != null && _tmpInput.text != _stringVal;

                //gameobject

                case Type_ID.GameObject_IsLayer:

                    return _objVal != null && LayerMask.LayerToName(_objVal.layer).Equals(SurferHelper.SO.Layers[_intVal]);


                case Type_ID.GameObject_IsName:

                    return _objVal != null && _objVal.name.Equals(_stringVal);

                case Type_ID.GameObject_IsNotLayer:

                    return _objVal != null && !LayerMask.LayerToName(_objVal.layer).Equals(SurferHelper.SO.Layers[_intVal]);


                case Type_ID.GameObject_IsNotName:

                    return _objVal != null && !_objVal.name.Equals(_stringVal);


                case Type_ID.GameObject_IsNotTag:

                    return _objVal != null && !_objVal.tag.Equals(SurferHelper.SO.Tags[_intVal]);


                case Type_ID.GameObject_IsTag:

                    return _objVal != null && _objVal.tag.Equals(SurferHelper.SO.Tags[_intVal]);


                //application

                case Type_ID.Application_IsConsolePlatform:

                    return Application.isConsolePlatform;

                case Type_ID.Application_IsLanguage:

                    return Application.systemLanguage == _language;

                case Type_ID.Application_IsMobileDevice:

                    return Application.isMobilePlatform;

                case Type_ID.Application_IsNotConsolePlatform:

                    return !Application.isConsolePlatform;

                case Type_ID.Application_IsNotLanguage:

                    return Application.systemLanguage != _language;

                case Type_ID.Application_IsNotMobileDevice:

                    return !Application.isMobilePlatform;

                case Type_ID.Application_IsNotPlatform:

                    return Application.platform != _platform;

                case Type_ID.Application_IsPlatform:

                    return Application.platform == _platform;


                //slider

                case Type_ID.Slider_IsGreaterThan:

                    return _slider.value > _floatVal;

                case Type_ID.Slider_IsLowerThan:

                    return _slider.value < _floatVal;

                case Type_ID.Slider_IsMax:

                    return Mathf.Abs(_slider.maxValue - _slider.value) < Mathf.Epsilon;

                case Type_ID.Slider_IsMin:

                    return Mathf.Abs(_slider.minValue - _slider.value) < Mathf.Epsilon;


                //dropdown

                case Type_ID.Dropdown_IsSelected:

                    return _dropdown.value == _intVal;


                //datetime

                case Type_ID.DateTime_IsChristmas:

                    return DateTime.Now.Day == 25 && DateTime.Now.Month == 12;

                case Type_ID.DateTime_IsDay:

                    return DateTime.Now.Day == _intVal;

                case Type_ID.DateTime_IsHalloween:

                    return DateTime.Now.Day == 31 && DateTime.Now.Month == 10;

                case Type_ID.DateTime_IsMonth:

                    return DateTime.Now.Month == _intVal;

                case Type_ID.DateTime_IsYear:

                    return DateTime.Now.Year == _intVal;


                //player prefs

                case Type_ID.PlayerPrefs_HasKey:

                    return PlayerPrefs.HasKey(_stringVal);

                case Type_ID.PlayerPrefs_IsFloatEqualTo:

                    return Mathf.Abs(PlayerPrefs.GetFloat(_stringVal) - _floatVal) < Mathf.Epsilon;


                case Type_ID.PlayerPrefs_IsFloatGreaterThan:

                    return PlayerPrefs.GetFloat(_stringVal) > _floatVal;


                case Type_ID.PlayerPrefs_IsFloatLowerThan:

                    return PlayerPrefs.GetFloat(_stringVal) < _floatVal;


                case Type_ID.PlayerPrefs_IsIntEqualTo:

                    return Mathf.Abs(PlayerPrefs.GetInt(_stringVal) - _intVal) < Mathf.Epsilon;


                case Type_ID.PlayerPrefs_IsIntGreaterThan:

                    return PlayerPrefs.GetInt(_stringVal) > _intVal;


                case Type_ID.PlayerPrefs_IsIntLowerThan:

                    return PlayerPrefs.GetInt(_stringVal) < _intVal;


                case Type_ID.PlayerPrefs_IsStringEqualTo:

                    return PlayerPrefs.GetString(_stringVal).Equals(_stringVal2);

                case Type_ID.PlayerPrefs_StringContains:

                    return PlayerPrefs.GetString(_stringVal).Contains(_stringVal2);


            }

            return false;

        }


        bool IsValidEmail(string email)
        {
            string pattern = @"^\s*[\w\-\+_']+(\.[\w\-\+_']+)*\@[A-Za-z0-9]([\w\.-]*[A-Za-z0-9])?\.[A-Za-z][A-Za-z\.]*[A-Za-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }



    }




}



