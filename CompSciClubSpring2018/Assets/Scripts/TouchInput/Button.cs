using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Byte Club 
 * 2/26/18
 * Author: Rory Glenn
 * TouchInput 
 */

public class Button : MonoBehaviour
{
    public Color defaultColor;
    public Color selectedColor;
    private Material mat;
    private Renderer rend;

    void Start()
    {
        mat = GetComponent<Renderer>().material;

        // mat = renderer.material;  Obsolete method,
        // so I tried to substitute with line below
        // mat = rend.material;
    }


    void OnTouchDown()
    {
        mat.color = selectedColor;
    }

    void OnTouchUp()
    {
        mat.color = defaultColor;
    }

    void OnTouchStay()
    {
        mat.color = selectedColor;
    }

    void OnTouchExit()
    {
        mat.color = defaultColor;
    }
}
