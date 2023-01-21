using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AsyncOperation = UnityEngine.AsyncOperation;


namespace Surfer
{

    /// <summary>
    /// Surfer Singleton that manages states, scene, conditions and actions.
    /// </summary>
    [DefaultExecutionOrder(-500)]
    public sealed class SurferManager : MonoBehaviour
    {

#if UNITY_2019_3_OR_NEWER

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            I = null;
            SUElementData.AllUIStates.Clear();
            SUAnimationData.AllPlaying.Clear();
        }
        
#endif

#region Public

        public static SurferManager I {get;private set;}

        /// <summary>
        /// Get all the closed states.This list is cleared after OpenState is called and performed.
        /// </summary>
        public HashSet<SUStateEventData> LastClosedStates {get;private set;} = new HashSet<SUStateEventData>();
        /// <summary>
        /// Get all the currently opened states
        /// </summary>
        public HashSet<SUStateEventData> OpenedStates {get;private set;} = new HashSet<SUStateEventData>();
        /// <summary>
        /// Get all the currently opened scenes
        /// </summary>
        public HashSet<string> OpenedScenes {get;private set;} = new HashSet<string>();
        /// <summary>
        /// Get all the scenes that are currently loading
        /// </summary>
        public HashSet<string> LoadingScenes {get;private set;} = new HashSet<string>();
        /// <summary>
        /// Get all the scenes that should be autoactivated once loaded
        /// </summary>
        HashSet<string> _autoActiveScenes = new HashSet<string>();
        /// <summary>
        /// Get all the scenes that are not allowed to be autoactivated (async)
        /// </summary>
        HashSet<string> _disallowedActivationScenes = new HashSet<string>();
        /// <summary>
        /// Get all the scenes that are currently unloading
        /// </summary>
        public HashSet<string> UnloadingScenes {get;private set;} = new HashSet<string>();
        /// <summary>
        /// Get the last unloaded scene
        /// </summary>
        /// <value></value>
        public string LastUnloaded {get;private set;} = string.Empty;
        /// <summary>
        /// Surfer Scriptable object that contains states, scenes lists and other data
        /// </summary>
        public SurferSO SO {get=>_so;}

#endregion


#region Private

        //Dictionaries where are stored all the objects that register to specific events, state-related or not.
        Dictionary<string,HashSet<ISUSceneLoadedHandler>> _sceneLoadedReg = new Dictionary<string, HashSet<ISUSceneLoadedHandler>>();
        Dictionary<string,HashSet<ISUSceneUnloadedHandler>> _sceneUnloadedReg = new Dictionary<string, HashSet<ISUSceneUnloadedHandler>>();
        Dictionary<string,HashSet<ISUSceneActivatedHandler>> _sceneActivReg = new Dictionary<string, HashSet<ISUSceneActivatedHandler>>();
        Dictionary<string,HashSet<ISUSceneDeactivatedHandler>> _sceneDeactivReg = new Dictionary<string, HashSet<ISUSceneDeactivatedHandler>>();
        Dictionary<string,HashSet<ISUSceneLoadingHandler>> _sceneLoadingReg = new Dictionary<string, HashSet<ISUSceneLoadingHandler>>();
        Dictionary<string,HashSet<ISUSceneUnloadingHandler>> _sceneUnloadingReg = new Dictionary<string, HashSet<ISUSceneUnloadingHandler>>();
        Dictionary<string,HashSet<ISUStateEnterHandler>> _enterReg = new Dictionary<string, HashSet<ISUStateEnterHandler>>();
        Dictionary<string,HashSet<ISUStateExitHandler>> _exitReg = new Dictionary<string, HashSet<ISUStateExitHandler>>();
        Dictionary<string,HashSet<ISUCustomEventHandler>> _cEventReg = new Dictionary<string, HashSet<ISUCustomEventHandler>>();

        Dictionary<string,List<SUStateInfo>> _statesStacked = new Dictionary<string, List<SUStateInfo>>();

        //Data classes to populate when a scene is loading or unloading 
        SUSceneLoadingEventData _loadingData = new SUSceneLoadingEventData();
        SUSceneUnloadingEventData _unloadingData = new SUSceneUnloadingEventData();
        
        SurferSO _so = default;

        const float _noDelay = 0f;

        WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

        AsyncOperation _asyncLoading = default;

        #endregion


        void Awake()
        {

            
            if(I==null)
            {
                I=this;
                gameObject.AddComponent<SUEventSystemManager>();
                gameObject.AddComponent<SUChildMonitorManager>();
                gameObject.AddComponent<SUOrientationManager>();
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.sceneUnloaded += OnSceneUnloaded;
                SceneManager.activeSceneChanged += OnSceneActivation;
                DontDestroyOnLoad(this);
                _so = Resources.Load<SurferSO>("SurferSettings");


                //audio slider managing
                if (PlayerPrefs.HasKey(SurferHelper.kOverallVolume))
                    AudioListener.volume = PlayerPrefs.GetFloat(SurferHelper.kOverallVolume);

            }
            else
                Destroy(this);

        }



        private void Update()
        {
            SUEventSystemManager.I?.MainLoop();
            SUChildMonitorManager.I?.MainLoop();
            SUOrientationManager.I?.MainLoop();
        }



        private void LateUpdate()
        {
            SUIndicatorsManager.I?.MainLoop();
            SUHealthBarsManager.I?.MainLoop();
        }




        #region Unity Scene Callbacks

        void OnSceneLoaded(Scene scene , LoadSceneMode mode)
        {
            
            SUEventSystemManager.I.GetAllEventSystems();
            
            OpenedScenes.Add(scene.name);

            LoadingScenes.Remove(scene.name);

            if(_autoActiveScenes.Contains(scene.name))
            {
                SceneManager.SetActiveScene(scene);
            }

            if(string.IsNullOrEmpty(scene.name))
            return;

            if(_sceneLoadedReg.TryGetValue(scene.name,out HashSet<ISUSceneLoadedHandler> set))
            {

                foreach (ISUSceneLoadedHandler item in set)
                {
                    item.OnSUSceneLoaded(new SUSceneLoadedEventData(scene.name));
                }
            }

        }

