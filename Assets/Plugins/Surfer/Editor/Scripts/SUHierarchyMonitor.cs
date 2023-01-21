using System.IO;
using UnityEditor;
using UnityEngine;

namespace Surfer
{
    [InitializeOnLoadAttribute]
    public static class SUHierarchyMonitor
    {
        
        static SUHierarchyMonitor()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            EditorApplication.projectChanged += OnProjectChanged;
        }
        static void OnHierarchyChanged()
        {

            SurferHelper.SO.UpdateSceneList();
            SurferHelper.SO.UpdateLayersList();
            SurferHelper.SO.UpdateTagsList();
            SurferHelper.SO.UpdateEventsList();

            SurferManager[] sms = GameObject.FindObjectsOfType<SurferManager>();

            if(sms.Length<=0)
            {
                new GameObject("Surfer").AddComponent<SurferManager>();
            }
            else
            {
                if(sms.Length > 1)
                {
                    GameObject.DestroyImmediate(sms[1].gameObject);
                }
            }


        }

        static void OnProjectChanged()
        {

            SurferHelper.SO.UpdateSceneList();
            SurferHelper.SO.UpdateLayersList();
            SurferHelper.SO.UpdateTagsList();
            SurferHelper.SO.UpdateEventsList();

        }



    }
}
 

 
