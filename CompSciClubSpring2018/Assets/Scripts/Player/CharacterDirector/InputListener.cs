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

public class InputListener : MonoBehaviour
{
    private PlayerController human;




    // Use this for initialization
    void Start()
    {
        //human = GameObject.Find("Player-Human");
        //yokai = GameObject.Find("");

        human = GameObject.Find("Player-Human").GetComponent<PlayerController>();
    }

    void Update()
    {

        // Movement Left
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            human.moveX = true; // for temp only
            human.TempStrafe(false);
        }
        
        // Movement Right
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            human.moveX = true; // for temp only
            human.TempStrafe(true);
        }
        
        // Jumping
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up") || Input.GetKeyDown("space"))
        {
            human.Jump();
        }



        // Character Swap
        if (Input.GetKeyDown("g"))
        {
            switcherScript.SetFacingRight(facingRight);
            switcherScript.SetProjection();
        }
    }
}
