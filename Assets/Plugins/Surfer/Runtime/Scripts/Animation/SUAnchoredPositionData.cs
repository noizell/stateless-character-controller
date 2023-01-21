using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Surfer
{
    /// <summary>
    /// Data to setup an AnchoredPosition animation by the inspector
    /// </summary>
    [System.Serializable]
    public sealed class SUAnchoredPositionData : SUAnchoredAnimationData
    {

        protected override void OnPlay(GameObject go)
        {

            if(_rectCp == null)
            return;

            Vector2 _toVal = GetToPosition();
            

            _rectCp.anchoredPosition = GetFromPosition();
            _rectCp.DOAnchorPos(_toVal,_duration)
            .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop)
            .SetDelay(Delay)
            .SetId(_idTween)
            .SetUpdate(_useUnscaledTime)
            .Play();
             
        }


        
    }


}
