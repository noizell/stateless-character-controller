using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public class SUHealthBarLink : MonoBehaviour
    {

        [SerializeField]
        bool _startOnAwake = default;

        [SerializeField]
        SUHealthBarLinkData _data = default;
        public SUHealthBarLinkData Data { get => _data; }


        private void Awake()
        {

            if (_startOnAwake)
                _data.StartFollow();
        }

    }


}


