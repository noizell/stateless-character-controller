using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Surfer
{

    /// <summary>
    /// Data that contains all the runtime logic for custom reactions
    /// </summary>
    public static class CustomReactions
    {
        

        //reactions container
        public readonly static Dictionary<string,PathAction> All = new Dictionary<string, PathAction>()
        {

           

        };


        /// <summary>
        /// Get all the names/paths of the reactions. Used for the inspector
        /// </summary>
        /// <returns>Names/paths list</returns>
        public static string[] GetAllNames()
        {
            return All.Select(x=>x.Value.Path).OrderBy(x=>x).Prepend(SurferHelper.Unset).ToArray();
        }

        /// <summary>
        /// Get the name/path of a specific reaction. Used for the inspector
        /// </summary>
        /// <param name="key">Reaction key to retrieve the name/path</param>
        /// <returns>Reaction name/path</returns>
        public static string GetName(string key)
        {
            if(All.TryGetValue(key,out PathAction value))
                return value.Path;

            return "";
        }

        /// <summary>
        /// Get the key of a specific reaction. Used for the inspector
        /// </summary>
        /// <param name="path">Reaction name/path to retrieve the key</param>
        /// <returns>Reaction key</returns>
        public static string GetKey(string path)
        {
            foreach(KeyValuePair<string, PathAction> pair in All)
            {
                if(pair.Value.Path.Equals(path))
                return pair.Key;
            }
            return "";
        }


    }

   
}


