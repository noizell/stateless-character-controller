using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public class SUIndicatorLink : MonoBehaviour
    {

        [SerializeField]
        bool _startOnAwake = default;

        [SerializeField]
        SUIndicatorLinkData _data = default;
        public SUIndicatorLinkData Data { get => _data; }


        private void Awake()
        {

            if(_startOnAwake)
                _data.StartFollow();

        }


    }

}
