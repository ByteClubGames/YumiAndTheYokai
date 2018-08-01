/*
 * 
 * Programmer: Brenden Plong
 * Date Created: 7/25/2018
 * Date Updated: 7/25/2018
 * Description: Script will make it so that a platform can be activated when the player collides or interacts with it in some way
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformToggle : MonoBehaviour {
    public GameObject Player;
    public Transform platform; // variable set to change it's position
    public Transform position1; // variable set to change it's position
    public Transform position2; // variable set to change it's position
    public Vector3 newPosition; // variable set to change it's position
    public string currentState; // Helps with informing users(Programmer) of the position the platform is transforming to
    public float smooth; // Sets the speed in which the platform will transform to its next position
    public float resetTime; // Sets the time interval in which the platform will move back to its origin
    
	// Use this for initialization
	void Start () {
        OnCollisionStay2D();//ChangeTarget();// Calls upon the ChangeTarget() method
	}

    // Update is called once per frame
    void FixedUpdate () {
         // This allows for the platform to move
	}
    void ChangeTarget() // Will monitor the currentState of the platform and update it accordingly 
    {
        if(currentState == "Moving to position 1") // Checks if the currrentState is moving to position 1
        {
            currentState = "Moving to position 2"; // if true the currentPosition will change its position to position 2
            newPosition = position2.position;
        }
        else if (currentState == "Moving to position 2") // Checks if the currentState is moving to position 2
        {
            currentState = "Moving to position 1"; // if true the currentPositon will change its position to position 1
            newPosition = position1.position;
        }
        else if (currentState == "") // Checks if the currentPosition is currently at nothing
        {
            currentState = "Moving to position 2"; // if true the position will head to position 2 by default and wraps back into the other if else "loop"
            newPosition = position2.position;
        }
        Invoke("ChangeTarget", resetTime); // When a position has been reached, this sets the time in which the platform will move again
    }
    private void OnCollisionStay2D()
    { 
            platform.position = Vector3.Lerp(platform.position, newPosition, smooth * Time.deltaTime);
            ChangeTarget();    
    }
}

