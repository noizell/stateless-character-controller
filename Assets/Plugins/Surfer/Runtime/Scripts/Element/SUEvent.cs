using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if SURew
using Rewired;
#endif

#if SUNew
using UnityEngine.InputSystem;
#endif

namespace Surfer
{

    [System.Serializable]
    public class SUEvent
    {
        public enum Type_ID
        {
            /// <summary>
            /// This event has SUStateEventData as eventData
            /// </summary>
            State_MyStateEnter,
            /// <summary>
            /// This event has SUStateEventData as eventData
            /// </summary>
            State_MyStateExit,
            /// <summary>
            /// This event has SUStateEventData as eventData
            /// </summary>
            State_Enter,
            /// <summary>
            /// This event has SUStateEventData as eventData
            /// </summary>
            State_Exit,
            /// <summary>
            /// This event has SUSceneLoadedEventData as eventData
            /// </summary>
            Scene_MySceneLoaded,
            /// <summary>
            /// This event has SUSceneLoadingEventData as eventData
            /// </summary>
            Scene_MySceneLoading,
            /// <summary>
            /// This event has SUSceneUnloadedEventData as eventData
            /// </summary>
            Scene_MySceneUnloaded,
            /// <summary>
            /// This event has SUSceneUnloadingEventData as eventData
            /// </summary>
            Scene_MySceneUnloading,
            /// <summary>
            /// This event has SUSceneActivatedEventData as eventData
            /// </summary>
            Scene_MySceneActivated,
            /// <summary>
            /// This event has SUSceneDeactivatedEventData as eventData
            /// </summary>
            Scene_MySceneDeactivated,
            /// <summary>
            /// This event has SUSceneLoadedEventData as eventData
            /// </summary>
            Scene_Loaded,
            /// <summary>
            /// This event has SUSceneLoadingEventData as eventData
            /// </summary>
            Scene_Loading,
            /// <summary>
            /// This event has SUSceneUnloadedEventData as eventData
            /// </summary>
            Scene_Unloaded,
            /// <summary>
            /// This event has SUSceneUnloadingEventData as eventData
            /// </summary>
            Scene_Unloading,
            /// <summary>
            /// This event has SUSceneActivatedEventData as eventData
            /// </summary>
            Scene_Activated,
            /// <summary>
            /// This event has SUSceneDeactivatedEventData as eventData
            /// </summary>
            Scene_Deactivated,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            MonoBehaviour_OnAwake,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            MonoBehaviour_OnStart,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            MonoBehaviour_OnEnable,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            MonoBehaviour_OnDisable,
            /// <summary>
            /// This event has PointerEventData as eventData
            /// </summary>
            UIGeneric_OnClick,
            /// <summary>
            /// This event has PointerEventData as eventData
            /// </summary>
            UIGeneric_OnPointerDown,
            /// <summary>
            /// This event has PointerEventData as eventData
            /// </summary>
            UIGeneric_OnPointerUp,
            /// <summary>
            /// This event has PointerEventData as eventData
            /// </summary>
            UIGeneric_OnDoubleClick,
            /// <summary>
            /// This event has PointerEventData as eventData
            /// </summary>
            UIGeneric_OnMouseRightClick,
            /// <summary>
            /// This event has BaseEventData as eventData
            /// </summary>
            UIGeneric_OnSelect,
            /// <summary>
            /// This event has BaseEventData as eventData
            /// </summary>
            UIGeneric_OnDeselect,
            /// <summary>
            /// When a new state is opened, this event will be fired on
            /// the last selected object of the previous state.
            /// Useful to show graphically the ui history selection.
            /// This event has BaseEventData as eventData
            /// </summary>
            UIGeneric_OnBecomeLastStateSelection,
            /// <summary>
            /// This event has PointerEventData as eventData
            /// </summary>
            UIGeneric_OnEnter,
            /// <summary>
            /// This event has PointerEventData as eventData
            /// </summary>
            UIGeneric_OnExit,
            /// <summary>
            /// This event has BaseEventData as eventData
            /// </summary>
            UIGeneric_OnSubmit,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Toggle_OnTrue,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Toggle_OnFalse,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Input_OldInput_OnKeyDown,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Input_OldInput_OnButtonDown,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Input_NewInput_OnAction,
            /// <summary>
            /// This event has InputActionEventData as eventData
            /// </summary>
            Input_Rewired_OnAction,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            InputField_OnValueChanged,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            InputField_OnEndEdit,
            //2.1
            /// <summary>
            /// This event has no eventData
            /// </summary>
            ScrollRect_OnReachedTop,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            ScrollRect_OnReachedBottom,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            ScrollRect_OnReachedLeft,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            ScrollRect_OnReachedRight,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            ScrollRect_OnNotReachedAnySide,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Dropdown_OnOptionSelected,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Dropdown_OnFirstOptionSelected,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Slider_OnGreaterThan,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Slider_OnLowerThan,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Slider_OnMax,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Slider_OnMin,
            /// <summary>
            /// This event has SUCustomEventEventData as eventData
            /// </summary>
            MyCustomEvent,
            //2.5
            /// <summary>
            /// This event has SUIndicatorEventData as eventData
            /// </summary>
            Indicator_OnStartOnScreenMode,
            /// <summary>
            /// This event has SUIndicatorEventData as eventData
            /// </summary>
            Indicator_OnStartOffScreenMode,
            /// <summary>
            /// This event has SUIndicatorEventData as eventData
            /// </summary>
            Indicator_OnStandby,
            /// <summary>
            /// This event has SUIndicatorEventData as eventData
            /// </summary>
            Indicator_OnClose,
            /// <summary>
            /// This event has SUIndicatorEventData as eventData
            /// </summary>
            Indicator_OnFar,
            /// <summary>
            /// This event has SUIndicatorEventData as eventData
            /// </summary>
            Indicator_OnCloseAndCentered,
            /// <summary>
            /// This event has SUHealthBarEventData as eventData
            /// </summary>
            HealthBar_OnFullHp,
            /// <summary>
            /// This event has SUHealthBarEventData as eventData
            /// </summary>
            HealthBar_OnEmptyHp,
            /// <summary>
            /// This event has SUHealthBarEventData as eventData
            /// </summary>
            HealthBar_OnLessThanHalfHp,
            /// <summary>
            /// This event has SUHealthBarEventData as eventData
            /// </summary>
            HealthBar_OnMoreThanHalfHp,
            //2.7
            /// <summary>
            /// This event has SUChildrenMonitorData as eventData
            /// </summary>
            Transform_OnChildAdded,
            /// <summary>
            /// This event has SUChildrenMonitorData as eventData
            /// </summary>
            Transform_OnChildRemoved,
            /// <summary>
            /// This event has SUChildrenMonitorData as eventData
            /// </summary>
            Transform_OnChildrenCountChanged,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Transform_OnParentChanged,
            /// <summary>
            /// This event has no eventData
            /// </summary>
            Transform_OnParentLost,
            /// <summary>
            /// This event has SUOrientationInfo as eventData
            /// </summary>
            Orientation_ToLandscapeLeft,
            /// <summary>
            /// This event has SUOrientationInfo as eventData
            /// </summary>
            Orientation_ToLandscapeRight,
            /// <summary>
            /// This event has SUOrientationInfo as eventData
            /// </summary>
            Orientation_ToPortrait,
            /// <summary>
            /// This event has SUOrientationInfo as eventData
            /// </summary>
            Orientation_ToPortraitUpsideDown,

        }


