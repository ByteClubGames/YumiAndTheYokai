/*
 * 
 * Programmer: Brenden Plong
 * Date Created: 8/10/2018
 * Date Updated: 8/10/2018
 * Description: Script will make it so that a platform will lower the platform when it is collided by the player
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTriggerDrop : MonoBehaviour {
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
    private void OnCollisionStay2D()
    {
        ChangeTarget();
        platform.position = Vector3.Lerp(platform.position, newPosition, smooth * Time.deltaTime);
    }
    void ChangeTarget() // Gives the newPosition vector its target position
    {
            newPosition = position1.position; // Will set the platform back to its first position
    }
}
