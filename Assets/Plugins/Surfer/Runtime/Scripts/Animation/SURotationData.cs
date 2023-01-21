using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Surfer
{

    /// <summary>
    /// Data to setup a Rotation animation by the inspector
    /// </summary>
    [System.Serializable]
    public sealed class SURotationData : SUAnimationData
    {


        [SerializeField]
        Vector3 _from = default, _to = default;

        Vector3 _starting = default;

    
        protected override void OnCache(GameObject go)
        {

            _idTween = RotationPrefix+_transf.GetInstanceID();

            _starting = _transf.localEulerAngles;

        }

        protected override void OnPlay(GameObject go)
        {

            Vector3 _toVal = GetToRotation();
            
            _transf.localEulerAngles = GetFromRotation();
            _transf.DOLocalRotate(_toVal,_duration)
            .SetId(_idTween)
            .SetDelay(Delay)
            .SetEase(_ease,6)
            .SetUpdate(_useUnscaledTime)
            .SetLoops(_looped ? -1 : 0,_loop).Play();

        }


        /// <summary>
        /// Get rotation "from" animation value based on StartMode
        /// </summary>
        /// <returns>rotation value</returns>
        Vector3 GetFromRotation()
        {

            if(_startMode == StartMode_ID.FromStarting)
            return _starting;

            if(_startMode == StartMode_ID.FromCurrent)
            return _transf.localEulerAngles;

            return _from;

        }

        /// <summary>
        /// Get rotation "to" animation value based on EndMode
        /// </summary>
        /// <returns>rotation value</returns>
        Vector3 GetToRotation()
        {

            if(_endMode == EndMode_ID.ToStarting)
            return _starting;

            if(_endMode == EndMode_ID.ToCurrent)
            return _transf.localEulerAngles;

            return _to;

        }


    }



}