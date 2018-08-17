/*
 * 
 * Programmer: Brenden Plong
 * Date Created: 7/25/2018
 * Date Updated: 8/17/2018
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
    public GameObject PlatformTrigger;
	// Use this for initialization
	void Start () {
        //OnCollisionStay2D(); // Will call upon the OnCollisionStay method // Note: Seems this method may not be needed as OnCollision Will be "called" when the player obj collides with the trigger
        //ChangeTarget(); // Moved ChangeTarget to be called in OnCollisionStay method; may keep here for future modification
	}

    // Update is called once per frame
    void FixedUpdate () {
        // Empty until further changes 
	}
    void ChangeTargetUp() // Will monitor the currentState of the platform and update it accordingly 
    {
        newPosition = position2.position; // Updates the position of the platform to the 2nd position
    }
    private void OnCollisionStay2D()
    {       
        ChangeTargetUp(); // Calls upon the ChangeTarget() method
        platform.position = Vector3.Lerp(platform.position, newPosition, smooth * Time.deltaTime);// This allows for the platform to move
    }
}

