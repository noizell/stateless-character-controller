using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Surfer
{

    public struct SUOrientationInfo
    {
        public DeviceOrientation FromOrientation {get;private set;} 
        public DeviceOrientation ToOrientation {get;private set;} 
        public SUOrientationInfo(DeviceOrientation from,DeviceOrientation to)
        {
            FromOrientation = from;
            ToOrientation = to;
        }

    }


}

