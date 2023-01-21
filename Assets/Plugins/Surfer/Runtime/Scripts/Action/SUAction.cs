using System.Collections.Generic;
using UnityEngine;

namespace Surfer
{


    /// <summary>
    /// Monobehaviour representation of the SUActionData with multiple entries
    /// </summary>
    public class SUAction : MonoBehaviour
    {
        [SerializeField]
        bool _playOnAwake = true;

        [SerializeField]
        List<SUActionData> _actions = new List<SUActionData>();


        
        void Awake()
        {
            if(_playOnAwake)
            Play();
        }

        /// <summary>
        /// Play the action setup in the inspector
        /// </summary>
        public void Play()
        {
            _actions.ForEach((x)=>x.Play(gameObject));
        }

    }


}


