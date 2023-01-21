using Animancer;
using System.Collections.Generic;
using UnityEngine;

namespace STVR.SMH.Characters.Controllers
{
    public class AnimationHandler
    {
        [System.Serializable]
        public struct AnimationClips
        {
            public AnimationClip Clip;
            public string ClipName;

            public AnimationClips(AnimationClip clip, string clipName)
            {
                Clip = clip;
                ClipName = clipName;
            }
        }

        private AnimancerComponent animancer;
        Dictionary<string, AnimationClip> clipDict = new Dictionary<string, AnimationClip>();
        private AnimancerState currentState;

        public AnimationHandler(AnimancerComponent component, params AnimationClips[] clips)
        {
            animancer = component;
            for (int i = 0; i < clips.Length; i++)
            {
                clipDict.Add(clips[i].ClipName, clips[i].Clip);
            }
        }

        public int CurrentFrame()
        {
            if (currentState == null)
                return 0;

            return (int)(currentState.NormalizedTime * (currentState.Clip.length * currentState.Clip.frameRate));
        }

        public void UpdateAnimation(CharacterState states)
        {
            Crossfade(StateToClipName(states));
        }

        private void Play(string nameclip)
        {
            if (!animancer.IsPlayingClip(clipDict[nameclip]))
                animancer.Play(clipDict[nameclip]);
        }

        private void Crossfade(string nameclip)
        {
            currentState = animancer.Playable.States.GetOrCreate(clipDict[nameclip]);
            if (!animancer.IsPlayingClip(clipDict[nameclip]))
                animancer.Play(clipDict[nameclip], .28f);
        }

        private string StateToClipName(CharacterState state)
        {
            switch (state)
            {
                case CharacterState.Run:
                    return "Run";

                case CharacterState.Attack:
                    return "Attack 1";

                case CharacterState.Attack2:
                    return "Attack 2";

                case CharacterState.Attack3:
                    return "Attack 3";

                case CharacterState.Attack4:
                    return "Attack 4";
                
                case CharacterState.Jump:
                    return "Jump";

                default:
                    return "Idle";
            }
        }
    }

}