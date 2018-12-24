using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSaveHealth : MonoBehaviour
{
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Yumi")
        {
            GameObject.Find("Yumi").GetComponent<YumiHealthSystem>().HealthWriter();
        }
    }
}