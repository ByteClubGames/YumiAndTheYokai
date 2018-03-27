/*
 * Programmer:   Hunter Goodin 
 * Date Created: 02/16/2018 @  4:00 PM 
 * Last Updated: 03/22/2018 @  6:45 PM 
 * File Name:    PlayerMovement.cs 
 * Description:  This script will be responsible for the player's movements. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;             // How fast the player character moves 
    public Rigidbody playerRB;      // This will be populated with the player character's rigidbody in-engine 
    public bool isLeft;             // This will be toggled in-engine if the object this script is attached to is the left button 
    public bool isRight;            // This will be toggled in-engine if the object this script is attached to is the right button 
    private bool touched;           // This will be true if the object this script is attached to is being touched by the player's finger 

    private void FixedUpdate()
    {
        if (touched && isLeft)      // If touched is true and isLeft is true 
        { playerRB.transform.Translate(-transform.right * Time.deltaTime * speed); }   // move the player left   

        if (touched && isRight)     // If touched is true and isRight is true 
        { playerRB.transform.Translate(transform.right * Time.deltaTime * speed); }    // move the player right 
    }

    void Move()             // This function will be accessed by the TouchInput.cs script 
    { touched = true; }     // Make touched true 

    void StopMoving()       // This function will be accessed by the TouchInput.cs script 
    { touched = false; }    // make touch false 
}
