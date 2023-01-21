using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    [System.Serializable]
    public abstract class SUCamData 
    {

        public Camera Cam { get; set; } = default;
        public Canvas CamCanvas { get; set; } = default;
        public RectTransform CamCanvasRect { get; set; } = default;

    }

}




