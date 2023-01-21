using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{
    public interface ISUChildrenMonitorHandler
    {

        void OnChildrenChanged(SUChildrenMonitorData data);

    }
}

