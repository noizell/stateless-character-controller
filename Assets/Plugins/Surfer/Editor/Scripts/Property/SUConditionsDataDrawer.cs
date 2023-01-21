using UnityEditor;
using UnityEngine;

namespace Surfer
{
    [CustomPropertyDrawer(typeof(SUConditionsData))]
    public class SUConditionsDataDrawer : PropertyDrawer
    {
        SerializedProperty _conditions = default;

        bool _showFoldout = false;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.indentLevel += EditorGUI.indentLevel == 0 ? 0 : 1;

            position.height = SurferHelper.lineHeight;

            _showFoldout = label.text != SurferHelper.kNoFoldout;


            if(_showFoldout)
                property.isExpanded = EditorGUI.Foldout(position,property.isExpanded,string.IsNullOrEmpty(label.text) ? "Conditions" : label.text,true);


            if( (property.isExpanded && _showFoldout) || (!_showFoldout))
            {
                if(_conditions == null)
                    _conditions = property.FindPropertyRelative("_conditions");
                
                for(int i=0;i<_conditions.arraySize;i++)
                {

                    position.y += (i== 0 && _showFoldout) ? SurferHelper.lineHeight : (i==0 && !_showFoldout) ? 0 : EditorGUI.GetPropertyHeight(_conditions.GetArrayElementAtIndex(i - 1));
                    EditorGUI.PropertyField(position,_conditions.GetArrayElementAtIndex(i),new GUIContent(""));
                    
                    if(!_showFoldout)
                        SurferHelper.DrawLine(ref position);


                    if(_conditions.GetArrayElementAtIndex(i).FindPropertyRelative("_name").stringValue.Equals(SurferHelper.Unset)
                    && _conditions.arraySize > 1)
                    {
                        _conditions.DeleteArrayElementAtIndex(i);
                    }
                }

                if(_conditions.arraySize <=0 || !_conditions.GetArrayElementAtIndex(_conditions.arraySize-1).FindPropertyRelative("_name").stringValue.Equals(SurferHelper.Unset) )
                {
                    _conditions.InsertArrayElementAtIndex(_conditions.arraySize);
                    _conditions.GetArrayElementAtIndex(_conditions.arraySize-1).FindPropertyRelative("_name").stringValue = SurferHelper.Unset;
                    _conditions.GetArrayElementAtIndex(_conditions.arraySize - 1).FindPropertyRelative("_key").stringValue = SurferHelper.Unset;
                }
            }
            
        }



        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            _conditions = property.FindPropertyRelative("_conditions");

            float height = EditorGUIUtility.singleLineHeight;

            if((property.isExpanded && _showFoldout) || (!_showFoldout))
            {
                for (int i = 0; i < _conditions.arraySize; i++)
                {
                    height += EditorGUI.GetPropertyHeight(_conditions.GetArrayElementAtIndex(i));
                }
            }

            return height;
        }
    }
}