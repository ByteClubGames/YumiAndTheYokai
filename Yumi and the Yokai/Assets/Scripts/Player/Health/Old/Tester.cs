using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("k"))
        {
            GameObject.Find("Yumi").GetComponent<YumiHealthSystem>().DamageDealer(1);
        }
    }
}
