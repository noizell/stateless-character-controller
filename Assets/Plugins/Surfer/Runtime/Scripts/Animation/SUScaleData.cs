using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Surfer
{

    /// <summary>
    /// Data to setup a Scale animation by the inspector
    /// </summary>
    [System.Serializable]
    public sealed class SUScaleData : SUAnimationData
    {

        [SerializeField]
        Vector3 _from = Vector3.one , _to = Vector3.one;

        //private fields
        Vector3 _starting = default;

        protected override void OnCache(GameObject go)
        {
            _idTween = ScalePrefix+_transf.GetInstanceID();
            
            _starting = go.transform.localScale;

        }


        protected override void OnPlay(GameObject go)
        {

            Vector3 _toVal = GetToScale();

            go.transform.localScale = GetFromScale();
            _tween = go.transform.DOScale(_toVal,_duration)
            .SetId(_idTween)
            .SetDelay(Delay)
            .SetEase(_ease,6)
            .SetUpdate(_useUnscaledTime)
            .SetLoops(_looped ? -1 : 0,_loop).Play();


        }

        /// <summary>
        /// Get scale "from" animation value based on StartMode
        /// </summary>
        /// <returns>scale value</returns>
        Vector3 GetFromScale()
        {

            if(_startMode == StartMode_ID.FromStarting)
            return _starting;

            if(_startMode == StartMode_ID.FromCurrent)
            return _transf.localScale;

            return _from;

        }

        /// <summary>
        /// Get scale "to" animation value based on EndMode
        /// </summary>
        /// <returns>scale value</returns>
        Vector3 GetToScale()
        {

            if(_endMode == EndMode_ID.ToStarting)
            return _starting;

            if(_endMode == EndMode_ID.ToCurrent)
            return _transf.localScale;

            return _to;

        }

    }


}