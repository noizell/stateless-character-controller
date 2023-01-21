using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer
{


    public static class DefaultConditionChecks
    {

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void GetUnifiedNames()
        {
            UnifiedNames = GetAllNames().Union(ConditionChecks.GetAllNames()).ToArray();

        }
#endif


        public static string[] UnifiedNames { get; private set; } = default;

        public readonly static Dictionary<string, PathFunc> All = new Dictionary<string, PathFunc>()
        {

            //canvas group
            {"_SUcg1",
            new PathFunc("CanvasGroup/IsAlpha",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
                new PathField("Value",PathFieldType_ID.Float_1)
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null
                && (input.fieldsValues.Object_1 as CanvasGroup).alpha == input.fieldsValues.Float_1;

            })},

            {"_SUcg2",
            new PathFunc("CanvasGroup/IsNotAlpha",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
                new PathField("Value",PathFieldType_ID.Float_1)
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null
                && (input.fieldsValues.Object_1 as CanvasGroup).alpha != input.fieldsValues.Float_1;

            })},

            {"_SUcg3",
            new PathFunc("CanvasGroup/IsAlphaGreaterThan",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
                new PathField("Value",PathFieldType_ID.Float_1)
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null
                && (input.fieldsValues.Object_1 as CanvasGroup).alpha > input.fieldsValues.Float_1;

            })},

            {"_SUcg4",
            new PathFunc("CanvasGroup/IsAlphaLowerThan",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
                new PathField("Value",PathFieldType_ID.Float_1)
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null
                && (input.fieldsValues.Object_1 as CanvasGroup).alpha < input.fieldsValues.Float_1;

            })},


            //state

            {"_SUs1",
            new PathFunc("State/IsMyStateOpen",
            new List<PathField>()
            {
                new PathField("Version",PathFieldType_ID.Int_1)
            },
            (input)=>
            {

                return SurferManager.I.IsMyStateOpen(input.gameObj,input.fieldsValues.Int_1);

            })},

            {"_SUs2",
            new PathFunc("State/IsOpen",
            new List<PathField>()
            {
                new PathField("State","_stateData"),
                new PathField("Version",PathFieldType_ID.Int_1)
            },
            (input)=>
            {

                return SurferManager.I.IsOpen(input.conditionData.StateData.Name,input.fieldsValues.Int_1);

            })},

            //scene

            {"_SUsc1",
            new PathFunc("Scene/IsLastClosed",
            new List<PathField>()
            {
                new PathField("State","_sceneData"),
            },
            (input)=>
            {

                return SurferManager.I.LastUnloaded == input.conditionData.SceneData.Name;

            })},


            //toggle

            {"_SUto1",
            new PathFunc("Toggle/IsOff",
            new List<PathField>()
            {
                new PathField("Toggle",PathFieldType_ID.Object_1,typeof(Toggle)),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && (input.fieldsValues.Object_1 as Toggle).isOn == false;

            })},


            {"_SUto2",
            new PathFunc("Toggle/IsOn",
            new List<PathField>()
            {
                new PathField("Toggle",PathFieldType_ID.Object_1,typeof(Toggle)),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && (input.fieldsValues.Object_1 as Toggle).isOn == true;

            })},

            //text mesh pro

            {"_SUte1",
            new PathFunc("TMProUGUI/IsNullOrEmpty",
            new List<PathField>()
            {
                new PathField("Tmp",PathFieldType_ID.Object_1,typeof(TextMeshProUGUI)),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && string.IsNullOrEmpty((input.fieldsValues.Object_1 as TextMeshProUGUI).text);

            })},

            {"_SUte2",
            new PathFunc("TMProUGUI/IsNotNullOrEmpty",
            new List<PathField>()
            {
                new PathField("Tmp",PathFieldType_ID.Object_1,typeof(TextMeshProUGUI)),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && !string.IsNullOrEmpty((input.fieldsValues.Object_1 as TextMeshProUGUI).text);

            })},

            {"_SUte3",
            new PathFunc("TMProUGUI/IsNotValidEmailAddress",
            new List<PathField>()
            {
                new PathField("Tmp",PathFieldType_ID.Object_1,typeof(TextMeshProUGUI)),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && !IsValidEmail((input.fieldsValues.Object_1 as TextMeshProUGUI).text);

            })},


            {"_SUte4",
            new PathFunc("TMProUGUI/IsValidEmailAddress",
            new List<PathField>()
            {
                new PathField("Tmp",PathFieldType_ID.Object_1,typeof(TextMeshProUGUI)),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && IsValidEmail((input.fieldsValues.Object_1 as TextMeshProUGUI).text);

            })},


            {"_SUte5",
            new PathFunc("TMProUGUI/IsEqual",
            new List<PathField>()
            {
                new PathField("Tmp",PathFieldType_ID.Object_1,typeof(TextMeshProUGUI)),
                new PathField("Value",PathFieldType_ID.String_1),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && (input.fieldsValues.Object_1 as TextMeshProUGUI).text == input.fieldsValues.String_1;

            })},

            {"_SUte6",
            new PathFunc("TMProUGUI/IsNotEqual",
            new List<PathField>()
            {
                new PathField("Tmp",PathFieldType_ID.Object_1,typeof(TextMeshProUGUI)),
                new PathField("Value",PathFieldType_ID.String_1),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && (input.fieldsValues.Object_1 as TextMeshProUGUI).text != input.fieldsValues.String_1;

            })},

            //input field

            {"_SUin1",
            new PathFunc("InputField/IsNullOrEmpty",
            new List<PathField>()
            {
                new PathField("InputField",PathFieldType_ID.Object_1,typeof(TMP_InputField)),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && string.IsNullOrEmpty((input.fieldsValues.Object_1 as TMP_InputField).text);

            })},


            {"_SUin2",
            new PathFunc("InputField/IsNotNullOrEmpty",
            new List<PathField>()
            {
                new PathField("InputField",PathFieldType_ID.Object_1,typeof(TMP_InputField)),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && !string.IsNullOrEmpty((input.fieldsValues.Object_1 as TMP_InputField).text);

            })},


            {"_SUin3",
            new PathFunc("InputField/IsNotValidEmailAddress",
            new List<PathField>()
            {
                new PathField("InputField",PathFieldType_ID.Object_1,typeof(TMP_InputField)),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && !IsValidEmail((input.fieldsValues.Object_1 as TMP_InputField).text);

            })},


            {"_SUin4",
            new PathFunc("InputField/IsValidEmailAddress",
            new List<PathField>()
            {
                new PathField("InputField",PathFieldType_ID.Object_1,typeof(TMP_InputField)),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && IsValidEmail((input.fieldsValues.Object_1 as TMP_InputField).text);

            })},


            {"_SUin5",
            new PathFunc("InputField/IsEqual",
            new List<PathField>()
            {
                new PathField("InputField",PathFieldType_ID.Object_1,typeof(TMP_InputField)),
                new PathField("Value",PathFieldType_ID.String_1),

            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && (input.fieldsValues.Object_1 as TMP_InputField).text == input.fieldsValues.String_1;


            })},

            {"_SUin6",
            new PathFunc("InputField/IsNotEqual",
            new List<PathField>()
            {
                new PathField("InputField",PathFieldType_ID.Object_1,typeof(TMP_InputField)),
                new PathField("Value",PathFieldType_ID.String_1),

            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null 
                && (input.fieldsValues.Object_1 as TMP_InputField).text != input.fieldsValues.String_1;


            })},


            // gameobject

            {"_SUgo1",
            new PathFunc("GameObject/IsLayer",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("Layer",PathFieldType_ID.CustomChoices_1,SurferHelper.SO.Layers)
            },
            (input)=>
            {

                if(input.fieldsValues.Object_1 == null)
                return false;

                var go = input.fieldsValues.Object_1 as GameObject;

                return LayerMask.LayerToName(go.layer).Equals(SurferHelper.SO.Layers[input.fieldsValues.CustomChoices_1]);

            })},


            {"_SUgo2",
            new PathFunc("GameObject/IsName",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("Name",PathFieldType_ID.String_1)
            },
            (input)=>
            {

                if(input.fieldsValues.Object_1 == null)
                return false;

                var go = input.fieldsValues.Object_1 as GameObject;

                return go.name.Equals(input.fieldsValues.String_1);

            })},


            {"_SUgo3",
            new PathFunc("GameObject/IsNotLayer",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("Layer",PathFieldType_ID.CustomChoices_1,SurferHelper.SO.Layers)
            },
            (input)=>
            {

                if(input.fieldsValues.Object_1 == null)
                return false;

                var go = input.fieldsValues.Object_1 as GameObject;

                return !LayerMask.LayerToName(go.layer).Equals(SurferHelper.SO.Layers[input.fieldsValues.CustomChoices_1]);

            })},


             {"_SUgo4",
            new PathFunc("GameObject/IsNotName",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("Name",PathFieldType_ID.String_1)
            },
            (input)=>
            {

                if(input.fieldsValues.Object_1 == null)
                return false;

                var go = input.fieldsValues.Object_1 as GameObject;

                return !go.name.Equals(input.fieldsValues.String_1);

            })},


            {"_SUgo5",
            new PathFunc("GameObject/IsNotTag",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("Tag",PathFieldType_ID.CustomChoices_1,SurferHelper.SO.Tags)
            },
            (input)=>
            {

                if(input.fieldsValues.Object_1 == null)
                return false;

                var go = input.fieldsValues.Object_1 as GameObject;

                return !go.tag.Equals(SurferHelper.SO.Tags[input.fieldsValues.CustomChoices_1]);

            })},


            {"_SUgo6",
            new PathFunc("GameObject/IsTag",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("Tag",PathFieldType_ID.CustomChoices_1,SurferHelper.SO.Tags)
            },
            (input)=>
            {

                if(input.fieldsValues.Object_1 == null)
                return false;

                var go = input.fieldsValues.Object_1 as GameObject;

                return go.tag.Equals(SurferHelper.SO.Tags[input.fieldsValues.CustomChoices_1]);

            })},

            


            //application
            {"_SUap1",
            new PathFunc("Application/IsConsolePlatform",
            (input)=>
            {

                return Application.isConsolePlatform;

            })},

            {"_SUap2",
            new PathFunc("Application/IsLanguage",
            new List<PathField>()
            {
                new PathField("Language",PathFieldType_ID.Enum_1,typeof(SystemLanguage)),
            },
            (input)=>
            {

                return Application.systemLanguage == (SystemLanguage)input.fieldsValues.Enum_1;

            })},

            {"_SUap3",
            new PathFunc("Application/IsMobileDevice",
            (input)=>
            {

                    return Application.isMobilePlatform;

            })},

            {"_SUap4",
            new PathFunc("Application/IsNotConsolePlatform",
            (input)=>
            {

                return !Application.isConsolePlatform;

            })},

            {"_SUap5",
            new PathFunc("Application/IsNotLanguage",
            new List<PathField>()
            {
                new PathField("Language",PathFieldType_ID.Enum_1,typeof(SystemLanguage)),
            },
            (input)=>
            {

                return Application.systemLanguage != (SystemLanguage)input.fieldsValues.Enum_1;

            })},


            {"_SUap6",
            new PathFunc("Application/IsNotMobileDevice",
            (input)=>
            {

                    return !Application.isMobilePlatform;

            })},


            {"_SUap7",
            new PathFunc("Application/IsNotPlatform",
            new List<PathField>()
            {
                new PathField("Platform",PathFieldType_ID.Enum_1,typeof(RuntimePlatform)),
            },
            (input)=>
            {

                return Application.platform != (RuntimePlatform)input.fieldsValues.Enum_1;

            })},


            {"_SUap8",
            new PathFunc("Application/IsPlatform",
            new List<PathField>()
            {
                new PathField("Platform",PathFieldType_ID.Enum_1,typeof(RuntimePlatform)),
            },
            (input)=>
            {

                return Application.platform == (RuntimePlatform)input.fieldsValues.Enum_1;

            })},



            //slider

            {"_SUsl1",
            new PathFunc("Slider/IsGreaterThan",
            new List<PathField>()
            {
                new PathField("Slider",PathFieldType_ID.Object_1,typeof(Slider)),
                new PathField("Value",PathFieldType_ID.Float_1)
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null
                && (input.fieldsValues.Object_1 as Slider).value > input.fieldsValues.Float_1;

            })},


            {"_SUsl2",
            new PathFunc("Slider/IsLowerThan",
            new List<PathField>()
            {
                new PathField("Slider",PathFieldType_ID.Object_1,typeof(Slider)),
                new PathField("Value",PathFieldType_ID.Float_1)
            },
            (input)=>
            {
                
                return input.fieldsValues.Object_1 != null
                && (input.fieldsValues.Object_1 as Slider).value < input.fieldsValues.Float_1;

            })},


            {"_SUsl3",
            new PathFunc("Slider/IsMax",
            new List<PathField>()
            {
                new PathField("Slider",PathFieldType_ID.Object_1,typeof(Slider)),
            },
            (input)=>
            {

                if(input.fieldsValues.Object_1 == null)
                return false;

                var slid = (input.fieldsValues.Object_1 as Slider);

                return Mathf.Abs(slid.maxValue - slid.value) < Mathf.Epsilon;

            })},


            {"_SUsl4",
            new PathFunc("Slider/IsMin",
            new List<PathField>()
            {
                new PathField("Slider",PathFieldType_ID.Object_1,typeof(Slider)),
            },
            (input)=>
            {

                if(input.fieldsValues.Object_1 == null)
                return false;

                var slid = (input.fieldsValues.Object_1 as Slider);

                return Mathf.Abs(slid.minValue - slid.value) < Mathf.Epsilon;

            })},

            //dropdown

            {"_SUdr1",
            new PathFunc("Dropdown/IsSelected",
            new List<PathField>()
            {
                new PathField("Dropdown",PathFieldType_ID.Object_1,typeof(TMP_Dropdown)),
                new PathField("Index",PathFieldType_ID.Int_1),
            },
            (input)=>
            {

                return input.fieldsValues.Object_1 != null
                && (input.fieldsValues.Object_1 as TMP_Dropdown).value == input.fieldsValues.Int_1 ;

            })},


            //date time

             {"_SUda1",
            new PathFunc("DateTime/IsChristmas",
            (input)=>
            {

                return DateTime.Now.Day == 25 && DateTime.Now.Month == 12;

            })},


            {"_SUda2",
            new PathFunc("DateTime/IsDay",
            new List<PathField>()
            {
                new PathField("Day",PathFieldType_ID.Int_1),
            },
            (input)=>
            {

                return DateTime.Now.Day == input.fieldsValues.Int_1;

            })},


             {"_SUda3",
            new PathFunc("DateTime/IsHalloween",
            (input)=>
            {

                return DateTime.Now.Day == 31 && DateTime.Now.Month == 10;

            })},

            {"_SUda4",
            new PathFunc("DateTime/IsMonth",
            new List<PathField>()
            {
                new PathField("Month",PathFieldType_ID.Int_1),
            },
            (input)=>
            {

                return DateTime.Now.Month == input.fieldsValues.Int_1;

            })},

            {"_SUda5",
            new PathFunc("DateTime/IsYear",
            new List<PathField>()
            {
                new PathField("Year",PathFieldType_ID.Int_1),
            },
            (input)=>
            {

                return DateTime.Now.Year == input.fieldsValues.Int_1;

            })},


            //playerprefs
            {"_SUpl1",
            new PathFunc("PlayerPrefs/HasKey",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
            },
            (input)=>
            {

                return PlayerPrefs.HasKey(input.fieldsValues.String_1);

            })},

            {"_SUpl2",
            new PathFunc("PlayerPrefs/IsFloatEqualTo",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
                new PathField("Value",PathFieldType_ID.Float_1),
            },
            (input)=>
            {

                return Mathf.Abs(PlayerPrefs.GetFloat(input.fieldsValues.String_1) - input.fieldsValues.Float_1) < Mathf.Epsilon;

            })},


            {"_SUpl3",
            new PathFunc("PlayerPrefs/IsFloatGreaterThan",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
                new PathField("Value",PathFieldType_ID.Float_1),
            },
            (input)=>
            {

                return PlayerPrefs.GetFloat(input.fieldsValues.String_1) > input.fieldsValues.Float_1;

            })},


            {"_SUpl4",
            new PathFunc("PlayerPrefs/IsFloatLowerThan",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
                new PathField("Value",PathFieldType_ID.Float_1),
            },
            (input)=>
            {

                return PlayerPrefs.GetFloat(input.fieldsValues.String_1) < input.fieldsValues.Float_1;

            })},


            {"_SUpl5",
            new PathFunc("PlayerPrefs/IsIntEqualTo",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
                new PathField("Value",PathFieldType_ID.Int_1)
            },
            (input)=>
            {

                return Mathf.Abs(PlayerPrefs.GetInt(input.fieldsValues.String_1) - input.fieldsValues.Int_1) < Mathf.Epsilon;

            })},


            {"_SUpl6",
            new PathFunc("PlayerPrefs/IsIntGreaterThan",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
                new PathField("Value",PathFieldType_ID.Int_1)
            },
            (input)=>
            {
                return PlayerPrefs.GetInt(input.fieldsValues.String_1) > input.fieldsValues.Int_1;
            })},



             {"_SUpl7",
            new PathFunc("PlayerPrefs/IsIntLowerThan",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
                new PathField("Value",PathFieldType_ID.Int_1)
            },
            (input)=>
            {

                return PlayerPrefs.GetInt(input.fieldsValues.String_1) < input.fieldsValues.Int_1;

            })},


            {"_SUpl8",
            new PathFunc("PlayerPrefs/IsStringEqualTo",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
                new PathField("Value",PathFieldType_ID.String_2)
            },
            (input)=>
            {

                return PlayerPrefs.GetString(input.fieldsValues.String_1).Equals(input.fieldsValues.String_2);


            })},


             {"_SUpl9",
            new PathFunc("PlayerPrefs/StringContains",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
                new PathField("Value",PathFieldType_ID.String_2)
            },
            (input)=>
            {

                return PlayerPrefs.GetString(input.fieldsValues.String_1).Contains(input.fieldsValues.String_2);


            })},

        };


        /// <summary>
        /// Get all the names/paths of all the conditions both Custom and Default. Used for the inspector
        /// </summary>
        /// <returns>Names/paths list</returns>
        public static string[] GetAllNames()
        {
            return All.Select(x=>x.Value.Path).OrderBy(x=>x).Prepend(SurferHelper.Unset).ToArray();
        }


        /// <summary>
        /// Get the name/path of a specific condition. Used for the inspector
        /// </summary>
        /// <param name="key">Condition key to retrieve the name/path</param>
        /// <returns>Condition name/path</returns>
        public static string GetName(string key)
        {
            if(All.TryGetValue(key,out PathFunc value))
                return value.Path;

            return "";
        }

        /// <summary>
        /// Get the key of a specific condition. Used for the inspector
        /// </summary>
        /// <param name="path">Condition name/path to retrieve the key</param>
        /// <returns>Condition key</returns>
        public static string GetKey(string path)
        {
            foreach(KeyValuePair<string,PathFunc> pair in All)
            {
                if(pair.Value.Path.Equals(path))
                return pair.Key;
            }
            return "";
        }


        static bool IsValidEmail(string email)
        {
            string pattern = @"^\s*[\w\-\+_']+(\.[\w\-\+_']+)*\@[A-Za-z0-9]([\w\.-]*[A-Za-z0-9])?\.[A-Za-z][A-Za-z\.]*[A-Za-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }



    }


}

