using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Surfer
{

    public partial class SUElement
    {

        void CheckBuildVersion()
        {

            if (ElementData.Type != SUElementData.Type_ID.BuildVersion_Text)
                return;

            _tmp = gameObject.GetComponent<TextMeshProUGUI>();

            if (_tmp == null)
                return;


            _tmp.SetText(SurferHelper.GetVersion());


        }

    }

}
