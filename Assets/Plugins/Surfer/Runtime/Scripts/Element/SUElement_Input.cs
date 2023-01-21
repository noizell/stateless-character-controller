using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public partial class SUElement
    {
        

        #if SUOld

        List<KeyCode> _oldKeyCodesDown = new List<KeyCode>();
        List<string> _oldBtnNamesDown = new List<string>();

#endif

#if SUNew

        List<UnityEngine.InputSystem.InputAction> _inputActions = new List<InputAction>();

#endif


        #region RunEvents


#if SUOld
        void RunEventBehaviourParams(SUEvent.Type_ID eventID, KeyCode keyCode)
        {

            if (Events.TryGetValue(eventID, out SUBehavioursData value))
            {
                value.RunWithParams(gameObject, keyCode);
            }

        }
#endif

        

#if SURew

        void RunEventBehaviourParams(SUEvent.Type_ID eventID, InputActionEventData eventData)
        {

            if (Events.TryGetValue(eventID, out SUBehavioursData value))
            {
                value.RunWithParams(gameObject, eventData);
            }
        }

#endif

        #endregion


#region Register

        public void CheckInput()
        {

#if SUOld

            if (Events.TryGetValue(SUEvent.Type_ID.Input_OldInput_OnKeyDown, out var valueDown))
            {
                for (int i = 0; i < valueDown.Behaviours.Count; i++)
                    _oldKeyCodesDown.Add(valueDown.Behaviours[i].Event.KeyCodeVal);

            }
            if (Events.TryGetValue(SUEvent.Type_ID.Input_OldInput_OnButtonDown, out var valueName))
            {
                for (int i = 0; i < valueName.Behaviours.Count; i++)
                    _oldBtnNamesDown.Add(valueName.Behaviours[i].Event.StringVal);

            }

#endif

#if SURew


            if (Events.TryGetValue(SUEvent.Type_ID.Input_Rewired_OnAction, out var valueRew))
            {
                if (!ReInput.isReady)
                    return;

                for (int i = 0; i < valueRew.Behaviours.Count; i++)
                {
                    int playerID = valueRew.Behaviours[i].Event.IntVal;

                    if (playerID == 0)
                    {

                        foreach (Player playerItem in ReInput.players.Players)
                        {
                            ReInput.players.GetPlayer(playerItem.id).AddInputEventDelegate(OnActionPerformed,
                                valueRew.Behaviours[i].Event.UpdateLoop,
                                valueRew.Behaviours[i].Event.EventType,
                                valueRew.Behaviours[i].Event.StringVal);
                        }

                    }
                    else
                    {

                        ReInput.players.GetPlayer(playerID).AddInputEventDelegate(OnActionPerformed,
                                valueRew.Behaviours[i].Event.UpdateLoop,
                                valueRew.Behaviours[i].Event.EventType,
                                valueRew.Behaviours[i].Event.StringVal);

                    }

                }

            }



#endif


#if SUNew
            if (Events.TryGetValue(SUEvent.Type_ID.Input_NewInput_OnAction, out var valueNew))
            {


                for(int i=0;i<valueNew.Behaviours.Count;i++)
                {

                    foreach (InputActionMap actionMap in valueNew.Behaviours[i].Event.PInput.actionMaps)
                    {

                        foreach (UnityEngine.InputSystem.InputAction action in actionMap.actions)
                        {
                            if (action.name.Equals(valueNew.Behaviours[i].Event.StringVal))
                            {
                                _inputActions.Add(action);

                                _inputActions[_inputActions.Count-1].performed += Performed;
                                _inputActions[_inputActions.Count - 1].Enable();
                            }

                        }
                    }

                }

                

            }


            

#endif


        }

#if SUNew

        void Performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            RunEventBehaviourParams(SUEvent.Type_ID.Input_NewInput_OnAction, ctx.action.name);
        }

#endif

        void RunEventBehaviourParams(SUEvent.Type_ID eventID,string stringVal)
        {

            if (_events.TryGetValue(eventID, out SUBehavioursData value))
            {
                value.RunWithParams(gameObject,stringVal);
            }
        }

#endregion



#if SURew

        void OnActionPerformed(InputActionEventData args)
        {
            RunEventBehaviourParams(SUEvent.Type_ID.Input_Rewired_OnAction, args);
        }

#endif



        public void CheckOldInput()
        {
#if SUOld

            if (_oldKeyCodesDown.Count > 0)
            {
                for (int i = 0; i < _oldKeyCodesDown.Count; i++)
                    if (Input.GetKeyDown(_oldKeyCodesDown[i]))
                        RunEventBehaviourParams(SUEvent.Type_ID.Input_OldInput_OnKeyDown, _oldKeyCodesDown[i]);
            }
            if (_oldBtnNamesDown.Count > 0)
            {
                for (int i = 0; i < _oldBtnNamesDown.Count; i++)
                    if (Input.GetButtonDown(_oldBtnNamesDown[i]))
                        RunEventBehaviourParams(SUEvent.Type_ID.Input_OldInput_OnButtonDown, _oldBtnNamesDown[i]);
            }

#endif

        }


        #region Unregister

        public void ResetInput()
        {

#if SUOld

            _oldKeyCodesDown.Clear();
            _oldBtnNamesDown.Clear();

#endif

#if SURew


            if (Events.TryGetValue(SUEvent.Type_ID.Input_Rewired_OnAction, out var valueRew))
            {
                if (!ReInput.isReady)
                    return;

                for (int i = 0; i < valueRew.Behaviours.Count; i++)
                {
                    int playerID = valueRew.Behaviours[i].Event.IntVal;

                    if (playerID == 0)
                    {

                        foreach (Player playerItem in ReInput.players.Players)
                        {
                            ReInput.players.GetPlayer(playerItem.id).RemoveInputEventDelegate(OnActionPerformed,
                                valueRew.Behaviours[i].Event.UpdateLoop,
                                valueRew.Behaviours[i].Event.EventType,
                                valueRew.Behaviours[i].Event.StringVal);
                        }

                    }
                    else
                    {
                        ReInput.players.GetPlayer(playerID).RemoveInputEventDelegate(OnActionPerformed,
                                valueRew.Behaviours[i].Event.UpdateLoop,
                                valueRew.Behaviours[i].Event.EventType,
                                valueRew.Behaviours[i].Event.StringVal);

                    }

                }

            }



#endif


#if SUNew

            for (int i = 0; i < _inputActions.Count; i++)
            {

                _inputActions[i].performed -= Performed;
                _inputActions[i].Disable();

            }

            _inputActions.Clear();


#endif


        }

#endregion



    }


}


