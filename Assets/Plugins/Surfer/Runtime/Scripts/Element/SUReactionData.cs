using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



namespace Surfer
{

    /// <summary>
    /// Data to setup a custom reaction in the inspector 
    /// </summary>
    [System.Serializable]
    public partial class SUReactionData
    {

        [SerializeField]
        string _key ;
        public string Key{get=>_key;}
    
        [SerializeField]
        string _name ;
        public string Name{get=>_name;}

        [SerializeField]
        SUFieldValuesData _fieldsValues = new SUFieldValuesData();
        public SUFieldValuesData FieldsValues { get => _fieldsValues; set => _fieldsValues = value; }


        #region Tween animations

        [SerializeField]
        SUPositionData _position = default;
        public SUPositionData PositionAnim { get => _position; }
        [SerializeField]
        SUAnchoredPositionData _anchoredPosition = default;
        public SUAnchoredPositionData AnchoredPositionAnim { get => _anchoredPosition; }
        [SerializeField]
        SURotationData _rotation = default;
        public SURotationData RotationAnim { get => _rotation; }
        [SerializeField]
        SUScaleData _scale = default;
        public SUScaleData ScaleAnim { get => _scale; }
        [SerializeField]
        SURectSizeData _rectSize = default;
        public SURectSizeData RectSizeAnim { get => _rectSize; }
        [SerializeField]
        SUColorData _color = default;
        public SUColorData ColorAnim { get => _color; }
        [SerializeField]
        SUShakeData _shake = default;
        public SUShakeData ShakeAnim { get => _shake; }
        [SerializeField]
        SUJumpData _jump = default;
        public SUJumpData JumpAnim { get => _jump; }
        [SerializeField]
        SUPunchData _punch = default;
        public SUPunchData PunchAnim { get => _punch; }
        [SerializeField]
        SUCGroupData _canvasGroup = default;
        public SUCGroupData CanvasGroupAnim { get => _canvasGroup; }
        [SerializeField]
        SUCharTweenData _charTweener = default;
        public SUCharTweenData CharTweenerAnim { get => _charTweener; }


        #endregion


        [SerializeField]
        SUStateData _stateData = default;
        public SUStateData StateData { get => _stateData; set => _stateData = value; }

        [SerializeField]
        SUSceneData _sceneData = default;
        public SUSceneData SceneData { get => _sceneData; }


        [SerializeField]
        SUCustomEventData _customEData = default;
        public SUCustomEventData CustomEData { get => _customEData; }



        bool IsValid
        {
            get
            {
                return !Key.Equals(SurferHelper.Unset)
                    && !Name.Equals(SurferHelper.Unset);
            }
        }


        public SUReactionData(string key,string name)
        {
            _key = key;
            _name = name;
        }

        public SUReactionData(string key)
        {
            _key = key;
            _name = DefaultCustomReactions.GetName(key);
        }


        /// <summary>
        /// Animations made with doTween or charTweener have values like "Starting". This function is called at awake in order to cache object components so that animations 
        /// with those values become possible
        /// </summary>
        /// <param name="go"></param>
        public void CacheAnimations(GameObject go)
        {
            if (!IsValid)
                return;

            _position?.CacheComponents(go);
            _anchoredPosition?.CacheComponents(go);
            _rotation?.CacheComponents(go);
            _scale?.CacheComponents(go);
            _rectSize?.CacheComponents(go);
            _color?.CacheComponents(go);
            _shake?.CacheComponents(go);
            _jump?.CacheComponents(go);
            _punch?.CacheComponents(go);
            _canvasGroup?.CacheComponents(go);
            _charTweener?.CacheComponents(go);
        }


        public void KillAnimations()
        {
            if (!IsValid)
                return;

            _position?.KillIt();
            _anchoredPosition?.KillIt();
            _rotation?.KillIt();
            _scale?.KillIt();
            _rectSize?.KillIt();
            _color?.KillIt();
            _shake?.KillIt();
            _jump?.KillIt();
            _punch?.KillIt();
            _canvasGroup?.KillIt();
            _charTweener?.KillIt();
        }

        public void Play(GameObject caller,object evtData)
        {
            if (!IsValid)
                return;

            CustomReactionsExtensions.PlayReaction(_key, caller, this, evtData);
        }

    }

    

}


