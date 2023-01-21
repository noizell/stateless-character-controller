using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Surfer
{
    /// <summary>
    /// Class to easily choose states in the inspector
    /// </summary>
    [System.Serializable]
    public class SUStatesData
    {

        [SerializeField]
        List<SUStateData> _list = default;
        public List<SUStateData> List {get=>_list;}

        public string[] AllNamesArray
        {
            get
            {
                return List.Select(x=>x.Name).Where(x=>!string.IsNullOrEmpty(x) && x!=SurferHelper.Unset).ToArray();
                
            }
        }

        public List<string> AllNames
        {
            get
            {
                
                return List.Select(x=>x.Name).Where(x=>!string.IsNullOrEmpty(x) && x!=SurferHelper.Unset).ToList();
                
            }
        }

        public List<SUStateInfo> GetStateInfos(SUElementData callerData)
        {
            var output = new List<SUStateInfo>();

            for (int i = 0; i < List.Count; i++)
            {
                if (string.IsNullOrEmpty(List[i].Name))
                    continue;
                if (List[i].Name == SurferHelper.Unset)
                    continue;

                output.Add(new SUStateInfo(List[i].Name, callerData.PlayerID));

            }

            return output;
        }

    }

}