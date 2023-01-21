using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Surfer
{
    /// <summary>
    /// Scriptable object of Surfer that stores the state names
    /// </summary>
    public class SurferSO : ScriptableObject
    {           

        [SerializeField]
        StatesDictionary _states = new StatesDictionary();

        [SerializeField]
        ScenesDictionary _scenes = new ScenesDictionary();

        [SerializeField]
        EventsDictionary _events = new EventsDictionary();
        


        [SerializeField]
        [HideInInspector]
        string[] _tags = default;
        public string[] Tags { get => _tags; }


        [SerializeField]
        [HideInInspector]
        string[] _layers = default;
        public string[] Layers { get => _layers; }


        [SerializeField]
        [HideInInspector]
        VersionsDictionary _buildVersions = new VersionsDictionary();
        public VersionsDictionary BuildVersions { get => _buildVersions; }


        /// <summary>
        /// Adds the default state names.Used when it is first created
        /// </summary>
        public void SetUp()
        {

            AddState("Splash");
            AddState("Menu");
            AddState("SideMenu");
            AddState("Logo");
            AddState("Loading");
            AddState("LoadingText");
            AddState("Overlay");
            AddState("HUD");
            AddState("Profile");
            AddState("Map");
            AddState("Friends");
            AddState("Matchmaking");
            AddState("Settings");
            AddState("Clan");
            AddState("Battle");
            AddState("Match");
            AddState("QuickMatch");
            AddState("Invite");
            AddState("Upload");
            AddState("Download");
            AddState("Share");
            AddState("Nickname");
            AddState("Login");
            AddState("Avatar");
            AddState("Notification");
            AddState("Internet");
            AddState("Error");
            AddState("Warning");
            AddState("Prize");
            AddState("GameOver");
            AddState("WinScreen");
            AddState("LoseScreen");
            AddState("Controls");
            AddState("Help");
            AddState("Message");
            AddState("Info");
            AddState("Instructions");
            AddState("Tutorial");
            AddState("Tutorial1");
            AddState("Tutorial2");
            AddState("Tutorial3");
            AddState("Tutorial4");
            AddState("Tutorial5");
            AddState("Step1");
            AddState("Step2");
            AddState("Step3");
            AddState("Step4");
            AddState("Step5");
            AddState("Step6");
            AddState("Step7");
            AddState("Step8");
            AddState("Step9");
            AddState("Step10");
            AddState("DailyReward");
            AddState("Waiting");
            AddState("Leaderboard");
            AddState("Characters");
            AddState("Shop");
            AddState("Showcase");
            AddState("Collection");
            AddState("Weapons");
            AddState("Chat");
            AddState("Story");
            AddState("Languages");
            AddState("BetaInfo");
            AddState("Privacy");
            AddState("Announcement");
            AddState("News");
            AddState("Pre-Match");
            AddState("Welcome");
            AddState("Countdown");
            AddState("Customization");
            AddState("Finish");
            AddState("Preview");
            AddState("Levels");
            AddState("Intro");
            AddState("Outro");
            AddState("Selection");
            AddState("Deletion");
            AddState("Confirm");


            //events
            AddEvent("MyCustomEvent");
            AddEvent("OnPlayFabLogin");

        }


        /// <summary>
        /// Add a new custom event. 
        /// </summary>
        /// <param name="newName">new event name</param>
        public void AddEvent(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                return;
            if (_events.ContainsValue(newName))
                return;

            _events.Add((System.Guid.NewGuid()).ToString(), newName);

        }


        /// <summary>
        /// Rename a specific event. Used only in the Surfer Settings Window
        /// </summary>
        /// <param name="idx">event key</param>
        /// <param name="newName">event new name</param>
        public void RenameEvent(string key, string newName)
        {
            if (_events.TryGetValue(key, out string name))
                _events[key] = newName;

        }

        /// <summary>
        /// Rename a specific state. Used only in the Surfer Settings Window
        /// </summary>
        /// <param name="idx">state key</param>
        /// <param name="newName">state new name</param>
        public void RenameState(string key,string newName)
        {
            if(_states.TryGetValue(key,out string name))
                _states[key] = newName;

        }

        /// <summary>
        /// Add a new state. 
        /// </summary>
        /// <param name="newName">new state name</param>
        public void AddState(string newName)
        {
            if(string.IsNullOrEmpty(newName))
            return;
            if(_states.ContainsValue(newName))
            return;

            _states.Add((System.Guid.NewGuid()).ToString(),newName);
            
        }

        /// <summary>
        /// Remove a specific event. 
        /// </summary>
        /// <param name="eventName">event name</param>
        public void RemoveEvent(string eventName)
        {
            string key = GetEventKey(eventName);

            if (string.IsNullOrEmpty(key))
                return;

            _events.Remove(key);
        }

        /// <summary>
        /// Remove a specific state. 
        /// </summary>
        /// <param name="stateName">state name</param>
        public void RemoveState(string stateName)
        {
            string key = GetStateKey(stateName);

            if(string.IsNullOrEmpty(key))
            return;
            
            _states.Remove(key);
        }


        /// <summary>
        /// Get the key of a specific event. 
        /// </summary>
        /// <param name="eventName">event name</param>
        /// <returns>key of the event</returns>
        public string GetEventKey(string eventName)
        {
            foreach (KeyValuePair<string, string> pair in _events)
            {
                if (pair.Value.Equals(eventName))
                    return pair.Key;
            }
            return "";
        }

        /// <summary>
        /// Get the key of a specific state. 
        /// </summary>
        /// <param name="stateName">state name</param>
        /// <returns>key of the state</returns>
        public string GetStateKey(string stateName)
        {
            foreach(KeyValuePair<string,string> pair in _states)
            {
                if(pair.Value.Equals(stateName))
                return pair.Key;
            }
            return "";
        }



        /// <summary>
        /// Get the name of the event with the specific key. 
        /// </summary>
        /// <param name="index">event key</param>
        /// <returns>event name</returns>
        public string GetEvent(string key)
        {
            if (_events.TryGetValue(key, out string name))
                return name;

            return "";

        }



        /// <summary>
        /// Get the name of the state with the specific key.
        /// </summary>
        /// <param name="index">state key</param>
        /// <returns>state name</returns>
        public string GetState(string key)
        {
            if(_states.TryGetValue(key,out string name))
            return name;
            
            return "";

        }

        /// <summary>
        /// Get a list of all the state names. 
        /// </summary>
        /// <returns>names list</returns>
        public string[] GetStatesList()
        {

            return _states.Select(x=>x.Value).Where(x=>!string.IsNullOrEmpty(x)).OrderBy(x=>x).Prepend(SurferHelper.Unset).ToArray();

        }


        /// <summary>
        /// Get a list of all the event names. 
        /// </summary>
        /// <returns>names list</returns>
        public string[] GetEventsList(bool addConstPrefix = false)
        {
            string[] names = new string[_events.Count + 1];
            names[0] = SurferHelper.Unset;

            int index = 1;
            foreach (KeyValuePair<string, string> pair in _events)
            {
                names[index] = addConstPrefix ? "*Event" + pair.Value : pair.Value;
                index++;
            }

            return names;
        }


#if UNITY_EDITOR

        [InitializeOnLoadMethod]
        static void UpdateSOEditorLists()
        {
            SurferHelper.SO.UpdateSceneList();
            SurferHelper.SO.UpdateEventsList();
            SurferHelper.SO.UpdateLayersList();
            SurferHelper.SO.UpdateTagsList();
        }


        /// <summary>
        /// Update the scenes list with GUID and names . Editor only
        /// </summary>
        public void UpdateSceneList()
        {
            _scenes.Clear();

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                var key = EditorBuildSettings.scenes[i].guid.ToString();

                if(!_scenes.ContainsKey(key))
                {
                    _scenes.Add(key,System.IO.Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path));
                }
            }
        }

        /// <summary>
        /// Update the events list . Editor only
        /// </summary>
        public void UpdateEventsList()
        {
            if(_events.Count <= 0)
            {
                AddEvent("OnSomethingHappened");
                AddEvent("OnLoginFailed");
            }
        }


        /// <summary>
        /// Update the tags list . Editor only
        /// </summary>
        public void UpdateTagsList()
        {
            _tags = UnityEditorInternal.InternalEditorUtility.tags;
        }

        /// <summary>
        /// Update the layers list . Editor only
        /// </summary>
        public void UpdateLayersList()
        {
            _layers = UnityEditorInternal.InternalEditorUtility.layers;
        }


