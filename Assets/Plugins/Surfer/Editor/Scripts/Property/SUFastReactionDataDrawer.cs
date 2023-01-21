using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer
{
    [CustomPropertyDrawer(typeof(SUFastReactionData))]
    public class SUFastReactionDataDrawer : PropertyDrawer
    {
        SerializedProperty _reactionID, _intVal, _stringVal, _stringVal2, _floatVal,
            _cGroup, _toggle, _tmp,_tmpInput, _objVal, _selectable, _graphic,
            _colVal, _aSource, _aClip, _vec3, _recT, _img, _sprite, _slider, _dropdown,
            _tex2D, _vec2, _canvas , _animator = default;

        Rect _pos = default;
        SerializedProperty _mainProp = default;

        SUFastReactionData.Type_ID _selected
        {
            get
            {
                return (SUFastReactionData.Type_ID)_reactionID.enumValueIndex;
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

                _reactionID.enumValueIndex = EditorGUI.Popup(position, !string.IsNullOrEmpty(label.text) ? "Reaction" : label.text, _reactionID.enumValueIndex, GetReactionsList(property));

                _pos = position;
                _mainProp = property;
                AddFields();

            }
        }


        void GetPropertyRelatives(SerializedProperty property)
        {
            _reactionID = property.FindPropertyRelative("_reactionID");
            _intVal = property.FindPropertyRelative("_intVal");
            _stringVal = property.FindPropertyRelative("_stringVal");
            _stringVal2 = property.FindPropertyRelative("_stringVal2");
            _floatVal = property.FindPropertyRelative("_floatVal");

            _selectable = property.FindPropertyRelative("_selectable");
            _cGroup = property.FindPropertyRelative("_cGroup");
            _toggle = property.FindPropertyRelative("_toggle");
            _tmp = property.FindPropertyRelative("_tmp");
            _tmpInput = property.FindPropertyRelative("_tmpInput");
            _objVal = property.FindPropertyRelative("_obj");
            _graphic = property.FindPropertyRelative("_graphic");
            _colVal = property.FindPropertyRelative("_colVal");
            _aSource = property.FindPropertyRelative("_aSource");
            _aClip = property.FindPropertyRelative("_aClip");
            _vec3 = property.FindPropertyRelative("_vec3");
            _vec2 = property.FindPropertyRelative("_vec2");
            _recT = property.FindPropertyRelative("_recT");
            _img = property.FindPropertyRelative("_img");
            _sprite = property.FindPropertyRelative("_sprite");
            _slider = property.FindPropertyRelative("_slider");
            _dropdown = property.FindPropertyRelative("_dropdown");
            _tex2D = property.FindPropertyRelative("_tex2D");
            _canvas = property.FindPropertyRelative("_canvas");
            _animator = property.FindPropertyRelative("_animator");
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return SurferHelper.lineHeight*2;
        }

        string[] GetReactionsList(SerializedProperty property)
        {

            string[] list = System.Enum.GetNames(typeof(SUFastReactionData.Type_ID));

            for (int i = 0; i < list.Length; i++)
            {
                list[i] = list[i].Replace("_", @"/");
            }

            return list;

        }


        #region Checks


        void AddFields()
        {

            switch((SUFastReactionData.Type_ID)_reactionID.enumValueIndex)
            {

                case SUFastReactionData.Type_ID.CanvasGroup_Alpha0:
                case SUFastReactionData.Type_ID.CanvasGroup_Alpha1:
                case SUFastReactionData.Type_ID.CanvasGroup_IgnoreParentGroupsOff:
                case SUFastReactionData.Type_ID.CanvasGroup_IgnoreParentGroupsOn:
                case SUFastReactionData.Type_ID.CanvasGroup_InteractableOff:
                case SUFastReactionData.Type_ID.CanvasGroup_InteractableOn:
                case SUFastReactionData.Type_ID.CanvasGroup_BlockRaycastOff:
                case SUFastReactionData.Type_ID.CanvasGroup_BlockRaycastOn:

                    _mainProp.AddField<CanvasGroup>(ref _pos,_cGroup);

                    break;

                case SUFastReactionData.Type_ID.CanvasGroup_SetAlpha:


                    _mainProp.AddField<CanvasGroup>(ref _pos, _cGroup);
                    _floatVal.AddValueField(ref _pos);

                    break;



                case SUFastReactionData.Type_ID.GameObject_Disable:
                case SUFastReactionData.Type_ID.GameObject_Enable:
                case SUFastReactionData.Type_ID.UIGenerics_FocusOnObject:
                case SUFastReactionData.Type_ID.UIGenerics_FocusOnObjectOrStateLast:

                case SUFastReactionData.Type_ID.Animations_StopCanvasGroup:
                case SUFastReactionData.Type_ID.Animations_StopColor:
                case SUFastReactionData.Type_ID.Animations_StopJump:
                case SUFastReactionData.Type_ID.Animations_StopPosition:
                case SUFastReactionData.Type_ID.Animations_StopPunch:
                case SUFastReactionData.Type_ID.Animations_StopRectSize:
                case SUFastReactionData.Type_ID.Animations_StopRotation:
                case SUFastReactionData.Type_ID.Animations_StopScale:
                case SUFastReactionData.Type_ID.Animations_StopShake:

                case SUFastReactionData.Type_ID.Transform_JustMoveIn:
                case SUFastReactionData.Type_ID.Transform_JustMoveOut:

                    _mainProp.AddGOField(ref _pos,_objVal);

                    break;


                case SUFastReactionData.Type_ID.GameObject_SetTag:

                    _mainProp.AddGOField(ref _pos, _objVal);
                    _intVal.AddIntList(ref _pos, SurferHelper.SO.Tags);


                    break;
                case SUFastReactionData.Type_ID.GameObject_SetLayer:


                    _mainProp.AddGOField(ref _pos, _objVal);
                    _intVal.AddIntList(ref _pos, SurferHelper.SO.Layers);

                    break;

                case SUFastReactionData.Type_ID.GameObject_SetName:

                    _mainProp.AddGOField(ref _pos, _objVal);
                    _stringVal.AddValueField(ref _pos);

                    break;

                case SUFastReactionData.Type_ID.Selectable_InteractableOff:
                case SUFastReactionData.Type_ID.Selectable_InteractableOn:

                    _mainProp.AddField<Selectable>(ref _pos, _selectable);

                    break;

                case SUFastReactionData.Type_ID.Graphic_RaycastOn:
                case SUFastReactionData.Type_ID.Graphic_RaycastOff:

                    _mainProp.AddField<Graphic>(ref _pos, _graphic);

                    break;

                case SUFastReactionData.Type_ID.Graphic_SetColor:

                    _mainProp.AddField<Graphic>(ref _pos, _graphic);
                    _colVal.AddValueField(ref _pos);

                    break;


                case SUFastReactionData.Type_ID.Audio_PlaySource:

                    _mainProp.AddField<AudioSource>(ref _pos, _aSource);

                    break;

                case SUFastReactionData.Type_ID.Audio_PlayClip:

                    _aClip.AddValueField(ref _pos);

                    break;

                case SUFastReactionData.Type_ID.Toggle_True:
                case SUFastReactionData.Type_ID.Toggle_False:
                case SUFastReactionData.Type_ID.Toggle_ToggleValue:

                    _mainProp.AddField<Toggle>(ref _pos, _toggle);

                    break;


                case SUFastReactionData.Type_ID.Text_SetText:

                    _mainProp.AddField<TextMeshProUGUI>(ref _pos, _tmp);
                    _stringVal.AddValueField(ref _pos);

                    break;

                case SUFastReactionData.Type_ID.Text_Empty:

                    _mainProp.AddField<TextMeshProUGUI>(ref _pos, _tmp);

                    break;

                case SUFastReactionData.Type_ID.InputField_SetText:

                    _mainProp.AddField<TMP_InputField>(ref _pos, _tmpInput);
                    _stringVal.AddValueField(ref _pos);

                    break;

                case SUFastReactionData.Type_ID.InputField_Empty:

                    _mainProp.AddField<TMP_InputField>(ref _pos, _tmpInput);

                    break;


                case SUFastReactionData.Type_ID.Transform_SetLocalPosition:
                case SUFastReactionData.Type_ID.Transform_SetLocalEuler:
                case SUFastReactionData.Type_ID.Transform_SetLocalScale:

                    _mainProp.AddGOField(ref _pos,_objVal);
                    _vec3.AddValueField(ref _pos);

                    break;


                case SUFastReactionData.Type_ID.Transform_SetAnchoredPosition:

                    _mainProp.AddField<RectTransform>(ref _pos, _recT);
                    _vec3.AddValueField(ref _pos);

                    break;


                case SUFastReactionData.Type_ID.Image_SetSprite:

                    _mainProp.AddField<Image>(ref _pos, _img);
                    _sprite.AddValueField(ref _pos);
                    break;



                case SUFastReactionData.Type_ID.Slider_SetValue:

                    _mainProp.AddField<Slider>(ref _pos, _slider);
                    _floatVal.AddValueField(ref _pos);
                    break;


                case SUFastReactionData.Type_ID.Dropdown_SelectOption:

                    _mainProp.AddField<TMP_Dropdown>(ref _pos, _dropdown);
                    _intVal.AddValueField(ref _pos);
                    break;


                case SUFastReactionData.Type_ID.Application_OpenURL:

                    _stringVal.AddValueField(ref _pos,0,new GUIContent("Url"));
                    break;


                case SUFastReactionData.Type_ID.Cursor_SetIcon:
                case SUFastReactionData.Type_ID.Cursor_SetIconCentered:

                    _tex2D.AddValueField(ref _pos, 0, new GUIContent("Icon"));
                    break;


                case SUFastReactionData.Type_ID.Transform_SetSizeDelta:

                    _mainProp.AddField<RectTransform>(ref _pos, _recT);
                    _vec2.AddValueField(ref _pos);
                    break;



                case SUFastReactionData.Type_ID.Canvas_BringToFront:
                case SUFastReactionData.Type_ID.Canvas_SendToBack:

                    _mainProp.AddField<Canvas>(ref _pos, _canvas,true);
                    break;


                case SUFastReactionData.Type_ID.UIGenerics_EnableClickConstraint:
                case SUFastReactionData.Type_ID.UIGenerics_DisableClickConstraint:

                    _mainProp.AddGOField(ref _pos, _objVal);

                    break;


                case SUFastReactionData.Type_ID.PlayerPrefs_DeleteKey:

                    _stringVal.AddValueField(ref _pos,0);

                    break;


                case SUFastReactionData.Type_ID.PlayerPrefs_SetString:

                    _stringVal.AddValueField(ref _pos, 1);
                    _stringVal2.AddValueField(ref _pos, 2);

                    break;


                case SUFastReactionData.Type_ID.PlayerPrefs_SetFloat:

                    _stringVal.AddValueField(ref _pos, 1);
                    _floatVal.AddValueField(ref _pos,2);

                    break;

                case SUFastReactionData.Type_ID.PlayerPrefs_SetInt:

                    _stringVal.AddValueField(ref _pos, 1);
                    _intVal.AddValueField(ref _pos, 2);

                    break;


                case SUFastReactionData.Type_ID.Animator_PlayState:

                    _mainProp.AddField<Animator>(ref _pos, _animator);
                    _stringVal.AddValueField(ref _pos);

                    break;


                case SUFastReactionData.Type_ID.Animator_Stop:

                    _mainProp.AddField<Animator>(ref _pos, _animator);

                    break;
            }




        }

      
        #endregion


    }
}