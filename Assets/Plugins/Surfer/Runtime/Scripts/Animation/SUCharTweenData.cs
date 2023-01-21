using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;




#if SUCharT
using CharTween;
#endif



namespace Surfer
{

    /// <summary>
    /// Data to setup a Shake animation by the inspector
    /// </summary>
    [System.Serializable]
    public sealed class SUCharTweenData : SUAnimationData
    {
        public enum Mode_ID
        {
            None,
            /// <summary>
            /// The text will be split in half and the left side will appear from left and the right one from right
            /// </summary>
            SplitIn,
            /// <summary>
            /// The text will be split in half and the left side will go left and the right one will go right
            /// </summary>
            SplitOut,
            /// <summary>
            /// Simple wave effect. Duration will determine the time required for the characters to make a single "move" up and down.
            /// </summary>
            Wave,
            /// <summary>
            /// As Wave, but looped.
            /// </summary>
            WaveLoop,
            /// <summary>
            /// Shaking characters effect. Duration will determine for how much time the should shake
            /// </summary>
            Shaking,
            /// <summary>
            /// Text characters will be shown with a up&down movement with fade and scale effect. 
            /// </summary>
            JumpIn,
            /// <summary>
            /// Text characters will be hidden with a up&down movement with fade and scale effect. 
            /// </summary>
            JumpOut,
            /// <summary>
            /// Entire Text will be shown with a up&down movement with fade and scale effect. 
            /// </summary>
            JumpInFull,
            /// <summary>
            /// Text characters will be hidden with a up&down movement with fade and scale effect. 
            /// </summary>
            JumpOutFull,
            /// <summary>
            /// Text characters will appear from left and disappear to right, one by one.
            /// </summary>
            LeftToRight,
            /// <summary>
            /// Text characters will appear from right and disappear to left, one by one.
            /// </summary>
            RightToLeft,
        }

        #region Serialized Fields

        [SerializeField]
        Mode_ID _mode = default;

        #endregion

        TextMeshProUGUI _tmp = default;


