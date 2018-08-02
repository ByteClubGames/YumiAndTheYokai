/*
 * Programmer: Keiran Glynn, Spencer Wilson
 * Date Created: 07/23/2018 @ 12:30 AM
 * Last Modified: 07/31/2018 @ 3:17 PM
 * File Name: InputListener.cs
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour {

    private HumanMovement human; // HumanMovement object named human that will hold the HumanMovement script.
    // public GameObject yokaiGameObj;
    public YokaiSwitcher switcherScript; // YokaiSwitcher object named switcherScript that will store the YokaiSwitcher script.
    private bool facingRight; // Boolean value that represents whether or not the player is facing right.
    


	// Use this for initialization
	void Start ()
    {
        //human = GameObject.Find("Player-Human");
        //yokai = GameObject.Find("");

        human = GameObject.Find("Player-Human").GetComponent<HumanMovement>();
        facingRight = true; // Player is initialized to face right.
	}
	
	void Update () {
        
        // Movement Left
        if(Input.GetKey("a") || Input.GetKey("left"))
        {
            human.MoveLeft(true);
            facingRight = false; // Boolean variable facingRight is set to false.
        }
        else
        {
            human.MoveLeft(false);
        }

        // Movement Right
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            human.MoveRight(true);
            facingRight = true; // Boolean variable facingRight is set to true.
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
            switcherScript.SetFacingRight(facingRight);
            switcherScript.SetProjection();
        }
    }
}
