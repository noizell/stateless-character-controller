using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



namespace Surfer
{

    /// <summary>
    /// Data to setup condition in the inspector and to check if it's satisfied
    /// </summary>
    [System.Serializable]
    public partial class SUConditionData
    {

        [SerializeField]
        string _key ;
        public string Key{get=>_key;}
    
        [SerializeField]
        string _name ;
        public string Name{get=>_name;}

        [SerializeField]
        SUFieldValuesData _fieldsValues = new SUFieldValuesData();
        public SUFieldValuesData FieldsValues { get => _fieldsValues; }



        [SerializeField]
        SUStateData _stateData = default;
        public SUStateData StateData { get => _stateData; }

        [SerializeField]
        SUSceneData _sceneData = default;
        public SUSceneData SceneData { get => _sceneData; }



        bool IsValid
        {
            get
            {
                return !Key.Equals(SurferHelper.Unset)
                    && !Name.Equals(SurferHelper.Unset);
            }
        }



        public SUConditionData(string key,string name)
        {
            _key = key;
            _name = name;
        }

        
        public bool IsSatisfied(GameObject caller,object evtData)
        {
            if (!IsValid)
                return true;

            return ConditionChecksExtensions.IsSatisfied(_key, caller, this,evtData);
        }

    }

    

}


