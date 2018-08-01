using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour {
    public GameObject Player;
    public GameObject Platform;
    public Transform platform; // variable set to change it's position
    public Transform position1; // variable set to change it's position
    public Transform position2; // variable set to change it's position
    public Vector3 newPosition; // variable set to change it's position
    public string currentState; // Helps with informing users(Programmer) of the position the platform is transforming to
    public float smooth; // Sets the speed in which the platform will transform to its next position
    public float resetTime; // Sets the time interval in which the platform will move back to its origin

    private void OnTriggerStay(Collider2D col)
    {
        platform.position = Vector3.Lerp(platform.position, newPosition, smooth * Time.deltaTime);
    }
}
