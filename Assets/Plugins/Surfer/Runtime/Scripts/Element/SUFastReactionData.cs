using System.Collections;
using System.Collections.Generic;
using Surfer;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


namespace Surfer
{

    [System.Serializable]
    public class SUFastReactionData
    {

        public enum Type_ID
        {
            None,
            CanvasGroup_Alpha1,
            CanvasGroup_Alpha0,
            CanvasGroup_SetAlpha,
            CanvasGroup_BlockRaycastOn,
            CanvasGroup_BlockRaycastOff,
            CanvasGroup_InteractableOn,
            CanvasGroup_InteractableOff,
            CanvasGroup_IgnoreParentGroupsOn,
            CanvasGroup_IgnoreParentGroupsOff,
            GameObject_Disable,
            GameObject_Enable,
            GameObject_SetTag,
            GameObject_SetLayer,
            GameObject_SetName,
            Selectable_InteractableOn,
            Selectable_InteractableOff,
            Graphic_RaycastOn,
            Graphic_RaycastOff,
            Graphic_SetColor,
            UIGenerics_FocusOnObject,
            /// <summary>
            /// Focus on/Select the last selected object of the caller state history.
            /// If null, focus on/select the specified object.
            /// </summary>
            UIGenerics_FocusOnObjectOrStateLast,
            Audio_PlaySource,
            Audio_PlayClip,
            Toggle_True,
            Toggle_False,
            Toggle_ToggleValue,
            Text_SetText,
            Text_Empty,
            InputField_SetText,
            InputField_Empty,
            /// <summary>
            /// Stop the Surfer Canvas Group animation (the one in the "Animated!" list)
            /// </summary>
            Animations_StopCanvasGroup,
            /// <summary>
            /// Stop the Surfer Color animation (the one in the "Animated!" list)
            /// </summary>
            Animations_StopColor,
            /// <summary>
            /// Stop the Surfer Jump animation (the one in the "Animated!" list)
            /// </summary>
            Animations_StopJump,
            /// <summary>
            /// Stop the Surfer Position/Anchor Position animation (the one in the "Animated!" list)
            /// </summary>
            Animations_StopPosition,
            /// <summary>
            /// Stop the Surfer Punch animation (the one in the "Animated!" list)
            /// </summary>
            Animations_StopPunch,
            /// <summary>
            /// Stop the Surfer RectSize animation (the one in the "Animated!" list)
            /// </summary>
            Animations_StopRectSize,
            /// <summary>
            /// Stop the Surfer Rotation animation (the one in the "Animated!" list)
            /// </summary>
            Animations_StopRotation,
            /// <summary>
            /// Stop the Surfer Scale animation (the one in the "Animated!" list)
            /// </summary>
            Animations_StopScale,
            /// <summary>
            /// Stop the Surfer Shake animation (the one in the "Animated!" list)
            /// </summary>
            Animations_StopShake,
            Transform_SetLocalPosition,
            Transform_SetAnchoredPosition,
            Transform_SetLocalEuler,
            Transform_SetLocalScale,
            /// <summary>
            /// Reset the state's history selection/focus.
            /// The last selected object of the caller's state , will become null.
            /// </summary>
            State_ResetMyStateHistoryFocus,
            //2.1
            Image_SetSprite,
            Dropdown_SelectOption,
            Slider_SetValue,
            //2.2
            Application_OpenURL,
            /// <summary>
            /// Set a new icon for the mouse cursor, with the default offset (top-left)
            /// </summary>
            Cursor_SetIcon,
            /// <summary>
            /// Set a new icon for the mouse cursor, without offset (centered)
            /// </summary>
            Cursor_SetIconCentered,
            Cursor_SetDefaultIcon,
            Transform_SetSizeDelta,
            Canvas_BringToFront,
            Canvas_SendToBack,
            UIGenerics_EnableClickConstraint,
            UIGenerics_DisableClickConstraint,
            //2.3
            PlayerPrefs_SetInt,
            PlayerPrefs_SetFloat,
            PlayerPrefs_SetString,
            PlayerPrefs_DeleteKey,
            PlayerPrefs_DeleteAll,
            Animator_PlayState,
            Animator_Stop,
            /// <summary>
            /// Stop the Surfer CharTweener animation (the one in the "Animated!" list)
            /// </summary>
            Animations_StopCharTweener,
            /// <summary>
            /// The object will be locally moved at x=0, y=0, z=0
            /// </summary>
            Transform_JustMoveIn,
            /// <summary>
            /// The object will be locally moved out of camera at x=0, y= -10_000, z=0
            /// </summary>
            Transform_JustMoveOut,

        }


