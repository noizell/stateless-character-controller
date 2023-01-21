using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public partial class SUElement : ISUHealthBarVisualHandler, ISUHealthBarHPStateHandler , ISUHealthBarStateHandler

    {
        
        public SUHealthBarVisualData OnHealthBarVisualSetUp()
        {
            return ElementData.HbVisualData;
        }

        public void OnHealthBarHPStateUpdate(SUHealthBarEventData eventData)
        {
            if(eventData.Data.HPState == SUHealthBarHPState_ID.Empty)
            {
                RunEventBehaviour(SUEvent.Type_ID.HealthBar_OnEmptyHp,eventData);
            }
            else if (eventData.Data.HPState == SUHealthBarHPState_ID.Full)
            {
                RunEventBehaviour(SUEvent.Type_ID.HealthBar_OnFullHp,eventData);
            }
            else if (eventData.Data.HPState == SUHealthBarHPState_ID.LessThanHalf)
            {
                RunEventBehaviour(SUEvent.Type_ID.HealthBar_OnLessThanHalfHp,eventData);
            }
            else if (eventData.Data.HPState == SUHealthBarHPState_ID.MoreThanHalf)
            {
                RunEventBehaviour(SUEvent.Type_ID.HealthBar_OnMoreThanHalfHp,eventData);
            }
        }

        public void OnHealthBarStateUpdate(SUHealthBarEventData eventData)
        {
        }
    }

}


