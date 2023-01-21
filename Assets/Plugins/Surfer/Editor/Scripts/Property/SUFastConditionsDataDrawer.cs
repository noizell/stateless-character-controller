using UnityEditor;
using UnityEngine;

namespace Surfer
{
    [CustomPropertyDrawer(typeof(SUFastConditionsData))]
    public class SUFastConditionsDataDrawer : PropertyDrawer
    {

        SerializedProperty _conditions = default;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.indentLevel += EditorGUI.indentLevel == 0 ? 0 : 1;

            position.height = SurferHelper.lineHeight;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, string.IsNullOrEmpty(label.text) ? "Fast Conditions" : label.text, true);


            if (property.isExpanded)
            {
                if (_conditions == null)
                    _conditions = property.FindPropertyRelative("_conditions");


                for (int i = 0; i < _conditions.arraySize; i++)
                {
                    if (i == 0)
                        position.y += SurferHelper.lineHeight;
                    else
                        position.y += EditorGUI.GetPropertyHeight(_conditions.GetArrayElementAtIndex(i-1));

                    EditorGUI.PropertyField(position, _conditions.GetArrayElementAtIndex(i), new GUIContent(""));

                    if (_conditions.GetArrayElementAtIndex(i).FindPropertyRelative("_conditionID").enumValueIndex == 0
                    && _conditions.arraySize > 1)
                    {
                        _conditions.DeleteArrayElementAtIndex(i);
                    }

                }

                if (_conditions.arraySize <= 0 || _conditions.GetArrayElementAtIndex(_conditions.arraySize - 1).FindPropertyRelative("_conditionID").enumValueIndex != 0)
                {
                    _conditions.InsertArrayElementAtIndex(_conditions.arraySize);
                    _conditions.GetArrayElementAtIndex(_conditions.arraySize - 1).FindPropertyRelative("_conditionID").enumValueIndex = 0;
                }
            }

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return SurferHelper.lineHeight;

            float height = default;
            _conditions = property.FindPropertyRelative("_conditions");

            for (int i = 0; i < _conditions.arraySize; i++)
            {

                height += EditorGUI.GetPropertyHeight(_conditions.GetArrayElementAtIndex(i));
            }

            height += SurferHelper.lineHeight;

            return height;
        }
    }
}