        protected override void OnCache(GameObject go)
        {
        
            _idTween = CharTweenPrefix + _transf.GetInstanceID();
            _tmp = go.GetComponent<TextMeshProUGUI>();

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

            if (_tmp == null)
                return;

#if SUCharT

            var tweener = _tmp.GetCharTweener();
            var leftOutPos = new Vector3(-5000,0,0);
            var rightOutPos = new Vector3(5000, 0, 0);


            switch (_mode)
            {

                case Mode_ID.SplitIn:


                    for (var i = 0; i < tweener.CharacterCount; ++i)
                    {

                        if (i <= tweener.CharacterCount / 2f)
                        {

                            tweener.SetPositionOffset(i, leftOutPos);

                            tweener.DOLocalMoveX(i, 0, _duration)
                                .SetEase(_ease)
                                .SetDelay(Delay)
                                .SetId(_idTween)
                                .SetUpdate(_useUnscaledTime)
                                .Play();
                        }
                        else
                        {

                            tweener.SetPositionOffset(i, rightOutPos);

                            tweener.DOLocalMoveX(i, 0, _duration)
                                .SetEase(_ease)
                                .SetDelay(Delay)
                                .SetId(_idTween)
                                .SetUpdate(_useUnscaledTime)
                                .Play();
                        }

                    }


                    break;

                case Mode_ID.SplitOut:


                    for (var i = 0; i < tweener.CharacterCount; ++i)
                    {

                        if (i <= tweener.CharacterCount / 2f)
                        {

                            tweener.SetPositionOffset(i, Vector3.zero);

                            tweener.DOLocalMoveX(i, leftOutPos.x, _duration)
                                .SetEase(Ease.Linear)
                                .SetDelay(Delay)
                                .SetId(_idTween)
                                .SetUpdate(_useUnscaledTime)
                                .Play();
                        }
                        else
                        {

                            tweener.SetPositionOffset(i, Vector3.zero);

                            tweener.DOLocalMoveX(i, rightOutPos.x, _duration)
                                .SetEase(Ease.Linear)
                                .SetDelay(Delay)
                                .SetId(_idTween)
                                .SetUpdate(_useUnscaledTime)
                                .Play();
                        }

                    }


                    break;


                case Mode_ID.Wave:

                    for (var i = 0; i < tweener.CharacterCount; ++i)
                    {


                        
                        tweener.DOLocalMoveY(i, 30, _duration/2f)
                        .SetEase(_ease).SetLoops(2, LoopType.Yoyo)
                        .SetDelay(Delay + (.05f * i))
                        .SetId(_idTween)
                        .SetUpdate(_useUnscaledTime)
                        .Play();
                        

                    }

                    break;


                case Mode_ID.WaveLoop:

                    for (var i = 0; i < tweener.CharacterCount; ++i)
                    {



                        tweener.DOLocalMoveY(i, 30, _duration/2f)
                        .SetEase(_ease).SetLoops(-1, LoopType.Yoyo)
                        .SetDelay(Delay + (.05f * i))
                        .SetId(_idTween)
                        .SetUpdate(_useUnscaledTime)
                        .Play();


                    }

                    break;


                case Mode_ID.Shaking:


                    for (var i = 0; i < tweener.CharacterCount; ++i)
                    {

                       
                        tweener.DOShakePosition(i,_duration, 5f, 50, 90, false, false)
                        .SetEase(_ease)
                        .SetDelay(Delay)
                        .SetId(_idTween)
                        .SetUpdate(_useUnscaledTime)
                        .Play();                            

                    }

                    break;


                case Mode_ID.JumpInFull:


                    for (var i = 0; i < tweener.CharacterCount; ++i)
                    {

                        var charSequence = DOTween.Sequence();

                        charSequence.Append(tweener.DOLocalMoveY(i, 25f, _duration/2f).SetEase(_ease))
                            .Join(tweener.DOFade(i, 0, _duration/2f).From())
                            .Join(tweener.DOScale(i, 0, _duration/2f).From().SetEase(Ease.OutBack, 5))
                            .Append(tweener.DOLocalMoveY(i, 0, _duration/2f).SetEase(_ease));

                        charSequence
                           .SetDelay(Delay)
                           .SetId(_idTween)
                           .SetUpdate(_useUnscaledTime)
                           .Play();
                    }


                    break;



                case Mode_ID.JumpIn:


                    for (var i = 0; i < tweener.CharacterCount; ++i)
                    {

                        var charSequence = DOTween.Sequence();
                        charSequence.Append(tweener.DOLocalMoveY(i, 25f, _duration/2f).SetEase(_ease))
                            .Join(tweener.DOFade(i, 0, _duration/2f).From())
                            .Join(tweener.DOScale(i, 0, _duration/2f).From().SetEase(Ease.OutBack, 5))
                            .Append(tweener.DOLocalMoveY(i, 0, _duration/2f).SetEase(_ease));


                        charSequence
                           .SetDelay(Delay + (.05f * i))
                           .SetId(_idTween)
                           .SetUpdate(_useUnscaledTime)
                           .Play();
                    }


                    break;


                case Mode_ID.JumpOut:


                    for (var i = 0; i < tweener.CharacterCount; ++i)
                    {

                        var charSequence = DOTween.Sequence();
                        charSequence.Append(tweener.DOLocalMoveY(i, 25f, _duration / 2f).SetEase(_ease))
                            .Join(tweener.DOFade(i, 0, _duration / 2f))
                            .Join(tweener.DOScale(i, 1.5f, _duration / 2f).SetEase(Ease.OutBack, 5))
                            .Append(tweener.DOLocalMoveY(i, 0, _duration / 2f).SetEase(_ease));

                        charSequence
                           .SetDelay(Delay + (.05f * (tweener.CharacterCount-i)))
                           .SetId(_idTween)
                           .SetUpdate(_useUnscaledTime)
                           .Play();
                    }


                    break;


                case Mode_ID.JumpOutFull:


                    for (var i = 0; i < tweener.CharacterCount; ++i)
                    {

                        var charSequence = DOTween.Sequence();
                        charSequence.Append(tweener.DOLocalMoveY(i, 25f, _duration / 2f).SetEase(_ease))
                            .Join(tweener.DOFade(i, 0, _duration / 2f))
                            .Join(tweener.DOScale(i, 1.5f, _duration / 2f).SetEase(Ease.OutBack, 5))
                            .Append(tweener.DOLocalMoveY(i, 0, _duration / 2f).SetEase(_ease));

                        charSequence
                           .SetDelay(Delay)
                           .SetId(_idTween)
                           .SetUpdate(_useUnscaledTime)
                           .Play();
                    }


                    break;


                case Mode_ID.LeftToRight:


                    for (var i = 0; i < tweener.CharacterCount; ++i)
                    {


                        var charSequence = DOTween.Sequence();

                        tweener.SetPositionOffset(i,leftOutPos);

                        charSequence
                            .Append(tweener.DOLocalMoveX(i, 0, _duration/2f)
                            .SetEase(_ease))
                            .Append(tweener.DOLocalMoveX(i, rightOutPos.x, _duration/2f)
                            .SetEase(_ease).SetDelay(1.25f + (.05f * (tweener.CharacterCount - i))));


                        charSequence
                           .SetDelay(Delay + (.05f * (tweener.CharacterCount - i)))
                           .SetId(_idTween)
                            .SetUpdate(_useUnscaledTime)
                           .Play();
                    }


                    break;


                case Mode_ID.RightToLeft:


                    for (var i = 0; i < tweener.CharacterCount; ++i)
                    {

                        var charSequence = DOTween.Sequence();

                        tweener.SetPositionOffset(i, rightOutPos);

                        charSequence
                            .Append(tweener.DOLocalMoveX(i, 0, _duration/2f)
                            .SetEase(_ease))
                            .Append(tweener.DOLocalMoveX(i, leftOutPos.x, _duration/2f)
                            .SetEase(_ease).SetDelay(1.25f + (.05f * i)));


                        charSequence
                           .SetDelay(Delay + (.05f * i))
                           .SetId(_idTween)
                           .SetUpdate(_useUnscaledTime)
                           .Play();
                    }


                    break;


            }
#endif


        }



    }


}