using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public interface ISUHealthBarStateHandler 
    {

        void OnHealthBarStateUpdate(SUHealthBarEventData eventData);

    }

}


