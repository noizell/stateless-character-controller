using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if SUBolt

using Bolt;
using Ludiq;

#endif


namespace Surfer
{


    [System.Serializable]
    public class SUBehaviourData
    {
        [SerializeField]
        SUEvent _event = default;
        public SUEvent Event { get => _event; }

        [SerializeField]
        SUConditionsData _customConds = default;

        [SerializeField]
        SUFastConditionsData _fastConds = default;

#region Success Reactions

        [SerializeField]
        SUFastReactionsData _fastReactions = default;

        [SerializeField]
        SUReactionsData _reactions = default;

        [SerializeField]
        SUAnimationsData _animations = default;

        [SerializeField]
        List<SUActionData> _actions = default;

        public UnityEvent OnSuccess = new UnityEvent();

#endregion

#region Fail Reactions

        [SerializeField]
        SUFastReactionsData _fastFailReactions = default;

        [SerializeField]
        SUReactionsData _failReactions = default;

        [SerializeField]
        SUAnimationsData _failAnimations = default;

        [SerializeField]
        List<SUActionData> _failActions = default;

        public UnityEvent OnFail = new UnityEvent();

#endregion



        public SUEvent.Type_ID EventType
        {
            get
            {
                return _event.Type;
            }
        }


        void PlaySuccess(GameObject go,object evtData = null)
        {
            _fastReactions?.Play(go);
            _reactions?.Play(go,evtData);
            _animations?.Play(go);

            for (int i = 0; i < _actions?.Count; i++)
            {
                _actions[i]?.Play(go);
            }

            OnSuccess?.Invoke();
        }



        void PlayFail(GameObject go,object evtData = null)
        {
            _fastFailReactions?.Play(go);
            _failReactions?.Play(go,evtData);
            _failAnimations?.Play(go);

            for (int i = 0; i < _failActions.Count; i++)
            {
                _failActions[i]?.Play(go);
            }

            OnFail?.Invoke();
        }



        public void Run(GameObject go,object evtData = null)
        {
            if(_event == null)
                return;

            var eventName = _event.Type.ToString();

            int idx = eventName.IndexOf("_");

            if (idx > -1)
            {
                eventName = eventName.Substring(idx);
                eventName = eventName.Replace("_", "");
            }

            Run(go, eventName,evtData);

        }


        public void Run(GameObject go,string eventName,object evtData = null)
        {
            if ( ( _customConds != null && !_customConds.AreAllSatisfied(go,evtData) )
                || ( _fastConds != null && !_fastConds.AreAllSatisfied(go))  )
            {
                PlayFail(go,evtData);

#if SUBolt
                CustomEvent.Trigger(go, eventName,false);
#endif
                return;
            }

            PlaySuccess(go,evtData);

#if SUBolt

            CustomEvent.Trigger(go, eventName,true);

#endif

        }


        public void CacheAnimations(GameObject go)
        {
            _animations?.CacheComponents(go);
            _reactions?.CacheAnimations(go);
            _failReactions?.CacheAnimations(go);
        }

        public void KillAnimations()
        {
            _animations?.KillAll();
            _reactions?.KillAnimations();
            _failReactions?.KillAnimations();
        }


        public void AddCustomReaction(SUReactionData rData)
        {

            if(_reactions == null)
                _reactions = new SUReactionsData();

            _reactions.Reactions.Add(rData);
        }

        public void SetEvent(SUEvent.Type_ID eventID)
        {
            if(_event == null)
                _event = new SUEvent(eventID);
        }

    }


}




