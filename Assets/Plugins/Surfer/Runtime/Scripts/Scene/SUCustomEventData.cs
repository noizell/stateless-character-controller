using UnityEngine;

namespace Surfer
{

    /// <summary>
    /// Class to choose a custom event in the inspector
    /// </summary>
    [System.Serializable]
    public class SUCustomEventData
    {

        [SerializeField]
        string _guid ;
        public string GUID {get=>_guid;}

        [SerializeField]
        string _name ;
        public string Name 
        {
            get
            {
                
                return SurferManager.I.SO.GetEvent(_guid);

            }
        }


        public SUCustomEventData(string name, string guid)
        {
            _name = name;
            _guid = guid;
        }


    }


}