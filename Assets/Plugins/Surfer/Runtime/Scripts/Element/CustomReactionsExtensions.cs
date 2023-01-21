using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Surfer
{


    public static class CustomReactionsExtensions 
    {
        
        public static List<PathField> GetFieldsList(string key )
        {
            
            if(CustomReactions.All.TryGetValue(key, out var value))
            {
                return value.Fields;
            }

            if(DefaultCustomReactions.All.TryGetValue(key, out var defaultValue))
            {
                return defaultValue.Fields;
            }

            return null;

        }

        
        /// <summary>
        /// Play a specific custom reaction 
        /// </summary>
        /// <param name="reactionKey">Reaction key</param>
        public static void PlayReaction(string key, GameObject go, SUReactionData data,object evtData)
        {
            if (string.IsNullOrEmpty(key))
                return;
            if (key.Equals(SurferHelper.Unset))
                return;

            if (CustomReactions.All.TryGetValue(key, out PathAction value))
            {
                value.Action.Invoke(new FuncInput(go, data,evtData));
            }
            else if (DefaultCustomReactions.All.TryGetValue(key, out PathAction defaultValue))
            {
                defaultValue.Action.Invoke(new FuncInput(go, data,evtData));
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("Reaction not set up: " + key);
#endif
            }



        }

        /// <summary>
        /// Get the name/path of a specific reaction. Used for the inspector
        /// </summary>
        /// <param name="key">Reaction key to retrieve the name/path</param>
        /// <returns>Reaction name/path</returns>
        public static string GetNameFromUnion(string key)
        {
            string output = CustomReactions.GetName(key);

            if (string.IsNullOrEmpty(output))
            {
                output = DefaultCustomReactions.GetName(key);
            }

            return output;
        }

        /// <summary>
        /// Get the key of a specific reaction. Used for the inspector
        /// </summary>
        /// <param name="path">Reaction name/path to retrieve the key</param>
        /// <returns>Reaction key</returns>
        public static string GetKeyFromUnion(string path)
        {

            string output = CustomReactions.GetKey(path);

            if(string.IsNullOrEmpty(output))
            {

                output = DefaultCustomReactions.GetKey(path);

            }

            return output;
        }


    }


}
