using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Surfer
{
    /// <summary>
    /// Data to setup a Punch animation by the inspector
    /// </summary>
    [System.Serializable]
    public sealed class SUPunchData : SUAnimationData
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
        Vector3 _punch = default;

        [SerializeField]
        float _elasticity = default;

        [SerializeField]
        int _vibrato = default;

#endregion

        RectTransform _rectCp = default;


        protected override void OnCache(GameObject go)
        {
            
            _idTween = PunchPrefix +_transf.GetInstanceID();
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

                _rectCp.DOPunchPosition(_punch,_duration,_vibrato,_elasticity)
                .SetId(_idTween)
                .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop)
                .SetDelay(Delay)
                .SetUpdate(_useUnscaledTime)
                .Play();

                break;

                case Mode_ID.Rotation: 

                _rectCp.DOPunchRotation(_punch,_duration,_vibrato,_elasticity)
                .SetId(_idTween)
                .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop)
                .SetDelay(Delay)
                .SetUpdate(_useUnscaledTime)
                .Play();

                break;

                case Mode_ID.Scale: 

                _rectCp.DOPunchScale(_punch,_duration,_vibrato,_elasticity)
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