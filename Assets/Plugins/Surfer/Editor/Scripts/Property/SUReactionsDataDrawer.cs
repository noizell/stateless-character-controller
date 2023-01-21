using UnityEditor;
using UnityEngine;

namespace Surfer
{
    [CustomPropertyDrawer(typeof(SUReactionsData))]
    public class SUReactionsDataDrawer : PropertyDrawer
    {
        SerializedProperty _reactions = default;

        bool _showFoldout = false;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.indentLevel += EditorGUI.indentLevel == 0 ? 0 : 1;

            position.height = SurferHelper.lineHeight;
            _showFoldout = label.text != SurferHelper.kNoFoldout;

            if(_showFoldout)
                property.isExpanded = EditorGUI.Foldout(position,property.isExpanded,string.IsNullOrEmpty(label.text) ? "Reactions" : label.text,true);


            if( (property.isExpanded && _showFoldout) || (!_showFoldout))
            {
                if(_reactions == null)
                    _reactions = property.FindPropertyRelative("_reactions");

                for(int i=0;i<_reactions.arraySize;i++)
                {
                    
                    position.y += (i== 0 && _showFoldout) ? SurferHelper.lineHeight : (i==0 && !_showFoldout) ? 0 : EditorGUI.GetPropertyHeight(_reactions.GetArrayElementAtIndex(i-1));
                    EditorGUI.PropertyField(position,_reactions.GetArrayElementAtIndex(i),new GUIContent(""));
                    
                    if(!_showFoldout)
                        SurferHelper.DrawLine(ref position);

                    if(_reactions.GetArrayElementAtIndex(i).FindPropertyRelative("_name").stringValue.Equals(SurferHelper.Unset)
                    && _reactions.arraySize > 1)
                    {
                        _reactions.DeleteArrayElementAtIndex(i);
                    }
                }

                if(_reactions.arraySize <=0 || !_reactions.GetArrayElementAtIndex(_reactions.arraySize-1).FindPropertyRelative("_name").stringValue.Equals(SurferHelper.Unset) )
                {
                    _reactions.InsertArrayElementAtIndex(_reactions.arraySize);
                    _reactions.GetArrayElementAtIndex(_reactions.arraySize-1).FindPropertyRelative("_name").stringValue = SurferHelper.Unset;
                    _reactions.GetArrayElementAtIndex(_reactions.arraySize - 1).FindPropertyRelative("_key").stringValue = SurferHelper.Unset;
                }
            }
            
        }



        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            _reactions = property.FindPropertyRelative("_reactions");

            float height = EditorGUIUtility.singleLineHeight;

            if((property.isExpanded && _showFoldout) || (!_showFoldout))
            {
                for (int i = 0; i < _reactions.arraySize; i++)
                {
                    height += EditorGUI.GetPropertyHeight(_reactions.GetArrayElementAtIndex(i));
                }
            }

            return height;
        }
    }
}