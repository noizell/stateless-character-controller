using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public static class SUHealthBarDataExtensions
    {


        public static void CallStateUpdate(this SUHealthBarData data, SUHealthBarState_ID newState)
        {

            if (data == null)
                return;
            if (data.RectT == null)
                return;

            data.State = newState;
            data.RectT.GetComponent<ISUHealthBarStateHandler>()?.OnHealthBarStateUpdate(new SUHealthBarEventData(data));

        }


        public static void CallHPStateUpdate(this SUHealthBarData data, SUHealthBarHPState_ID newHPState)
        {

            if (data == null)
                return;
            if (data.RectT == null)
                return;

            data.HPState = newHPState;
            data.RectT.GetComponent<ISUHealthBarHPStateHandler>()?.OnHealthBarHPStateUpdate(new SUHealthBarEventData(data));

        }


    }


}

