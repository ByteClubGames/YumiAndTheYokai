/*
 * 
 * Authors: Spencer Wilson, Keiran Glynn
 * Date Created: 3/5/2018 @ 3:15 pm
 * Date Modified: 3/8/2018 @ 7:15 pm
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
}
