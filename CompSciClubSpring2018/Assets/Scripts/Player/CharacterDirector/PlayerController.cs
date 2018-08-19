/*
 *  Authors: Kieran Glynn, Darrell Wong
 *  Date Created: 7/26/2018
 *  Last Modified: 8/3/2018
 *  PlayerController.cs
 *  Description: This script is the definition of the players physics. It handles:
 *      1. Movement (Jump and Strafe)
 *      2. Collisions using raycasts
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float gravity = -25f;
    public float horizontalSpeed = 8f;
    public float jumpSpeed = 3f;
    public bool isGrounded = false;
    public bool isOnSlope = false;

    private bool right = false;
    private bool left = false;
    private bool jump = false;


    private float normalizedHorizontalSpeed = 0f;

    private Vector3 velocity;
    
    void Start()
    {

    }

    void Update()
    {
        if (isGrounded)
            velocity.y = 0;

        if (right)
        {
            normalizedHorizontalSpeed = 1;
            //if (transform.localScale.x < 0f)
            //    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            
        }
        else if (left)
        {
            normalizedHorizontalSpeed = -1;
            //if (transform.localScale.x > 0f)
            //    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            
        }
        else
        {
            normalizedHorizontalSpeed = 0;            
        }


        // we can only jump whilst grounded
        if (isGrounded && jump)
        {
            velocity.y = Mathf.Sqrt(2f * jumpSpeed * -gravity);
            
        }


        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        //var smoothedMovementFactor = isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * horizontalSpeed, Time.deltaTime * 20f);

        // apply gravity before moving
        //velocity.y += gravity * Time.deltaTime;

        
        Move(velocity * Time.deltaTime);

        
    }

    #region Movement Calls
    public void CallRight(bool call)
    {
        if (call)
        {
            right = true;
        }
        else
        {
            right = false;
        }
    }

    public void CallLeft(bool call)
    {
        if (call)
        {
            left = true;
        }
        else
        {
            left = false;
        }
    }

    public void CallJump()
    {
        jump = true;
        jump = false;
    }
    #endregion

    public void Move(Vector3 deltaMovement)
    {
        //// save off our current grounded state which we will use for wasGroundedLastFrame and becameGroundedThisFrame
        //collisionState.wasGroundedLastFrame = collisionState.below;

        //// clear our state
        //collisionState.reset();
        isOnSlope = false;

        //RaycastStartPoints();


        //////// first, we check for a slope below us before moving
        //////// only check slopes if we are going down and grounded
        //////if (deltaMovement.y < 0f) // && collisionState.wasGroundedLastFrame
        //////    handleVerticalSlope(ref deltaMovement);

        //////// now we check movement in the horizontal dir
        //////if (deltaMovement.x != 0f)
        //////    moveHorizontally(ref deltaMovement);

        //////// next, check movement in the vertical dir
        //////if (deltaMovement.y != 0f)
        //////    moveVertically(ref deltaMovement);

        // move then update our state
        deltaMovement.z = 0;
        transform.Translate(deltaMovement, Space.World);

        // only calculate velocity if we have a non-zero deltaTime
        if (Time.deltaTime > 0f)
            velocity = deltaMovement / Time.deltaTime;

        //// set our becameGrounded state based on the previous and current collision state
        //if (!collisionState.wasGroundedLastFrame && collisionState.below)
        //    collisionState.becameGroundedThisFrame = true;

        // if we are going up a slope we artificially set a y velocity so we need to zero it out here
        //////if (_isGoingUpSlope)
        //////    velocity.y = 0;

        
    }
}
