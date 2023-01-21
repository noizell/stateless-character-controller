using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Surfer
{

    /// <summary>
    /// Data to setup a Shake animation by the inspector
    /// </summary>
    [System.Serializable]
    public sealed class SUShakeData : SUAnimationData
    {
        public enum Mode_ID
        {
            None,
            Position,
            Rotation,
            Scale
        }

        #region Serialized Fields

        [SerializeField]
        Mode_ID _mode = default;

        [SerializeField]
        Vector3 _shake = default;

        [SerializeField]
        float _randomness = default;

        [SerializeField]
        int _vibrato = default;

        #endregion

        RectTransform _rectCp = default;


        protected override void OnCache(GameObject go)
        {
        
            _idTween = ShakePrefix+_transf.GetInstanceID();
            _rectCp = go.GetComponent<RectTransform>();

        }

        protected override bool IsAvailable
        {
            get
            {
                return _mode != Mode_ID.None;
            }
        }


        protected override void OnPlay(GameObject go)
        {

            if(_rectCp == null)
            return;


            switch(_mode)
            {

                case Mode_ID.Position:

                _rectCp.DOShakePosition(_duration,_shake,_vibrato,_randomness)
                .SetId(_idTween)
                .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop)
                .SetDelay(Delay)
                .SetUpdate(_useUnscaledTime)
                .Play();

                break;

                case Mode_ID.Rotation: 

                _rectCp.DOShakeRotation(_duration,_shake,_vibrato,_randomness)
                .SetId(_idTween)
                .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop)
                .SetDelay(Delay)
                .SetUpdate(_useUnscaledTime)
                .Play();

                break;

                case Mode_ID.Scale: 

                _rectCp.DOShakeScale(_duration,_shake,_vibrato,_randomness)
                .SetId(_idTween)
                .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop)
                .SetDelay(Delay)
                .SetUpdate(_useUnscaledTime)
                .Play();

                break;

            }
            
           
        }

    }


}