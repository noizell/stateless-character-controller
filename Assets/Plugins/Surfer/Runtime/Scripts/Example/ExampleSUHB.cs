using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Surfer;

public class ExampleSUHB : MonoBehaviour
{

    SUHealthBarLink _hbLink = default;


    private void Awake()
    {
        _hbLink = GetComponent<SUHealthBarLink>();

        StartCoroutine(TestHB());
    }



    IEnumerator TestHB()
    {

        yield return new WaitForSeconds(1.0f);

        _hbLink?.Data.DamageHp(50);

        yield return new WaitForSeconds(1.5f);

        _hbLink?.Data.AddHp(100);

        yield return new WaitForSeconds(1.5f);

        _hbLink?.Data.RemoveHp(50);

        yield return new WaitForSeconds(1.5f);

        _hbLink?.Data.DamageHp(50);


    }

}
