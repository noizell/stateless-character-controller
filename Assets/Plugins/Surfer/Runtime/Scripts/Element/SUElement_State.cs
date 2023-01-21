using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Surfer
{

    public partial class SUElement : ISUStateEnterHandler, ISUStateExitHandler
    {
        
        void RunEventBehaviourParamsState(SUEvent.Type_ID eventID, SUStateEventData eventInfo, bool onlyVersionCheck = false)
        {

            if (_events.TryGetValue(eventID, out SUBehavioursData value))
            {
                value.RunWithParamsState(gameObject, eventInfo, onlyVersionCheck);
            }
        }

        void CheckStateEvents()
        {


            if (_events.TryGetValue(SUEvent.Type_ID.State_Enter, out var valueEnter))
            {
                for (int i = 0; i < valueEnter.Behaviours.Count; i++)
                {
                    SurferManager.I.RegisterStateEnter(this, valueEnter.Behaviours[i].Event.StatesData.AllNamesArray);
                }

            }
            if (_events.TryGetValue(SUEvent.Type_ID.State_Exit, out var valueExit))
            {
                for (int i = 0; i < valueExit.Behaviours.Count; i++)
                {
                    SurferManager.I.RegisterStateExit(this, valueExit.Behaviours[i].Event.StatesData.AllNamesArray);
                }

            }

            if (_events.ContainsKey(SUEvent.Type_ID.State_MyStateExit)
                || _elementData.IsTooltip || _elementData.IsStackable)
            {

                if (string.IsNullOrEmpty(ElementData.StateName))
                    return;

                SurferManager.I.RegisterStateExit(this, ElementData.StateName);

            }

            if (_events.ContainsKey(SUEvent.Type_ID.State_MyStateEnter)
                || _elementData.IsTooltip)
            {

                if (string.IsNullOrEmpty(ElementData.StateName))
                    return;

                SurferManager.I.RegisterStateEnter(this, ElementData.StateName);

            }

        }

        public void OnSUStateEnter(SUStateEventData eventInfo)
        {
            RunEventBehaviourParamsState(SUEvent.Type_ID.State_Enter,eventInfo);


            if (ElementData.StateName == eventInfo.StateName && ElementData.PlayerID == eventInfo.PlayerID)
            {
                RunEventBehaviourParamsState(SUEvent.Type_ID.State_MyStateEnter, eventInfo, true);

                CheckTooltipTypeForMyStateEnter();

                CheckDragTypeForMyStateEnter();

            }
            
                
        }

        public void OnSUStateExit(SUStateEventData eventInfo)
        {
            RunEventBehaviourParamsState(SUEvent.Type_ID.State_Exit, eventInfo);

            if (ElementData.StateName == eventInfo.StateName && ElementData.PlayerID == eventInfo.PlayerID)
            {
                RunEventBehaviourParamsState(SUEvent.Type_ID.State_MyStateExit, eventInfo, true);

                CheckTooltipTypeForMyStateExit();
                CheckDragTypeForMyStateExit();


                //stackable check
                if (_elementData.IsStackable && _stackRoutine == null)
                    _stackRoutine = StartCoroutine(StackRoutine());


            }



        }

        void ResetStateEvents()
        {


            if (_events.TryGetValue(SUEvent.Type_ID.State_Enter, out var valueEnter))
            {
                for (int i = 0; i < valueEnter.Behaviours.Count; i++)
                {
                    SurferManager.I.UnregisterStateEnter(this, valueEnter.Behaviours[i].Event.StatesData.AllNamesArray);
                }

            }
            if (_events.TryGetValue(SUEvent.Type_ID.State_Exit, out var valueExit))
            {
                for (int i = 0; i < valueExit.Behaviours.Count; i++)
                {
                    SurferManager.I.UnregisterStateExit(this, valueExit.Behaviours[i].Event.StatesData.AllNamesArray);
                }

            }
            if (_events.ContainsKey(SUEvent.Type_ID.State_MyStateExit))
            {
                if (string.IsNullOrEmpty(ElementData.StateName))
                    return;

                SurferManager.I.UnregisterStateExit(this, ElementData.StateName);

            }

            if (_events.ContainsKey(SUEvent.Type_ID.State_MyStateEnter))
            {

                if (string.IsNullOrEmpty(ElementData.StateName))
                    return;

                SurferManager.I.UnregisterStateEnter(this, ElementData.StateName);

            }

        }


    }

}