        [SerializeField]
        Type_ID _reactionID = default;


        [SerializeField]
        int _intVal = default;
        [SerializeField]
        string _stringVal = default;
        [SerializeField]
        string _stringVal2 = default;
        [SerializeField]
        float _floatVal = default;
        [SerializeField]
        Color _colVal = default;
        [SerializeField]
        AudioClip _audioClip = default;
        [SerializeField]
        Image _img = default;
        [SerializeField]
        Sprite _sprite = default;
        [SerializeField]
        AudioSource _aSource = default;
        [SerializeField]
        CanvasGroup _cGroup = default;
        [SerializeField]
        Toggle _toggle = default;
        [SerializeField]
        Selectable _selectable = default;
        [SerializeField]
        Graphic _graphic = default;
        [SerializeField]
        TextMeshProUGUI _tmp = default;
        [SerializeField]
        TMP_InputField _tmpInput = default;
        [SerializeField]
        GameObject _obj = default;
        [SerializeField]
        Vector3 _vec3 = default;
        [SerializeField]
        Vector2 _vec2 = default;
        [SerializeField]
        RectTransform _recT = default;
        [SerializeField]
        TMP_Dropdown _dropdown = default;
        [SerializeField]
        Slider _slider = default;
        [SerializeField]
        Texture2D _tex2D = default;
        [SerializeField]
        Canvas _canvas = default;
        [SerializeField]
        Animator _animator = default;


        GameObject _owner = default;


        public void Play(GameObject go)
        {
            if (go == null)
                return;

            _owner = go;

            CheckGameObject();
            CheckCanvasGroup();
            CheckAudio();
            CheckToggle();
            CheckGraphic();
            CheckUIGenerics();
            CheckAnimations();
            CheckTransform();
            CheckState();
            CheckSelectable();
            CheckText();
            CheckTextInput();
            CheckImage();
            CheckSlider();
            CheckDropdown();
            CheckApplication();
            CheckCursor();
            CheckCanvas();
            CheckPlayerPrefs();
            CheckAnimator();

        }





        void CheckAnimator()
        {

            switch(_reactionID)
            {

                case Type_ID.Animator_PlayState:

                    if (_animator != null)
                        _animator.Play(_stringVal);

                    break;

                case Type_ID.Animator_Stop:

                    if (_animator != null)
                        _animator.StopPlayback();

                    break;



            }



        }


        void CheckPlayerPrefs()
        {


            switch(_reactionID)
            {

                case Type_ID.PlayerPrefs_DeleteAll:

                    PlayerPrefs.DeleteAll();

                    break;

                case Type_ID.PlayerPrefs_DeleteKey:

                    PlayerPrefs.DeleteKey(_stringVal);

                    break;

                case Type_ID.PlayerPrefs_SetFloat:

                    PlayerPrefs.SetFloat(_stringVal,_floatVal);

                    break;

                case Type_ID.PlayerPrefs_SetInt:

                    PlayerPrefs.SetInt(_stringVal, _intVal);

                    break;

                case Type_ID.PlayerPrefs_SetString:

                    PlayerPrefs.SetString(_stringVal, _stringVal2);

                    break;

            }



        }


