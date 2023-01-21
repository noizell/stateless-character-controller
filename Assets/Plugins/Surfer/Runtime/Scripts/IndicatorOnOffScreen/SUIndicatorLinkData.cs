using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Surfer
{
    [System.Serializable]
    public class SUIndicatorLinkData : SULinkData
    {

        [SerializeField]
        List<SUIndicatorData> _allData = new List<SUIndicatorData>();
        public List<SUIndicatorData> AllData { get => _allData; }

        /// <summary>
        /// Make all indicators of this linkData start following the target
        /// </summary>
        public void StartFollow()
        {

            if (AllData.Count <= 0)
                return;

            if(SUIndicatorsManager.I == null)
            {
                SurferManager.I.gameObject.AddComponent<SUIndicatorsManager>();
            }

            SUIndicatorsManager.I.StartFollow(this);
        }

        /// <summary>
        /// Stop and destroy all indicators of this linkData
        /// </summary>
        public void StopFollow()
        {

            if (AllData.Count <= 0)
                return;
            if (SUIndicatorsManager.I == null)
                return;

            SUIndicatorsManager.I.StopFollow(this);
        }

        #region Constructors
        public SUIndicatorLinkData() { }
        public SUIndicatorLinkData(Transform target,List<SUIndicatorData> allData)
        {
            _target = target;
            _allData = allData;
        }
        #endregion

    }
}