#endif
        /// <summary>
        /// Get all scene names
        /// </summary>
        /// <returns>array of scene names</returns>
        public string[] GetScenesNameStrings(bool addConstPrefix = false)
        {
            
            string[] names = new string[_scenes.Count+1];
            names[0] = SurferHelper.Unset;

            int index = 1;
            foreach(KeyValuePair<string,string> pair in _scenes)
            {
                names[index] = addConstPrefix ? "*Scene"+pair.Value : pair.Value;
                index++;
            }

            return names;
        }


        /// <summary>
        /// Get the name of a scene
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>scene name</returns>
        public string GetSceneName(string guid)
        {
            if(_scenes.TryGetValue(guid,out string value))
            {
                return value;
            }

            return "";
        }


        /// <summary>
        /// Get a GUID of a scene
        /// </summary>
        /// <param name="name"></param>
        /// <returns>scene guid</returns>
        public string GetSceneGUID(string name)
        {
            foreach(KeyValuePair<string,string> pair in _scenes)
            {
                if(pair.Value.Equals(name))
                return pair.Key;
            }

            return "";
        }





    }

    [Serializable]
    public class VersionsDictionary : SerializableDictionary<RuntimePlatform, string> { }
    [Serializable]
    public class StatesDictionary : SerializableDictionary<string, string> {}
    [Serializable]
    public class ScenesDictionary : SerializableDictionary<string, string> { }
    [Serializable]
    public class EventsDictionary : SerializableDictionary<string, string> { }

}

