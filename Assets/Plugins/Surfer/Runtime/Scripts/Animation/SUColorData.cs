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
    /// Data to setup a Color animation by the inspector
    /// </summary>
    [System.Serializable]
    public sealed class SUColorData : SUAnimationData
    {



        [SerializeField]
        Color _from = default,_to = default;
        

        //private fields
        Color _starting = default;
        Graphic _graCp = default;
        Renderer _rend = default;


        protected override void OnCache(GameObject go)
        {

            _idTween = ColorPrefix + _transf.GetInstanceID();

            if(_graCp == null && _rend == null)
            {
                _graCp = go.GetComponent<Graphic>();
            
                if(_graCp != null)
                    _starting = _graCp.color;
                else
                {
                    _rend = go.GetComponent<Renderer>();
                    if(_rend!=null)
                        _starting = _rend.material.color;
                }
            }

        }

        protected override void OnPlay(GameObject go)
        {

            if(_rend == null)
                Play(_graCp);
            else
                Play(_rend);
        }

        
        /// <summary>
        /// Play the color animation on a generic Renderer component
        /// </summary>
        /// <param name="rend">the object renderer</param>
        void Play(Renderer rend)
        {
            if(rend == null)
            return;

            Color _toVal = GetToColor(true);

            _rend.material.color = GetFromColor(true);
            _rend.material.DOColor(_toVal,_duration)
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
        /// Play the color animation on a generic Graphic UI component
        /// </summary>
        /// <param name="rend">the object graphic</param>
        void Play(Graphic graph)
        {
            if(graph == null)
            return;

            Color _toVal = GetToColor(false);

            graph.color = GetFromColor(false);
            graph.DOColor(_toVal,_duration)
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
        /// Get color "from" animation value based on StartMode
        /// </summary>
        /// <param name="renderer">if the object has a renderer or not</param>
        /// <returns>color value</returns>
        Color GetFromColor(bool renderer)
        {

            if(_startMode == StartMode_ID.FromStarting)
            return _starting;

            if(_startMode == StartMode_ID.FromCurrent)
            return renderer ? _rend.material.color : _graCp.color;

            return _from;

        }

        /// <summary>
        /// Get color "to" animation value based on EndMode
        /// </summary>
        /// <param name="renderer">if the object has a renderer or not</param>
        /// <returns>color value</returns>
        Color GetToColor(bool renderer)
        {

            if(_endMode == EndMode_ID.ToStarting)
            return _starting;

            if(_endMode == EndMode_ID.ToCurrent)
            return renderer ? _rend.material.color : _graCp.color;

            return _to;

        }


        

    }



}