        void OnSceneUnloaded(Scene scene)
        {

            if(string.IsNullOrEmpty(scene.name))
            return;

            OpenedScenes.Remove(scene.name);
            LastUnloaded = scene.name;

            if(_sceneUnloadedReg.TryGetValue(scene.name,out HashSet<ISUSceneUnloadedHandler> set))
            {
                foreach(ISUSceneUnloadedHandler item in set)
                {
                    item.OnSUSceneUnloaded(new SUSceneUnloadedEventData(scene.name));
                }
            }
        }

        void OnSceneActivation(Scene scene, Scene other)
        {


            if (!string.IsNullOrEmpty(scene.name) && _sceneDeactivReg.TryGetValue(scene.name,out HashSet<ISUSceneDeactivatedHandler> set))
            {
                foreach(ISUSceneDeactivatedHandler item in set)
                {
                    item.OnSUSceneDeactivated(new SUSceneDeactivatedEventData(scene.name));
                }
            }
            
            
            
            if(!string.IsNullOrEmpty(other.name) && _sceneActivReg.TryGetValue(other.name,out HashSet<ISUSceneActivatedHandler> set2))
            {
                foreach(ISUSceneActivatedHandler item in set2)
                {
                    item.OnSUSceneActivated(new SUSceneActivatedEventData(other.name));
                }
            }
            

            
        }

        #endregion


        #region Events

        #region Register



        /// <summary>
        /// Register to the CustomEvent callback
        /// </summary>
        /// <param name="interf">object interface used to register</param>
        /// <param name="eventsID">all the names of the custom events to register to</param>
        public void RegisterCustomEvent(ISUCustomEventHandler interf, params string[] eventsID)
        {

            for (int i = 0; i < eventsID.Length; i++)
            {
                if (string.IsNullOrEmpty(eventsID[i]))
                    continue;

                if (_cEventReg.TryGetValue(eventsID[i], out HashSet<ISUCustomEventHandler> set))
                {
                    set.Add(interf);
                }
                else
                {
                    _cEventReg.Add(eventsID[i], new HashSet<ISUCustomEventHandler>() { interf });
                }

            }

        }



        /// <summary>
        /// Register to the StateEnter callback of specific states
        /// </summary>
        /// <param name="interf">object interface used to register</param>
        /// <param name="statesID">all the names of the states to register to</param>
        public void RegisterStateEnter(ISUStateEnterHandler interf,params string[] statesID)
        {
            
            for(int i=0;i<statesID.Length;i++)
            {
                if(string.IsNullOrEmpty(statesID[i]))
                continue;
                if (statesID[i]== SurferHelper.Unset)
                    continue;

                if (_enterReg.TryGetValue(statesID[i],out HashSet<ISUStateEnterHandler> set))
                {
                    set.Add(interf);
                }
                else
                {
                    _enterReg.Add(statesID[i], new HashSet<ISUStateEnterHandler>(){interf});
                }

            }

        }


        
        /// <summary>
        /// Register to the StateExit callback of specific states
        /// </summary>
        /// <param name="interf">object interface used to register</param>
        /// <param name="statesID">all the names of the states to register to</param>
        public void RegisterStateExit(ISUStateExitHandler interf,params string[] statesID)
        {
            
            for(int i=0;i<statesID.Length;i++)
            {
                if(string.IsNullOrEmpty(statesID[i]))
                continue;
                if (statesID[i] == SurferHelper.Unset)
                    continue;

                if (_exitReg.TryGetValue(statesID[i],out HashSet<ISUStateExitHandler> set))
                {
                    set.Add(interf);
                }
                else
                {
                    _exitReg.Add(statesID[i], new HashSet<ISUStateExitHandler>(){interf});
                }

            }

        }

        /// <summary>
        /// Register to the SceneLoaded callback of specific scenes
        /// </summary>
        /// <param name="interf">object interface used to register</param>
        /// <param name="scenes">all the names of the scenes to register to</param>
        public void RegisterSceneLoaded(ISUSceneLoadedHandler interf,params string[] scenes)
        {
            
            for(int i=0;i<scenes.Length;i++)
            {
                if(string.IsNullOrEmpty(scenes[i]))
                continue;
                if (scenes[i] == SurferHelper.Unset)
                    continue;

                if (_sceneLoadedReg.TryGetValue(scenes[i],out HashSet<ISUSceneLoadedHandler> set))
                {
                    set.Add(interf);
                }
                else
                {
                    _sceneLoadedReg.Add(scenes[i], new HashSet<ISUSceneLoadedHandler>(){interf});
                }

            }


        }


        /// <summary>
        /// Register to the SceneUnloaded callback of specific scenes
        /// </summary>
        /// <param name="interf">object interface used to register</param>
        /// <param name="scenes">all the names of the scenes to register to</param>
        public void RegisterSceneUnloaded(ISUSceneUnloadedHandler interf,params string[] scenes)
        {

            for(int i=0;i<scenes.Length;i++)
            {
                if(string.IsNullOrEmpty(scenes[i]))
                continue;
                if (scenes[i] == SurferHelper.Unset)
                    continue;

                if (_sceneUnloadedReg.TryGetValue(scenes[i],out HashSet<ISUSceneUnloadedHandler> set))
                {
                    set.Add(interf);
                }
                else
                {
                    _sceneUnloadedReg.Add(scenes[i], new HashSet<ISUSceneUnloadedHandler>(){interf});
                }

            }


        }


