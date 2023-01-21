using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



namespace Surfer
{
    public partial class SUElement
    {

        TMP_InputField _inputF = default;


        public void CheckInputField()
        {

            if (Events.ContainsKey(SUEvent.Type_ID.InputField_OnValueChanged))
            {

                GetCp();

                if (_inputF == null)
                    return;

                _inputF.onValueChanged.AddListener(OnInputFieldValueChanged);

            }


            if (Events.ContainsKey(SUEvent.Type_ID.InputField_OnEndEdit))
            {
                GetCp();

                if (_inputF == null)
                    return;                

                _inputF.onEndEdit.AddListener(OnInputFieldEndEdit);
            }
        }


        void GetCp()
        {
            if(_inputF != null)
                return;

            _inputF = gameObject.GetComponent<TMP_InputField>();
        }


        void OnInputFieldEndEdit(string value)
        {
            RunEventBehaviour(SUEvent.Type_ID.InputField_OnEndEdit,null);
        }

        void OnInputFieldValueChanged(string value)
        {
            RunEventBehaviour(SUEvent.Type_ID.InputField_OnValueChanged,null);
        }


        public void ResetInputField()
        {
            if (_inputF == null)
                return;

            if (Events.ContainsKey(SUEvent.Type_ID.InputField_OnValueChanged))
            {
                _inputF.onValueChanged.RemoveListener(OnInputFieldValueChanged);
            }
            if (Events.ContainsKey(SUEvent.Type_ID.InputField_OnEndEdit))
            {
                _inputF.onEndEdit.RemoveListener(OnInputFieldEndEdit);
            }


        }
    }

}

