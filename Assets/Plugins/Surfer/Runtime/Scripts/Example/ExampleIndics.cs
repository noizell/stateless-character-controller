using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ExampleIndics : MonoBehaviour
{



    private void Awake()
    {
        transform.localPosition = new Vector3(-7,0,0);
        transform.DOLocalMove(new Vector3(7,Random.Range(-5,5),0),5f).SetLoops(-1,LoopType.Yoyo).Play();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