        /// <summary>
        /// Register to the SceneActivated callback of specific scenes
        /// </summary>
        /// <param name="interf">object interface used to register</param>
        /// <param name="scenes">all the names of the scenes to register to</param>
        public void RegisterSceneActivated(ISUSceneActivatedHandler interf,params string[] scenes)
        {

            for(int i=0;i<scenes.Length;i++)
            {
                if(string.IsNullOrEmpty(scenes[i]))
                continue;
                if (scenes[i] == SurferHelper.Unset)
                    continue;

                if (_sceneActivReg.TryGetValue(scenes[i],out HashSet<ISUSceneActivatedHandler> set))
                {
                    set.Add(interf);
                }
                else
                {
                    _sceneActivReg.Add(scenes[i], new HashSet<ISUSceneActivatedHandler>(){interf});
                }

            }


        }

        /// <summary>
        /// Register to the SceneDeactivated callback of specific scenes
        /// </summary>
        /// <param name="interf">object interface used to register</param>
        /// <param name="scenes">all the names of the scenes to register to</param>
        public void RegisterSceneDeactivated(ISUSceneDeactivatedHandler interf,params string[] scenes)
        {

            for(int i=0;i<scenes.Length;i++)
            {
                if(string.IsNullOrEmpty(scenes[i]))
                continue;
                if (scenes[i] == SurferHelper.Unset)
                    continue;

                if (_sceneDeactivReg.TryGetValue(scenes[i],out HashSet<ISUSceneDeactivatedHandler> set))
                {
                    set.Add(interf);
                }
                else
                {
                    _sceneDeactivReg.Add(scenes[i], new HashSet<ISUSceneDeactivatedHandler>(){interf});
                }

            }


        }

        /// <summary>
        /// Register to the SceneLoading callback of specific scenes
        /// </summary>
        /// <param name="interf">object interface used to register</param>
        /// <param name="scenes">all the names of the scenes to register to</param>
        public void RegisterSceneLoading(ISUSceneLoadingHandler interf,params string[] scenes)
        {

            for(int i=0;i<scenes.Length;i++)
            {
                if(string.IsNullOrEmpty(scenes[i]))
                continue;
                if (scenes[i] == SurferHelper.Unset)
                    continue;


                if (_sceneLoadingReg.TryGetValue(scenes[i],out HashSet<ISUSceneLoadingHandler> set))
                {
                    set.Add(interf);
                }
                else
                {
                    _sceneLoadingReg.Add(scenes[i], new HashSet<ISUSceneLoadingHandler>(){interf});
                }

            }


        }
        
        /// <summary>
        /// Register to the SceneUnloading callback of specific scenes
        /// </summary>
        /// <param name="interf">object interface used to register</param>
        /// <param name="scenes">all the names of the scenes to register to</param>
        public void RegisterSceneUnloading(ISUSceneUnloadingHandler interf,params string[] scenes)
        {

            for(int i=0;i<scenes.Length;i++)
            {
                if(string.IsNullOrEmpty(scenes[i]))
                continue;
                if (scenes[i] == SurferHelper.Unset)
                    continue;

                if (_sceneUnloadingReg.TryGetValue(scenes[i],out HashSet<ISUSceneUnloadingHandler> set))
                {
                    set.Add(interf);
                }
                else
                {
                    _sceneUnloadingReg.Add(scenes[i], new HashSet<ISUSceneUnloadingHandler>(){interf});
                }

            }


        }




        #endregion

        #region Unregister



        /// <summary>
        /// Unregister to the CustomEvent callback
        /// </summary>
        /// <param name="interf">object interface used to unregister</param>
        /// <param name="eventsID">all the names of the custom events to unregister to</param>
        public void UnregisterCustomEvent(ISUCustomEventHandler interf, params string[] eventsID)
        {

            for (int i = 0; i < eventsID.Length; i++)
            {
                if (string.IsNullOrEmpty(eventsID[i]))
                    continue;

                if (_cEventReg.TryGetValue(eventsID[i], out HashSet<ISUCustomEventHandler> set))
                {
                    set.Remove(interf);
                }

            }

        }




        /// <summary>
        /// Unregister to the StateEnter callback of specific states
        /// </summary>
        /// <param name="interf">object interface used to unregister</param>
        /// <param name="statesID">all the names of the states to unregister to</param>
        public void UnregisterStateEnter(ISUStateEnterHandler interf,params string[] statesID)
        {
            
            for(int i=0;i<statesID.Length;i++)
            {
                if(string.IsNullOrEmpty(statesID[i]))
                continue;

                if(_enterReg.TryGetValue(statesID[i],out HashSet<ISUStateEnterHandler> set))
                {
                    set.Remove(interf);
                }

            }

        }


        
        /// <summary>
        /// Unegister to the StateExit callback of specific states
        /// </summary>
        /// <param name="interf">object interface used to unregister</param>
        /// <param name="statesID">all the names of the states to unregister to</param>
        public void UnregisterStateExit(ISUStateExitHandler interf,params string[] statesID)
        {
            
            for(int i=0;i<statesID.Length;i++)
            {
                if(string.IsNullOrEmpty(statesID[i]))
                continue;

                if(_exitReg.TryGetValue(statesID[i],out HashSet<ISUStateExitHandler> set))
                {
                    set.Remove(interf);
                }

            }

        }

        /// <summary>
        /// Unregister to the SceneLoaded callback of specific scenes
        /// </summary>
        /// <param name="interf">object interface used to unregister</param>
        /// <param name="scenes">all the names of the scenes to unregister to</param>
        public void UnregisterSceneLoaded(ISUSceneLoadedHandler interf,params string[] scenes)
        {
            
            for(int i=0;i<scenes.Length;i++)
            {
                if(string.IsNullOrEmpty(scenes[i]))
                continue;

                if(_sceneLoadedReg.TryGetValue(scenes[i],out HashSet<ISUSceneLoadedHandler> set))
                {
                    set.Remove(interf);
                }

            }


        }


