using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public struct DictCamKey 
    {

        public RenderMode RenderMode { get; private set; }
        public Camera Cam { get; private set; }

        public DictCamKey(RenderMode renderMode, Camera cam)
        {
            RenderMode = renderMode;
            Cam = cam;
        }

    }

}


