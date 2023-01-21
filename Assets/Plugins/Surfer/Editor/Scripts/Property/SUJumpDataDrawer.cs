using UnityEditor;
using UnityEngine;


namespace Surfer
{

    [CustomPropertyDrawer(typeof(SUJumpData))]
    public class SUJumpDataDrawer : SUScreenAnimationDataDrawer
    {

        protected override void OnSupplementaryFields(ref Rect position,SerializedProperty property)
        {
            position.y += SurferHelper.lineHeight;
            EditorGUI.PropertyField(position,property.FindPropertyRelative("_power"));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float baseHeight = base.GetPropertyHeight(property,label);

            if(IsNone() || !property.isExpanded )
            return baseHeight;

            return baseHeight + SurferHelper.lineHeight;

        }

    }



}