        [SerializeField]
        Type_ID _type = default;
        public Type_ID Type { get => _type; }


        [SerializeField]
        SUStatesData _statesData = default;
        public SUStatesData StatesData { get => _statesData; }


        [SerializeField]
        SUScenesData _scenesData = default;
        public SUScenesData ScenesData { get => _scenesData; }

        [SerializeField]
        SUCustomEventsData _cEventsData = default;
        public SUCustomEventsData CEventsData { get => _cEventsData; }


        [SerializeField]
        KeyCode _keyCodeVal = default;
        public KeyCode KeyCodeVal { get => _keyCodeVal; }


        [SerializeField]
        int _intVal = default;
        public int IntVal { get => _intVal; }

        [SerializeField]
        int _intVal2 = default;
        public int IntVal2 { get => _intVal2; }


        [SerializeField]
        float _floatVal = default;
        public float FloatVal { get => _floatVal; }

        [SerializeField]
        string _stringVal = default;
        public string StringVal { get => _stringVal; }

#if SURew

        [SerializeField]
        UpdateLoopType _updateLoop = default;
        public UpdateLoopType UpdateLoop { get => _updateLoop; }

        [SerializeField]
        InputActionEventType _eventType = InputActionEventType.ButtonJustPressed;
        public InputActionEventType EventType { get => _eventType; }

#endif


#if SUNew

        [SerializeField]
        InputActionAsset _pInput = default;
        public InputActionAsset PInput { get => _pInput; }

#endif

        public SUEvent(SUEvent.Type_ID type)
        {
            _type = type;
        }

    }

}

