using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public enum SUHealthBarState_ID
    {
        None,
        Started,
        Stopped
    }

    public enum SUHealthBarHPState_ID
    {
        None,
        Full,
        MoreThanHalf,
        LessThanHalf,
        Empty
    }


    [System.Serializable]
    public class SUHealthBarData : SUItemLinkData
    {

        public SUHealthBarVisualData VisualData { get; set; }
        public SUHealthBarState_ID State { get; set; }
        public SUHealthBarHPState_ID HPState { get; set; }


        public SUHealthBarData() { }
        public SUHealthBarData(GameObject prefab,Camera cam,RenderMode renderMode = RenderMode.ScreenSpaceOverlay,string customTag = "",Vector2 onScreenOffset = default(Vector2))
        {

            _prefab = prefab;
            _cam = cam;
            _customTag = customTag;
            _onScreenOffset = onScreenOffset;
            _renderMode = renderMode;

        }

    }


}
