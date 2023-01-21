using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Surfer
{

    [System.Serializable]
    public class SUIndicatorVisualData
    {
        /// <summary>
        /// TextMeshPro component that will show the distance between the playerObject (if any) and the targetObject
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _distanceText = default;
        public TextMeshProUGUI DistanceText { get => _distanceText; }

        /// <summary>
        /// Suffix for the distance text (like meters,feet...)
        /// </summary>
        [SerializeField]
        string _suffix = default;
        public string Suffix { get => _suffix; }

        /// <summary>
        /// The UI (transform) object that will rotate to indicate the target (like an arrow for example)
        /// </summary>
        [SerializeField]
        Transform _rotationObj = default;
        public Transform RotationObj { get => _rotationObj; }


        /// <summary>
        /// Angle offset of the rotationObj
        /// </summary>
        [SerializeField]
        float _angleOffset = default;
        public float AngleOffset { get => _angleOffset; }

    }

}