        /// <summary>
        /// Unregister to the SceneUnloaded callback of specific scenes
        /// </summary>
        /// <param name="interf">object interface used to unregister</param>
        /// <param name="scenes">all the names of the scenes to unregister to</param>
        public void UnregisterSceneUnloaded(ISUSceneUnloadedHandler interf,params string[] scenes)
        {

            for(int i=0;i<scenes.Length;i++)
            {
                if(string.IsNullOrEmpty(scenes[i]))
                continue;

                if(_sceneUnloadedReg.TryGetValue(scenes[i],out HashSet<ISUSceneUnloadedHandler> set))
                {
                    set.Remove(interf);
                }

            }


        }


        /// <summary>
        /// Unregister to the SceneActivated callback of specific scenes
        /// </summary>
        /// <param name="interf">object interface used to unregister</param>
        /// <param name="scenes">all the names of the scenes to unregister to</param>
        public void UnregisterSceneActivated(ISUSceneActivatedHandler interf,params string[] scenes)
        {

            for(int i=0;i<scenes.Length;i++)
            {
                if(string.IsNullOrEmpty(scenes[i]))
                continue;

                if(_sceneActivReg.TryGetValue(scenes[i],out HashSet<ISUSceneActivatedHandler> set))
                {
                    set.Remove(interf);
                }

            }


        }

        /// <summary>
        /// Unregister to the SceneDeactivated callback of specific scenes
        /// </summary>
        /// <param name="interf">object interface used to unregister</param>
        /// <param name="scenes">all the names of the scenes to unregister to</param>
        public void UnregisterSceneDeactivated(ISUSceneDeactivatedHandler interf,params string[] scenes)
        {

            for(int i=0;i<scenes.Length;i++)
            {
                if(string.IsNullOrEmpty(scenes[i]))
                continue;

                if(_sceneDeactivReg.TryGetValue(scenes[i],out HashSet<ISUSceneDeactivatedHandler> set))
                {
                    set.Remove(interf);
                }

            }


        }

        /// <summary>
        /// Unregister to the SceneLoading callback of specific scenes
        /// </summary>
        /// <param name="interf">object interface used to unregister</param>
        /// <param name="scenes">all the names of the scenes to unregister to</param>
        public void UnregisterSceneLoading(ISUSceneLoadingHandler interf,params string[] scenes)
        {

            for(int i=0;i<scenes.Length;i++)
            {
                if(string.IsNullOrEmpty(scenes[i]))
                continue;

                if(_sceneLoadingReg.TryGetValue(scenes[i],out HashSet<ISUSceneLoadingHandler> set))
                {
                    set.Remove(interf);
                }

            }


        }
        
        /// <summary>
        /// Unregister to the SceneUnloading callback of specific scenes
        /// </summary>
        /// <param name="interf">object interface used to unregister</param>
        /// <param name="scenes">all the names of the scenes to unregister to</param>
        public void UnregisterSceneUnloading(ISUSceneUnloadingHandler interf,params string[] scenes)
        {

            for(int i=0;i<scenes.Length;i++)
            {
                if(string.IsNullOrEmpty(scenes[i]))
                continue;

                if(_sceneUnloadingReg.TryGetValue(scenes[i],out HashSet<ISUSceneUnloadingHandler> set))
                {
                    set.Remove(interf);
                }

            }


        }


#endregion

        void CallSceneLoadingEvents(SUSceneLoadingEventData data)
        {
            if(_sceneLoadingReg.TryGetValue(data.SceneName,out HashSet<ISUSceneLoadingHandler> set))
            {
                foreach(ISUSceneLoadingHandler item in set)
                {
                    item.OnSUSceneLoading(data);
                }
            }
        }

        void CallSceneUnloadingEvents(SUSceneUnloadingEventData data)
        {
            if(_sceneUnloadingReg.TryGetValue(data.SceneName,out HashSet<ISUSceneUnloadingHandler> set))
            {
                foreach(ISUSceneUnloadingHandler item in set)
                {
                    item.OnSUSceneUnloading(data);
                }
            }
        }


        #endregion



        #region Custom Events


        public void SendCustomEvent(string eventName,float delay,params object[] customData)
        {

            if (delay < Mathf.Epsilon)
                SendCustomEvent(eventName, customData);
            else
                StartCoroutine(DelaySendCustomEvent(eventName,delay,customData));


        }


        public void SendCustomEvent(string eventName, params object[] customData)
        {

            if (_cEventReg.TryGetValue(eventName, out HashSet<ISUCustomEventHandler> set))
            {
                foreach (ISUCustomEventHandler item in set)
                {
                    item.OnSUCustomEvent(new SUCustomEventEventData(eventName, customData));
                }
            }


        }




        #endregion




        #region Open State

        /// <summary>
        /// Open a prefab state , with a specific version, delay, custom list of data and in a specific parent.
        /// </summary>
        /// <param name="prefab">state prefab</param>
        /// <param name="parent">parent</param>
        /// <param name="version">state version</param>
        /// <param name="delay">delay</param>
        /// <param name="customData">list of custom variables</param>
        public void OpenPrefabState(GameObject prefab,Transform parent = null, int version = SurferHelper.kWhateverVersion,float delay = _noDelay, params object[] customData)
        {
            if (prefab == null)
                return;

            var stateName = string.Empty;
            var pID = SurferHelper.kPlayerIDFallback;

            //2.0
            var cp = prefab.GetComponent<SUElement>();

            if(cp!=null && cp.IsState)
            {
                stateName = prefab.GetComponent<SUElement>().ElementData.StateData.Name;
                pID = cp.ElementData.PlayerID;
            }
            

            if (string.IsNullOrEmpty(stateName))
                return;

            GameObject.Instantiate(prefab,parent);

            OpenPlayerState(pID, stateName, version,delay,customData);

        }

        /// <summary>
        /// Open a specific state , with a specific version, delay and custom list of data.
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="version">state version</param>
        /// <param name="delay">delay</param>
        /// <param name="customData">list of custom variables</param>
        public void OpenState(string state, int version = SurferHelper.kWhateverVersion, float delay = _noDelay, params object[] customData)
        {

            if(delay < Mathf.Epsilon)
                OpenPlayerState(SurferHelper.kPlayerIDFallback,state,version,customData);
            else
                StartCoroutine(DelayOpenPlayerState(SurferHelper.kPlayerIDFallback,state,version,delay,customData));

        }

