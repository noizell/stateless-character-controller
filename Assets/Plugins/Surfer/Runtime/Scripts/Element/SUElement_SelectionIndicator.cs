using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Surfer
{

    public partial class SUElement
    {
        
        void CheckSelectionIndicator()
        {
            if (_elementData.Type != SUElementData.Type_ID.Selection_Indicator)
                return;

            SUEventSystemManager.I?.RegisterSelectionIndicator(this);

        }

    }


}

