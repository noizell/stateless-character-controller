using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.RectTransform;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Surfer
{

    /// <summary>
    /// Data to setup a Canvas Group animation by the inspector
    /// </summary>
    [System.Serializable]
    public sealed class SUCGroupData : SUAnimationData
    {
        
        
        [SerializeField]
        [Range(0f,1f)]
        float _from = default, _to = default, _starting = default;

        CanvasGroup _cgCp = default;
        

        protected override void OnCache(GameObject go)
        {
            _idTween = CGroupPrefix + _transf.GetInstanceID();
            _cgCp = go.GetComponent<CanvasGroup>();
            _starting = _cgCp.alpha;
        }

        protected override void OnPlay(GameObject go)
        {

            if(_cgCp == null)
            return;


            float _toVal = GetToAlpha();

            _cgCp.alpha = GetFromAlpha();
            _cgCp.DOFade(_toVal,_duration)
            .OnPlay(() =>
            {
                OnPlayTweenLogic();
            })
            .OnComplete(() =>
            {
                OnCompleteTweenLogic();
            })
            .SetId(_idTween)
            .SetDelay(Delay)
            .SetEase(_ease,6)
            .SetUpdate(_useUnscaledTime)
            .SetLoops(_looped ? -1 : 0,_loop).Play();
            
        }

        /// <summary>
        /// Get alpha "from" animation value based on StartMode
        /// </summary>
        /// <returns>alpha value</returns>
        float GetFromAlpha()
        {

            if(_startMode == StartMode_ID.FromStarting)
            return _starting;

            if(_startMode == StartMode_ID.FromCurrent)
            return _cgCp.alpha;

            return _from;

        }

        /// <summary>
        /// Get alpha "to" animation value based on EndMode
        /// </summary>
        /// <returns>alpha value</returns>
        float GetToAlpha()
        {

            if(_endMode == EndMode_ID.ToStarting)
            return _starting;

            if(_endMode == EndMode_ID.ToCurrent)
            return _cgCp.alpha;

            return _to;

        }


    }


}