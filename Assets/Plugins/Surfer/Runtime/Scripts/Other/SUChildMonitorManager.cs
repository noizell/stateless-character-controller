using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{


    public class SUChildMonitorManager : MonoBehaviour
    {
    
        public static SUChildMonitorManager I {get;private set;}


        List<SUChildrenMonitorData> _eventReg = new List<SUChildrenMonitorData>();

        SUChildrenMonitorData _loopItem = default;


        private void Awake() {
            
            if(I==null)
            {
                I = this;
            }
            else
            {
                Destroy(this);
            }

        }


        public void MainLoop()
        {

            if(_eventReg.Count <= 0)
            return;


            for(int i=0;i< _eventReg.Count;i++)
            {

                _loopItem = _eventReg[i];

                if(_loopItem.Count != _loopItem.Object.childCount)
                {
                    

                    if(_loopItem.Object.childCount > _loopItem.Count)
                        _loopItem.Mode = SUChildrenMonitorData.Mode_ID.Added;
                    else
                        _loopItem.Mode = SUChildrenMonitorData.Mode_ID.Removed;


                    _loopItem.Count = _loopItem.Object.childCount;
                    _loopItem.SendCallback();

                }

            }

            

        }


        public void RegisterChildrenEvents(ISUChildrenMonitorHandler inter, Transform tf)
        {
            if(!_eventReg.Exists(x=>x.Object == tf))
            {
                _eventReg.Add(new SUChildrenMonitorData(tf,inter));
            }

        }



        public void UnregisterChildrenEvents(Transform tf)
        {
            _eventReg.RemoveAll(x=>x.Object == tf);
        }

    }


}
