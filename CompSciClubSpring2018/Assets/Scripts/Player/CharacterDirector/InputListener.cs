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
    private HumanMovement human;
    
    


	// Use this for initialization
	void Start ()
    {
        //human = GameObject.Find("Player-Human");
        //yokai = GameObject.Find("");

        human = GameObject.Find("Player-Human").GetComponent<HumanMovement>();
	}
	
	void Update () {
        
        // Movement Left
        if(Input.GetKey("a") || Input.GetKey("left"))
        {
            human.MoveLeft(true);
        }
        else
        {
            human.MoveLeft(false);
        }

        // Movement Right
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            human.MoveRight(true);
        }
        else
        {
            human.MoveRight(false);
        }

        // Jumping
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up") || Input.GetKeyDown("space"))
        {
            human.Jump(true);
        }
        else
        {
            human.Jump(false);
        }



        // Character Swap
        if (Input.GetKeyDown("g"))
        {
            // soon to be implemented
        }
    }
}
