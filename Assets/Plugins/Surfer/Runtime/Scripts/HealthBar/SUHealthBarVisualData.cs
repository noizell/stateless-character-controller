using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Surfer
{


    [System.Serializable]
    public class SUHealthBarVisualData
    {

        /// <summary>
        /// Image component to use as HealthPoint bar. Must have Filled as Mode
        /// </summary>
        [SerializeField]
        Image _hpBar = default;
        public Image HpBar { get => _hpBar; }

        /// <summary>
        /// Image component to use as bar that show the damage applied to the health. Must have Filled as Mode
        /// </summary>
        [SerializeField]
        Image _hpDamageBar = default;
        public Image HpDamageBar { get => _hpDamageBar; }

        /// <summary>
        /// Image component to use as ManaPoint bar. Must have Filled as Mode
        /// </summary>
        [SerializeField]
        Image _mpBar = default;
        public Image MpBar { get => _mpBar; }

        /// <summary>
        /// RawImage component that contains the "vertical black lines" (used in the HealthPoints bar) that will be tiled based on total HP
        /// </summary>
        [SerializeField]
        RawImage _linesImage = default;
        public RawImage LinesImage { get => _linesImage; }

    }


}

