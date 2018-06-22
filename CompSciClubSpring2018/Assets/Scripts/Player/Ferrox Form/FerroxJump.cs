/*
 * 
 * Authors: Spencer Wilson, Keiran Glynn, Andrew Ramirez
 * Date Created: 3/5/2018 @ 3:15 pm
 * Date Modified: 4/7/2018 @ 10:20am
 * Project: CompSciClubSpring2018
 * File: FerroxJump.cs
 * Description: This class houses the code for the jump mechanics of the ferrox.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerroxJump : MonoBehaviour {

    public Rigidbody2D ferroxRB;
    public float jumpSpeed;
    public bool isGrounded;

    void Start()
    {
        ferroxRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Jump();
    }

    private void Jump()
    {
        if (Input.GetKey("space") && (ferroxRB.velocity.y == 0f))
        {
            ferroxRB.AddForce(Vector2.up * Time.deltaTime * jumpSpeed);

        }
    }

    public void SetIsGrounded(bool value) // Allows the player to set isGrounded.
    {
        isGrounded = value;
    }
}
