/*
 * Programmer: Keiran Glynn
 * Date Created: 07/23/2018 @ 12:30 AM
 * Last Modified: 07/23/2018 @ 12:30 AM
 * File Name: InputListener.cs
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour {

    


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        // Movement Left
        if(Input.GetKeyDown("a") || Input.GetKeyDown("left"))
        {

        }

        // Movement Right
        if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
        {

        }

        // Jumping
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up") || Input.GetKeyDown("space"))
        {

        }

        // Character Swap
        if (Input.GetKeyDown("g"))
        {

        }
    }
}
