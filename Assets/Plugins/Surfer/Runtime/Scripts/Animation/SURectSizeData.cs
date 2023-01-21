using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.RectTransform;
using UnityEngine.Events;

namespace Surfer
{

    /// <summary>
    /// Data to setup a Rect Size animation by the inspector
    /// </summary>
    [System.Serializable]
    public sealed class SURectSizeData : SUAnimationData
    {

        public enum Mode_ID
        {
            None,
            /// <summary>
            /// To the sizeDelta cached during Awake (if CacheComponents method is used )
            /// </summary>
            ToStarting,
            /// <summary>
            /// Multiply the starting sizeDelta (if CacheComponents method is used 
            /// </summary>
            Multiply,
            /// <summary>
            /// The sizeDelta width will be the same as the parent one
            /// </summary>
            XFitX,
            /// <summary>
            /// The sizeDelta width will be the same as the parent sizeDelta height
            /// </summary>
            XFitY,
            /// <summary>
            /// The sizeDelta height will be the same as the parent one
            /// </summary>
            YFitY,
            /// <summary>
            /// The sizeDelta height will be the same as the parent width
            /// </summary>
            YFitX,
        }
        
        #region Serialized Fields

        [SerializeField]
        Mode_ID _mode = Mode_ID.None;

        [SerializeField]
        Vector2 _multi = Vector2.one;

        #endregion


        RectTransform _rectCp = default;
        Vector3 _starting = default;
        RectTransform _parentRect = default;


        protected override void OnCache(GameObject go)
        {

            _idTween = RectSizePrefix +_transf.GetInstanceID();

            _rectCp = go.GetComponent<RectTransform>();
            _parentRect = SurferHelper.GetParentRect(go.transform);

            if(_rectCp == null)
            return;

            _starting = _rectCp.sizeDelta;

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
            if(_parentRect == null)
            return;


            switch(_mode)
            {

                case Mode_ID.ToStarting:

                _rectCp.DOSizeDelta(_starting,_duration)
                .SetId(_idTween)
                .SetDelay(_delay)
                .SetUpdate(_useUnscaledTime)
                .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop).Play();

                break;

                case Mode_ID.Multiply:

                _rectCp.DOSizeDelta(_starting*_multi,_duration)
                .SetId(_idTween)
                .SetDelay(_delay)
                .SetUpdate(_useUnscaledTime)
                .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop).Play();

                break;

                case Mode_ID.XFitX:

                float value = _rectCp.rect.size.x;

                DOTween.To(()=>value,x=>value=x,_parentRect.rect.size.x,_duration).OnUpdate(()=>
                {
                    _rectCp.SetSizeWithCurrentAnchors(Axis.Horizontal,value);
                })
                .SetId(_idTween)
                .SetDelay(_delay)
                .SetUpdate(_useUnscaledTime)
                .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop).Play();

                break;

                case Mode_ID.XFitY:

                float val2 = _rectCp.rect.size.x;

                DOTween.To(()=>val2,x=>val2=x,_parentRect.rect.size.y,_duration).OnUpdate(()=>
                {
                    _rectCp.SetSizeWithCurrentAnchors(Axis.Horizontal,val2);
                })
                .SetId(_idTween)
                .SetDelay(_delay)
                .SetUpdate(_useUnscaledTime)
                .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop).Play();

                break;

                case Mode_ID.YFitY:

                float val3 = _rectCp.rect.size.y;

                DOTween.To(()=>val3,x=>val3=x,_parentRect.rect.size.y,_duration).OnUpdate(()=>
                {
                    _rectCp.SetSizeWithCurrentAnchors(Axis.Vertical,val3);
                })
                .SetId(_idTween)
                .SetDelay(_delay)
                .SetUpdate(_useUnscaledTime)
                .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop).Play();

                break;

                case Mode_ID.YFitX:

                float val4 = _rectCp.rect.size.y;

                DOTween.To(()=>val4,x=>val4=x,_parentRect.rect.size.x,_duration).OnUpdate(()=>
                {
                    _rectCp.SetSizeWithCurrentAnchors(Axis.Vertical,val4);
                })
                .SetId(_idTween)
                .SetDelay(_delay)
                .SetUpdate(_useUnscaledTime)
                .SetEase(_ease,6).SetLoops(_looped ? -1 : 0,_loop).Play();

                break;

            }

            
        }


    }


}