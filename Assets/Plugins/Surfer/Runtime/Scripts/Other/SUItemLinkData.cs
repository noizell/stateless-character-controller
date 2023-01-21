using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{
    [System.Serializable]
    public abstract class SUItemLinkData
    {


        [SerializeField]
        protected string _customTag = default;
        public string CustomTag { get => _customTag; }

        /// <summary>
        /// UI prefab that should follow the target
        /// </summary>
        [SerializeField]
        protected GameObject _prefab = default;
        public GameObject Prefab { get => _prefab; }

        /// <summary>
        /// Camera of the UI prefab ( YOU MUST NOT SET THIS PROPERTY! )
        /// </summary>
        [SerializeField]
        protected Camera _cam = default;
        public Camera Cam { get => _cam; set => _cam = value; }

        [SerializeField]
        protected RenderMode _renderMode = default;
        public RenderMode RenderMode { get => _renderMode; }

        [SerializeField]
        protected Vector2 _onScreenOffset = default;
        public Vector2 OnScreenOffset { get => _onScreenOffset; }

        /// <summary>
        /// RectTransform of the UI prefab ( YOU MUST NOT SET THIS PROPERTY! )
        /// </summary>
        public RectTransform RectT { get; set; }
        /// <summary>
        /// Target to follow ( YOU MUST NOT SET THIS PROPERTY! )
        /// </summary>
        public Transform Target { get; set; }

    }

}


