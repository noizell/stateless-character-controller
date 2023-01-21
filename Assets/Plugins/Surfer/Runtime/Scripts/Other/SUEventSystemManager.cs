using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Surfer
{

    public class SUEventSystemManager : MonoBehaviour
    {


        Dictionary<int,GameObject> _selectedObjects = new Dictionary<int, GameObject>();
        Dictionary<int, RectTransform> _selectedObjectsRT = new Dictionary<int, RectTransform>();

        GameObject _objectSelected = default;

        Dictionary<int, string> _currentStatesFocus = new Dictionary<int, string>();

        string _currentFocusState = default;
        string _newFocusState = default;

        Dictionary<string,Dictionary<int,GameObject>> _statesSelections = new Dictionary<string, Dictionary<int, GameObject>>();
        Dictionary<GameObject,SUStateInfo> _historyFocusReceivers = new Dictionary<GameObject, SUStateInfo>();

        //max 8 local players
        List<EventSystem> _allEventSystems = new List<EventSystem>() { null,null,null,null
        ,null,null,null,null
        ,null};

        List<RectTransform> _allSelectionIndicators = new List<RectTransform>(){ null,null,null,null
        ,null,null,null,null
        ,null};

        public static SUEventSystemManager I { get; private set; } = default;

        EventSystem _loopItem = default;


        Vector3 _outPos = new Vector3(0,-10000,0);


        private void Awake()
        {

            if(I==null)
                I = this;
            else
                Destroy(this);

        }



        public void MainLoop()
        {


            for(int i=0;i<_allEventSystems.Count;i++)
            {
                _loopItem = _allEventSystems[i];

                if (_loopItem == null)
                {
                    Follow(i, false);
                    continue;
                }
                if (_loopItem.gameObject == null)
                    continue;


                _objectSelected = null;
                _currentFocusState = string.Empty;

                if (_selectedObjects.TryGetValue(i, out GameObject obj))
                    _objectSelected = obj;

                if (_currentStatesFocus.TryGetValue(i, out string state))
                    _currentFocusState = state;


                if (_loopItem.currentSelectedGameObject != _objectSelected)
                {

                    //checking his state
                    if (_loopItem.currentSelectedGameObject == null)
                        _newFocusState = string.Empty;
                    else
                        _newFocusState = SurferManager.I.GetObjectStateName(_loopItem.currentSelectedGameObject,true);


                    //if (!string.IsNullOrEmpty(_newFocusState) && _newFocusState != _currentFocusState)
                    //{


                    //    if (SurferManager.I.IsOpen(_currentFocusState,i) && _objectSelected != null)
                    //    {

                    //        //call history focus event on last object of previous state
                    //        var interf = _objectSelected.GetComponent<ILastStateSelectionHandler>();
                    //        if (interf != null)
                    //            interf.OnBecomeLastStateSelection(new SULastSelectionEventData(_currentFocusState));

                    //        //save object that has received the history focus event of the previous state
                    //        if (!_historyFocusReceivers.ContainsKey(_currentFocusState))
                    //        {
                    //            _historyFocusReceivers.Add(_currentFocusState, new Dictionary<int, GameObject>() { { i, null } }  );
                    //        }

                    //        _historyFocusReceivers[_currentFocusState][i] = _objectSelected;
                    //    }


                    //    ResetHistoryReceiver(i, _newFocusState);


                    //}


                    //update history focus/selection of the new state
                    if (!string.IsNullOrEmpty(_newFocusState))
                    {
                        if (!_statesSelections.ContainsKey(_newFocusState))
                        {
                            _statesSelections.Add(_newFocusState, new Dictionary<int, GameObject>() { { i, null } } );
                        }

                        _statesSelections[_newFocusState][i] = _objectSelected;
                    }


                    _currentFocusState = _newFocusState;
                    _objectSelected = _loopItem.currentSelectedGameObject;

                    _selectedObjects[i] = _objectSelected;
                    _selectedObjectsRT[i] = _objectSelected?.GetComponent<RectTransform>();
                    _currentStatesFocus[i] = _currentFocusState;


                    Follow(i, true);

                }
                else
                {
                    Follow(i, false);
                }



            }


        }


        void Follow(int playerID, bool isNewTarget)
        {

            var indicator = _allSelectionIndicators[playerID];

            if (indicator == null)
                return;

            RectTransform toFollow = null;

            if (_selectedObjectsRT.TryGetValue(playerID, out RectTransform value))
                toFollow = value;


            if (toFollow == null)
            {
                if (indicator.transform.position.y > _outPos.y)
                    indicator.transform.position = _outPos;
                return;
            }

            if(isNewTarget)
            {

                var canFollow = toFollow.GetComponentInParent<Canvas>();
                var canIndic = indicator.GetComponentInParent<Canvas>();

                if (canFollow == null)
                    return;
                if (canIndic == null)
                    return;

                canIndic.worldCamera = canFollow.worldCamera;
                canIndic.planeDistance = canFollow.planeDistance;
                canIndic.renderMode = canFollow.renderMode;


                if (canIndic.renderMode == RenderMode.WorldSpace)
                    canIndic.transform.localScale = canFollow.rootCanvas.transform.localScale;

                /*Code below can be used for a new ScaleMode
                indicator.transform.localScale = Vector3.one;
                _selectedObjectsLossScale[playerID] = indicator.transform.lossyScale.y;
                */


                CopyCanvasScaler(canFollow.gameObject, canIndic.gameObject);

                return;
            }


            /*Code below can be used for a new ScaleMode

            float diff = (toFollow.rect.size.y * toFollow.transform.lossyScale.y) / (indicator.rect.size.y * _selectedObjectsLossScale[playerID]);
            float diffx = (toFollow.rect.size.x * toFollow.transform.lossyScale.x) / (indicator.rect.size.x * _selectedObjectsLossScale[playerID]);
            indicator.transform.localScale = Vector3.one * diff;

            */

            indicator.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (toFollow.rect.size.y * (toFollow.transform.lossyScale.y / indicator.transform.lossyScale.y)) );
            indicator.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (toFollow.rect.size.x * (toFollow.transform.lossyScale.x / indicator.transform.lossyScale.x)) );

            indicator.transform.position = toFollow.position;


        }




        void CopyCanvasScaler(GameObject toFollow,GameObject indicator)
        {

            var cScaler = toFollow.GetComponentInParent<CanvasScaler>();


            if (cScaler == null)
                return;

            var cScalerInd = indicator.GetComponentInParent<CanvasScaler>();


            if(cScaler.uiScaleMode == CanvasScaler.ScaleMode.ConstantPixelSize)
            {

                cScalerInd.scaleFactor = cScaler.scaleFactor;
                cScalerInd.referencePixelsPerUnit = cScaler.referencePixelsPerUnit;

            }
            else if (cScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {

                cScalerInd.referenceResolution = cScaler.referenceResolution;
                cScalerInd.screenMatchMode = cScaler.screenMatchMode;
                cScalerInd.matchWidthOrHeight = cScaler.matchWidthOrHeight;
                cScalerInd.referencePixelsPerUnit = cScaler.referencePixelsPerUnit;

            }
            else if (cScaler.uiScaleMode == CanvasScaler.ScaleMode.ConstantPhysicalSize)
            {

                cScalerInd.physicalUnit = cScaler.physicalUnit;
                cScalerInd.fallbackScreenDPI = cScaler.fallbackScreenDPI;
                cScalerInd.defaultSpriteDPI = cScaler.defaultSpriteDPI;
                cScalerInd.referencePixelsPerUnit = cScaler.referencePixelsPerUnit;

            }

            cScalerInd.uiScaleMode = cScaler.uiScaleMode;

        }


        /// <summary>
        /// Call OnDeselect on the object that was the last object of its state
        /// </summary>
        /// <param name="stateName"></param>
        public void ResetHistoryReceiver(int playerID,string stateName)
        {

            GameObject objFound = null;

            foreach(KeyValuePair<GameObject,SUStateInfo> pair in _historyFocusReceivers)
            {
                if (pair.Value.Name == stateName && pair.Value.PlayerID == playerID)
                {
                    objFound = pair.Key;
                }
            }


            if (objFound == null)
                return;

            _historyFocusReceivers.Remove(objFound);
            objFound.GetComponent<IResetLastStateSelectionHandler>()?.OnResetLastStateSelection();

        }



        /// <summary>
        /// Reset the history of a state which contains the last object selected
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="stateName"></param>
        public void ResetStateHistoryFocus(int playerID,string stateName)
        {
            if (string.IsNullOrEmpty(stateName))
                return;

            if (_statesSelections.TryGetValue(stateName, out Dictionary<int, GameObject> value))
            {
                value.Remove(playerID);
            }

        }


        /// <summary>
        /// Make the EventSystem focus on a particular gameObject by making it selected
        /// </summary>
        /// <param name="obj">object to select/focus on</param>
        public void FocusOnObject(GameObject obj)
        {
            if (obj == null)
                return;

            SUElement parentState = default;

            SurferManager.I.RecursiveGetParentStateUI(obj.transform, ref parentState);

            if (parentState == null)
                return;

            //send log!
            if (_allEventSystems[parentState.ElementData.PlayerID] == null)
                return;
            if (_allEventSystems[parentState.ElementData.PlayerID].gameObject == null)
                return;

            _allEventSystems[parentState.ElementData.PlayerID].SetSelectedGameObject(null);
            _allEventSystems[parentState.ElementData.PlayerID].SetSelectedGameObject(obj);

        }



        /// <summary>
        /// Make the EventSystem focus on a particular gameObject by making it selected
        /// </summary>
        /// <param name="obj">object to select/focus on</param>
        public void FocusOnObject(GameObject obj,int playerID)
        {

            if (playerID < 0)
                return;
            if (playerID >= _allEventSystems.Count)
                return;
            if (obj == null)
                return;

            //send log!
            if (_allEventSystems[playerID] == null)
                return;
            if (_allEventSystems[playerID].gameObject == null)
                return;

            _allEventSystems[playerID].SetSelectedGameObject(null);
            _allEventSystems[playerID].SetSelectedGameObject(obj);

        }



        /// <summary>
        /// If the state of the object has a "last object selection" the EventSystem will focus on that object,
        /// otherwise it will focus on the object chosen
        /// </summary>
        /// <param name="obj">object to select/focus on</param>
        public void FocusOnObjectOrLast(GameObject obj)
        {

            if (obj == null)
                return;

            SUElement state = SurferManager.I.GetObjectStateElement(obj);

            if (string.IsNullOrEmpty(state.ElementData.StateData.Name))
            {
                FocusOnObject(obj);
                return;
            }

            if (_statesSelections.TryGetValue(state.ElementData.StateData.Name, out Dictionary<int,GameObject> value)
                && value.TryGetValue(state.ElementData.PlayerID, out GameObject selection))
            {
                FocusOnObject(selection);
            }
            else
            {
                FocusOnObject(obj);
            }


        }




        public EventSystem GetEventSystem(int playerID)
        {
            if (_allEventSystems[playerID] == null
                || _allEventSystems[playerID].gameObject == null)
                return null;

            return _allEventSystems[playerID];
        }


        /// <summary>
        /// Check if the object was the last state object and therefore it received
        /// the history event callback
        /// </summary>
        /// <returns></returns>
        public bool IsHistoryReceiver(GameObject obj)
        {

            if(_historyFocusReceivers.ContainsKey(obj))
            {
                return true;
            }

            return false;

        }



        public void GetAllEventSystems()
        {
            var list = FindObjectsOfType<EventSystem>().ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].name.Contains("1P"))
                    _allEventSystems[1] = list[i];
                else if (list[i].name.Contains("2P"))
                    _allEventSystems[2] = list[i];
                else if (list[i].name.Contains("3P"))
                    _allEventSystems[3] = list[i];
                else if (list[i].name.Contains("4P"))
                    _allEventSystems[4] = list[i];
                else if (list[i].name.Contains("5P"))
                    _allEventSystems[5] = list[i];
                else if (list[i].name.Contains("6P"))
                    _allEventSystems[6] = list[i];
                else if (list[i].name.Contains("7P"))
                    _allEventSystems[7] = list[i];
                else if (list[i].name.Contains("8P"))
                    _allEventSystems[8] = list[i];
                else
                    _allEventSystems[0] = list[i];


            }

        }


        /// <summary>
        /// When a state is open, check and register history focus receiver
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="state"></param>
        public void CheckHistoryFocus(int playerID, string stateOpened)
        {

            if(_selectedObjects.TryGetValue(playerID, out GameObject selected)
                && stateOpened != SurferManager.I.GetObjectStateName(selected) )
            {

                if (selected == null)
                    return;

                //call history focus event on last object of previous state
                //var interf = selected.GetComponent<ILastStateSelectionHandler>();
                //if (interf != null)
                //    interf.OnBecomeLastStateSelection(new SULastSelectionEventData(currentState));

                var item = new SUStateInfo(stateOpened, playerID);

                //save object that has received the history focus event of the previous state
                if (!_historyFocusReceivers.ContainsKey(selected))
                {
                    _historyFocusReceivers.Add(selected, item);
                }
                else
                {
                    _historyFocusReceivers[selected] = item;
                }



            }


        }



        /// <summary>
        /// Register a SUElement as selection indicator for a Player's EventSystem
        /// </summary>
        /// <param name="item"></param>
        public void RegisterSelectionIndicator(SUElement item)
        {
            var rt = item.GetComponent<RectTransform>();

            if (rt == null)
                return;

            _allSelectionIndicators[item.ElementData.PlayerID] = rt;

        }


    }



}



