using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Surfer
{
    /// <summary>
    /// Data to setup a Position animation by the inspector
    /// </summary>
    [System.Serializable]
    public sealed class SUPositionData : SUScreenAnimationData
    {

        protected override void OnPlay(GameObject go)
        {

            if(_rectCp == null)
            return;


            Vector2 _toVal = GetToPosition();
            

            _rectCp.localPosition = GetFromPosition();
            _rectCp.DOLocalMove(_toVal,_duration)
            .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop)
            .SetDelay(Delay)
            .SetId(_idTween)
            .SetUpdate(_useUnscaledTime)
            .Play();
             
        }


        
    }


}
