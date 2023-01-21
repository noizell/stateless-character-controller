using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer
{


    public partial class SUElement
    {
        Toggle _toggle = default;

        void CheckToggle()
        {

            if (Events.ContainsKey(SUEvent.Type_ID.Toggle_OnFalse)
                || Events.ContainsKey(SUEvent.Type_ID.Toggle_OnTrue))
            {

                _toggle = gameObject.GetComponent<Toggle>();

                if (_toggle == null)
                    return;

                _toggle.onValueChanged.AddListener(OnValueChanged);
            }


        }

        void OnValueChanged(bool value)
        {

            if (value)
            {
                RunEventBehaviour(SUEvent.Type_ID.Toggle_OnTrue,null);
            }
            else
            {
                RunEventBehaviour(SUEvent.Type_ID.Toggle_OnFalse,null);
            }

        }



        public void ResetToggle()
        {
            if (_toggle == null)
                return;

            _toggle.onValueChanged.RemoveListener(OnValueChanged);

        }

    }


}