        void CheckCanvas()
        {

            if (_canvas == null)
                return;

            switch (_reactionID)
            {
                case Type_ID.Canvas_BringToFront:


                    var canvases = GameObject.FindObjectsOfType<Canvas>();

                    int highestOrder = -40000;

                    for(int i=0;i<canvases.Length;i++)
                    {
                        highestOrder = Mathf.Max(highestOrder,canvases[i].sortingOrder);
                    }

                    highestOrder += 1;

                    _canvas.overrideSorting = true;
                    _canvas.sortingOrder = highestOrder;

                    
                    break;

                case Type_ID.Canvas_SendToBack:

                    var allCanvases = GameObject.FindObjectsOfType<Canvas>();

                    int lowestOrder = 40000;

                    for (int i = 0; i < allCanvases.Length; i++)
                    {
                        lowestOrder = Mathf.Min(lowestOrder, allCanvases[i].sortingOrder);
                    }

                    lowestOrder -= 1;

                    _canvas.overrideSorting = true;
                    _canvas.sortingOrder = lowestOrder;


                    break;

            }


        }


        void CheckCursor()
        {
            switch (_reactionID)
            {
                case Type_ID.Cursor_SetDefaultIcon:

                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

                    break;

                case Type_ID.Cursor_SetIcon:

                    if(_tex2D != null)
                        Cursor.SetCursor(_tex2D, Vector2.zero, CursorMode.Auto);

                    break;

                case Type_ID.Cursor_SetIconCentered:

                    if (_tex2D != null)
                        Cursor.SetCursor(_tex2D, new Vector2(_tex2D.width/2f,_tex2D.height/2f), CursorMode.Auto);

                    break;

            }
        }


        void CheckApplication()
        {
            switch (_reactionID)
            {
                case Type_ID.Application_OpenURL:

                    Application.OpenURL(_stringVal);

                    break;

            }
        }


        void CheckDropdown()
        {
            switch (_reactionID)
            {
                case Type_ID.Dropdown_SelectOption:

                    if (_dropdown != null)
                        _dropdown.value = _intVal;

                    break;

            }
        }


        void CheckSlider()
        {
            switch (_reactionID)
            {
                case Type_ID.Slider_SetValue:

                    if (_slider != null)
                        _slider.value = _floatVal;

                    break;

            }
        }


        void CheckImage()
        {
            switch(_reactionID)
            {
                case Type_ID.Image_SetSprite:

                    if (_img != null)
                        _img.sprite = _sprite;

                    break;

            }
        }


        void CheckSelectable()
        {
            switch (_reactionID)
            {

                case Type_ID.Selectable_InteractableOff:

                    if (_selectable != null)
                        _selectable.interactable = false ;

                    break;

                case Type_ID.Selectable_InteractableOn:

                    if (_selectable != null)
                        _selectable.interactable = true;

                    break;


            }
        }

        void CheckText()
        {
            switch (_reactionID)
            {

                case Type_ID.Text_Empty:

                    if (_tmp != null)
                        _tmp.text = string.Empty;

                    break;

                case Type_ID.Text_SetText:

                    if (_tmp != null)
                        _tmp.text = _stringVal;

                    break;


            }
        }

        void CheckTextInput()
        {
            switch (_reactionID)
            {

                case Type_ID.InputField_Empty:

                    if (_tmpInput != null)
                        _tmpInput.text = string.Empty;

                    break;

                case Type_ID.InputField_SetText:

                    if (_tmpInput != null)
                        _tmpInput.text = _stringVal;

                    break;


            }
        }




        void CheckState()
        {
            switch (_reactionID)
            {

                case Type_ID.State_ResetMyStateHistoryFocus:

                    var state = SurferManager.I.GetObjectStateElement(_owner);

                    if (state == null)
                        return;

                    SUEventSystemManager.I.ResetStateHistoryFocus(state.ElementData.PlayerID,state.ElementData.StateData.Name);

                    break;


            }
        }

