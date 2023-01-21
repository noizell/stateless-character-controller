using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Surfer
{

    public partial class SUElement
    {

        void CheckLoadingElements()
        {
            if (!_elementData.IsLoading)
                return;

            SurferManager.I.RegisterSceneLoading(this,SurferHelper.SO.GetScenesNameStrings());

        }


        void UpdateLoadingType(SUSceneLoadingEventData eventInfo)
        {
            
            if(_elementData.IsLoading)
            {
                if (_elementData.Type == SUElementData.Type_ID.Loading_Text && GetTmp()!=null)
                    GetTmp().SetText(_elementData.StringVal + eventInfo.Progress + _elementData.StringVal2);
                else if (_elementData.Type == SUElementData.Type_ID.Loading_Image && GetImg()!=null)
                    GetImg().fillAmount = eventInfo.Progress / 100f;

            }

        }

        
        void ResetLoadingElements()
        {

            if (!_elementData.IsLoading)
                return;

            SurferManager.I.UnregisterSceneLoading(this, SurferHelper.SO.GetScenesNameStrings());

        }
    }

}

