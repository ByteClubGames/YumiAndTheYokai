/*
 * Programmer:   Hunter Goodin 
 * Date Created: 06/06/2018 @  11:55 PM 
 * Last Updated: 06/07/2018 @  12:30 AM 
 * File Name:    MeshRendererToggle.cs 
 * Description:  This script will be responsible for toggling the mesh renderer of the environment art so we don't have to 
 *               see the white boxes everywhere, rather we can toggle them off so we can properly view the game. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererToggle : MonoBehaviour
{
    public bool visable;
    public Renderer[] rendArr = new Renderer[0];

	void Start ()
    {
        rendArr = GetComponentsInChildren<Renderer>();
        ArrLoop(); 
    }

    private void Update()
    {
        ArrLoop();
    }

    void ArrLoop()
    {
        if (visable)
        {
            for (int i = 0; i < rendArr.Length; i++)
            {
                rendArr[i].enabled = true;
            }
        }
        if (!visable)
        {
            for (int i = 0; i < rendArr.Length; i++)
            {
                rendArr[i].enabled = false;
            }
        }
    }
}