        void CheckTransform()
        {

            switch (_reactionID)
            {

                case Type_ID.Transform_SetLocalPosition:

                    if (_obj == null)
                        return;

                    DOTween.Kill(SUAnimationData.PositionPrefix + _obj.transform.GetInstanceID());
                    _obj.transform.localPosition = _vec3;

                    break;


                case Type_ID.Transform_SetAnchoredPosition:

                    if (_recT == null)
                        return;

                    DOTween.Kill(SUAnimationData.PositionPrefix + _recT.transform.GetInstanceID());
                    
                    _recT.anchoredPosition = _vec3;

                    break;

                case Type_ID.Transform_SetLocalEuler:

                    if (_obj == null)
                        return;

                    DOTween.Kill(SUAnimationData.RotationPrefix + _obj.transform.GetInstanceID());
                    _obj.transform.localEulerAngles = _vec3;

                    break;


                case Type_ID.Transform_SetLocalScale:

                    if (_obj == null)
                        return;

                    DOTween.Kill(SUAnimationData.ScalePrefix + _obj.transform.GetInstanceID());
                    _obj.transform.localScale = _vec3;

                    break;


                case Type_ID.Transform_SetSizeDelta:

                    if (_recT == null)
                        return;

                    DOTween.Kill(SUAnimationData.RectSizePrefix + _recT.transform.GetInstanceID());

                    if (_recT != null)
                        _recT.sizeDelta = _vec2;

                    break;

                case Type_ID.Transform_JustMoveIn:

                    if (_obj == null)
                        return;

                _obj.transform.localPosition = Vector3.zero;

                break;

                case Type_ID.Transform_JustMoveOut:

                    if (_obj == null)
                        return;

                _obj.transform.localPosition = new Vector3(0,-10_000,0);

                break;

            }


        }





        void CheckAnimations()
        {
            if (_obj == null)
                return;

            switch (_reactionID)
            {

                case Type_ID.Animations_StopCanvasGroup:

                    DOTween.Kill(SUAnimationData.CGroupPrefix + _obj.transform.GetInstanceID());

                    break;

                case Type_ID.Animations_StopColor:

                    DOTween.Kill(SUAnimationData.ColorPrefix + _obj.transform.GetInstanceID());

                    break;
                case Type_ID.Animations_StopJump:

                    DOTween.Kill(SUAnimationData.JumpPrefix + _obj.transform.GetInstanceID());

                    break;
                case Type_ID.Animations_StopPosition:

                    DOTween.Kill(SUAnimationData.PositionPrefix + _obj.transform.GetInstanceID());

                    break;
                case Type_ID.Animations_StopPunch:

                    DOTween.Kill(SUAnimationData.PunchPrefix + _obj.transform.GetInstanceID());

                    break;
                case Type_ID.Animations_StopRectSize:

                    DOTween.Kill(SUAnimationData.RectSizePrefix + _obj.transform.GetInstanceID());

                    break;
                case Type_ID.Animations_StopRotation:

                    DOTween.Kill(SUAnimationData.RotationPrefix + _obj.transform.GetInstanceID());

                    break;
                case Type_ID.Animations_StopScale:

                    DOTween.Kill(SUAnimationData.ScalePrefix + _obj.transform.GetInstanceID());

                    break;
                case Type_ID.Animations_StopShake:

                    DOTween.Kill(SUAnimationData.ShakePrefix + _obj.transform.GetInstanceID());

                    break;

                case Type_ID.Animations_StopCharTweener:

                    DOTween.Kill(SUAnimationData.CharTweenPrefix + _obj.transform.GetInstanceID());

                    break;
            }
        }


        void CheckUIGenerics()
        {
            if (_obj == null)
                return;

            switch (_reactionID)
            {

                case Type_ID.UIGenerics_FocusOnObject:

                    SUEventSystemManager.I.FocusOnObject(_obj);

                    break;


                case Type_ID.UIGenerics_FocusOnObjectOrStateLast:

                    SUEventSystemManager.I.FocusOnObjectOrLast(_obj);

                    break;


                case Type_ID.UIGenerics_EnableClickConstraint:


                    var cg = _obj.GetComponent<CanvasGroup>();

                    if (cg == null)
                        cg = _obj.AddComponent<CanvasGroup>();

                    cg.interactable = true;
                    cg.blocksRaycasts = true;
                    cg.ignoreParentGroups = true;

                    var stateTf = SurferManager.I.GetObjectStateTransfom(_obj);

                    cg = stateTf.GetComponent<CanvasGroup>();

                    if (cg == null)
                        cg = stateTf.gameObject.AddComponent<CanvasGroup>();


                    cg.interactable = false;
                    cg.blocksRaycasts = true;
                    cg.ignoreParentGroups = true;

                    break;


                case Type_ID.UIGenerics_DisableClickConstraint:

                    var cgg = _obj.GetComponent<CanvasGroup>();

                    if (cgg == null)
                        return;

                    cgg.ignoreParentGroups = false;


                    break;

            }
        }



