using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Surfer
{

    [System.Serializable]
    public abstract class SULinkData
    {
        /// <summary>
        /// The target object to follow
        /// </summary>
        [SerializeField]
        protected Transform _target = default;
        public Transform Target { get => _target; }

    }


}
