using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Surfer
{

    public partial class SUElement : ISUIndicatorDistanceHandler,ISUIndicatorStateHandler, ISUIndicatorVisualHandler
    {
        
        public void OnIndicatorStateUpdate(SUIndicatorEventData eventData)
        {

            if (eventData.Data.State == SUIndicatorState_ID.FollowingOffScreen)
            {
                RunEventBehaviour(SUEvent.Type_ID.Indicator_OnStartOffScreenMode,eventData);
            }
            else if (eventData.Data.State == SUIndicatorState_ID.FollowingOnScreen)
            {
                RunEventBehaviour(SUEvent.Type_ID.Indicator_OnStartOnScreenMode,eventData);
            }
            else if (eventData.Data.State == SUIndicatorState_ID.Standby)
            {
                RunEventBehaviour(SUEvent.Type_ID.Indicator_OnStandby,eventData);
            }

        }

        public void OnIndicatorDistanceUpdate(SUIndicatorEventData eventData)
        {

            if (eventData.Data.DistanceState == SUIndicatorDistanceState_ID.Close)
            {
                RunEventBehaviour(SUEvent.Type_ID.Indicator_OnClose,eventData);
            }
            else if(eventData.Data.DistanceState == SUIndicatorDistanceState_ID.Far)
            {
                RunEventBehaviour(SUEvent.Type_ID.Indicator_OnFar,eventData);
            }
            else if (eventData.Data.DistanceState == SUIndicatorDistanceState_ID.CloseAndCentered)
            {
                RunEventBehaviour(SUEvent.Type_ID.Indicator_OnCloseAndCentered,eventData);
            }

        }


        public SUIndicatorVisualData OnIndicatorVisualSetUp()
        {
            return ElementData.IndVisualData;
        }
    }


}

