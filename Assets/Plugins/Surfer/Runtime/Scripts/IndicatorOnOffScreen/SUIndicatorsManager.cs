using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer
{
    public class SUIndicatorsManager : SUCamItemsManager
    {

        public static SUIndicatorsManager I { get; set; }
        protected override string MainParentName { get ; set ; } = "Indicators";
        protected override int SortOrder { get; set; } = 24000;


        Dictionary<DictCamKey, SUIndicatorCamData> _camInfos = new Dictionary<DictCamKey, SUIndicatorCamData>();

        SUIndicatorState_ID _indState = default;
        float _distance = default;
        float _centerDistance = default;
        Vector2 _targetPosClamped = default;

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


            foreach (KeyValuePair<DictCamKey, SUIndicatorCamData> pair in _camInfos)
            {

                if (pair.Key.Cam == null)
                {
                    _mustClean = true;
                    Destroy(pair.Value.CamCanvasRect.gameObject);
                    continue;
                }

                foreach(SUIndicatorData ind in pair.Value.AllIndicators)
                {

                    if (ind.RectT == null)
                        continue;

                    _targetPos = RectTransformUtility.WorldToScreenPoint(pair.Key.Cam, ind.Target.position);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(pair.Value.CamCanvasRect, _targetPos,pair.Key.RenderMode == RenderMode.ScreenSpaceOverlay ? null : pair.Key.Cam, out _targetPos);

                    //updates state
                    _indState = SUIndicatorState_ID.Standby;

                    if (_targetPos.x < -pair.Value.CamCanvasRect.rect.width / 2f || _targetPos.x > pair.Value.CamCanvasRect.rect.width / 2f
                        || _targetPos.y < -pair.Value.CamCanvasRect.rect.height / 2f || _targetPos.y > pair.Value.CamCanvasRect.rect.height / 2f)
                        _indState = SUIndicatorState_ID.FollowingOffScreen;
                    else
                        _indState = SUIndicatorState_ID.FollowingOnScreen;

                    if (_indState == SUIndicatorState_ID.FollowingOffScreen && !ind.IsOffScreenType)
                        _indState = SUIndicatorState_ID.Standby;

                    if (_indState == SUIndicatorState_ID.FollowingOnScreen && !ind.IsOnScreenType)
                        _indState = SUIndicatorState_ID.Standby;



                    if(_indState == SUIndicatorState_ID.FollowingOffScreen)
                    {
                        _targetPosClamped.x = Mathf.Clamp(_targetPos.x, -pair.Value.CamCanvasRect.rect.width / 2f + ind.RectT.rect.size.x / 2f, pair.Value.CamCanvasRect.rect.width / 2f - ind.RectT.rect.size.x / 2f);
                        _targetPosClamped.y = Mathf.Clamp(_targetPos.y, -pair.Value.CamCanvasRect.rect.height / 2f + ind.RectT.rect.size.y / 2f, pair.Value.CamCanvasRect.rect.height / 2f - ind.RectT.rect.size.y / 2f);
                        ind.RectT.localPosition = _targetPosClamped;

                        if (ind.VisualData?.RotationObj!=null)
                            ind.VisualData.RotationObj.eulerAngles = new Vector3(0, 0, Mathf.Atan2(_targetPos.y - ind.RectT.localPosition.y, _targetPos.x - ind.RectT.localPosition.x) * Mathf.Rad2Deg + ind.VisualData.AngleOffset);

                        if (ind.State != SUIndicatorState_ID.FollowingOffScreen)
                        {

                            ind.State = SUIndicatorState_ID.FollowingOffScreen;
                            ind.RectT.GetComponent<ISUIndicatorStateHandler>()?
                                .OnIndicatorStateUpdate(new SUIndicatorEventData(ind));


                        }

                    }
                    else if (_indState == SUIndicatorState_ID.FollowingOnScreen)
                    {
                        _targetPosClamped.x = Mathf.Clamp(_targetPos.x, -pair.Value.CamCanvasRect.rect.width / 2f + 5f, pair.Value.CamCanvasRect.rect.width / 2f - 5f);
                        _targetPosClamped.y = Mathf.Clamp(_targetPos.y, -pair.Value.CamCanvasRect.rect.height / 2f + 5f, pair.Value.CamCanvasRect.rect.height / 2f - 5f);
                        _targetPosClamped += ind.OnScreenOffset;

                        ind.RectT.localPosition = _targetPosClamped;


                        if (ind.State != SUIndicatorState_ID.FollowingOnScreen)
                        {

                            ind.State = SUIndicatorState_ID.FollowingOnScreen;
                            ind.RectT.GetComponent<ISUIndicatorStateHandler>()?
                                .OnIndicatorStateUpdate(new SUIndicatorEventData(ind));


                        }

                    }
                    else if(_indState == SUIndicatorState_ID.Standby)
                    {
                        if (ind.State != SUIndicatorState_ID.Standby)
                        {

                            ind.RectT.position = new Vector3(0, -100000, 0);

                            ind.State = SUIndicatorState_ID.Standby;
                            ind.RectT.GetComponent<ISUIndicatorStateHandler>()?
                                .OnIndicatorStateUpdate(new SUIndicatorEventData(ind));

                        }


                    }




                    //check distance
                    if (ind.PlayerObj == null)
                        continue;

                    _distance = Vector3.Distance(ind.Target.position,ind.PlayerObj.position);
                    _centerDistance = Vector2.Distance(Vector2.zero, _targetPos);
                    ind.VisualData?.DistanceText?.SetText(""+(int)_distance+ind.VisualData.Suffix);

                    if(_distance < ind.Radius)
                    {

                        if(ind.DistanceState != SUIndicatorDistanceState_ID.CloseAndCentered && _centerDistance < 50f)
                        {
                            ind.DistanceState = SUIndicatorDistanceState_ID.CloseAndCentered;

                            ind.RectT.GetComponent<ISUIndicatorDistanceHandler>()?
                                .OnIndicatorDistanceUpdate(new SUIndicatorEventData(ind));
                        }
                        else if (ind.DistanceState != SUIndicatorDistanceState_ID.Close && _centerDistance > 50f)
                        {

                            ind.DistanceState = SUIndicatorDistanceState_ID.Close;

                            ind.RectT.GetComponent<ISUIndicatorDistanceHandler>()?
                                .OnIndicatorDistanceUpdate(new SUIndicatorEventData(ind));

                        }


                    }
                    else if (_distance > ind.Radius && ind.DistanceState != SUIndicatorDistanceState_ID.Far)
                    {
                        ind.DistanceState = SUIndicatorDistanceState_ID.Far;

                        ind.RectT.GetComponent<ISUIndicatorDistanceHandler>()?
                            .OnIndicatorDistanceUpdate(new SUIndicatorEventData(ind));
                    }


                }



            }


            if (_mustClean)
            {

                _camInfos = _camInfos.Where(x => x.Key.Cam != null).Select(x => x).ToDictionary(kv => kv.Key, kv => kv.Value);
                _mustClean = false;
            }


        }

        public void StartFollow(SUIndicatorLinkData linkData)
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

                var key = CreateCameKey(item.Cam,item.RenderMode);

                if (_camInfos.TryGetValue(key, out SUIndicatorCamData info) && item.RenderMode == info.CamCanvas.renderMode)
                {

                    if (!info.AllIndicators.Contains(item))
                    {

                        var cp = (Instantiate(item.Prefab) as GameObject).GetComponent<RectTransform>();
                        var defaultScale = cp.transform.localScale;

                        cp.SetParent(info.CamCanvas.transform);
                        cp.transform.localScale = defaultScale;

                        item.Target = linkData.Target;
                        item.RectT = cp;
                        item.LinkID = linkData.GetHashCode();
                        item.VisualData = item.RectT?.GetComponent<ISUIndicatorVisualHandler>()?.OnIndicatorVisualSetUp();

                        info.AllIndicators.Add(item);
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
                    var newEntry = new SUIndicatorCamData();
                    newEntry.Cam = item.Cam;
                    newEntry.CamCanvas = canvas;
                    newEntry.CamCanvasRect = canvas.GetComponent<RectTransform>();



                    var cp = (Instantiate(item.Prefab) as GameObject).GetComponent<RectTransform>();
                    var defaultScale = cp.transform.localScale;

                    cp.SetParent(camParent.transform);
                    cp.transform.localScale = defaultScale;


                    item.Target = linkData.Target;
                    item.RectT = cp;
                    item.LinkID = linkData.GetHashCode();
                    item.VisualData = item.RectT?.GetComponent<ISUIndicatorVisualHandler>()?.OnIndicatorVisualSetUp();


                    newEntry.AllIndicators.Add(item);


                    _camInfos.Add(key, newEntry);

                }


            }


        }

        /// <summary>
        /// Stop and Destroy indicators of a specific SUIndicatorLinkData
        /// </summary>
        /// <param name="data"></param>
        public void StopFollow(SUIndicatorLinkData data)
        {

            if (data == null)
                return;

            List<SUIndicatorData> all = new List<SUIndicatorData>();

            foreach (KeyValuePair<DictCamKey, SUIndicatorCamData> pair in _camInfos)
            {

                foreach (var item in pair.Value.AllIndicators)
                {

                    if(data.AllData.Contains(item))
                    {
                        data.AllData.Remove(item);
                        all.Add(item);
                    }

                }


            }



            for (int i = 0; i < all.Count; i++)
            {
               
                GameObject.Destroy(all[i].RectT.gameObject);

                var key = CreateCameKey(all[i].Cam, all[i].RenderMode);

                if (_camInfos[key].AllIndicators.Count <= 0)
                {
                    GameObject.Destroy(_camInfos[key].CamCanvasRect.gameObject);
                    _camInfos.Remove(key);
                }
            }



        }

        /// <summary>
        /// Stop and Destroy indicators of a specific Camera
        /// </summary>
        /// <param name="data"></param>
        public override void StopFollow(Camera cam)
        {


            foreach(var render in _rendersList)
            {
                var key = CreateCameKey(cam, render);

                if (!_camInfos.ContainsKey(key))
                    continue;

                GameObject.Destroy(_camInfos[key].CamCanvasRect.gameObject);
                _camInfos.Remove(key);
            }

            

        }

        /// <summary>
        /// Stop and Destroy indicators with a specific customTag
        /// </summary>
        /// <param name="data"></param>
        public override void StopFollow(string customTag)
        {

            GetAllByCustomTag(customTag, (data) =>
             {

                 StopFollow(data);

             });

        }

        /// <summary>
        /// Stop and Destroy a specific Indicator
        /// </summary>
        /// <param name="data"></param>
        public void StopFollow(SUIndicatorData data)
        {

            bool removed = false;

            var key = CreateCameKey(data.Cam,data.RenderMode);

            if (_camInfos.TryGetValue(key, out SUIndicatorCamData value))
            {

                removed = value.AllIndicators.Remove(data);

            }


            if (removed)
            {

                GameObject.Destroy(data.RectT.gameObject);

                if (_camInfos[key].AllIndicators.Count <= 0)
                {
                    GameObject.Destroy(_camInfos[key].CamCanvasRect.gameObject);
                    _camInfos.Remove(key);
                }

            }
        }


        /// <summary>
        /// Get all indicators with a specific customTag
        /// </summary>
        /// <param name="customTag"></param>
        /// <param name="OnIndicator"></param>
        public void GetAllByCustomTag(string customTag,System.Action<SUIndicatorData> OnIndicator)
        {


            foreach (KeyValuePair<DictCamKey, SUIndicatorCamData> pair in _camInfos)
            {

                if (pair.Key.Cam == null)
                    continue;

                foreach (var item in pair.Value.AllIndicators)
                {

                    if (item.CustomTag == customTag)
                    {
                        OnIndicator?.Invoke(item);
                    }

                }


            }

        }

        /// <summary>
        /// Get all aliases of a specific indicator (only useful with multiple cameras).
        /// If you have an IndicatorData shown on P1Camera , this call will give you the IndicatorsData shown on P2Camera,P3Camera
        /// and so on (namely the indicators created from the same SUIndicatorDataLink)
        /// </summary>
        public List<SUIndicatorData> GetAliases(SUIndicatorData data,bool includeCaller = false)
        {

            List<SUIndicatorData> aliases = new List<SUIndicatorData>();

            if (includeCaller)
                aliases.Add(data);

            foreach(var pair in _camInfos)
            {

                foreach(var item in pair.Value.AllIndicators)
                {
                    if (item.LinkID == data.LinkID && data != item)
                        aliases.Add(item);
                }

            }

            return aliases;

        }


        private void OnDestroy()
        {
            I = null;
        }


    }

}


