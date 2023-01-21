using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public abstract class SUCamItemsManager : MonoBehaviour
    {

        protected GameObject _suPersistentCanvas = default;
        protected GameObject _mainParent = default;
        protected virtual string MainParentName { get; set; }
        protected virtual int SortOrder { get; set; }
        protected List<RenderMode> _rendersList = new List<RenderMode>() { RenderMode.ScreenSpaceCamera, RenderMode.ScreenSpaceOverlay, RenderMode.WorldSpace };
        protected bool _mustClean = default;
        protected Vector2 _targetPos = default;


        public virtual void MainLoop() { }
        public virtual void StopFollow(Camera cam) { }
        public virtual void StopFollow(string customTag) { }



        protected void GetPersistentCanvas()
        {
            if (_suPersistentCanvas != null)
                return;

            _suPersistentCanvas = GameObject.Find("SUPersistentCanvas");

            if (_suPersistentCanvas == null)
                return;

            _mainParent = new GameObject(MainParentName);
            _mainParent.transform.parent = _suPersistentCanvas.transform;

        }


        protected DictCamKey CreateCameKey(Camera cam, RenderMode renderMode)
        {

            return new DictCamKey(renderMode, cam);

        }


    }


}

