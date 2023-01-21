using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public partial class SUElement : ISUCustomEventHandler
    {

        public void OnSUCustomEvent(SUCustomEventEventData eventInfo)
        {
            RunCustomEventBehaviour(eventInfo);
        }


        void CheckCustomEvents()
        {
            
            if(_events.TryGetValue(SUEvent.Type_ID.MyCustomEvent, out var valueCustom))
            {
                for (int i = 0; i < valueCustom.Behaviours.Count; i++)
                {
                    SurferManager.I.RegisterCustomEvent(this, valueCustom.Behaviours[i].Event.CEventsData.AllNamesArray);
                }
            }

        }


        public void RunCustomEventBehaviour(SUCustomEventEventData cEvent)
        {

            if (_events.TryGetValue(SUEvent.Type_ID.MyCustomEvent, out SUBehavioursData value))
            {
                value.RunWithParamsCustomEvent(gameObject,cEvent);
            }
        }


        void ResetCustomEvents()
        {
            
            if (_events.TryGetValue(SUEvent.Type_ID.MyCustomEvent, out var valueCustom))
            {
                for (int i = 0; i < valueCustom.Behaviours.Count; i++)
                {
                    SurferManager.I.UnregisterCustomEvent(this, valueCustom.Behaviours[i].Event.CEventsData.AllNamesArray);
                }
            }
        }

    }

}
