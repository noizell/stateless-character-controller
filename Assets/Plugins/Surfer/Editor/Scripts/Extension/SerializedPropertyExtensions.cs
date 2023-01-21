using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer
{

    public static class SerializedPropertyExtensions
    {


        public static void AddField<T>(this SerializedProperty prop, ref Rect pos, SerializedProperty fieldProperty, bool inParent = false) where T : Component
        {

            pos.y += SurferHelper.lineHeight;

            if (fieldProperty.objectReferenceValue == null)
                fieldProperty.objectReferenceValue = inParent ? (prop.serializedObject.targetObject as Component).GetComponentInParent<T>()
                    : (prop.serializedObject.targetObject as Component).GetComponent<T>();

            EditorGUI.PropertyField(CreateRect(ref pos,1), fieldProperty, GUIContent.none);
        }

        public static void AddGOField(this SerializedProperty prop, ref Rect pos, SerializedProperty fieldProperty)
        {

            pos.y += SurferHelper.lineHeight;

            if (fieldProperty.objectReferenceValue == null)
                fieldProperty.objectReferenceValue = (prop.serializedObject.targetObject as Component).gameObject;

            EditorGUI.PropertyField(CreateRect(ref pos, 1), fieldProperty, GUIContent.none);
        }


        static Rect CreateRect(ref Rect pos,int position = 0)
        {


            if (position == 0)
            {
                return pos;
            }
            else if (position == 1)
            {
                return new Rect(pos.x, pos.y, pos.width / 3f, pos.height);
            }
            else if (position == 2)
            {
                return new Rect(pos.x + pos.width / 1.75f, pos.y, pos.width / 3f, pos.height);
            }

            return pos;
        }



        public static void AddValueField(this SerializedProperty prop,ref Rect pos, int position = 2, GUIContent content = null)
        {
            if (position == 2)
            {
                EditorGUI.PropertyField(CreateRect(ref pos, position), prop, content ?? GUIContent.none);
            }
            else if (position == 0)
            {
                pos.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(CreateRect(ref pos, position), prop, content ?? GUIContent.none);
            }
            else if (position == 1)
            {
                pos.y += SurferHelper.lineHeight;
                EditorGUI.PropertyField(CreateRect(ref pos, position), prop, content ?? GUIContent.none);
            }


        }



        public static void AddIntList(this SerializedProperty prop, ref Rect pos,string[] list)
        {
            prop.intValue = EditorGUI.Popup(new Rect(pos.x + pos.width / 1.75f, pos.y, pos.width / 2.5f, pos.height), "", prop.intValue, list);
        }


        public static void AddCustomUserFields(this SerializedProperty prop, ref Rect pos, List<PathField> fields, ref SerializedProperty vals)
        {
            for (int i = 0; i < fields.Count; i++)
            {

                pos.height = EditorGUIUtility.singleLineHeight;

                pos.y += SurferHelper.lineHeight;

                if(fields[i].Field_ID == PathFieldType_ID.Object_1
                    || fields[i].Field_ID == PathFieldType_ID.Object_2
                    || fields[i].Field_ID == PathFieldType_ID.Object_3
                    || fields[i].Field_ID == PathFieldType_ID.Object_4
                    || fields[i].Field_ID == PathFieldType_ID.Object_5)
                {

                    var propertyFound = vals.FindPropertyRelative(fields[i].Field_ID.ToString());
                    var cp = fields[i].Type;

                    if(propertyFound.objectReferenceValue != null && cp != propertyFound.objectReferenceValue.GetType()
                    && !propertyFound.objectReferenceValue.GetType().IsSubclassOf(cp) )
                    {
                        propertyFound.objectReferenceValue = null;
                        prop.serializedObject.ApplyModifiedProperties();

                    }

                    if(propertyFound.objectReferenceValue == null || cp != propertyFound.objectReferenceValue.GetType())
                    {
                        var go = (prop.serializedObject.targetObject as Component).gameObject;

                        if(cp == typeof(GameObject))
                        {
                            propertyFound.objectReferenceValue = go;
                        }
                        else if(cp == typeof(Transform))
                        {
                            propertyFound.objectReferenceValue = go.transform;
                        }
                        else if(cp == typeof(Rigidbody))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<Rigidbody>();
                        }
                        else if(cp == typeof(TextMeshProUGUI))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<TextMeshProUGUI>();
                        }
                        else if(cp == typeof(TMP_InputField))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<TMP_InputField>();
                        }
                        else if(cp == typeof(Toggle))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<Toggle>();
                        }
                        else if(cp == typeof(CanvasGroup))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<CanvasGroup>();
                        }
                        else if(cp == typeof(Canvas))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<Canvas>();
                        }
                        else if(cp == typeof(Selectable))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<Selectable>();
                        }
                        else if(cp == typeof(Animator))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<Animator>();
                        }
                        else if(cp == typeof(Image))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<Image>();
                        }
                        else if(cp == typeof(Graphic))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<Graphic>();
                        }
                        else if(cp == typeof(Slider))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<Slider>();
                        }
                        else if(cp == typeof(Button))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<Button>();
                            
                        }
                        else if(cp == typeof(Camera))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<Camera>();
                            
                        }
                        else if(cp == typeof(AudioSource))
                        {
                            propertyFound.objectReferenceValue = go.GetComponent<AudioSource>();
                            
                        }
                    }



                    propertyFound.objectReferenceValue =
                        EditorGUI.ObjectField(pos, fields[i].Name, propertyFound.objectReferenceValue, fields[i].Type, true );



                }
                else if (fields[i].Field_ID == PathFieldType_ID.Enum_1
                    || fields[i].Field_ID == PathFieldType_ID.Enum_2
                    || fields[i].Field_ID == PathFieldType_ID.Enum_3)
                {

                    vals.FindPropertyRelative(fields[i].Field_ID.ToString()).intValue =
                        EditorGUI.Popup(pos, fields[i].Name, vals.FindPropertyRelative(fields[i].Field_ID.ToString()).intValue, System.Enum.GetNames(fields[i].Type));

                }
                else if (fields[i].Field_ID == PathFieldType_ID.CustomChoices_1
                    || fields[i].Field_ID == PathFieldType_ID.CustomChoices_2
                    || fields[i].Field_ID == PathFieldType_ID.CustomChoices_3)
                {

                    vals.FindPropertyRelative(fields[i].Field_ID.ToString()).intValue =
                        EditorGUI.Popup(pos, fields[i].Name, vals.FindPropertyRelative(fields[i].Field_ID.ToString()).intValue,fields[i].Choices);

                }
                else if(fields[i].Field_ID == PathFieldType_ID.SerializedField)
                {
                    var fieldProp = prop.FindPropertyRelative(fields[i].SerializedFieldName);

                    if(fieldProp == null)
                        continue;

                    EditorGUI.PropertyField(pos, fieldProp, new GUIContent(fields[i].Name),true);
                }
                else
                {
                    EditorGUI.PropertyField(pos, vals.FindPropertyRelative(fields[i].Field_ID.ToString()), new GUIContent(fields[i].Name));
                }

            }
        }


    }

}



