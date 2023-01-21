using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Surfer
{

    /// <summary>
    /// Data to setup a Jump animation by the inspector
    /// </summary>
    [System.Serializable]
    public sealed class SUJumpData : SUScreenAnimationData
    {


        [SerializeField]
        float _power = default;

        protected override void OnPlay(GameObject go)
        {

            if(_rectCp == null)
            return;

            Vector2 _toVal = GetToPosition();
            
            _rectCp.localPosition = GetFromPosition();
            _rectCp.DOLocalJump(_toVal,_power,1,_duration)
            .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop)
            .SetDelay(Delay)
            .SetId(_idTween)
            .SetUpdate(_useUnscaledTime)
            .Play();
             
        }



    }


}