using System.Collections;
using System.Collections.Generic;

#if SURew
using Rewired;
#endif
using UnityEngine;


namespace Surfer
{

    [System.Serializable]
    public class SUBehavioursData
    {

        [SerializeField]
        List<SUBehaviourData> _behaviours = new List<SUBehaviourData>();
        public List<SUBehaviourData> Behaviours { get => _behaviours; }


        public void Run(GameObject go,object evtData = null)
        {
            for(int i=0;i<_behaviours.Count;i++)
            {
                _behaviours[i].Run(go, evtData);
            }
        }



#if SUOld

        public void RunWithParams(GameObject go, KeyCode keyVal)
        {


            for (int i = 0; i < _behaviours.Count; i++)
            {

                if (keyVal != _behaviours[i].Event.KeyCodeVal)
                    continue;

                _behaviours[i].Run(go,null);
            }


        }

#endif

        public void RunWithParams(GameObject go, string stringVal)
        {

            for (int i = 0; i < _behaviours.Count; i++)
            {

                if (stringVal != _behaviours[i].Event.StringVal)
                    continue;

                _behaviours[i].Run(go);
            }

        }

        public void RunWithParamsCustomEvent(GameObject go, SUCustomEventEventData cEvent)
        {

            for (int i = 0; i < _behaviours.Count; i++)
            {

                if (!_behaviours[i].Event.CEventsData.AllNames.Contains(cEvent.Name))
                    continue;

                _behaviours[i].Run(go,eventName : cEvent.Name,cEvent);
            }

        }

        public void RunWithParamsScene(GameObject go, string stringVal,object evtData = null)
        {


            for (int i = 0; i < _behaviours.Count; i++)
            {

                if (!_behaviours[i].Event.ScenesData.AllNames.Contains(stringVal) )
                    continue;

                _behaviours[i].Run(go,evtData);
            }

        }


        public void RunWithParamsState(GameObject go, SUStateEventData eventData, bool onlyVersionCheck = false)
        {

            List<string> _list = default;

            for (int i = 0; i < _behaviours.Count; i++)
            {

                
                if(!onlyVersionCheck)
                {
                    
                    //playerID check 
                    var pID = _behaviours[i].Event.IntVal2;

                    if(pID == SurferHelper.kNestedPlayerID)
                    {
                        pID = SurferManager.I.GetObjectStatePlayerID(go,true);
                    }

                    if (pID != eventData.PlayerID)
                        continue;

                    
                    //state name check
                    _list = _behaviours[i].Event.StatesData.AllNames;

                    if (!_list.Contains(eventData.StateName))
                        continue;
                }

                if (_behaviours[i].Event.IntVal != eventData.Version &&
                    _behaviours[i].Event.IntVal != SurferHelper.kWhateverVersion && eventData.Version != SurferHelper.kWhateverVersion)
                    continue;

                _behaviours[i].Run(go,eventData);
            }

        }


#if SURew

        public void RunWithParams(GameObject go, InputActionEventData eventData)
        {


            for (int i = 0; i < _behaviours.Count; i++)
            {
                if (eventData.updateLoop != _behaviours[i].Event.UpdateLoop)
                    continue;
                if (eventData.eventType != _behaviours[i].Event.EventType)
                    continue;
                if (eventData.playerId != _behaviours[i].Event.IntVal && _behaviours[i].Event.IntVal != 0)
                    continue;
                if (eventData.actionName != _behaviours[i].Event.StringVal)
                    continue;

                _behaviours[i].Run(go,eventData);
            }

        }

#endif



        public void AddCustomReaction(SUReactionData rData,SUEvent.Type_ID eventID)
        {

            SUBehaviourData bhv = new SUBehaviourData();
            bhv.AddCustomReaction(rData);
            bhv.SetEvent(eventID);

            _behaviours.Add(bhv);
        }


    }

}




