using UnityEditor;
using UnityEngine;


 namespace Surfer
 {

    [CustomPropertyDrawer(typeof(SURectSizeData))]
    public class SURectSizeDataDrawer : SUAnimationDataDrawer
    {

        SerializedProperty  _mode ;


        protected override bool OnCanUseDefaultModes(ref Rect position,SerializedProperty property)
        {   
            
            position.y += SurferHelper.lineHeight;
            EditorGUI.PropertyField(position,_mode);
            
            if(IsMultiply())
            {
                position.y += SurferHelper.lineHeight;
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position,property.FindPropertyRelative("_multi"));
            }

            return false;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            _mode = property.FindPropertyRelative("_mode");
            base.GetPropertyHeight(property,label);

            if(!property.isExpanded)
            return SurferHelper.lineHeight;

            if(IsNone())
            return SurferHelper.lineHeight*3;

            if(IsMultiply())
            return SurferHelper.lineHeight * (3+GetHeightCommonFields());

            return SurferHelper.lineHeight * (2+GetHeightCommonFields());
        }

    
        protected override bool IsNone()
        {
            return _mode.enumValueIndex == 0; 
        }

        bool IsMultiply()
        {
            return _mode.enumValueIndex == (int)SURectSizeData.Mode_ID.Multiply; 
        }
    }


 }
 
