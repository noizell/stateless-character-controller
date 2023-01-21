using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Surfer
{

    
    public class SUChildrenMonitorData
    {

        public enum Mode_ID
        {
            None,
            Added,
            Removed
        }
        
        public Transform Object {get;private set;}
        public int Count {get; set;}
        public Mode_ID Mode {get;set;} = Mode_ID.None;
        ISUChildrenMonitorHandler _interface = default;



        public SUChildrenMonitorData(Transform obj,ISUChildrenMonitorHandler interf)
        {
            Object = obj;
            Count = obj.transform.childCount;
            _interface = interf;
        }

        public void SendCallback()
        {
            _interface?.OnChildrenChanged(this);
        }


    }


}