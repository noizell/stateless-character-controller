using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Surfer
{

    /// <summary>
    /// Class to choose a scene in the inspector
    /// </summary>
    [System.Serializable]
    public class SUScenesData
    {

        [SerializeField]
        List<SUSceneData> _list = default;
        public List<SUSceneData> List { get => _list; }


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