        /// <summary>
        /// Open a specific state , with a specific version and custom list of data.
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="version">state version</param>
        /// <param name="customData">list of custom variables</param>
        public void OpenState(string state, int version = SurferHelper.kWhateverVersion, params object[] customData)
        {

            OpenPlayerState(SurferHelper.kPlayerIDFallback, state, version, _noDelay, customData);

        }

        /// <summary>
        /// Open a specific state with a custom list of data. 
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="customData">list of custom variables</param>
        public void OpenState(string state,params object[] customData)
        {

            OpenPlayerState(SurferHelper.kPlayerIDFallback,state,SurferHelper.kWhateverVersion,_noDelay,customData);

        }


        /// <summary>
        /// Open a specific player state, with a specific version and with a custom list of data.
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="state"></param>
        /// <param name="version"></param>
        /// <param name="delay"></param>
        /// <param name="customData"></param>
        public void OpenPlayerState(int playerID,string state, int version = SurferHelper.kWhateverVersion , params object[] customData)
        {


            if (IsStackable(state,playerID) && (IsOpen(state,playerID) || IsStackDelayRunning(state,playerID)))
            {
                var newItem = new SUStateInfo(state,version, playerID, customData);

                if (!_statesStacked.ContainsKey(state))
                    _statesStacked.Add(state, new List<SUStateInfo>() { newItem });
                else
                    _statesStacked[state].Add(newItem);

                return;
            }

            if (IsOpen(state, version,playerID))
                return;
            if (string.IsNullOrEmpty(state))
                return;


            //if we are calling the same state but with another version
            //close the older version of it
            if (IsOpen(state, playerID: playerID))
                ClosePlayerState(playerID, state);


            if (!IsStackable(state, playerID))
                SUEventSystemManager.I.CheckHistoryFocus(playerID,state);



            SUStateEventData eventData = new SUStateEventData(state, version,playerID, customData);
            OpenedStates.Add(eventData);
            CheckParentNesting(state,playerID);
            CheckCloseMode(state,playerID);


            if (_enterReg.TryGetValue(state, out HashSet<ISUStateEnterHandler> set))
            {

                foreach (ISUStateEnterHandler item in set)
                {
                    item.OnSUStateEnter(eventData);
                }
            }

            LastClosedStates.Clear();
        }




        /// <summary>
        /// Open a specific player state, with a specific version and with a custom list of data.
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="state"></param>
        /// <param name="version"></param>
        /// <param name="delay"></param>
        /// <param name="customData"></param>
        public void OpenPlayerState(int playerID, string state, int version = SurferHelper.kWhateverVersion, float delay = _noDelay, params object[] customData)
        {

            if (delay < Mathf.Epsilon)
                OpenPlayerState(playerID,state, version, customData);
            else
                StartCoroutine(DelayOpenPlayerState(playerID,state, version, delay, customData));
        }


        /// <summary>
        /// Open a specific player state with a custom list of data.
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="state">state name</param>
        /// <param name="customData">list of custom variables</param>
        public void OpenPlayerState(int playerID, string state, params object[] customData)
        {

            OpenPlayerState(playerID, state, SurferHelper.kWhateverVersion, _noDelay, customData);

        }


        #endregion


        #region Close State 

        /// <summary>
        /// Close a specific state , with a specific delay,version and custom list of data.
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="delay">delay</param>
        /// <param name="customData">list of custom variables</param>
        public void CloseState(string state, int version = SurferHelper.kWhateverVersion,float delay = _noDelay,params object[] customData)
        {

            ClosePlayerState(SurferHelper.kPlayerIDFallback,state,version,delay,customData);

        }

        /// <summary>
        /// Close a specific state with a custom list of data.
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="customData">list of custom variables</param>
        public void CloseState(string state, params object[] customData)
        {

            ClosePlayerState(SurferHelper.kPlayerIDFallback, state, SurferHelper.kWhateverVersion, _noDelay, customData);

        }

        /// <summary>
        /// Close a specific state of a spcific playerID with a custom list of data.
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="customData">list of custom variables</param>
        public void ClosePlayerState(int playerID,string state, params object[] customData)
        {

            ClosePlayerState(playerID, state, SurferHelper.kWhateverVersion, _noDelay, customData);

        }

        /// <summary>
        /// Close a specific state of a spcific playerID, with a delay and with a custom list of data.
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="delay"></param>
        /// <param name="customData">list of custom variables</param>
        public void ClosePlayerState(int playerID, string state, float delay ,params object[] customData)
        {

            ClosePlayerState(playerID, state, SurferHelper.kWhateverVersion, delay, customData);

        }

        /// <summary>
        /// Close a specific state with a custom list of data and a version.
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="customData">list of custom variables</param>
        public void CloseState(string state, int version = SurferHelper.kWhateverVersion, params object[] customData)
        {

            ClosePlayerState(SurferHelper.kPlayerIDFallback,state,version,_noDelay,customData);
        }


        /// <summary>
        /// Close a specific state of a specific player with a custom list of data.
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="customData">list of custom variables</param>
        public void ClosePlayerState(int playerID,string state, int version = SurferHelper.kWhateverVersion, params object[] customData)
        {

            if (string.IsNullOrEmpty(state))
                return;
            if (!IsOpen(state, version, playerID))
                return;


            SUEventSystemManager.I.ResetHistoryReceiver(playerID,state);
            CheckChildNesting(state,playerID);

            SUStateEventData eventData = default;

            foreach (SUStateEventData item in OpenedStates)
            {

                if (item.StateName.Equals(state))
                {
                    eventData = new SUStateEventData(state, item.Version, playerID, customData);
                    OpenedStates.Remove(item);
                    LastClosedStates.Add(eventData);
                    break;
                }

            }


            if (_exitReg.TryGetValue(state, out HashSet<ISUStateExitHandler> set2))
            {

                foreach (ISUStateExitHandler item in set2)
                {

                    item.OnSUStateExit(eventData);

                }
            }

        }


