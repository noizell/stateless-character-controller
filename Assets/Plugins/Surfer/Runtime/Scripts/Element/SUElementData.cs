using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer
{


    [System.Serializable]
    public class SUElementData
    {

        public enum Type_ID
        {
            /// <summary>
            /// Generic type of element
            /// </summary>
            Normal,
            /// <summary>
            /// State element type, like a menu, popup, notification or whatever kind of generic panel
            /// </summary>
            State,
            /// <summary>
            /// State element that behaves like a tooltip shown next to the mouse cursor
            /// </summary>
            Tooltip_State,
            /// <summary>
            /// State element that behaves like a draggable Menu can be dragged to the right
            /// </summary>
            DragRight_State,
            /// <summary>
            /// State element that behaves like a draggable Menu can be dragged to the left
            /// </summary>
            DragLeft_State,
            /// <summary>
            /// State element that behaves like a draggable Menu can be dragged to the top
            /// </summary>
            DragUp_State,
            /// <summary>
            /// State element that behaves like a draggable Menu can be dragged to the bottom 
            /// </summary>
            DragDown_State,
            /// <summary>
            /// If the object is a TextMeshPro element, it will show the progress of whatever scene loading
            /// </summary>
            Loading_Text,
            /// <summary>
            /// If the object is an Image element, it will "fill" itself with the progress of whatever scene loading
            /// </summary>
            Loading_Image,
            /// <summary>
            /// If the object is a Slider element, it will control the overall game audio volume
            /// </summary>
            Slider_OverallVolume,
            /// <summary>
            /// If the object is a TextMeshPro element, it will show the app/game version and build number
            /// </summary>
            BuildVersion_Text,
            /// <summary>
            /// The element will positionate and scale itself to match the current selected UI object position and scale, in order to "indicate" it
            /// </summary>
            Selection_Indicator,
            /// <summary>
            /// On/Off screen indicator-like element
            /// </summary>
            Indicator,
            /// <summary>
            /// HealthBar-like element
            /// </summary>
            HealthBar,
            /// <summary>
            /// Some children will become States and will activate on enter and deactivate on exit
            /// </summary>
            GroupStates_OnOff,
            /// <summary>
            /// Some children will become States and will move in on enter and move out on exit
            /// </summary>
            GroupStates_InOut,
            /// <summary>
            /// Some children will become States and will move in and activate on enter and move out and deactivate on exit
            /// </summary>
            GroupStates_OnOffAndInOut,
            /// <summary>
            /// Some children will become buttons that will open specific states
            /// </summary>
            GroupButtons,
        }

        /// <summary>
        /// When a state has just been opened, should automatically close other states?
        /// Example : a TabMenu panel should automatically close sibling states with the mode set to Siblings
        /// </summary>
        public enum StateCloseMode_ID
        {
            None,
            /// <summary>
            /// It will close all the sibling states
            /// </summary>
            Siblings,
            CustomList,
            SiblingsAndCustomList,
            Myself

        }

        [SerializeField]
        Type_ID _type = default;
        public Type_ID Type { get => _type; }

        public bool IsState
        {
            get
            {
                return _type == Type_ID.State
                    || IsTooltip
                    || IsDrag;
            }
        }

        public bool IsTooltip
        {
            get
            {
                return _type == Type_ID.Tooltip_State;
            }
        }

        public bool IsDrag
        {
            get
            {
                return _type == Type_ID.DragRight_State
                    || _type == Type_ID.DragLeft_State
                    || _type == Type_ID.DragUp_State
                    || _type == Type_ID.DragDown_State;
            }
        }

        public bool IsLoading
        {
            get
            {
                return _type == Type_ID.Loading_Image
                    || _type == Type_ID.Loading_Text;
            }
        }


        public bool IsGroupStates
        {
            get
            {
                return _type == Type_ID.GroupStates_OnOff
                    || _type == Type_ID.GroupStates_InOut
                    || _type == Type_ID.GroupStates_OnOffAndInOut;
            }
        }


        public bool IsGroup
        {
            get
            {
                return IsGroupButtons || IsGroupStates;
            }
        }

        public bool IsGroupButtons
        {
            get
            {
                return _type == Type_ID.GroupButtons;
            }
        }



        [SerializeField]
        bool _persistent = false;

        [SerializeField]
        SUStateData _stateData = default;
        public SUStateData StateData { get => _stateData; }

        //stack
        [SerializeField]
        bool _isStackable = default;
        public bool IsStackable { get => _isStackable; }
        
        [SerializeField]
        float _stackDelay = default;
        public float StackDelay { get => _stackDelay; }

        public bool IsStackDelayRunning;

        //close mode

        [SerializeField]
        StateCloseMode_ID _closeMode = default;

        [SerializeField]
        SUStatesData _statesData = default;
        [SerializeField]
        SUStatesData _groupStates = default;
        public SUStatesData GroupStates { get => _groupStates; }
        [SerializeField]
        [Min(0)]
        float _closeDelay = default;
        public float CloseDelay { get => _closeDelay; }

        //tooltip
        [SerializeField]
        Vector2 _vector = new Vector2(0.65f,0.65f);
        public Vector2 Vector { get => _vector; }

        //players
        [SerializeField]
        int _playerID = SurferHelper.kDefaultPlayerID;

         /// <summary>
        /// Get the PlayerID of the object linked to this ElementData.
        /// </summary>
        public int PlayerID
        {

            get
            {
                if (_playerID == SurferHelper.kNestedPlayerID)
                {
                    _playerID = SurferManager.I.GetObjectStatePlayerID(_obj);
                }

                return _playerID;
            }

        }

        
        string _stateName = null;
        /// <summary>
        /// Get the StateName of the object linked to this ElementData.
        /// </summary>
        public string StateName
        {

            get
            {

                if(_stateName != null)
                    return _stateName;

                if(StateData != null && IsState && !string.IsNullOrEmpty(StateData.Name))
                {
                    _stateName = StateData.Name;
                    return _stateName;
                }

                _stateName = SurferManager.I.GetObjectStateName(_obj);
                return _stateName;
            }

        }

        //generic string used for element data types; now for loading text prefix and suffix
        [SerializeField]
        string _stringVal, _stringVal2 = default;
        public string StringVal { get => _stringVal; }
        public string StringVal2 { get => _stringVal2; }

        [SerializeField]
        SUIndicatorVisualData _indVisualData = default;
        public SUIndicatorVisualData IndVisualData { get => _indVisualData; }

        [SerializeField]
        SUHealthBarVisualData _hbVisualData = default;
        public SUHealthBarVisualData HbVisualData { get => _hbVisualData; }


        [SerializeField]
        int _intVal = default;
        public int IntVal { get => _intVal; }

        GameObject _obj = default;

        #region Collections





        public List<SUStateInfo> StatesToClose
        {

            get
            {

                if (_closeMode == StateCloseMode_ID.Siblings)
                {
                    return SiblingStates;
                }
                else if (_closeMode == StateCloseMode_ID.CustomList)
                {

                    return _statesData.GetStateInfos(this);

                }
                else if (_closeMode == StateCloseMode_ID.SiblingsAndCustomList)
                {

                    List<SUStateInfo> _states = _statesData.GetStateInfos(this);
                    _states.AddRange(SiblingStates);

                    return _states;

                }
                else if(_closeMode == StateCloseMode_ID.Myself)
                {
                    return new List<SUStateInfo> { new SUStateInfo(this) };
                }

                return default;

            }

        }

        List<SUStateInfo> SiblingStates
        {

            get
            {
                List<SUStateInfo> _statesToClose = new List<SUStateInfo>();

                if (_obj.transform.parent == null)
                    return _statesToClose;

                SUElement cp = null;
                foreach (Transform child in _obj.transform.parent)
                {
                    cp = child.GetComponent<SUElement>();
                    if (cp == null)
                        continue;
                    if (cp.ElementData.StateData.Name.Equals(StateData.Name))
                        continue;

                    _statesToClose.Add(new SUStateInfo(cp.ElementData));
                }

                return _statesToClose;
            }

        }


        List<SUStateInfo> _parentUIStates = null;
        public List<SUStateInfo> ParentUIStates
        {

            get
            {

                if (_parentUIStates != null)
                    return _parentUIStates;

                _parentUIStates = new List<SUStateInfo>();
                SurferManager.I.RecursiveGetParentStatesUI(_obj.transform, ref _parentUIStates);

                return _parentUIStates;
            }

        }

        List<SUStateInfo> _childUIStates = null;
        public List<SUStateInfo> ChildUIStates
        {

            get
            {

                if (_childUIStates != null)
                    return _childUIStates;

                _childUIStates = new List<SUStateInfo>();
                SurferManager.I.RecursiveGetChildStatesUI(_obj.transform, ref _childUIStates);

                return _childUIStates;
            }

        }

        /// <summary>
        /// List that contains all StatesUI loaded in the scene/game
        /// </summary>
        public static HashSet<SUElementData> AllUIStates { get; private set; } = new HashSet<SUElementData>();


        #endregion


#region GroupStates Logic

        /// <summary>
        /// Helper for groupStates element types
        /// </summary>
        /// <param name="OnChild"></param>
        public void ForEachChild(System.Action<Transform,int> OnChild)
        {

            int total = GroupStates.List.Count - 1;

            if (total <= 0)
                return;

            int totToSkip = IntVal;

            for (int i = totToSkip; i < _obj.transform.childCount; i++)
            {
                if ((i + 1 - totToSkip) > total)
                    continue;

                OnChild?.Invoke(_obj.transform.GetChild(i),i-totToSkip);

            }


        }


        public Transform GetGroupStateOfObject(GameObject go,GameObject groupStatesOwner)
        {


            if(_obj == null)
                _obj = groupStatesOwner;

            Transform found = null;

            ForEachChild((tr, i) =>
            {

                Transform parent = go.transform;

                while(parent != null && !found)
                {
                    if(parent == tr)
                    {
                        found = tr;
                        break;
                    }

                    parent = parent.parent;
                }

            });

            if(found == null)
            {
                //object is not inside a groupState so , ignore main groupState parent element and re-get state 
                found = SurferManager.I.GetObjectStateTransfom(_obj,false);
            }

            return found;

        }


        public string GetGroupStateNameOfObject(GameObject go,GameObject groupStatesOwner)
        {
            if(groupStatesOwner == null)
                return string.Empty;

            if(_obj == null)
                _obj = groupStatesOwner;

            string found = string.Empty;

            ForEachChild((tr, i) =>
            {

                Transform parent = go.transform;

                while(parent != null && string.IsNullOrEmpty(found))
                {
                    if(parent == tr)
                    {
                        found = GroupStates.AllNames[i];
                        break;
                    }

                    parent = parent.parent;
                }

            });


            if(string.IsNullOrEmpty(found))
            {
                //object is not inside a groupState so , ignore main groupState parent element and re-get state name
                found = SurferManager.I.GetObjectStateName(_obj,false);
            }

            return found;

        }

        #endregion


        public void SetUp(GameObject go)
        {

            if (go == null)
                return;

            _obj = go;


            if (IsState)
                AllUIStates.Add(this);


            if (_persistent && IsState)
                Object.DontDestroyOnLoad(_obj.transform.root);

        }


        public void HandleOnDestroy()
        {
            if (!IsState)
                return;

            AllUIStates.Remove(this);

            SUEventSystemManager.I.ResetStateHistoryFocus(PlayerID,_stateData.Name);
            SurferManager.I.ClosePlayerState(PlayerID,_stateData.Name);

            
        }

        

        #region Runtime Injection

        public void InjectStateData(string stateName,int playerID,StateCloseMode_ID closeModeID)
        {
            _type = Type_ID.State;
            _playerID = playerID;
            _stateData = new SUStateData(stateName,SurferManager.I.SO.GetStateKey(stateName));
            _closeMode = closeModeID;
        }


        #endregion

    }




}




