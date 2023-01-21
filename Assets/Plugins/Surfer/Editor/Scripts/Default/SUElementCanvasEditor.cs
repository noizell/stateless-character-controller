using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Surfer
{
    [CustomEditor(typeof(SUElementCanvas))]
    public class SUElementCanvasEditor : SUElementEditor    
    {
        
        protected override bool IsNewComponent()
        {
            return true;
        }
        

    }

}

