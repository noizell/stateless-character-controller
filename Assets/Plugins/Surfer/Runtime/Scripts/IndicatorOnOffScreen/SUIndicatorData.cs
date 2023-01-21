using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Surfer
{

    public enum SUIndicatorState_ID
    {
        None,
        FollowingOnScreen,
        FollowingOffScreen,
        Standby
    }


    public enum SUIndicatorDistanceState_ID
    {
        None,
        Far,
        Close,
        CloseAndCentered
    }



    [System.Serializable]
    public class SUIndicatorData : SUItemLinkData
    {

        public enum Type_ID
        {
            OnScreen,
            OffScreen,
            Both
        }

        /// <summary>
        /// The player/character object used to check the distance with the target indicated
        /// </summary>
        [SerializeField]
        Transform _playerObj = default;
        public Transform PlayerObj { get => _playerObj; }


        /// <summary>
        /// Radius within which the playerObj (if any) is considered to be close to the target
        /// </summary>
        [SerializeField]
        float _radius = default;
        public float Radius { get => _radius; }

        [SerializeField]
        Type_ID _type = default;
        public Type_ID Type { get => _type; }
        public bool IsOnScreenType
        {
            get
            {
                return _type == Type_ID.Both || _type == Type_ID.OnScreen;
            }
        }
        public bool IsOffScreenType
        {
            get
            {
                return _type == Type_ID.Both || _type == Type_ID.OffScreen;
            }
        }


        public SUIndicatorState_ID State { get; set; } = SUIndicatorState_ID.None;
        public SUIndicatorDistanceState_ID DistanceState { get; set; } = SUIndicatorDistanceState_ID.None;

        public SUIndicatorVisualData VisualData { get; set; }

        public int LinkID { get; set; }


        public SUIndicatorData() { }
        public SUIndicatorData(GameObject prefab,Camera cam, Type_ID type, RenderMode renderMode = RenderMode.ScreenSpaceOverlay, string customTag = "" , Vector2 onScreenOffset = default(Vector2), Transform playerObj = null, float radius = 0)
        {

            _prefab = prefab;
            _cam = cam;
            _type = type;
            _customTag = customTag;
            _onScreenOffset = onScreenOffset;
            _playerObj = playerObj;
            _radius = radius;
            _renderMode = renderMode;

        }

    }

}



