using System.Collections;
using UnityEngine;


namespace Surfer
{

    /// <summary>
    /// Data used to setup Surfer Actions by the inspector
    /// </summary>
    [System.Serializable]
    public class SUActionData
    {
        
        public enum SUAction_ID
        {
            None,
            OpenState,
            CloseState,
            ToggleState,
            LoadScene,
            LoadSceneAsync,
            UnloadSceneAsync,
            SetActiveScene,
            OpenPrefabState,
            CloseMyState,
            SendCustomEvent
        }

        public enum SUPrefabParent_ID
        {
            /// <summary>
            /// The scene will be the parent
            /// </summary>
            Scene,
            /// <summary>
            /// The main-root canvas of the caller will be the parent 
            /// </summary>
            RootCanvas,
            /// <summary>
            /// The state of the caller will be the parent
            /// </summary>
            ThisState,
            /// <summary>
            /// The parent state of caller's state will be the parent
            /// </summary>
            ThisStateParent
        }

#region Serialized Fields

        [SerializeField]
        SUConditionsData _conds = default;

        [SerializeField]
        SUAction_ID _mode = default;

        [SerializeField]
        GameObject _prefab = default;

        [SerializeField]
        SUPrefabParent_ID _parentMode = SUPrefabParent_ID.RootCanvas;

        [SerializeField]
        SUStateData _stateData = default;
        
        [SerializeField]
        SUSceneData _sceneData = default;

        [SerializeField]
        SUCustomEventData _cEventData = default;

        [SerializeField]
        bool _additive = default,_autoActivation = default;

        [SerializeField]
        AudioClip _audioClip = default;

        [SerializeField]
        int _version = 0;
        
        [SerializeField][Min(0f)]
        float _delay = default;

        [SerializeField]
        int _playerID = SurferHelper.kDefaultPlayerID;

#endregion

        /// <summary>
        /// Execute the action logic with conditions and sound
        /// </summary>
        /// <param name="go">GameObject where to play AudioClip on (if setup)</param>
        void RunAction(GameObject go)
        {
            if(!_conds.AreAllSatisfied(go,null))
            return;

            if(_playerID == SurferHelper.kNestedPlayerID)
            {
                _playerID = SurferManager.I.GetObjectStatePlayerID(go);
            }


            if(_mode == SUAction_ID.OpenState)
            {
                SurferManager.I.OpenPlayerState(_playerID,_stateData.Name,_version,_delay);
            }
            else if(_mode == SUAction_ID.OpenPrefabState)
            {
                if(_parentMode == SUPrefabParent_ID.Scene)
                    SurferManager.I.OpenPrefabState(_prefab,null, _version, _delay);
                else if(_parentMode == SUPrefabParent_ID.RootCanvas)
                    SurferManager.I.OpenPrefabState(_prefab,go.GetComponentInParent<Canvas>().rootCanvas.transform, _version, _delay);
                else if (_parentMode == SUPrefabParent_ID.ThisState)
                    SurferManager.I.OpenPrefabState(_prefab, SurferManager.I.GetObjectStateTransfom(go), _version, _delay);
                else if (_parentMode == SUPrefabParent_ID.ThisStateParent)
                    SurferManager.I.OpenPrefabState(_prefab, SurferManager.I.GetObjectStateTransfom(SurferManager.I.GetObjectStateTransfom(go).parent.gameObject), _version, _delay);

            }
            else if(_mode == SUAction_ID.CloseState)
            {
                SurferManager.I.ClosePlayerState(_playerID,_stateData.Name,_version,_delay);
            }
            else if(_mode == SUAction_ID.ToggleState)
            {
                SurferManager.I.TogglePlayerState(_playerID,_stateData.Name,_version,_delay);
            }
            else if(_mode == SUAction_ID.LoadScene)
            {
                SurferManager.I.OpenPlayerState(_playerID,_stateData.Name, _version,_delay);
                SurferManager.I.LoadScene(_sceneData.Name,_delay,_additive);
            }
            else if(_mode == SUAction_ID.LoadSceneAsync)
            {
                SurferManager.I.OpenPlayerState(_playerID,_stateData.Name, _version, _delay);
                SurferManager.I.LoadSceneAsync(_sceneData.Name,_delay,_additive, _autoActivation);
            }
            else if(_mode == SUAction_ID.UnloadSceneAsync) 
            {
                SurferManager.I.OpenPlayerState(_playerID,_stateData.Name, _version, _delay);
                SurferManager.I.UnloadSceneAsync(_sceneData.Name,_delay);
            }
            else if(_mode == SUAction_ID.SetActiveScene)
            {
                SurferManager.I.OpenPlayerState(_playerID,_stateData.Name, _version, _delay);
                SurferManager.I.SetActiveScene(_sceneData.Name,_delay);
            }
            else if (_mode == SUAction_ID.CloseMyState)
            {
                SurferManager.I.ClosePlayerState(SurferManager.I.GetObjectStatePlayerID(go,true), SurferManager.I.GetObjectStateName(go,true), 0, _delay);
            }
            else if (_mode == SUAction_ID.SendCustomEvent)
            {
                SurferManager.I.SendCustomEvent(_cEventData.Name,_delay,null);
            }

            SurferHelper.PlaySound(_audioClip,go,_delay);
            
        }


        /// <summary>
        /// Play the action setup in the inspector
        /// </summary>
        /// <param name="go">GameObject where to play AudioClip on (if setup)</param>
        public virtual void Play(GameObject go)
        {   
            RunAction(go);
        }

    }



}


