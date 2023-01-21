using UnityEngine;

namespace Surfer
{
    /// <summary>
    /// Class to choose a state in the inspector
    /// </summary>
    [System.Serializable]
    public class SUStateData
    {

        [SerializeField]
        string _guid = default;
        public string GUID {get=>_guid;}

        [SerializeField]
        string _name = default;
        public string Name 
        {
            get
            {
                
                return SurferManager.I.SO.GetState(_guid);

            }
        }


        public SUStateData(string name, string guid)
        {
            _name = name;
            _guid = guid;
        }

        public SUStateData(SUStateData data)
        {
            _name = data._name;
            _guid = data._guid;
        }

        public SUStateData(string name)
        {
            _name = name;
            _guid = SurferHelper.SO.GetStateKey(name);
        }

    }
}