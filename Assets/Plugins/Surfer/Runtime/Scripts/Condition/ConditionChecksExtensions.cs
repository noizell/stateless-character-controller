using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Surfer
{
    public static class ConditionChecksExtensions 
    {
        
        public static List<PathField> GetFieldsList(string key )
        {
            
            if(ConditionChecks.All.TryGetValue(key, out var value))
            {
                return value.Fields;
            }

            if(DefaultConditionChecks.All.TryGetValue(key, out var defaultValue))
            {
                return defaultValue.Fields;
            }


            return null;

        }


        /// <summary>
        /// Check if a specific condition is satisfied
        /// </summary>
        /// <param name="conditionKey">Condition key</param>
        /// <returns>true if satisfied, false otherwise</returns>
        public static bool IsSatisfied(string conditionKey,GameObject go,SUConditionData data,object evtData)
        {
            if(string.IsNullOrEmpty(conditionKey))
            return true;
            if(conditionKey.Equals(SurferHelper.Unset))
            return true;

            if(ConditionChecks.All.TryGetValue(conditionKey,out PathFunc value))
            {
                return value.Function.Invoke(new FuncInput(go,data,evtData)) == true;
            }
            if(DefaultConditionChecks.All.TryGetValue(conditionKey,out PathFunc valueDefault))
            {
                return valueDefault.Function.Invoke(new FuncInput(go,data,evtData)) == true;
            }


#if UNITY_EDITOR
            Debug.LogWarning("Condition not set up: "+conditionKey);
#endif
            return false;
        }


        /// <summary>
        /// Get the name/path of a specific condition. Used for the inspector
        /// </summary>
        /// <param name="key">Condition key to retrieve the name/path</param>
        /// <returns>Condition name/path</returns>
        public static string GetNameFromUnion(string key)
        {
            string output = ConditionChecks.GetName(key);

            if(string.IsNullOrEmpty(output))
            {
                output = DefaultConditionChecks.GetName(key);
            }

            return output;
        }

        /// <summary>
        /// Get the key of a specific condition. Used for the inspector
        /// </summary>
        /// <param name="path">Condition name/path to retrieve the key</param>
        /// <returns>Condition key</returns>
        public static string GetKeyFromUnion(string path)
        {

            string output = ConditionChecks.GetKey(path);

            if(string.IsNullOrEmpty(output))
            {

                output = DefaultConditionChecks.GetKey(path);

            }

            return output;
        }

    }


}

