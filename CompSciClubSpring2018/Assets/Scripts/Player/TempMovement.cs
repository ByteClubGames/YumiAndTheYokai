/*
 * Programmer:   Keiran Glynn
 * Date Created: 07/20/2018 @  12:30 AM 
 * Last Edited: 07/20/2018 @  12:30 AM 
 * File Name:    HumanHealth.cs 
 * Description:  Temporary movement script for the temporary player game object. The current player game object has 3D colliders on it, and they are incompatible with the 2D based demo level. This is just a quick fix so 
 * that design team can do their job. A better fix is on the way.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMovement : MonoBehaviour
{
    private GameObject player;

    public float speed = 10; // Speed of horizontal movement
    public float jumpSpeed = 5; // Jump Height

    private void Start()
    {
        player = GameObject.Find("Player_Temp2D"); // Picks out the game object we want to control
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            player.transform.Translate(-transform.right * Time.deltaTime * speed); // Horizontal movement left
        }

        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            player.transform.Translate(transform.right * Time.deltaTime * speed); // Horizontal movement right
        }

        if (Input.GetKey("w") || Input.GetKey("up") || Input.GetKey("space") && player.GetComponent<Rigidbody2D>().velocity.y == 0f)
        {
            player.GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse); // Jumping
        }
    }
}
    