        /// <summary>
        /// Close a specific state of a specific player with a custom list of data.
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="customData">list of custom variables</param>
        public void ClosePlayerState(int playerID, string state, int version = SurferHelper.kWhateverVersion, float delay = _noDelay, params object[] customData)
        {

            if (delay < Mathf.Epsilon)
                ClosePlayerState(playerID,state, version, customData);
            else
                StartCoroutine(DelayClosePlayerState(playerID,state, version, delay, customData));


        }


        #endregion

        #region Toggle State

        /// <summary>
        /// Toggle a specific state with a specific version,delay and custom list of data.
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="version">state version</param>
        /// <param name="delay">delay</param>
        /// <param name="customData">list of custom variables</param>
        public void ToggleState(string state, int version = SurferHelper.kWhateverVersion, float delay = _noDelay, params object[] customData)
        {

            TogglePlayerState(SurferHelper.kPlayerIDFallback,state,version,delay,customData);

        }


        /// <summary>
        /// Toggle a specific state with a specific version and custom list of data.
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="version">state version</param>
        /// <param name="customData">list of custom variables</param>
        public void ToggleState(string state, int version = SurferHelper.kWhateverVersion, params object[] customData)
        {

            TogglePlayerState(SurferHelper.kPlayerIDFallback,state,version,_noDelay,customData);

        }

        /// <summary>
        /// Toggle a specific state with a specific version, a playerID and custom list of data.
        /// </summary>
        /// <param name="playerID">player ID</param>
        /// <param name="state">state name</param>
        /// <param name="version">state version</param>
        /// <param name="customData">list of custom variables</param>
        public void TogglePlayerState(int playerID,string state, int version = SurferHelper.kWhateverVersion, params object[] customData)
        {

            if (IsOpen(state,version,playerID))
            {
                ClosePlayerState(playerID,state, version, customData);
            }
            else
            {
                OpenPlayerState(playerID,state, version, customData);
            }

        }

        /// <summary>
        /// Toggle a specific state with a specific version, a playerID and custom list of data.
        /// </summary>
        /// <param name="playerID">player ID</param>
        /// <param name="state">state name</param>
        /// <param name="version">state version</param>
        /// <param name="customData">list of custom variables</param>
        public void TogglePlayerState(int playerID, string state, int version = SurferHelper.kWhateverVersion, float delay = _noDelay, params object[] customData)
        { 

            if (delay < Mathf.Epsilon)
                TogglePlayerState(playerID, state, version, customData);
            else
                StartCoroutine(DelayTogglePlayerState(playerID,state, version, delay, customData));

        }

        #endregion


        #region Open Scene


        void LoadScene(string name, bool additive = false, bool async = false,bool autoactivate = false)
        {
            if(!Application.CanStreamedLevelBeLoaded(name))
            return;
            if(LoadingScenes.Contains(name))
            return;
            if(UnloadingScenes.Contains(name))
            return;

            LoadingScenes.Add(name);

            if(autoactivate)
                _autoActiveScenes.Add(name);
            else if (!autoactivate && async)
                _disallowedActivationScenes.Add(name);

            if(async)
                StartCoroutine(AsyncLoadScene(name,additive,autoactivate));
            else
                SceneManager.LoadScene(name, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);

        }

        /// <summary>
        /// Load a specific scene after a specific delay
        /// </summary>
        /// <param name="name">scene name</param>
        /// <param name="delay">delay</param>
        /// <param name="additive">additive mode or not?</param>
        public void LoadScene(string name,float delay = _noDelay,bool additive = false)
        {
            if(delay < Mathf.Epsilon)
                LoadScene(name,additive,false,true);
            else
                StartCoroutine(DelayLoadScene(name,delay,additive,false,true));
        }

        /// <summary>
        /// Load a specific scene (async) after a specific delay
        /// </summary>
        /// <param name="name">scene name</param>
        /// <param name="delay">delay</param>
        /// <param name="additive">additive mode or not?</param>
        /// <param name="autoActivate">autoActivate when loaded or not?</param>
        public void LoadSceneAsync(string name,float delay = _noDelay,bool additive = false,bool autoActivate = true)
        {
            if(delay < Mathf.Epsilon)
                LoadScene(name,additive,true,autoActivate);
            else
                StartCoroutine(DelayLoadScene(name,delay,additive,true,autoActivate));
        }

#endregion

#region Close Scene


        void UnloadScene(string name)
        {
            if(!OpenedScenes.Contains(name))
            return;
            if(LoadingScenes.Contains(name))
            return;
            if(UnloadingScenes.Contains(name))
            return;
            
            UnloadingScenes.Add(name);
            StartCoroutine(AsyncUnloadScene(name));
        }


        /// <summary>
        /// Unload a specific scene after a specific delay
        /// </summary>
        /// <param name="name">scene name</param>
        /// <param name="delay">delay</param>
        public void UnloadSceneAsync(string name , float delay = _noDelay)
        {
            if(delay < Mathf.Epsilon)
                UnloadScene(name);
            else
                StartCoroutine(DelayUnloadScene(name,delay));
        }

#endregion


#region  Other

        /// <summary>
        /// Set a specific scene as active after a specific delay
        /// </summary>
        /// <param name="name">scene name</param>
        /// <param name="delay">delay</param>
        public void SetActiveScene(string name,float delay = _noDelay)
        {

            if(delay < Mathf.Epsilon)
                SetActiveScene(name);
            else
                StartCoroutine(DelaySceneActivation(name,delay));

        }

