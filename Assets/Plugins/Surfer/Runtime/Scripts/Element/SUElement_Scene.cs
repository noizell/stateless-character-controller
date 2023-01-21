using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Surfer
{

    public partial class SUElement : ISUSceneActivatedHandler, ISUSceneLoadedHandler, 
    ISUSceneUnloadedHandler, ISUSceneUnloadingHandler, 
    ISUSceneDeactivatedHandler, ISUSceneLoadingHandler
    {
        

        void RunEventBehaviourParamsScene(SUEvent.Type_ID eventID, string stringVal,object evtData = null)
        {

            if (_events.TryGetValue(eventID, out SUBehavioursData value))
            {
                value.RunWithParamsScene(gameObject, stringVal,evtData);
            }
        }




        void CheckSceneEvents()
        {


            //my scene
            if (_events.ContainsKey(SUEvent.Type_ID.Scene_MySceneActivated))
            {
                SurferManager.I.RegisterSceneActivated(this, _mySceneName);
            }
            if (_events.ContainsKey(SUEvent.Type_ID.Scene_MySceneLoaded))
            {
                SurferManager.I.RegisterSceneLoaded(this, _mySceneName);
            }
            if (_events.ContainsKey(SUEvent.Type_ID.Scene_MySceneUnloaded))
            {
                SurferManager.I.RegisterSceneUnloaded(this, _mySceneName);
            }
            if (_events.ContainsKey(SUEvent.Type_ID.Scene_MySceneDeactivated))
            {
                SurferManager.I.RegisterSceneDeactivated(this, _mySceneName);
            }
            if (_events.ContainsKey(SUEvent.Type_ID.Scene_MySceneLoading))
            {
                SurferManager.I.RegisterSceneLoading(this, _mySceneName);
            }
            if (_events.ContainsKey(SUEvent.Type_ID.Scene_MySceneUnloading))
            {
                SurferManager.I.RegisterSceneUnloading(this, _mySceneName);
            }


            //custom scene
            if (_events.TryGetValue(SUEvent.Type_ID.Scene_Activated, out var valueAct))
            {
                for (int i = 0; i < valueAct.Behaviours.Count; i++)
                    SurferManager.I.RegisterSceneActivated(this, valueAct.Behaviours[i].Event.ScenesData.AllNamesArray);
            }
            if (_events.TryGetValue(SUEvent.Type_ID.Scene_Loaded, out var valueLoa))
            {
                for (int i = 0; i < valueLoa.Behaviours.Count; i++)
                    SurferManager.I.RegisterSceneLoaded(this, valueLoa.Behaviours[i].Event.ScenesData.AllNamesArray);
            }
            if (_events.TryGetValue(SUEvent.Type_ID.Scene_Unloaded, out var valueUnl))
            {
                for (int i = 0; i < valueUnl.Behaviours.Count; i++)
                    SurferManager.I.RegisterSceneUnloaded(this, valueUnl.Behaviours[i].Event.ScenesData.AllNamesArray);
            }
            if (_events.TryGetValue(SUEvent.Type_ID.Scene_Deactivated, out var valueDea))
            {
                for (int i = 0; i < valueDea.Behaviours.Count; i++)
                    SurferManager.I.RegisterSceneDeactivated(this, valueDea.Behaviours[i].Event.ScenesData.AllNamesArray);
            }
            if (_events.TryGetValue(SUEvent.Type_ID.Scene_Loading, out var valueLoai))
            {
                for (int i = 0; i < valueLoai.Behaviours.Count; i++)
                    SurferManager.I.RegisterSceneLoading(this, valueLoai.Behaviours[i].Event.ScenesData.AllNamesArray);
            }
            if (_events.TryGetValue(SUEvent.Type_ID.Scene_Unloading, out var valueUnli))
            {
                for (int i = 0; i < valueUnli.Behaviours.Count; i++)
                    SurferManager.I.RegisterSceneUnloading(this, valueUnli.Behaviours[i].Event.ScenesData.AllNamesArray);
            }



        }

        public void OnSUSceneActivated(SUSceneActivatedEventData eventInfo)
        {
            RunEventBehaviourParamsScene(SUEvent.Type_ID.Scene_Activated,eventInfo.SceneName,eventInfo);

            if(_mySceneName == eventInfo.SceneName)
                RunEventBehaviour(SUEvent.Type_ID.Scene_MySceneActivated,eventInfo);
        }

        public void OnSUSceneUnloaded(SUSceneUnloadedEventData eventInfo)
        {
            RunEventBehaviourParamsScene(SUEvent.Type_ID.Scene_Unloaded, eventInfo.SceneName,eventInfo);

            if (_mySceneName == eventInfo.SceneName)
                RunEventBehaviour(SUEvent.Type_ID.Scene_MySceneUnloaded,eventInfo);
        }

        public void OnSUSceneLoaded(SUSceneLoadedEventData eventInfo)
        {

            RunEventBehaviourParamsScene(SUEvent.Type_ID.Scene_Loaded, eventInfo.SceneName,eventInfo);

            if (_mySceneName == eventInfo.SceneName)
                RunEventBehaviour(SUEvent.Type_ID.Scene_MySceneLoaded,eventInfo);
        }


        public void OnSUSceneLoading(SUSceneLoadingEventData eventInfo)
        {

            RunEventBehaviourParamsScene(SUEvent.Type_ID.Scene_Loading, eventInfo.SceneName,eventInfo);

            if (_mySceneName == eventInfo.SceneName)
                RunEventBehaviourParamsScene(SUEvent.Type_ID.Scene_MySceneLoading, eventInfo.SceneName,eventInfo);

            UpdateLoadingType(eventInfo);
        }

        public void OnSUSceneUnloading(SUSceneUnloadingEventData eventInfo)
        {

            RunEventBehaviourParamsScene(SUEvent.Type_ID.Scene_Unloading, eventInfo.SceneName,eventInfo);

            if (_mySceneName == eventInfo.SceneName)
                RunEventBehaviourParamsScene(SUEvent.Type_ID.Scene_MySceneUnloading, eventInfo.SceneName,eventInfo);
        }


        public void OnSUSceneDeactivated(SUSceneDeactivatedEventData eventInfo)
        {
            RunEventBehaviourParamsScene(SUEvent.Type_ID.Scene_Deactivated, eventInfo.SceneName,eventInfo);

            if (_mySceneName == eventInfo.SceneName)
                RunEventBehaviour(SUEvent.Type_ID.Scene_MySceneDeactivated,eventInfo);
        }


        void ResetSceneEvents()
        {


            //my scene
            if (_events.ContainsKey(SUEvent.Type_ID.Scene_MySceneActivated))
            {
                SurferManager.I.UnregisterSceneActivated(this, _mySceneName);
            }
            if (_events.ContainsKey(SUEvent.Type_ID.Scene_MySceneLoaded))
            {
                SurferManager.I.UnregisterSceneLoaded(this, _mySceneName);
            }
            if (_events.ContainsKey(SUEvent.Type_ID.Scene_MySceneUnloaded))
            {
                SurferManager.I.UnregisterSceneUnloaded(this, _mySceneName);
            }
            if (_events.ContainsKey(SUEvent.Type_ID.Scene_MySceneDeactivated))
            {
                SurferManager.I.UnregisterSceneDeactivated(this, _mySceneName);
            }
            if (_events.ContainsKey(SUEvent.Type_ID.Scene_MySceneLoading))
            {
                SurferManager.I.UnregisterSceneLoading(this, _mySceneName);
            }
            if (_events.ContainsKey(SUEvent.Type_ID.Scene_MySceneUnloading))
            {
                SurferManager.I.UnregisterSceneUnloading(this, _mySceneName);
            }


            //custom scene
            if (_events.TryGetValue(SUEvent.Type_ID.Scene_Activated, out var valueAct))
            {
                for (int i = 0; i < valueAct.Behaviours.Count; i++)
                    SurferManager.I.UnregisterSceneActivated(this, valueAct.Behaviours[i].Event.ScenesData.AllNamesArray);
            }
            if (_events.TryGetValue(SUEvent.Type_ID.Scene_Loaded, out var valueLoa))
            {
                for (int i = 0; i < valueLoa.Behaviours.Count; i++)
                    SurferManager.I.UnregisterSceneLoaded(this, valueLoa.Behaviours[i].Event.ScenesData.AllNamesArray);
            }
            if (_events.TryGetValue(SUEvent.Type_ID.Scene_Unloaded, out var valueUnl))
            {
                for (int i = 0; i < valueUnl.Behaviours.Count; i++)
                    SurferManager.I.UnregisterSceneUnloaded(this, valueUnl.Behaviours[i].Event.ScenesData.AllNamesArray);
            }
            if (_events.TryGetValue(SUEvent.Type_ID.Scene_Deactivated, out var valueDea))
            {
                for (int i = 0; i < valueDea.Behaviours.Count; i++)
                    SurferManager.I.UnregisterSceneDeactivated(this, valueDea.Behaviours[i].Event.ScenesData.AllNamesArray);
            }
            if (_events.TryGetValue(SUEvent.Type_ID.Scene_Loading, out var valueLoai))
            {
                for (int i = 0; i < valueLoai.Behaviours.Count; i++)
                    SurferManager.I.UnregisterSceneLoading(this, valueLoai.Behaviours[i].Event.ScenesData.AllNamesArray);
            }
            if (_events.TryGetValue(SUEvent.Type_ID.Scene_Unloading, out var valueUnli))
            {
                for (int i = 0; i < valueUnli.Behaviours.Count; i++)
                    SurferManager.I.UnregisterSceneUnloading(this, valueUnli.Behaviours[i].Event.ScenesData.AllNamesArray);
            }



        }




    }

}