        void CheckGraphic()
        {

            switch (_reactionID)
            {
                case Type_ID.Graphic_RaycastOn:

                    if (_graphic != null)
                        _graphic.raycastTarget = true;

                    break;

                case Type_ID.Graphic_RaycastOff:

                    if (_graphic != null)
                        _graphic.raycastTarget = false;

                    break;

                case Type_ID.Graphic_SetColor:

                    if (_graphic != null)
                        _graphic.color = _colVal;

                    break;

            }


        }



        void CheckToggle()
        {
            switch (_reactionID)
            {
                case Type_ID.Toggle_True:

                    if (_toggle != null)
                        _toggle.isOn = true;

                    break;

                case Type_ID.Toggle_False:

                    if (_toggle != null)
                        _toggle.isOn = false;

                    break;

                case Type_ID.Toggle_ToggleValue:

                    if (_toggle != null)
                        _toggle.isOn = !_toggle.isOn;

                    break;
            }
        }


        void CheckAudio()
        {
            switch (_reactionID)
            {
                case Type_ID.Audio_PlayClip:

                    SurferHelper.PlaySound(_audioClip, _owner);

                    break;

                case Type_ID.Audio_PlaySource:

                    if(_aSource!=null)
                        _aSource.Play();

                    break;
            }
        }


        void CheckGameObject()
        {

            if (_obj == null)
                return;

                switch (_reactionID)
            {
                case Type_ID.GameObject_SetLayer:

                    _obj.layer = LayerMask.NameToLayer(SurferHelper.SO.Layers[_intVal]);

                    break;

                case Type_ID.GameObject_Disable:

                    _obj.SetActive(false);

                    break;


                case Type_ID.GameObject_Enable:

                    _obj.SetActive(true);

                    break;

                case Type_ID.GameObject_SetTag:

                    _obj.tag = SurferHelper.SO.Tags[_intVal];

                    break;

                case Type_ID.GameObject_SetName:

                    _obj.name = _stringVal;

                    break;
            }
        }


        void CheckCanvasGroup()
        {


            switch (_reactionID)
            {
                case Type_ID.CanvasGroup_Alpha0:

                    if (_cGroup != null)
                        _cGroup.alpha = 0;

                    break;

                case Type_ID.CanvasGroup_Alpha1:

                    if (_cGroup != null)
                        _cGroup.alpha = 1;

                    break;

                case Type_ID.CanvasGroup_IgnoreParentGroupsOff:

                    if (_cGroup != null)
                        _cGroup.ignoreParentGroups = false;

                    break;

                case Type_ID.CanvasGroup_IgnoreParentGroupsOn:

                    if (_cGroup != null)
                        _cGroup.ignoreParentGroups = true;

                    break;

                case Type_ID.CanvasGroup_InteractableOff:

                    if (_cGroup != null)
                        _cGroup.interactable = false;

                    break;

                case Type_ID.CanvasGroup_InteractableOn:

                    if (_cGroup != null)
                        _cGroup.interactable = true;

                    break;

                case Type_ID.CanvasGroup_BlockRaycastOff:

                    if (_cGroup != null)
                        _cGroup.blocksRaycasts = false;

                    break;

                case Type_ID.CanvasGroup_BlockRaycastOn:

                    if (_cGroup != null)
                        _cGroup.blocksRaycasts = true;

                    break;

                case Type_ID.CanvasGroup_SetAlpha:

                    if (_cGroup != null)
                        _cGroup.alpha = _floatVal;

                    break;
            }

        }




    }




}