        void SetActiveScene(string name)
        {
            if(_asyncLoading!=null && _disallowedActivationScenes.Contains(name))
            {
                _disallowedActivationScenes.Remove(name);
                StartCoroutine(AllowSceneActivation(name));
                return;
            }

            try
            {
                if(!SceneManager.SetActiveScene(SceneManager.GetSceneByName(name)))
                {
#if UNITY_EDITOR
                    //Cannot activate scene : it's already active"
#endif
                }
            }
            catch(Exception e)
            {
#if UNITY_EDITOR
                Debug.LogWarning(e.Message);
#endif
            }
            
        }


        /// <summary>
        /// Check if a state of an object is open or not
        /// </summary>
        /// <param name="state">state name</param>
        /// <returns>true if open, false otherwise</returns>
        public bool IsMyStateOpen(GameObject caller,int version = SurferHelper.kWhateverVersion)
        {
            
            return IsOpen(GetObjectStateName(caller,true),version,GetObjectStatePlayerID(caller,true));

        }



        /// <summary>
        /// Check if a state is open or not
        /// </summary>
        /// <param name="state">state name</param>
        /// <returns>true if open, false otherwise</returns>
        public bool IsOpen(string state, int version = SurferHelper.kWhateverVersion, int playerID = SurferHelper.kPlayerIDFallback)
        {
            if (string.IsNullOrEmpty(state))
                return false;

            foreach(SUStateEventData item in OpenedStates)
            {

                if(item.StateName.Equals(state)
                && ( version == SurferHelper.kWhateverVersion || version == item.Version )
                && playerID == item.PlayerID )
                    return true;

            }

            return false;

        }



        /// <summary>
        /// Check if a state is open or not
        /// </summary>
        /// <param name="state">state name</param>
        /// <returns>true if open, false otherwise</returns>
        public bool IsOpen(string state, int playerID = SurferHelper.kPlayerIDFallback)
        {
            return IsOpen(state, SurferHelper.kWhateverVersion, playerID);
        }




        /// <summary>
        /// Check if a state has just been closed
        /// </summary>
        /// <param name="state">state name</param>
        /// <returns>true if closed, false otherwise</returns>
        public bool IsClosed(string state, int playerID = SurferHelper.kPlayerIDFallback)
        {

            foreach (SUStateEventData item in LastClosedStates)
            {

                if (item.StateName.Equals(state)
                    && playerID == item.PlayerID )
                    return true;

            }

            return false;

        }


        /// <summary>
        /// Opens all the parent states of a sub-state
        /// </summary>
        /// <param name="state">sub-state name</param>
        void CheckParentNesting(string state,int playerID)
        {

            //2.0
            foreach (SUElementData item in SUElementData.AllUIStates)
            {

                if (item.StateData.Name.Equals(state) &&
                    item.PlayerID == playerID)
                {
                    for (int i = 0; i < item.ParentUIStates.Count; i++)
                    {
                        OpenPlayerState(item.ParentUIStates[i].PlayerID,item.ParentUIStates[i].Name);
                    }
                }

            }

        }


        /// <summary>
        /// Closes states base on the close Mode 
        /// </summary>
        /// <param name="state">state name</param>
        void CheckCloseMode(string state, int playerID)
        {

            //2.0
            foreach (SUElementData item in SUElementData.AllUIStates)
            {

                if (item.StateData.Name.Equals(state) &&
                    item.PlayerID == playerID)
                {
                    List<SUStateInfo> toClose = item.StatesToClose;

                    for (int i = 0; i < toClose?.Count; i++)
                    {
                        ClosePlayerState(toClose[i].PlayerID, toClose[i].Name, item.CloseDelay,null);
                    }
                }

            }

        }


        /// <summary>
        /// Check if a state is stackable or not
        /// </summary>
        /// <param name="state">state to check </param>
        bool IsStackable(string state, int playerID)
        {
            foreach (SUElementData item in SUElementData.AllUIStates)
            {

                if (item.StateData.Name.Equals(state) && item.IsStackable
                    && playerID == item.PlayerID )
                    return true;

            }

            return false;
        }


        /// <summary>
        /// Check if a stackable state has its stack delay running or not
        /// </summary>
        /// <param name="state">state to check </param>
        bool IsStackDelayRunning(string state, int playerID )
        {
            foreach (SUElementData item in SUElementData.AllUIStates)
            {

                if (item.StateData.Name.Equals(state) && item.IsStackable && item.IsStackDelayRunning
                     && playerID == item.PlayerID )
                    return true;

            }

            return false;
        }


        /// <summary>
        /// Check if a state has a stack of "opening states"
        /// </summary>
        /// <param name="state">state to check </param>
        public void CheckStateStack(string state,int playerID)
        {

            if(_statesStacked.TryGetValue(state, out List<SUStateInfo> list))
            {

                for (int i = 0; i < list.Count; i++)
                {

                    if (list[i].PlayerID == playerID)
                    {

                        OpenPlayerState(playerID, state, list[i].Version, _noDelay, list[i].CustomData);
                        list.RemoveAt(i);

                        if (list.Count <= 0)
                            _statesStacked.Remove(state);

                    }


                }
            }


        }



        /// <summary>
        /// Closes all the child states of a parent state
        /// </summary>
        /// <param name="state">parent state name</param>
        void CheckChildNesting(string state, int playerID)
        {

            //2.0
            foreach (SUElementData item in SUElementData.AllUIStates)
            {

                if (item.StateData.Name.Equals(state)
                    && item.PlayerID == playerID)
                {
                    for (int i = 0; i < item.ChildUIStates.Count; i++)
                    {
                        ClosePlayerState(item.ChildUIStates[i].PlayerID,item.ChildUIStates[i].Name);
                    }
                }

            }

        }


        /// <summary>
        /// Gets the parent state element of a game object
        /// </summary>
        /// <param name="obj"></param>
        public SUElement GetObjectStateElement(GameObject obj,bool checkCaller = false)
        {
            if (obj == null)
                return null;

            SUElement eleState = default;

            if(checkCaller)
            {

                eleState = obj.GetComponent<SUElement>();

                if (eleState != null && eleState.IsState)
                    return eleState;

                eleState = null;
            }

            RecursiveGetParentStateUI(obj.transform, ref eleState);

            if (eleState != null && eleState.ElementData.IsGroupStates)
                eleState = null;

            return eleState;
        }



