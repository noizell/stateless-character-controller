using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public interface ISUIndicatorStateHandler
    {
        void OnIndicatorStateUpdate(SUIndicatorEventData eventData);
    }


}

