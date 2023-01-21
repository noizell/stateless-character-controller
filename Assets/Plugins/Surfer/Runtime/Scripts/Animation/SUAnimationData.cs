using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Surfer
{

    /// <summary>
    /// Data used to setup (DOTween) Animations by the inspector
    /// </summary>
    [System.Serializable]
    public abstract class SUAnimationData
    {

#region  Enums

        public enum StartMode_ID
        {
            None,
            /// <summary>
            /// From the current values
            /// </summary>
            FromCurrent,
            /// <summary>
            /// From the values cached during Awake (if CacheComponents method is used )
            /// </summary>
            FromStarting,
            /// <summary>
            /// From the values setup in the inspector
            /// </summary>
            From,
            /// <summary>
            /// From the left (out) side of the parent rect
            /// </summary>
            FromLeft,
            /// <summary>
            /// From the bottom (out) side of the parent rect
            /// </summary>
            FromBottom,
            /// <summary>
            /// From the right (out) side of the parent rect
            /// </summary>
            FromRight,
            /// <summary>
            /// From the top (out) side of the parent rect
            /// </summary>
            FromTop,
            /// <summary>
            /// From the center of the parent rect
            /// </summary>
            FromCenter
        }

        public enum EndMode_ID
        {
            /// <summary>
            /// To the current values (the ones before starting the whole animation)
            /// </summary>
            ToCurrent,
            /// <summary>
            /// To the values cached during Awake (if CacheComponents method is used )
            /// </summary>
            ToStarting,
            /// <summary>
            /// To the values setup in the inspector
            /// </summary>
            To,
            /// <summary>
            /// To the left (out) side of the parent rect
            /// </summary>
            ToLeft,
            /// <summary>
            /// To the bottom (out) side of the parent rect
            /// </summary>
            ToBottom,
            /// <summary>
            /// To the right (out) side of the parent rect
            /// </summary>
            ToRight,
            /// <summary>
            /// To the top (out) side of the parent rect
            /// </summary>
            ToTop,
            /// <summary>
            /// To the center of the parent rect
            /// </summary>
            ToCenter
        }


        public enum DelayMode_ID
        {
            /// <summary>
            /// Uses the delay setup in the inspector
            /// </summary>
            Normal,
            /// <summary>
            /// Uses the delay setup in the inspector multiplied by the sibling index of the object
            /// </summary>
            WithSiblingIndex,/// <summary>
            /// Uses the delay setup in the inspector multiplied by the sibling index of the object
            /// (sibling indexes will be shifted starting from 1 )
            /// </summary>
            WithSiblingIndexShifted,
        }

        #endregion


#region Serialized Fields


        [SerializeField]
        protected StartMode_ID _startMode = default;

        [SerializeField]
        protected EndMode_ID _endMode = default;

        [SerializeField]
        [Min(0)]
        protected float _duration = default, _delay = default;

        [SerializeField]
        protected DelayMode_ID _delayMode = default;

        [SerializeField]
        protected Ease _ease;

        [SerializeField]
        protected bool _looped = default;

        [SerializeField]
        protected LoopType _loop;

        [SerializeField]
        [Tooltip("Who will play this animation?")]
        GameObject _obj = default;

        [SerializeField]
        protected bool _useUnscaledTime = default;

        #endregion

        protected Transform _transf;
        protected string _idTween = default;
        protected bool _alreadyCached = false;
        protected Tween _tween = default;

        /// <summary>
        /// Check if animation can be played (i.e. not set to None)
        /// </summary>
        protected virtual bool IsAvailable 
        {
            get
            {
                return _startMode != StartMode_ID.None;
            }
        }


        public static Dictionary<string, Tween> AllPlaying = new Dictionary<string, Tween>();
        public const string CGroupPrefix = "_cg";
        public const string ColorPrefix = "_col";
        public const string JumpPrefix = "_po";
        public const string PositionPrefix = "_po";
        public const string PunchPrefix = "_pu";
        public const string RectSizePrefix = "_rec";
        public const string RotationPrefix = "_rot";
        public const string ScalePrefix = "_sc";
        public const string ShakePrefix = "_sh";
        public const string CharTweenPrefix = "_charT";


        /// <summary>
        /// Cache all the components need to play the animation
        /// </summary>
        /// <param name="go">GameObject where to cache the components from</param>
        public void CacheComponents(GameObject go)
        {
            if (!IsAvailable)
                return;
            if (_alreadyCached)
                return;

            if (_obj == null)
                _obj = go;

            _transf = _obj.transform;

            OnCache(_obj);

            _alreadyCached = true;

                
        }

        protected virtual void OnCache(GameObject go) { }


        /// <summary>
        /// Play animation set up in the inspector
        /// </summary>
        /// <param name="go">GameObject that will play the animation</param>
        public void Play(GameObject go)
        {
            if(!IsAvailable)
            return;

            if (_obj == null)
                _obj = go;

            CacheComponents(_obj);

            OnPlay(_obj);

            if(_tween != null)
            {
                _tween.OnPlay(() =>
                {
                    if (AllPlaying.TryGetValue(_idTween, out Tween value))
                    {

                        value.Kill();
                        AllPlaying.Remove(_idTween);

                    }

                    AllPlaying.Add(_idTween, _tween);
                })
                .OnComplete(() =>
                {
                    AllPlaying.Remove(_idTween);
                });
            }

        }

        protected virtual void OnPlay(GameObject go) { }


        /// <summary>
        /// Adjusted delay based (or not) on the sibling index of the gameObject
        /// </summary>
        public float Delay
        {
            get
            {
                if(_delayMode == DelayMode_ID.WithSiblingIndex)
                    return _delay * _transf.GetSiblingIndex();
                if(_delayMode == DelayMode_ID.WithSiblingIndexShifted)
                return _delay * (_transf.GetSiblingIndex()+1);

                return _delay;
            }
        }

        /// <summary>
        /// Kill the tween by using its id
        /// </summary>
        public void KillIt()
        {
            if(!IsAvailable)
                return;

            DOTween.Kill(_idTween);
        }

        protected void OnPlayTweenLogic()
        {
            if (AllPlaying.TryGetValue(_idTween, out Tween value))
            {

                value.Kill();
                AllPlaying.Remove(_idTween);

            }

            AllPlaying.Add(_idTween, _tween);
        }

        protected void OnCompleteTweenLogic()
        {

            AllPlaying.Remove(_idTween);
        }

    }

}

