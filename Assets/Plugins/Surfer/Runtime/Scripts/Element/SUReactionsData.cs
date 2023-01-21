using System.Collections.Generic;
using UnityEngine;

namespace Surfer
{

    /// <summary>
    /// Data to better view/group/setup multiple custom reactions in the inspector
    /// </summary>
    [System.Serializable]
    public class SUReactionsData
    {

        [SerializeField]
        List<SUReactionData> _reactions = new List<SUReactionData>();
        public List<SUReactionData> Reactions { get => _reactions; }


        /// <summary>
        /// Animations made with doTween or charTweener have values like "Starting". This function is called at awake in order to cache object components so that animations 
        /// with those values become possible
        /// </summary>
        /// <param name="go"></param>
        public void CacheAnimations(GameObject go)
        {
            
            foreach(var item in _reactions)
            {
                item.CacheAnimations(go);

            }

        }


        public void KillAnimations()
        {
            
            foreach(var item in _reactions)
            {
                item.KillAnimations();

            }

        }


        public void Play(GameObject go,object evtData)
        {
            for (int i = 0; i < _reactions.Count; i++)
            {
                _reactions[i].Play(go, evtData);
            }
        }

    }
}