using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Surfer.SUActionData;

namespace Surfer
{


    public static class DefaultCustomReactions
    {

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void GetUnifiedNames()
        {

            UnifiedNames = GetAllNames().Union(CustomReactions.GetAllNames()).ToArray();

        }
#endif

        public static string[] UnifiedNames { get; private set; } = default;

        public const string kSetAnchoredPosition = "_SUtr2";
        public const string kGODisable = "_SUgo2";
        public const string kGOEnable = "_SUgo3";
        public const string kOpenState = "_SUsu1";

        public readonly static Dictionary<string,PathAction> All = new Dictionary<string, PathAction>()
        {

            //canvas group
            {"_SUcg1",
            new PathAction("CanvasGroup/Alpha1",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                (input.fieldsValues.Object_1 as CanvasGroup).alpha = 1;

            })},

            {"_SUcg2",
            new PathAction("CanvasGroup/Alpha0",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as CanvasGroup).alpha = 0;

            })},

             {"_SUcg3",
            new PathAction("CanvasGroup/IgnoreParentGroupsOff",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as CanvasGroup).ignoreParentGroups = false;

            })},

             {"_SUcg4",
            new PathAction("CanvasGroup/IgnoreParentGroupsOn",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as CanvasGroup).ignoreParentGroups = true;

            })},



             {"_SUcg5",
            new PathAction("CanvasGroup/InteractableOff",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as CanvasGroup).interactable = false;

            })},


             {"_SUcg6",
            new PathAction("CanvasGroup/InteractableOn",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as CanvasGroup).interactable = true;

            })},


             {"_SUcg7",
            new PathAction("CanvasGroup/BlockRaycastOff",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as CanvasGroup).blocksRaycasts = false;

            })},


            {"_SUcg8",
            new PathAction("CanvasGroup/BlockRaycastOn",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as CanvasGroup).blocksRaycasts = true;

            })},


            {"_SUcg9",
            new PathAction("CanvasGroup/SetAlpha",
            new List<PathField>()
            {
                new PathField("CanvasGroup",PathFieldType_ID.Object_1,typeof(CanvasGroup)),
                new PathField("Value",PathFieldType_ID.Float_1)
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as CanvasGroup).alpha = input.fieldsValues.Float_1;

            })},



            //gameobject

            {"_SUgo1",
            new PathAction("GameObject/SetLayer",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("Layer",PathFieldType_ID.CustomChoices_1,SurferHelper.SO.Layers)
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as GameObject).layer 
                = LayerMask.NameToLayer(SurferHelper.SO.Layers[input.fieldsValues.CustomChoices_1]);


            })},

            {kGODisable,
            new PathAction("GameObject/Disable",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as GameObject).SetActive(false);


            })},


            {kGOEnable,
            new PathAction("GameObject/Enable",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as GameObject).SetActive(true);


            })},


            {"_SUgo4",
            new PathAction("GameObject/SetTag",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("Tags",PathFieldType_ID.CustomChoices_1,SurferHelper.SO.Tags)

            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as GameObject).tag 
                = SurferHelper.SO.Tags[input.fieldsValues.CustomChoices_1];


            })},


            

            {"_SUgo5",
            new PathAction("GameObject/SetName",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("Name",PathFieldType_ID.String_1)

            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as GameObject).name = 
                input.fieldsValues.String_1;

            })},



            //selectable

            {"_SUse1",
            new PathAction("Selectable/InteractableOff",
            new List<PathField>()
            {
                new PathField("Selectable",PathFieldType_ID.Object_1,typeof(Selectable)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as Selectable).interactable = false ;


            })},
            

            {"_SUse2",
            new PathAction("Selectable/InteractableOn",
            new List<PathField>()
            {
                new PathField("Selectable",PathFieldType_ID.Object_1,typeof(Selectable)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as Selectable).interactable = true ;


            })},


            //graphic

            {"_SUgr1",
            new PathAction("Graphic/RaycastOn",
            new List<PathField>()
            {
                new PathField("Graphic",PathFieldType_ID.Object_1,typeof(Graphic)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as Graphic).raycastTarget = true;


            })},

            {"_SUgr2",
            new PathAction("Graphic/RaycastOff",
            new List<PathField>()
            {
                new PathField("Graphic",PathFieldType_ID.Object_1,typeof(Graphic)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as Graphic).raycastTarget = false;


            })},

            {"_SUgr3",
            new PathAction("Graphic/SetColor",
            new List<PathField>()
            {
                new PathField("Graphic",PathFieldType_ID.Object_1,typeof(Graphic)),
                new PathField("Color",PathFieldType_ID.Color_1),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                (input.fieldsValues.Object_1 as Graphic).color = input.fieldsValues.Color_1;


            })},


            //ui generics
            {"_SUui1",
            new PathAction("UIGenerics/FocusOnObject",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                SUEventSystemManager.I.FocusOnObject((input.fieldsValues.Object_1 as GameObject));

            })},


            {"_SUui2",
            new PathAction("UIGenerics/FocusOnObjectOrStateLast",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;


                    SUEventSystemManager.I.FocusOnObjectOrLast((input.fieldsValues.Object_1 as GameObject));

            })},


            {"_SUui3",
            new PathAction("UIGenerics/EnableClickConstraint",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                var cg = obj.GetComponent<CanvasGroup>();

                    if (cg == null)
                        cg = obj.AddComponent<CanvasGroup>();

                    cg.interactable = true;
                    cg.blocksRaycasts = true;
                    cg.ignoreParentGroups = true;

                    var stateTf = SurferManager.I.GetObjectStateTransfom(obj);

                    cg = stateTf.GetComponent<CanvasGroup>();

                    if (cg == null)
                        cg = stateTf.gameObject.AddComponent<CanvasGroup>();


                    cg.interactable = false;
                    cg.blocksRaycasts = true;
                    cg.ignoreParentGroups = true;

            })},


             {"_SUui4",
            new PathAction("UIGenerics/DisableClickConstraint",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                var cgg = obj.GetComponent<CanvasGroup>();

                if (cgg == null)
                    return;

                cgg.ignoreParentGroups = false;


            })},


            //audio

            {"_SUau1",
            new PathAction("Audio/PlayClip",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("AudioClip",PathFieldType_ID.Object_2,typeof(AudioClip)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;
                if(input.fieldsValues.Object_2 == null)
                return;

                SurferHelper.PlaySound((input.fieldsValues.Object_2 as AudioClip), 
                (input.fieldsValues.Object_1 as GameObject));


            })},

            {"_SUau2",
            new PathAction("Audio/PlaySource",
            new List<PathField>()
            {
                new PathField("AudioSource",PathFieldType_ID.Object_1,typeof(AudioSource)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                (input.fieldsValues.Object_1 as AudioSource).Play();

            })},


            //toggle

            {"_SUto1",
            new PathAction("Toggle/True",
            new List<PathField>()
            {
                new PathField("Toggle",PathFieldType_ID.Object_1,typeof(Toggle)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                (input.fieldsValues.Object_1 as Toggle).isOn = true;

            })},


             {"_SUto2",
            new PathAction("Toggle/False",
            new List<PathField>()
            {
                new PathField("Toggle",PathFieldType_ID.Object_1,typeof(Toggle)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                (input.fieldsValues.Object_1 as Toggle).isOn = false;

            })},

             {"_SUto3",
            new PathAction("Toggle/ToggleValue",
            new List<PathField>()
            {
                new PathField("Toggle",PathFieldType_ID.Object_1,typeof(Toggle)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                (input.fieldsValues.Object_1 as Toggle).isOn =
                !(input.fieldsValues.Object_1 as Toggle).isOn;

            })},


            //textmeshpro

             {"_SUte1",
            new PathAction("Text/Empty",
            new List<PathField>()
            {
                new PathField("Tmp",PathFieldType_ID.Object_1,typeof(TextMeshProUGUI)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                (input.fieldsValues.Object_1 as TextMeshProUGUI).text = string.Empty;

            })},

             {"_SUte2",
            new PathAction("Text/SetText",
            new List<PathField>()
            {
                new PathField("Tmp",PathFieldType_ID.Object_1,typeof(TextMeshProUGUI)),
                new PathField("Value",PathFieldType_ID.String_1)
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                (input.fieldsValues.Object_1 as TextMeshProUGUI).text =
                input.fieldsValues.String_1;

            })},

            
            //inputfield

             {"_SUin1",
            new PathAction("InputField/Empty",
            new List<PathField>()
            {
                new PathField("InputField",PathFieldType_ID.Object_1,typeof(TMP_InputField)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                (input.fieldsValues.Object_1 as TMP_InputField).text = string.Empty;

            })},

             {"_SUin2",
            new PathAction("InputField/SetText",
            new List<PathField>()
            {
                new PathField("InputField",PathFieldType_ID.Object_1,typeof(TMP_InputField)),
                new PathField("Value",PathFieldType_ID.String_1)
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                (input.fieldsValues.Object_1 as TMP_InputField).text =
                input.fieldsValues.String_1;

            })},

            //animations

             {"_SUan1",
            new PathAction("Animations/StopCanvasGroup",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.CGroupPrefix + obj.transform.GetInstanceID());


            })},



             {"_SUan2",
            new PathAction("Animations/StopColor",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.ColorPrefix + obj.transform.GetInstanceID());


            })},


             {"_SUan3",
            new PathAction("Animations/StopJump",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.JumpPrefix + obj.transform.GetInstanceID());


            })},


            
             {"_SUan4",
            new PathAction("Animations/StopPosition",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.PositionPrefix + obj.transform.GetInstanceID());


            })},


             {"_SUan5",
            new PathAction("Animations/StopPunch",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.PunchPrefix + obj.transform.GetInstanceID());


            })},


             {"_SUan6",
            new PathAction("Animations/StopRectSize",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.RectSizePrefix + obj.transform.GetInstanceID());


            })},


             {"_SUan7",
            new PathAction("Animations/StopRotation",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.RotationPrefix + obj.transform.GetInstanceID());


            })},


             {"_SUan8",
            new PathAction("Animations/StopScale",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.ScalePrefix + obj.transform.GetInstanceID());


            })},


             {"_SUan9",
            new PathAction("Animations/StopShake",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.ShakePrefix + obj.transform.GetInstanceID());


            })},


             {"_SUan10",
            new PathAction("Animations/StopCharTweener",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.CharTweenPrefix + obj.transform.GetInstanceID());


            })},


            //transform

             {"_SUtr1",
            new PathAction("Transform/SetLocalPosition",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("Position",PathFieldType_ID.Vector3_1,typeof(Vector3)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.PositionPrefix + obj.transform.GetInstanceID());
                    obj.transform.localPosition = (Vector3)input.fieldsValues.Vector3_1;

            })},


             {kSetAnchoredPosition,
            new PathAction("Transform/SetAnchoredPosition",
            new List<PathField>()
            {
                new PathField("RectTransform",PathFieldType_ID.Object_1,typeof(RectTransform)),
                new PathField("Position",PathFieldType_ID.Vector3_1,typeof(Vector3)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var recT = input.fieldsValues.Object_1 as RectTransform;


                DOTween.Kill(SUAnimationData.PositionPrefix + recT.transform.GetInstanceID());
                    
                recT.anchoredPosition = (Vector3)input.fieldsValues.Vector3_1;

            })},


             {"_SUtr3",
            new PathAction("Transform/SetLocalEuler",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("Rotation",PathFieldType_ID.Vector3_1,typeof(Vector3)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.RotationPrefix + obj.transform.GetInstanceID());
                obj.transform.localEulerAngles = (Vector3)input.fieldsValues.Vector3_1;

            })},


             {"_SUtr4",
            new PathAction("Transform/SetLocalScale",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
                new PathField("Scale",PathFieldType_ID.Vector3_1,typeof(Vector3)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                DOTween.Kill(SUAnimationData.ScalePrefix + obj.transform.GetInstanceID());
                obj.transform.localScale = (Vector3)input.fieldsValues.Vector3_1;
            })},


             {"_SUtr5",
            new PathAction("Transform/SetSizeDelta",
            new List<PathField>()
            {
                new PathField("RectTransform",PathFieldType_ID.Object_1,typeof(RectTransform)),
                new PathField("Size",PathFieldType_ID.Vector2_1,typeof(Vector2)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var recT = input.fieldsValues.Object_1 as RectTransform;


                DOTween.Kill(SUAnimationData.RectSizePrefix + recT.transform.GetInstanceID());

                recT.sizeDelta = (Vector2)input.fieldsValues.Vector2_1;

            })},


             {"_SUtr6",
            new PathAction("Transform/JustMoveIn",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                obj.transform.localPosition = Vector3.zero;


            })},


             {"_SUtr7",
            new PathAction("Transform/JustMoveOut",
            new List<PathField>()
            {
                new PathField("GameObject",PathFieldType_ID.Object_1,typeof(GameObject)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as GameObject;

                obj.transform.localPosition = new Vector3(0,-10_000,0);

            })},


            //state

             {"_SUs1",
            new PathAction("State/ResetMyStateHistoryFocus",
            (input)=>
            {
                var state = SurferManager.I.GetObjectStateElement(input.gameObj);

                if (state == null)
                    return;

                SUEventSystemManager.I.ResetStateHistoryFocus(state.ElementData.PlayerID,state.ElementData.StateData.Name);


            })},


            //image
              {"_SUim1",
            new PathAction("Image/SetSprite",
            new List<PathField>()
            {
                new PathField("Image",PathFieldType_ID.Object_1,typeof(Image)),
                new PathField("Sprite",PathFieldType_ID.Object_2,typeof(Sprite)),
            },
            (input)=>
            {
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as Image;

                obj.sprite = input.fieldsValues.Object_2 as Sprite;

            })},


            //dropdown
              {"_SUdr1",
            new PathAction("Dropdown/SelectOption",
            new List<PathField>()
            {
                new PathField("Dropdown",PathFieldType_ID.Object_1,typeof(TMP_Dropdown)),
                new PathField("Index",PathFieldType_ID.Int_1),
            },
            (input)=>
            {

                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as TMP_Dropdown;

                obj.value =  input.fieldsValues.Int_1;

            })},


            //slider
              {"_SUsl1",
            new PathAction("Slider/SetValue",
            new List<PathField>()
            {
                new PathField("Slider",PathFieldType_ID.Object_1,typeof(Slider)),
                new PathField("Value",PathFieldType_ID.Float_1),
            },
            (input)=>
            {
                
                if(input.fieldsValues.Object_1 == null)
                return;

                var obj = input.fieldsValues.Object_1 as Slider;

                obj.value =  input.fieldsValues.Float_1;

            })},


            //application
              {"_SUap1",
            new PathAction("Application/OpenURL",
            new List<PathField>()
            {
                new PathField("Url",PathFieldType_ID.String_1),
            },
            (input)=>
            {
                
                Application.OpenURL(input.fieldsValues.String_1);

            })},


            //cursor
             {"_SUcu1",
            new PathAction("Cursor/SetDefaultIcon",
            (input)=>
            {
                
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);


            })},


             {"_SUcu2",
            new PathAction("Cursor/SetIcon",
            new List<PathField>()
            {
                new PathField("Texture",PathFieldType_ID.Object_1,typeof(Texture2D)),
            },
            (input)=>
            {
                
                if(input.fieldsValues.Object_1 == null)
                return;

                var tex = input.fieldsValues.Object_1 as Texture2D;

                Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);

            })},


             {"_SUcu3",
            new PathAction("Cursor/SetIconCentered",
            new List<PathField>()
            {
                new PathField("Texture",PathFieldType_ID.Object_1,typeof(Texture2D)),
            },
            (input)=>
            {
                
                if(input.fieldsValues.Object_1 == null)
                return;

                var tex = input.fieldsValues.Object_1 as Texture2D;

                Cursor.SetCursor(tex, new Vector2(tex.width/2f,tex.height/2f), CursorMode.Auto);

            })},



            //canvas
             {"_SUca1",
            new PathAction("Canvas/BringToFront",
            new List<PathField>()
            {
                new PathField("Canvas",PathFieldType_ID.Object_1,typeof(Canvas)),
            },
            (input)=>
            {
                
                if(input.fieldsValues.Object_1 == null)
                return;

                var canvas = input.fieldsValues.Object_1 as Canvas;
                var canvases = GameObject.FindObjectsOfType<Canvas>();

                int highestOrder = -40000;

                for(int i=0;i<canvases.Length;i++)
                {
                    highestOrder = Mathf.Max(highestOrder,canvases[i].sortingOrder);
                }

                highestOrder += 1;

                canvas.overrideSorting = true;
                canvas.sortingOrder = highestOrder;

            })},


             {"_SUca2",
            new PathAction("Canvas/SendToBack",
            new List<PathField>()
            {
                new PathField("Canvas",PathFieldType_ID.Object_1,typeof(Canvas)),
            },
            (input)=>
            {
                
                if(input.fieldsValues.Object_1 == null)
                return;

                var canvas = input.fieldsValues.Object_1 as Canvas;
                var allCanvases = GameObject.FindObjectsOfType<Canvas>();

                int lowestOrder = 40000;

                for (int i = 0; i < allCanvases.Length; i++)
                {
                    lowestOrder = Mathf.Min(lowestOrder, allCanvases[i].sortingOrder);
                }

                lowestOrder -= 1;

                canvas.overrideSorting = true;
                canvas.sortingOrder = lowestOrder;


            })},



            //playerprefs
             {"_SUpl1",
            new PathAction("PlayerPrefs/DeleteAll",
            (input)=>
            {
                
                PlayerPrefs.DeleteAll();
            })},

             {"_SUpl2",
            new PathAction("PlayerPrefs/DeleteKey",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
            },
            (input)=>
            {
                
                PlayerPrefs.DeleteKey(input.fieldsValues.String_1);
            })},


             {"_SUpl3",
            new PathAction("PlayerPrefs/SetFloat",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
                new PathField("Value",PathFieldType_ID.Float_1),
            },
            (input)=>
            {
                
                PlayerPrefs.SetFloat(input.fieldsValues.String_1,input.fieldsValues.Float_1);
            })},


             {"_SUpl4",
            new PathAction("PlayerPrefs/SetInt",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
                new PathField("Value",PathFieldType_ID.Int_1),
            },
            (input)=>
            {
                
                PlayerPrefs.SetInt(input.fieldsValues.String_1,input.fieldsValues.Int_1);
            })},


            {"_SUpl5",
            new PathAction("PlayerPrefs/SetString",
            new List<PathField>()
            {
                new PathField("Key",PathFieldType_ID.String_1),
                new PathField("Value",PathFieldType_ID.String_2),
            },
            (input)=>
            {
                
                PlayerPrefs.SetString(input.fieldsValues.String_1,input.fieldsValues.String_2);
            })},



            //animator

             {"_SUant1",
            new PathAction("Animator/PlayState",
            new List<PathField>()
            {
                new PathField("Animator",PathFieldType_ID.Object_1,typeof(Animator)),
                new PathField("State",PathFieldType_ID.String_1),
            },
            (input)=>
            {
                
                if(input.fieldsValues.Object_1 == null)
                return;

                var animator = input.fieldsValues.Object_1 as Animator;
                
                animator.Play(input.fieldsValues.String_1);

            })},


             {"_SUant2",
            new PathAction("Animator/Stop",
            new List<PathField>()
            {
                new PathField("Animator",PathFieldType_ID.Object_1,typeof(Animator)),
            },
            (input)=>
            {
                
                if(input.fieldsValues.Object_1 == null)
                return;

                var animator = input.fieldsValues.Object_1 as Animator;

                animator.StopPlayback();

            })},


            //tween animations

            {"_SUatw1",
            new PathAction("Animations/Position",
            new List<PathField>()
            {
                new PathField("Position","_position"),
            },
            (input)=>
            {

                input.reactionData.PositionAnim.Play(input.gameObj);

            })},


             {"_SUatw2",
            new PathAction("Animations/AnchoredPosition",
            new List<PathField>()
            {
                new PathField("AnchoredPosition","_anchoredPosition"),
            },
            (input)=>
            {
                
                input.reactionData.AnchoredPositionAnim.Play(input.gameObj);

            })},


             {"_SUatw3",
            new PathAction("Animations/Rotation",
            new List<PathField>()
            {
                new PathField("Rotation","_rotation"),
            },
            (input)=>
            {

                input.reactionData.RotationAnim.Play(input.gameObj);

            })},


             {"_SUatw4",
            new PathAction("Animations/Scale",
            new List<PathField>()
            {
                new PathField("Scale","_scale"),
            },
            (input)=>
            {

                input.reactionData.ScaleAnim.Play(input.gameObj);

            })},


             {"_SUatw5",
            new PathAction("Animations/RectSize",
            new List<PathField>()
            {
                new PathField("RectSize","_rectSize"),
            },
            (input)=>
            {

                input.reactionData.RectSizeAnim.Play(input.gameObj);

            })},


             {"_SUatw6",
            new PathAction("Animations/Color",
            new List<PathField>()
            {
                new PathField("Color","_color"),
            },
            (input)=>
            {

                input.reactionData.ColorAnim.Play(input.gameObj);

            })},


             {"_SUatw7",
            new PathAction("Animations/Shake",
            new List<PathField>()
            {
                new PathField("Shake","_shake"),
            },
            (input)=>
            {

                input.reactionData.ShakeAnim.Play(input.gameObj);

            })},


             {"_SUatw8",
            new PathAction("Animations/Jump",
            new List<PathField>()
            {
                new PathField("Jump","_jump"),
            },
            (input)=>
            {

                input.reactionData.JumpAnim.Play(input.gameObj);

            })},


             {"_SUatw9",
            new PathAction("Animations/Punch",
            new List<PathField>()
            {
                new PathField("Punch","_punch"),
            },
            (input)=>
            {

                input.reactionData.PunchAnim.Play(input.gameObj);

            })},


             {"_SUatw10",
            new PathAction("Animations/CGroup",
            new List<PathField>()
            {
                new PathField("CanvasGroup","_canvasGroup"),
            },
            (input)=>
            {

                input.reactionData.CanvasGroupAnim.Play(input.gameObj);

            })},


             {"_SUatw11",
            new PathAction("Animations/CharTween",
            new List<PathField>()
            {
                new PathField("CharTween","_charTweener"),
            },
            (input)=>
            {

                input.reactionData.CharTweenerAnim.Play(input.gameObj);

            })},


            //surfer actions
            
            {kOpenState,
            new PathAction("Surfer/OpenState",
            new List<PathField>()
            {
                new PathField("State","_stateData"),
                new PathField("Version",PathFieldType_ID.Int_1),
                new PathField("PlayerID",PathFieldType_ID.Int_2),
                new PathField("Delay",PathFieldType_ID.Float_1),
                new PathField("Play Sound",PathFieldType_ID.Object_1,typeof(AudioClip))

            },
            (input)=>
            {


                SurferManager.I.OpenPlayerState(GetPlayerID(input),
                input.reactionData.StateData.Name,
                input.fieldsValues.Int_1,
                input.fieldsValues.Float_1);

                
                SurferHelper.PlaySound((input.fieldsValues.Object_1 as AudioClip),
                input.gameObj,
                input.fieldsValues.Float_1);

            })},


            {"_SUsu2",
            new PathAction("Surfer/OpenPrefabState",
            new List<PathField>()
            {
                new PathField("Parent",PathFieldType_ID.Enum_1,typeof(SUActionData.SUPrefabParent_ID)),
                new PathField("Prefab",PathFieldType_ID.Object_2,typeof(GameObject)),
                new PathField("Version",PathFieldType_ID.Int_1),
                new PathField("Delay",PathFieldType_ID.Float_1),
                new PathField("Play Sound",PathFieldType_ID.Object_1,typeof(AudioClip))
            },
            (input)=>
            {
                
                var prefab = input.fieldsValues.Object_2 as GameObject;

                if(prefab == null)
                return;

                var delay = input.fieldsValues.Float_1;
                var version = input.fieldsValues.Int_1;
                var parentMode = (SUPrefabParent_ID)input.fieldsValues.Enum_1;

                if(parentMode == SUPrefabParent_ID.Scene)
                    SurferManager.I.OpenPrefabState(prefab,null, version, delay);
                else if(parentMode == SUPrefabParent_ID.RootCanvas)
                    SurferManager.I.OpenPrefabState(prefab,input.gameObj.GetComponentInParent<Canvas>().rootCanvas.transform, version, delay);
                else if (parentMode == SUPrefabParent_ID.ThisState)
                    SurferManager.I.OpenPrefabState(prefab, SurferManager.I.GetObjectStateTransfom(input.gameObj), version, delay);
                else if (parentMode == SUPrefabParent_ID.ThisStateParent)
                    SurferManager.I.OpenPrefabState(prefab, SurferManager.I.GetObjectStateTransfom(SurferManager.I.GetObjectStateTransfom(input.gameObj).parent.gameObject), version, delay);


                
                SurferHelper.PlaySound((input.fieldsValues.Object_1 as AudioClip),
                input.gameObj,
                input.fieldsValues.Float_1);

            })},



            {"_SUsu3",
            new PathAction("Surfer/CloseState",
            new List<PathField>()
            {
                new PathField("State","_stateData"),
                new PathField("Version",PathFieldType_ID.Int_1),
                new PathField("PlayerID",PathFieldType_ID.Int_2),
                new PathField("Delay",PathFieldType_ID.Float_1),
                new PathField("Play Sound",PathFieldType_ID.Object_1,typeof(AudioClip))

            },
            (input)=>
            {

                SurferManager.I.ClosePlayerState(GetPlayerID(input),
                input.reactionData.StateData.Name,
                input.fieldsValues.Int_1,
                input.fieldsValues.Float_1);


                SurferHelper.PlaySound((input.fieldsValues.Object_1 as AudioClip),
                input.gameObj,
                input.fieldsValues.Float_1);

            })},



            {"_SUsu4",
            new PathAction("Surfer/ToggleState",
            new List<PathField>()
            {
                new PathField("State","_stateData"),
                new PathField("Version",PathFieldType_ID.Int_1),
                new PathField("PlayerID",PathFieldType_ID.Int_2),
                new PathField("Delay",PathFieldType_ID.Float_1),
                new PathField("Play Sound",PathFieldType_ID.Object_1,typeof(AudioClip))

            },
            (input)=>
            {

                SurferManager.I.TogglePlayerState(GetPlayerID(input),
                input.reactionData.StateData.Name,
                input.fieldsValues.Int_1,
                input.fieldsValues.Float_1);


                SurferHelper.PlaySound((input.fieldsValues.Object_1 as AudioClip),
                input.gameObj,
                input.fieldsValues.Float_1);

            })},



            {"_SUsu5",
            new PathAction("Surfer/LoadScene",
            new List<PathField>()
            {
                new PathField("Scene","_sceneData"),
                new PathField("Additive",PathFieldType_ID.Bool_1),
                new PathField("Overlay State","_stateData"),
                new PathField("Version",PathFieldType_ID.Int_1),
                new PathField("PlayerID",PathFieldType_ID.Int_2),
                new PathField("Delay",PathFieldType_ID.Float_1),
                new PathField("Play Sound",PathFieldType_ID.Object_1,typeof(AudioClip))

            },
            (input)=>
            {

                SurferManager.I.OpenPlayerState(GetPlayerID(input),
                input.reactionData.StateData.Name,
                input.fieldsValues.Int_1,
                input.fieldsValues.Float_1);

                SurferManager.I.LoadScene(input.reactionData.SceneData.Name,
                input.fieldsValues.Float_1,
                input.fieldsValues.Bool_1);


                SurferHelper.PlaySound((input.fieldsValues.Object_1 as AudioClip),
                input.gameObj,
                input.fieldsValues.Float_1);

            })},



            {"_SUsu6",
            new PathAction("Surfer/LoadSceneAsync",
            new List<PathField>()
            {
                new PathField("Scene","_sceneData"),
                new PathField("Additive",PathFieldType_ID.Bool_1),
                new PathField("AutoActivation",PathFieldType_ID.Bool_2),
                new PathField("Overlay State","_stateData"),
                new PathField("Version",PathFieldType_ID.Int_1),
                new PathField("PlayerID",PathFieldType_ID.Int_2),
                new PathField("Delay",PathFieldType_ID.Float_1),
                new PathField("Play Sound",PathFieldType_ID.Object_1,typeof(AudioClip))

            },
            (input)=>
            {


                SurferManager.I.OpenPlayerState(GetPlayerID(input),
                input.reactionData.StateData.Name,
                input.fieldsValues.Int_1,
                input.fieldsValues.Float_1);

                SurferManager.I.LoadSceneAsync(input.reactionData.SceneData.Name,
                input.fieldsValues.Float_1,
                input.fieldsValues.Bool_1,
                input.fieldsValues.Bool_2);


                SurferHelper.PlaySound((input.fieldsValues.Object_1 as AudioClip),
                input.gameObj,
                input.fieldsValues.Float_1);

            })},


            {"_SUsu7",
            new PathAction("Surfer/UnloadSceneAsync",
            new List<PathField>()
            {
                new PathField("Scene","_sceneData"),
                new PathField("Overlay State","_stateData"),
                new PathField("Version",PathFieldType_ID.Int_1),
                new PathField("PlayerID",PathFieldType_ID.Int_2),
                new PathField("Delay",PathFieldType_ID.Float_1),
                new PathField("Play Sound",PathFieldType_ID.Object_1,typeof(AudioClip))

            },
            (input)=>
            {

                SurferManager.I.OpenPlayerState(GetPlayerID(input),
                input.reactionData.StateData.Name,
                input.fieldsValues.Int_1,
                input.fieldsValues.Float_1);

                SurferManager.I.UnloadSceneAsync(input.reactionData.SceneData.Name,
                input.fieldsValues.Float_1);


                SurferHelper.PlaySound((input.fieldsValues.Object_1 as AudioClip),
                input.gameObj,
                input.fieldsValues.Float_1);

            })},


            {"_SUsu8",
            new PathAction("Surfer/SetActiveScene",
            new List<PathField>()
            {
                new PathField("Scene","_sceneData"),
                new PathField("Overlay State","_stateData"),
                new PathField("Version",PathFieldType_ID.Int_1),
                new PathField("PlayerID",PathFieldType_ID.Int_2),
                new PathField("Delay",PathFieldType_ID.Float_1),
                new PathField("Play Sound",PathFieldType_ID.Object_1,typeof(AudioClip))

            },
            (input)=>
            {

                SurferManager.I.OpenPlayerState(GetPlayerID(input),
                input.reactionData.StateData.Name,
                input.fieldsValues.Int_1,
                input.fieldsValues.Float_1);

                SurferManager.I.SetActiveScene(input.reactionData.SceneData.Name,
                input.fieldsValues.Float_1);


                SurferHelper.PlaySound((input.fieldsValues.Object_1 as AudioClip),
                input.gameObj,
                input.fieldsValues.Float_1);

            })},


            {"_SUsu9",
            new PathAction("Surfer/CloseMyState",
            new List<PathField>()
            {
                new PathField("Delay",PathFieldType_ID.Float_1),
                new PathField("Play Sound",PathFieldType_ID.Object_1,typeof(AudioClip))

            },
            (input)=>
            {

                SurferManager.I.ClosePlayerState(SurferManager.I.GetObjectStatePlayerID(input.gameObj,true), SurferManager.I.GetObjectStateName(input.gameObj,true), 0,input.fieldsValues.Float_1);

                SurferHelper.PlaySound((input.fieldsValues.Object_1 as AudioClip),
                input.gameObj,
                input.fieldsValues.Float_1);

            })},



            {"_SUsu10",
            new PathAction("Surfer/SendCustomEvent",
            new List<PathField>()
            {
                new PathField("CustomEvent","_customEData"),
                new PathField("Delay",PathFieldType_ID.Float_1),
                new PathField("Play Sound",PathFieldType_ID.Object_1,typeof(AudioClip))

            },
            (input)=>
            {

                SurferManager.I.SendCustomEvent(input.reactionData.CustomEData.Name,input.fieldsValues.Float_1,null);

                SurferHelper.PlaySound((input.fieldsValues.Object_1 as AudioClip),
                input.gameObj,
                input.fieldsValues.Float_1);

            })},


        };


        static int GetPlayerID(FuncInput input)
        {

            var playerID = input.fieldsValues.Int_2;

            if(playerID == -1)
            {
                playerID = SurferManager.I.GetObjectStatePlayerID(input.gameObj);
            }

            return playerID;
        }


        /// <summary>
        /// Get all the names/paths of all the reactions both Custom and Default. Used for the inspector
        /// </summary>
        /// <returns>Names/paths list</returns>
        public static string[] GetAllNames()
        {
            return All.Select(x=>x.Value.Path).OrderBy(x=>x).Prepend(SurferHelper.Unset).ToArray();
        }

        /// <summary>
        /// Get the name/path of a specific reaction. Used for the inspector
        /// </summary>
        /// <param name="key">Reaction key to retrieve the name/path</param>
        /// <returns>Reaction name/path</returns>
        public static string GetName(string key)
        {
            if(All.TryGetValue(key,out PathAction value))
                return value.Path;

            return "";
        }

        /// <summary>
        /// Get the key of a specific reaction. Used for the inspector
        /// </summary>
        /// <param name="path">Reaction name/path to retrieve the key</param>
        /// <returns>Reaction key</returns>
        public static string GetKey(string path)
        {
            foreach(KeyValuePair<string, PathAction> pair in All)
            {
                if(pair.Value.Path.Equals(path))
                return pair.Key;
            }
            return "";
        }


    

    }


}

