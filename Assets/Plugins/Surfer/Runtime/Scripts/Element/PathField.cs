using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    public enum PathFieldType_ID
    {
        None,

        String_1,
        String_2,
        String_3,

        Int_1,
        Int_2,
        Int_3,

        Bool_1,
        Bool_2,
        Bool_3,

        Float_1,
        Float_2,
        Float_3,

        Color_1,
        Color_2,
        Color_3,

        Object_1,
        Object_2,
        Object_3,

        Enum_1,
        Enum_2,
        Enum_3,

        Object_4,
        Object_5,

        SerializedField,
        CustomChoices_1,
        CustomChoices_2,
        CustomChoices_3,
        Vector3_1,
        Vector3_2,
        Vector3_3,
        Vector2_1,
        Vector2_2,
        Vector2_3,
    }

    public struct PathField
    {

        public PathFieldType_ID Field_ID { get; private set; }
        public string Name { get; private set; }
        public string SerializedFieldName { get; private set; }

        public string[] Choices { get; private set; }
        public System.Type Type { get; private set; }

        public PathField(string name,PathFieldType_ID fieldID)
        {
            Name = name;
            Field_ID = fieldID;
            Type = null;
            SerializedFieldName = null;
            Choices = null;
        }

        public PathField(string name, PathFieldType_ID fieldID, System.Type objType)
        {
            Name = name;
            Field_ID = fieldID;
            Type = objType;
            SerializedFieldName = null;
            Choices = null;
        }

        public PathField(string name,string serializedFieldName)
        {
            Name = name;
            Field_ID = PathFieldType_ID.SerializedField;
            Type = null;
            SerializedFieldName = serializedFieldName;
            Choices = null;
        }

        public PathField(string name,PathFieldType_ID fieldID,string[] choices)
        {
            Name = name;
            Field_ID = fieldID;
            Type = null;
            SerializedFieldName = null;
            Choices = choices;

        }

    }

}


