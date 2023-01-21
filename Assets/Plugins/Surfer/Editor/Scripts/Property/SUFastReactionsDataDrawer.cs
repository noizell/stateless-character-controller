using UnityEditor;
using UnityEngine;

namespace Surfer
{
    [CustomPropertyDrawer(typeof(SUFastReactionsData))]
    public class SUFastReactionsDataDrawer : PropertyDrawer
    {

        SerializedProperty _reactions = default;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.indentLevel += EditorGUI.indentLevel == 0 ? 0 : 1;

            position.height = SurferHelper.lineHeight;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, string.IsNullOrEmpty(label.text) ? "Fast Reactions" : label.text, true);


            if (property.isExpanded)
            {
                if (_reactions == null)
                    _reactions = property.FindPropertyRelative("_reactions");


                for (int i = 0; i < _reactions.arraySize; i++)
                {
                    if (i == 0)
                        position.y += SurferHelper.lineHeight;
                    else
                        position.y += EditorGUI.GetPropertyHeight(_reactions.GetArrayElementAtIndex(i-1));

                    EditorGUI.PropertyField(position, _reactions.GetArrayElementAtIndex(i), new GUIContent(""));

                    if (_reactions.GetArrayElementAtIndex(i).FindPropertyRelative("_reactionID").enumValueIndex == 0
                    && _reactions.arraySize > 1)
                    {
                        _reactions.DeleteArrayElementAtIndex(i);
                    }
                }

                if (_reactions.arraySize <= 0 || _reactions.GetArrayElementAtIndex(_reactions.arraySize - 1).FindPropertyRelative("_reactionID").enumValueIndex != 0)
                {
                    _reactions.InsertArrayElementAtIndex(_reactions.arraySize);
                    _reactions.GetArrayElementAtIndex(_reactions.arraySize - 1).FindPropertyRelative("_reactionID").enumValueIndex = 0;
                }
            }

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            float height = default;
            _reactions = property.FindPropertyRelative("_reactions");

            for (int i = 0; i < _reactions.arraySize; i++)
            {

                height += EditorGUI.GetPropertyHeight(_reactions.GetArrayElementAtIndex(i));
            }

            height += SurferHelper.lineHeight;

            return height;
        }
    }
}