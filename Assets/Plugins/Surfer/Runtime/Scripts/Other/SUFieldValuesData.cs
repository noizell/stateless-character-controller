using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Surfer
{

    [System.Serializable]
    public class SUFieldValuesData 
    {

        [SerializeField]
        public bool Bool_1, Bool_2, Bool_3 = default;
        [SerializeField]
        public float Float_1, Float_2, Float_3 = default;
        [SerializeField]
        public int Int_1, Int_2, Int_3 = default;
        [SerializeField]
        public string String_1, String_2, String_3 = default;
        [SerializeField]
        public Color Color_1, Color_2, Color_3 = default;
        [SerializeField]
        public Object Object_1, Object_2, Object_3, Object_4, Object_5 = default;
        [SerializeField]
        public int Enum_1, Enum_2, Enum_3 = default;
        [SerializeField]
        public int CustomChoices_1, CustomChoices_2, CustomChoices_3 = default;
        [SerializeField]
        public Vector3 Vector3_1, Vector3_2, Vector3_3 = default;
        [SerializeField]
        public Vector2 Vector2_1, Vector2_2, Vector2_3 = default;



    }


}

