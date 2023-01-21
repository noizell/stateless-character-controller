using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Surfer
{

    /// <summary>
    /// Class to choose a custom event in the inspector
    /// </summary>
    [System.Serializable]
    public class SUCustomEventsData
    {

        [SerializeField]
        List<SUCustomEventData> _list = default;
        public List<SUCustomEventData> List { get => _list; }


        string[] _allNamesArray = null;

        public string[] AllNamesArray
        {
            get
            {
                if (_allNamesArray == null)
                    _allNamesArray = List.Select(x => x.Name).Where(x => !string.IsNullOrEmpty(x) && x != SurferHelper.Unset).ToArray();

                return _allNamesArray;

            }
        }



        List<string> _allNames = null;

        public List<string> AllNames
        {
            get
            {
                if (_allNames == null)
                    _allNames = List.Select(x => x.Name).Where(x => !string.IsNullOrEmpty(x) && x != SurferHelper.Unset).ToList();

                return _allNames;

            }
        }


    }


}