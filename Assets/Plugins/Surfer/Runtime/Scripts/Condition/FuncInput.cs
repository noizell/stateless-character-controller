using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{
    /// <summary>
    /// Input parameters of a custom condition or reaction
    /// </summary>
    public struct FuncInput
    {

        public GameObject gameObj { get; private set; }
        public SUFieldValuesData fieldsValues { get; private set; }
        public SUReactionData reactionData { get; private set; }
        public SUConditionData conditionData { get; private set; }
        public object eventData { get; private set; }

        public FuncInput(GameObject obj,SUReactionData data,object evtData)
        {
            gameObj = obj;
            fieldsValues = data.FieldsValues;
            reactionData = data;
            conditionData = null;
            eventData = evtData;
        }

        public FuncInput(GameObject obj,SUConditionData data,object evtData)
        {
            gameObj = obj;
            fieldsValues = data.FieldsValues;
            conditionData = data;
            reactionData = null;
            eventData = evtData;
        }

    }
}


