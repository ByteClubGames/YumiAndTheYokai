/* 
 * Programmer: Darrell Wong
 * Creation Date: 6/2/2018
 * Last Updated: 6/2/2018
 * ToggleOuterLayer.cs
 * Purpose: Fade out and in outter layer objects and art when the player moves behind them.
 *      1) A 2D collision zone and the outer objects must be assigned to this script
 *      2) Art textures should be assigned to the Albedo of the shader textures
 */

//Implimentations notes: The script must be assigned to a 2D collision box. The collision box and the outer layer need to be attached to the script.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOuterLayer : MonoBehaviour {
    public GameObject OuterLayer;
    public GameObject CollisionZone;
    public float fadeSpeed = .05f;

    private float transparency = 1f;

    Renderer LayerRenderer;

    private void Start()
    {
        LayerRenderer = OuterLayer.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StopCoroutine("FadeIn");
            StartCoroutine("FadeOut");
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StopCoroutine("FadeOut");
            StartCoroutine("FadeIn");
        }
    }

    IEnumerator FadeOut()
    {
        for (float f = transparency; f >= 0; f -= .2f)
        {
            print("I don't feel so good");
            Color c = LayerRenderer.material.color;
            c.a = f;
            print(c.a);
            LayerRenderer.material.color = c;
            transparency = f;
            yield return new WaitForSeconds(fadeSpeed);
        }
        LayerRenderer.enabled = false;
    }

    IEnumerator FadeIn()
    {
        // if (transparency == 0f)
        LayerRenderer.enabled = true;
        Color c = LayerRenderer.material.color;
        for (float f = transparency; f <= 1f; f += .2f)
        {
            c.a = f;
            print(c.a);
            LayerRenderer.material.color = c;
            transparency = f;
            yield return new WaitForSeconds(fadeSpeed);
        }
        transparency = 1f;
        c.a = transparency;
        LayerRenderer.material.color = c;
    }
}
