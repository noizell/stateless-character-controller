using System.Collections.Generic;
using UnityEngine;

namespace Surfer
{

    /// <summary>
    /// Data to better view/group/setup multiple conditions in the inspector and to check if they're satisfied
    /// </summary>
    [System.Serializable]
    public class SUConditionsData
    {

        [SerializeField]
        List<SUConditionData> _conditions = new List<SUConditionData>();
        public bool AreAllSatisfied(GameObject go,object evtData = null)
        {
            for (int i = 0; i < _conditions.Count; i++)
            {
                if (!_conditions[i].IsSatisfied(go,evtData))
                    return false;
            }

            return true;
        }

    }
}