using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleSwitcher : MonoBehaviour
{
    public float time = 1.0f; 

	void Start ()
    {
        Time.timeScale = time;
    }

    void Update()
    {
        Time.timeScale = time; 
    }
}
