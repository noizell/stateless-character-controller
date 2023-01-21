using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Surfer
{

    public partial class SUElement : ISUOrientationHandler
    {


        void CheckOrientationEvents()
        {
            if(_events.ContainsKey(SUEvent.Type_ID.Orientation_ToLandscapeLeft)
            || _events.ContainsKey(SUEvent.Type_ID.Orientation_ToLandscapeRight)
            || _events.ContainsKey(SUEvent.Type_ID.Orientation_ToPortrait)
            || _events.ContainsKey(SUEvent.Type_ID.Orientation_ToPortraitUpsideDown))
            {
                SUOrientationManager.I?.RegisterOrientationEvent(this);
            }

        }


        public void OnOrientationChanged(SUOrientationInfo info)
        {
            if(info.ToOrientation == DeviceOrientation.Portrait)
            {
                RunEventBehaviour(SUEvent.Type_ID.Orientation_ToPortrait,info);
            }
            else if(info.ToOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                RunEventBehaviour(SUEvent.Type_ID.Orientation_ToPortraitUpsideDown,info);
            }
            else if(info.ToOrientation == DeviceOrientation.LandscapeLeft)
            {
                RunEventBehaviour(SUEvent.Type_ID.Orientation_ToLandscapeLeft,info);
            }
            else if(info.ToOrientation == DeviceOrientation.LandscapeRight)
            {
                RunEventBehaviour(SUEvent.Type_ID.Orientation_ToLandscapeRight,info);
            }
        }
        
        void ResetOrientationEvents()
        {

            if(_events.ContainsKey(SUEvent.Type_ID.Orientation_ToLandscapeLeft)
            || _events.ContainsKey(SUEvent.Type_ID.Orientation_ToLandscapeRight)
            || _events.ContainsKey(SUEvent.Type_ID.Orientation_ToPortrait)
            || _events.ContainsKey(SUEvent.Type_ID.Orientation_ToPortraitUpsideDown))
            {
                SUOrientationManager.I?.UnregisterOrientationEvent(this);
            }

        }
        
        
    }


}
