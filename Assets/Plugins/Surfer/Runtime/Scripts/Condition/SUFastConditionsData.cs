using System.Collections;
using System.Collections.Generic;
using Surfer;
using UnityEngine;
using UnityEngine.UI;


namespace Surfer
{


    [System.Serializable]
    public class SUFastConditionsData
    {

        [SerializeField]
        List<SUFastConditionData> _conditions = default;


        public bool AreAllSatisfied(GameObject go)
        {
            if(_conditions == null)
                return true;

            for (int i = 0; i < _conditions.Count; i++)
            {
                if (!_conditions[i].IsSatisfied(go))
                    return false;
            }

            return true;
        }

    }



}



