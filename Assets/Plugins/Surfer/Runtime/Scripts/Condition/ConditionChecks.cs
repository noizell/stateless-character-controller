using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Surfer
{

    /// <summary>
    /// Data that contains all the runtime logic for conditions
    /// </summary>
    public static class ConditionChecks {
        

        //conditions container
        public readonly static Dictionary<string,PathFunc> All = new Dictionary<string,PathFunc>()
        {

            

        };


        /// <summary>
        /// Get all the names/paths of the conditions. Used for the inspector
        /// </summary>
        /// <returns>Names/paths list</returns>
        public static string[] GetAllNames()
        {
            return All.Select(x=>x.Value.Path).OrderBy(x=>x).Prepend(SurferHelper.Unset).ToArray();
        }

        /// <summary>
        /// Get the name/path of a specific condition. Used for the inspector
        /// </summary>
        /// <param name="key">Condition key to retrieve the name/path</param>
        /// <returns>Condition name/path</returns>
        public static string GetName(string key)
        {
            if(All.TryGetValue(key,out PathFunc value))
                return value.Path;

            return "";
        }

        /// <summary>
        /// Get the key of a specific condition. Used for the inspector
        /// </summary>
        /// <param name="path">Condition name/path to retrieve the key</param>
        /// <returns>Condition key</returns>
        public static string GetKey(string path)
        {
            foreach(KeyValuePair<string,PathFunc> pair in All)
            {
                if(pair.Value.Path.Equals(path))
                return pair.Key;
            }
            return "";
        }


    }

   
}


