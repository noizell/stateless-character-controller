using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Surfer
{

    [System.Serializable]
    public class SUFastReactionsData
    {

        [SerializeField]
        List<SUFastReactionData> _reactions = default;


        public void Play(GameObject go)
        {
            if(_reactions == null)
                return;

            for(int i=0;i<_reactions.Count;i++)
            {
                _reactions[i].Play(go);
            }

        }

    }


}

