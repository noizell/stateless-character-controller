using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer
{


    public partial class SUElement 
    {

        Slider _slider = default;

        List<bool> _greatersDone = new List<bool>();
        List<SUBehaviourData> _greatersBehavs = new List<SUBehaviourData>();
        List<bool> _lowersDone = new List<bool>();
        List<SUBehaviourData> _lowersBehavs = new List<SUBehaviourData>();

        bool _isVolume = default;

        SUBehaviourData _item = default;
        float _valueToCheck = default;
        float _offset = 0.01f;

        void CheckSlider()
        {

            _slider = gameObject.GetComponent<Slider>();

            if (_slider == null)
                return;


            _isVolume = ElementData.Type == SUElementData.Type_ID.Slider_OverallVolume;


            bool hasEvent = false;

            if (Events.TryGetValue(SUEvent.Type_ID.Slider_OnGreaterThan, out var valueActG))
            {

                hasEvent = true;

                for (int i = 0; i < valueActG.Behaviours.Count; i++)
                {
                    _greatersBehavs.Add(valueActG.Behaviours[i]);
                    _greatersDone.Add(false);
                }
            }


            if (Events.TryGetValue(SUEvent.Type_ID.Slider_OnLowerThan, out var valueActL))
            {
                hasEvent = true;

                for (int i = 0; i < valueActL.Behaviours.Count; i++)
                {
                    _lowersBehavs.Add(valueActL.Behaviours[i]);
                    _lowersDone.Add(false);
                }
            }
            

            if (Events.TryGetValue(SUEvent.Type_ID.Slider_OnMax, out var valueMax))
            {
                hasEvent = true;


                for (int i = 0; i < valueMax.Behaviours.Count; i++)
                {
                    _greatersBehavs.Add(valueMax.Behaviours[i]);
                    _greatersDone.Add(false);

                }
            }

            if (Events.TryGetValue(SUEvent.Type_ID.Slider_OnMin, out var valueMin))
            {
                hasEvent = true;


                for (int i = 0; i < valueMin.Behaviours.Count; i++)
                {
                    _lowersBehavs.Add(valueMin.Behaviours[i]);
                    _lowersDone.Add(false);
                }
            }



            if (hasEvent || _isVolume)
            {

                if(_isVolume)
                {

                    _slider.minValue = 0;
                    _slider.maxValue = 1;


                    if (PlayerPrefs.HasKey(SurferHelper.kOverallVolume))
                        _slider.value = PlayerPrefs.GetFloat(SurferHelper.kOverallVolume);
                    else
                        _slider.value = _slider.maxValue;

                }

                _slider.onValueChanged.AddListener(OnValueChanged);
                OnValueChanged(_slider.value);
            }

        }

        void OnValueChanged(float value)
        {

            for(int i=0;i<_greatersBehavs.Count;i++)
            {

                _item = _greatersBehavs[i];

                if (_item.Event.Type == SUEvent.Type_ID.Slider_OnMax)
                    _valueToCheck = _slider.maxValue - _offset;
                else
                    _valueToCheck = _item.Event.FloatVal;


                if(value > _valueToCheck && !_greatersDone[i])
                {
                    _greatersBehavs[i].Run(gameObject,null);
                    _greatersDone[i] = true;
                }
                if (value < _valueToCheck && _greatersDone[i])
                {
                    _greatersDone[i] = false;
                }
            }


            for (int i = 0; i < _lowersBehavs.Count; i++)
            {

                _item = _lowersBehavs[i];

                if (_item.Event.Type == SUEvent.Type_ID.Slider_OnMin)
                    _valueToCheck = _slider.minValue + _offset;
                else
                    _valueToCheck = _item.Event.FloatVal;

                if (value < _valueToCheck && !_lowersDone[i])
                {
                    _lowersBehavs[i].Run(gameObject,null);
                    _lowersDone[i] = true;
                }
                if (value > _valueToCheck && _lowersDone[i])
                {
                    _lowersDone[i] = false;
                }
            }

            if(_isVolume)
            {
                AudioListener.volume = value;
                PlayerPrefs.SetFloat(SurferHelper.kOverallVolume,value);
            }

        }



        public void ResetSlider()
        {
            if (_slider == null)
                return;

            _slider.onValueChanged.RemoveListener(OnValueChanged);

        }

    }


}

