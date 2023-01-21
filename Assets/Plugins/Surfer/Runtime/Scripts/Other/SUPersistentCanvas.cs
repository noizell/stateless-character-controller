using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUPersistentCanvas : MonoBehaviour
{

    private void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);
    }


}
