using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    /// <summary>
    /// Data used to easily view/group/setup multiple (DOTween) Animations by the inspector
    /// </summary>
    [System.Serializable]
    public class SUAnimationsData
    {
        
        #region Serialized Fields

        [SerializeField]
        SUPositionData _position = default;
        [SerializeField]
        SUAnchoredPositionData _anchoredPosition = default;
        [SerializeField]
        SURotationData _rotation = default;
        [SerializeField]
        SUScaleData _scale = default;
        [SerializeField]
        SURectSizeData _rectSize = default;
        [SerializeField]
        SUColorData _color = default;
        [SerializeField]
        SUShakeData _shake = default;
        [SerializeField]
        SUJumpData _jump = default;
        [SerializeField]
        SUPunchData _punch = default;
        [SerializeField]
        SUCGroupData _canvasGroup = default;
        [SerializeField]
        SUCharTweenData _charTweener = default;


        #endregion


        /// <summary>
        /// Cache components for all the animations
        /// </summary>
        /// <param name="go">GameObject where to cache the components from</param>
        public void CacheComponents(GameObject go)
        {
            _position.CacheComponents(go);
            _anchoredPosition.CacheComponents(go);
            _rotation.CacheComponents(go);
            _scale.CacheComponents(go);
            _rectSize.CacheComponents(go);
            _color.CacheComponents(go);
            _shake.CacheComponents(go);
            _jump.CacheComponents(go);
            _punch.CacheComponents(go);
            _canvasGroup.CacheComponents(go);
            _charTweener.CacheComponents(go);
        }

        /// <summary>
        /// Play all the animations set up in the inspector
        /// </summary>
        /// <param name="go">GameObject that will play the animations</param>
        public void Play(GameObject go)
        {
            _position.Play(go);
            _anchoredPosition.Play(go);
            _rotation.Play(go);
            _scale.Play(go);
            _rectSize.Play(go);
            _color.Play(go);
            _shake.Play(go);
            _jump.Play(go);
            _punch.Play(go);
            _canvasGroup.Play(go);
            _charTweener.Play(go);
        }


        /// <summary>
        /// Kill all the animations by their id
        /// </summary>
        public void KillAll()
        {
            _position.KillIt();
            _anchoredPosition.KillIt();
            _rotation.KillIt();
            _scale.KillIt();
            _rectSize.KillIt();
            _color.KillIt();
            _shake.KillIt();
            _jump.KillIt();
            _punch.KillIt();
            _canvasGroup.KillIt();
            _charTweener.KillIt();
        }


    }


}