        /// <summary>
        /// Gets the parent state name of a game object
        /// </summary>
        /// <param name="obj"></param>
        public string GetObjectStateName(GameObject obj, bool checkCaller = false)
        {
            var ele = GetObjectStateElement(obj, checkCaller);

            if(ele == null)
                return string.Empty;

            if(ele.ElementData.IsGroupStates)
            {
                return ele.ElementData.GetGroupStateNameOfObject(obj,ele.gameObject);
            }

            return ele.ElementData.StateData.Name;
        }

        /// <summary>
        /// Gets the parent state of a game object
        /// </summary>
        /// <param name="obj"></param>
        public Transform GetObjectStateTransfom(GameObject obj, bool checkCaller = false)
        {
            var ele = GetObjectStateElement(obj, checkCaller);

            if(ele == null)
                return null;

            if(ele.ElementData.IsGroupStates)
            {
                return ele.ElementData.GetGroupStateOfObject(obj,ele.gameObject);
            }

            return ele.transform;

        }

        /// <summary>
        /// Gets the playerID of the parent state of a game object
        /// </summary>
        /// <param name="obj"></param>
        public int GetObjectStatePlayerID(GameObject obj, bool checkCaller = false)
        {

            var ele = GetObjectStateElement(obj, checkCaller);

            return ele != null ? ele.ElementData.PlayerID : SurferHelper.kPlayerIDFallback;
        }


        /// <summary>
        /// Recursively get the parent state of an object
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="output"></param>
        public void RecursiveGetParentStateUI(Transform caller, ref SUElement output)
        {

            SUElement cp = null;

            if (caller.transform.parent != null)
            {
                cp = caller.transform.parent.GetComponent<SUElement>();

                if (cp != null && (cp.IsState || cp.ElementData.IsGroupStates))
                    output = cp;
                else
                    RecursiveGetParentStateUI(caller.transform.parent, ref output);

            }

        }




        /// <summary>
        /// Get a list of all parent states 
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="output"></param>
        public void RecursiveGetParentStatesUI(Transform caller, ref List<SUStateInfo> output)
        {

            SUElement cp = null;

            if (caller.transform.parent != null)
            {
                cp = caller.transform.parent.GetComponent<SUElement>();

                if (cp != null && cp.IsState)
                    output.Add(new SUStateInfo(cp.ElementData));

                RecursiveGetParentStatesUI(caller.transform.parent, ref output);

            }

        }

        /// <summary>
        /// Get a list of all child states
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="output"></param>
        public void RecursiveGetChildStatesUI(Transform caller, ref List<SUStateInfo> output)
        {

            SUElement cp = null;

            foreach (Transform child in caller)
            {
                cp = child.GetComponent<SUElement>();

                if (cp != null && cp.IsState)
                    output.Add(new SUStateInfo(cp.ElementData));

                RecursiveGetChildStatesUI(child, ref output);
            }

        }


        #endregion


        #region COROUTINES


        IEnumerator DelaySendCustomEvent(string customEvent, float delay, params object[] customData)
        {
            yield return new WaitForSeconds(delay);

            SendCustomEvent(customEvent, customData);
        }

        IEnumerator DelayClosePlayerState(int playerID,string state,int version,float delay, params object[] customData)
        {
            yield return new WaitForSeconds(delay);

            ClosePlayerState(playerID,state,version,customData);
        }

        IEnumerator DelayTogglePlayerState(int playerID,string state, int version, float delay, params object[] customData)
        {
            yield return new WaitForSeconds(delay);

            TogglePlayerState(playerID,state, version, customData);
        }

        IEnumerator DelayOpenPlayerState(int playerID,string state,int version,float delay = _noDelay, params object[] customData)
        {
            yield return new WaitForSeconds(delay);

            OpenPlayerState(playerID,state,version,customData);
        }

        IEnumerator DelaySceneActivation(string scene,float delay = _noDelay)
        {
            yield return new WaitForSeconds(delay);

            SetActiveScene(scene);
        }

        IEnumerator AsyncLoadScene(string scene,bool additive,bool autoActivate)
        {
            _asyncLoading = SceneManager.LoadSceneAsync(scene, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            if (!autoActivate)
                _asyncLoading.allowSceneActivation = false;

            while(!_asyncLoading.isDone)
            {
                _loadingData.UpdateProgress(scene,_asyncLoading.progress*100f);
                CallSceneLoadingEvents( _loadingData );
                yield return _waitForEndOfFrame;

            }
            
            _loadingData.UpdateProgress(scene,_asyncLoading.progress*100f);
            CallSceneLoadingEvents(_loadingData);

        }

        IEnumerator AllowSceneActivation(string sceneName)
        {
            if (_asyncLoading == null)
                yield break;

            _asyncLoading.allowSceneActivation = true;
            
            while(!_asyncLoading.isDone)
            {
                yield return _waitForEndOfFrame;
            }
            SetActiveScene(sceneName);
            yield break;
        }

        IEnumerator AsyncUnloadScene(string scene)
        {

            AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(scene);
     

            while(!asyncLoad.isDone)
            {
                _unloadingData.UpdateProgress(scene,asyncLoad.progress*100f);
                CallSceneUnloadingEvents(_unloadingData);
                yield return _waitForEndOfFrame;

            }
            
            _unloadingData.UpdateProgress(scene,asyncLoad.progress*100f);
            CallSceneUnloadingEvents(_unloadingData);

        }

        IEnumerator DelayUnloadScene(string scene,float delay)
        {
            yield return new WaitForSeconds(delay);

            UnloadScene(scene);
        }

        IEnumerator DelayLoadScene(string scene,float delay, bool additive = false, bool async = false,bool autoactivate = false)
        {
            yield return new WaitForSeconds(delay);

            LoadScene(scene,additive,async,autoactivate);
        }

#endregion





    }


}