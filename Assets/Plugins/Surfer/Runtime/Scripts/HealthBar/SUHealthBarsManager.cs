using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer
{

    public class SUHealthBarsManager : SUCamItemsManager
    {

        public static SUHealthBarsManager I { get; set; }
        protected override string MainParentName { get; set; } = "HealthBars";
        protected override int SortOrder { get; set; } = 25000;


        Dictionary<DictCamKey, SUHealthBarCamData> _camInfos = new Dictionary<DictCamKey, SUHealthBarCamData>();
        Dictionary<int, Coroutine> _mpRoutineInfos = new Dictionary<int, Coroutine>();


        private void Awake()
        {
            if (I == null)
            {
                I = this;
                GetPersistentCanvas();
            }
            else
                Destroy(this);
        }



        public override void MainLoop()
        {

            if (_camInfos.Count <= 0)
            {
                Destroy(this);
                return;
            }


            foreach (KeyValuePair<DictCamKey, SUHealthBarCamData> pair in _camInfos)
            {
                if (pair.Key.Cam == null)
                {
                    _mustClean = true;
                    Destroy(pair.Value.CamCanvasRect.gameObject);
                    continue;
                }

                foreach (SUHealthBarData ind in pair.Value.AllHealthBars)
                {

                    if (ind.RectT == null)
                        continue;

                    _targetPos = RectTransformUtility.WorldToScreenPoint(pair.Key.Cam, ind.Target.position);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(pair.Value.CamCanvasRect, _targetPos, pair.Key.RenderMode == RenderMode.ScreenSpaceOverlay ? null : pair.Key.Cam, out _targetPos);


                    ind.RectT.localPosition = _targetPos + ind.OnScreenOffset;


                }



            }


            if(_mustClean)
            {
                _camInfos = _camInfos.Where(x=>x.Key.Cam!=null).Select(x=>x).ToDictionary(kv => kv.Key,kv => kv.Value);
                _mustClean = false;
            }


        }



        public void StartFollow(SUHealthBarLinkData linkData)
        {

            GetPersistentCanvas();

            if (_suPersistentCanvas == null)
                return;
            if (linkData == null)
                return;

            //set up the camera for the indicator data

            for (int i = 0; i < linkData.AllData.Count; i++)
            {

                var item = linkData.AllData[i];

                if (linkData.Target == null)
                    continue;
                if (item.Prefab == null)
                    continue;

                if (item.Cam == null)
                {
                    item.Cam = Camera.main;

                    if (item.Cam == null)
                        continue;
                }

                var key = CreateCameKey(item.Cam, item.RenderMode);


                if (_camInfos.TryGetValue(key, out SUHealthBarCamData info) && item.RenderMode == info.CamCanvas.renderMode)
                {

                    if (!info.AllHealthBars.Contains(item))
                    {

                        var cp = (Instantiate(item.Prefab) as GameObject).GetComponent<RectTransform>();
                        var defaultScale = cp.transform.localScale;

                        cp.SetParent(info.CamCanvas.transform);
                        cp.transform.localScale = defaultScale;

                        item.Target = linkData.Target;
                        item.RectT = cp;
                        item.VisualData = item.RectT?.GetComponent<ISUHealthBarVisualHandler>()?.OnHealthBarVisualSetUp();

                        info.AllHealthBars.Add(item);

                        item.CallStateUpdate(SUHealthBarState_ID.Started);

                    }

                }
                else
                {


                    GameObject camParent = new GameObject(item.Cam.name);
                    camParent.transform.parent = _mainParent.transform;

                    var canvas = camParent.AddComponent<Canvas>();
                    canvas.worldCamera = item.Cam;
                    canvas.planeDistance = 1;
                    canvas.renderMode = item.RenderMode;
                    canvas.sortingOrder = SortOrder;

                    camParent.AddComponent<CanvasScaler>();

                    //save new cam info entry
                    var newEntry = new SUHealthBarCamData();
                    newEntry.Cam = item.Cam;
                    newEntry.CamCanvas = canvas;
                    newEntry.CamCanvasRect = canvas.GetComponent<RectTransform>();



                    var cp = (Instantiate(item.Prefab) as GameObject).GetComponent<RectTransform>();
                    var defaultScale = cp.transform.localScale;

                    cp.SetParent(camParent.transform);
                    cp.transform.localScale = defaultScale;


                    item.Target = linkData.Target;
                    item.RectT = cp;
                    item.VisualData = item.RectT?.GetComponent<ISUHealthBarVisualHandler>()?.OnHealthBarVisualSetUp();
                    item.State = SUHealthBarState_ID.Started;


                    newEntry.AllHealthBars.Add(item);


                    _camInfos.Add(key, newEntry);

                    item.CallStateUpdate(SUHealthBarState_ID.Started);
                }


            }

        }


        /// <summary>
        /// Stop and Destroy healthBars of a specific SUHealthBarLinkData
        /// </summary>
        /// <param name="data"></param>
        public void StopFollow(SUHealthBarLinkData data)
        {

            if (data == null)
                return;

            List<SUHealthBarData> all = new List<SUHealthBarData>();

            foreach (KeyValuePair<DictCamKey, SUHealthBarCamData> pair in _camInfos)
            {

                foreach (var item in pair.Value.AllHealthBars)
                {

                    if (data.AllData.Contains(item))
                    {
                        data.AllData.Remove(item);
                        all.Add(item);

                        item.CallStateUpdate(SUHealthBarState_ID.Stopped);
                    }

                }


            }



            for (int i = 0; i < all.Count; i++)
            {

                GameObject.Destroy(all[i].RectT.gameObject);

                var key = CreateCameKey(all[i].Cam, all[i].RenderMode);

                if (_camInfos[key].AllHealthBars.Count <= 0)
                {
                    GameObject.Destroy(_camInfos[key].CamCanvasRect.gameObject);
                    _camInfos.Remove(key);
                }
            }


        }

        /// <summary>
        /// Stop and Destroy healthBars of a specific Camera
        /// </summary>
        /// <param name="data"></param>
        public override void StopFollow(Camera cam)
        {

            foreach (var render in _rendersList)
            {
                var key = CreateCameKey(cam, render);

                if (!_camInfos.ContainsKey(key))
                    continue;

                GameObject.Destroy(_camInfos[key].CamCanvasRect.gameObject);
                _camInfos.Remove(key);
            }

        }

        /// <summary>
        /// Stop and Destroy healthBars with a specific customTag
        /// </summary>
        /// <param name="data"></param>
        public override void StopFollow(string customTag)
        {

            GetAllByCustomTag(customTag,(data) =>
            {

                StopFollow(data);

            });

            
        }

        /// <summary>
        /// Stop and Destroy a specific HealthBar
        /// </summary>
        /// <param name="data"></param>
        public void StopFollow(SUHealthBarData data)
        {

            bool removed = false;
            var key = CreateCameKey(data.Cam, data.RenderMode);


            if (_camInfos.TryGetValue(key, out SUHealthBarCamData value))
            {

                if(value.AllHealthBars.Remove(data))
                {

                    removed = true;
                    data.CallStateUpdate(SUHealthBarState_ID.Stopped);
                    
                }

            }


            if (removed)
            {

                GameObject.Destroy(data.RectT.gameObject);

                if (_camInfos[key].AllHealthBars.Count <= 0)
                {
                    GameObject.Destroy(_camInfos[key].CamCanvasRect.gameObject);
                    _camInfos.Remove(key);
                }

            }
        }

        /// <summary>
        /// Get all healthBars with a specific customTag
        /// </summary>
        /// <param name="customTag"></param>
        /// <param name="OnHealthBar"></param>
        public void GetAllByCustomTag(string customTag, System.Action<SUHealthBarData> OnHealthBar)
        {


            foreach (KeyValuePair<DictCamKey, SUHealthBarCamData> pair in _camInfos)
            {

                if (pair.Key.Cam == null)
                    continue;

                foreach (var item in pair.Value.AllHealthBars)
                {

                    if (item.CustomTag == customTag)
                    {
                        OnHealthBar?.Invoke(item);
                    }

                }


            }

        }


        public void StartMPRoutine(SUHealthBarLinkData linkData,IEnumerator coroutine)
        {
            if (_mpRoutineInfos.ContainsKey(linkData.GetHashCode()))
                return;


            _mpRoutineInfos.Add(linkData.GetHashCode(),StartCoroutine(coroutine));

        }



        private void OnDestroy()
        {
            I = null;
        }


    }

}


