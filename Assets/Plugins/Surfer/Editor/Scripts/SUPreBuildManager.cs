using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;



namespace Surfer
{

    public class SUPreBuildManager : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {

            SurferHelper.UpdateBuildVersionStrings();

        }


    }



}


