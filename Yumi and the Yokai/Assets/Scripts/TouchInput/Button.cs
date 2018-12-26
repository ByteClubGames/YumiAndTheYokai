using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Byte Club 
 * Date Created: 2/26/18
 * Last Edited: 7/15/18
 * Author: Rory Glenn
 * TouchInput 
 */

public class ButtonRory : MonoBehaviour
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
