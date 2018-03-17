﻿/*
 * Programmer:   Hunter Goodin & Spencer Wilson
 * Date Created: 02/16/2018 @  7:15 PM 
 * Last Updated: 02/16/2018 @  9:35 PM 
 * File Name:    HumanMovement.cs 
 * Description:  This script will be responsible for the player's movements in their human form. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour
{
    public float speed;
    public float jumpSpeed;
    public Rigidbody2D playerRB;
    public bool isJump;
    public bool isLeft;
    public bool isRight;
    public bool touched;

    public bool isActive; // True an false variable that determines whether or not the player's current movement is active.

    private void FixedUpdate()
    {
        CheckIfActive(); // Calls on the CheckIfActive() function.
    }

    private void CheckIfActive() // Private function that detects whether or not this script is active.
    {
        if(isActive) // If isActive is equal to true, then call the function Movement().
        {
            Movement();
        }
    }

    private void Movement() // Function that houses the code that accounts for the human's movement.k
    {
        if (/*touched && isLeft*/ Input.GetKey("a"))
        {
            playerRB.transform.Translate(-transform.right * Time.deltaTime * speed);
        }
        if (/*touched && isRight*/ Input.GetKey("d"))
        {
            playerRB.transform.Translate(transform.right * Time.deltaTime * speed);
        }
        //if (/*touched && isJump*/ Input.GetKey("space"))
        //{
        //    playerRB.transform.Translate(transform.up * Time.deltaTime * jumpSpeed);
        //}
    }

    public void SetIfActive(bool incomingVal) // Function that sets isActive to incomingVal.
    {
        isActive = incomingVal;
    }

    void OnMouseDown()
    {
        touched = true;
    }

    private void OnMouseUp()
    {
        touched = false;
    }
}