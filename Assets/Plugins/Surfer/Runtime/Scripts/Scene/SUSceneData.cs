using UnityEngine;

namespace Surfer
{

    /// <summary>
    /// Class to choose a scene in the inspector
    /// </summary>
    [System.Serializable]
    public class SUSceneData
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
                
                return SurferManager.I.SO.GetSceneName(_guid);

            }
        }


        public SUSceneData(string name, string guid)
        {
            _name = name;
            _guid = guid;
        }


    }


}