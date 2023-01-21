using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Surfer
{
    [System.Serializable]
    public class SUHealthBarLinkData : SULinkData
    {

        [SerializeField]
        float _totalHp = 200f;
        [SerializeField]
        float _totalMp = default;

        /// <summary>
        /// How many HPs correspond to 10 "vertical black lines" ?
        /// </summary>
        [SerializeField]
        float _hpFor10Lines = 100f;

        /// <summary>
        /// How many MP are reloaded every second?
        /// </summary>
        [SerializeField]
        float _mpToReload = 15f;


        /// <summary>
        /// "How fast the MP bar should animate when you use MP ? ( Seconds ) "
        /// </summary>
        [SerializeField]
        [Tooltip("How fast the MP bar should animate when you use MP ? ( Seconds ) ")]
        float _mpBarDuration = .15f;

        /// <summary>
        /// How fast the Damage bar should animate when you lose HP ? ( Seconds ) 
        /// </summary>
        [SerializeField]
        [Tooltip("How fast the Damage bar should animate when you lose HP ? ( Seconds ) ")]
        float _damageBarDuration = .5f;

        /// <summary>
        /// After how many seconds the Damage bar should animate when you lose HP ?
        /// </summary>
        [SerializeField]
        [Tooltip("After how many seconds the Damage bar should animate when you lose HP ?")]
        float _damageBarDelay = .25f;

        [SerializeField]
        List<SUHealthBarData> _allData = new List<SUHealthBarData>();
        public List<SUHealthBarData> AllData { get => _allData; }




        float _currentHp = default;
        public float CurrentHp
        {
            get
            {
                return _currentHp;
            }
            set
            {

                _currentHp = value;

                SUHealthBarHPState_ID newHPState = default;

                if (HasFullHp)
                    newHPState = SUHealthBarHPState_ID.Full;
                else if (HasNoHp)
                {

                    newHPState = SUHealthBarHPState_ID.Empty;

                    foreach (var item in AllData)
                    {
                        item.RectT?.gameObject.SetActive(false);
                    }

                }
                else if (_currentHp > _totalHp / 2.0f)
                    newHPState = SUHealthBarHPState_ID.MoreThanHalf;
                else if (_currentHp <= _totalHp / 2.0f)
                    newHPState = SUHealthBarHPState_ID.LessThanHalf;



                foreach(var item in AllData)
                {
                    if (item.HPState != newHPState)
                    {
                        item.CallHPStateUpdate(newHPState);
                    }
                }

            }
        }

        public float CurrentMp { get; private set; }
        public bool HasFullHp { get => CurrentHp >= _totalHp; }
        public bool HasNoHp { get => CurrentHp <= 0f; }
        public bool HasFullMp { get => CurrentMp >= _totalMp; }
        public bool HasNoMp { get => CurrentMp <= 0f; }
        public bool IsEmpty
        {
            get
            {

                if (AllData.Count <= 0)
                    return false;

                return AllData[0].HPState == SUHealthBarHPState_ID.Empty;

            }
        }

        float _maxPossibleValue = 1_000_000_000f;


        bool _hasDoneSetup = false;

        Coroutine _mpRoutine = default;

        float _startingHp = -1;
        float _startingMp = -1;


        #region Constructors

        public SUHealthBarLinkData() { }
        public SUHealthBarLinkData(Transform target, List<SUHealthBarData> allData,
            float hp,float mp = 0,
            float mpReload = 15,float mpBarDuration = .15f,
            float damageBarDuration = .5f, float damageBarDelay = .25f,
            float hpFor10Lines = 100f)
        {
            _target = target;
            _allData = allData;
            _totalHp = hp;
            _totalMp = mp;
            _mpToReload = mpReload;
            _mpBarDuration = mpBarDuration;
            _damageBarDuration = damageBarDuration;
            _damageBarDelay = damageBarDelay;
            _hpFor10Lines = hpFor10Lines;
        }

        #endregion



        void Setup()
        {

            if (_hasDoneSetup)
                return;

            if(_startingHp < 0 )
            {
                _startingHp = _totalHp;
                _startingMp = _totalMp;
            }

            _totalHp = _startingHp;
            _totalMp = _startingMp;
            CurrentHp = _totalHp;
            CurrentMp = _totalMp;

            UpdateHpBar(false);
            UpdateMpBar(false);
            UpdateLinesUV();

            foreach (var item in AllData)
            {
                if (item == null)
                    continue;
                if (item.RectT == null)
                    continue;

                item.RectT.gameObject.SetActive(true);
            }


            if (_mpRoutine == null && SUHealthBarsManager.I != null)
                _mpRoutine = SUHealthBarsManager.I.StartCoroutine(ReloadingMp());


            _hasDoneSetup = true;

        }

        /// <summary>
        /// Make all indicators of this linkData start following the target
        /// </summary>
        public void StartFollow()
        {

            if (AllData.Count <= 0)
                return;


            if (SUHealthBarsManager.I == null)
            {
                SurferManager.I.gameObject.AddComponent<SUHealthBarsManager>();
            }

            SUHealthBarsManager.I.StartFollow(this);
            Setup();
        }

        /// <summary>
        /// Stop and destroy all indicators of this linkData
        /// </summary>
        public void StopFollow()
        {

            if (AllData.Count <= 0)
                return;
            if (SUHealthBarsManager.I == null)
                return;

            SUHealthBarsManager.I.StopFollow(this);
        }


        /// <summary>
        /// Apply damage to the health points
        /// </summary>
        /// <param name="damage"></param>
        public void DamageHp(float damage)
        {
            CurrentHp = Mathf.Clamp(CurrentHp - damage, 0f, CurrentHp);
            UpdateHpBar(true);

        }



        /// <summary>
        /// Call this when the player takes an health potion
        /// </summary>
        /// <param name="hp">Amount of Hp to heal</param>
        public void HealHp(float hp)
        {

            CurrentHp = Mathf.Clamp(CurrentHp + hp, CurrentHp, _totalHp);
            UpdateHpBar(false);

        }



        /// <summary>
        /// Call this when you're adding an armor or item that gives extra hp to the player
        /// </summary>
        /// <param name="hp">Amount of Hp to add</param>
        public void AddHp(float hp)
        {

            _totalHp = Mathf.Clamp(_totalHp + hp, 0f, _maxPossibleValue);

            UpdateLinesUV();
            UpdateHpBar(false);

        }


        /// <summary>
        /// Call this when you're removing an armor or item that gives extra hp to the player
        /// </summary>
        /// <param name="hp">Amount of Hp to remove</param>
        public void RemoveHp(float hp)
        {

            _totalHp = Mathf.Clamp(_totalHp - hp, 0f, _maxPossibleValue);
            CurrentHp = Mathf.Clamp(CurrentHp,CurrentHp,_totalHp);

            UpdateLinesUV();
            UpdateHpBar(false);

        }


        void UpdateLinesUV()
        {

            ForEachVisualData((data) =>
            {
                if (data.LinesImage != null)
                    data.LinesImage.uvRect = new Rect(data.LinesImage.uvRect.x, data.LinesImage.uvRect.y, Mathf.Clamp(_totalHp / _hpFor10Lines, 0f, _maxPossibleValue), data.LinesImage.uvRect.height);
            });
            
        }

        void UpdateHpBar(bool isAnimated)
        {

            ForEachVisualData((data) =>
            {

                if(data.HpDamageBar!=null && data.HpBar!=null)
                    data.HpDamageBar.fillAmount = data.HpBar.fillAmount;

                if (data.HpBar != null)
                    data.HpBar.fillAmount = CurrentHp / _totalHp;


                if (data.HpDamageBar != null && data.HpBar!=null)
                {
                    if (!isAnimated)
                        data.HpDamageBar.fillAmount = 0f;
                    else
                        data.HpDamageBar.DOFillAmount(data.HpBar.fillAmount, _damageBarDuration).SetEase(Ease.Linear).SetDelay(_damageBarDelay).Play();
                }




            });


        }


        IEnumerator ReloadingMp()
        {

            while (true)
            {

                yield return new WaitUntil(() => !HasFullMp && !IsEmpty);


                while (!HasFullMp && !IsEmpty)
                {
                    CurrentMp = Mathf.Clamp(CurrentMp + _mpToReload * Time.deltaTime, CurrentMp, _totalMp);

                    UpdateMpBar(false);

                    yield return null;

                }

            }



        }

        ///// <summary>
        ///// Call this when you're adding an armor or item that gives extra mp to the player
        ///// </summary>
        ///// <param name="mp">Amount of Mp to add</param>
        public void AddMp(float mp)
        {

            _totalMp = Mathf.Clamp(_totalMp + mp, 0f, _maxPossibleValue);

            UpdateMpBar(false);

        }

        ///// <summary>
        ///// Call this when you're removing an armor or item that gives extra mp to the player
        ///// </summary>
        ///// <param name="mp">Amount of Mp to remove</param>
        public void RemoveMp(float mp)
        {

            _totalMp = Mathf.Clamp(_totalMp - mp, 0f, _maxPossibleValue);
            CurrentMp = Mathf.Clamp(CurrentMp, CurrentMp, _totalMp);

            UpdateMpBar(false);

        }


        ///// <summary>
        ///// Call this when you're casting spells
        ///// </summary>
        ///// <param name="mp">Mp cost of the spell</param>
        public void UseMP(float mp)
        {

            if (!HasEnoughMp(mp))
                return;

            CurrentMp = Mathf.Clamp(CurrentMp - mp, 0f, CurrentMp);
            UpdateMpBar(true);

        }

        void UpdateMpBar(bool isAnimated)
        {

            ForEachVisualData((data) =>
            {

                if(data.MpBar != null)
                {
                    if (!isAnimated)
                        data.MpBar.fillAmount = CurrentMp / _totalMp;
                    else
                        data.MpBar.DOFillAmount(CurrentMp / _totalMp, _mpBarDuration).SetEase(Ease.Linear).Play();
                }

            });

        }

        public bool HasEnoughMp(float mpCost)
        {

            return CurrentMp >= mpCost;

        }

        /// <summary>
        /// Call this when the character/enemy has respawned after a death.
        /// The bar will refill and reset its values to the starting ones
        /// </summary>
        public void Restart()
        {

            _hasDoneSetup = false;
            Setup();

        }


        void ForEachVisualData(System.Action<SUHealthBarVisualData> OnItem)
        {
            foreach(SUHealthBarData data in AllData)
            {
                if (data.VisualData == null)
                    continue;

                OnItem?.Invoke(data.VisualData);
            }
        }

    }


}

