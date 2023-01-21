using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Surfer
{

    public partial class SUElement
    {

        TMP_Dropdown _dropD = default;


        void CheckDropdown()
        {

            if (Events.ContainsKey(SUEvent.Type_ID.Dropdown_OnFirstOptionSelected)
                || Events.ContainsKey(SUEvent.Type_ID.Dropdown_OnOptionSelected))
            {

                _dropD = gameObject.GetComponent<TMP_Dropdown>();

                if (_dropD == null)
                    return;

                _dropD.onValueChanged.AddListener(OnValueChanged);
                OnValueChanged(_dropD.value);
            }

        }




        void OnValueChanged(int value)
        {

            if (value == 0)
                RunEventBehaviour(SUEvent.Type_ID.Dropdown_OnFirstOptionSelected,null);
            
            RunEventBehaviour(SUEvent.Type_ID.Dropdown_OnOptionSelected,null);

        }



        public void ResetDropdown()
        {
            if (_dropD == null)
                return;

            if (Events.ContainsKey(SUEvent.Type_ID.Dropdown_OnFirstOptionSelected)
                || Events.ContainsKey(SUEvent.Type_ID.Dropdown_OnOptionSelected))
            {
                _dropD.onValueChanged.RemoveListener(OnValueChanged);
            }

        }


    }

}

