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
    public float skinWidth;
    public int horizontalRays = 8;
    public int verticalRays = 4;
    public LayerMask platformMask;
    public float error = .01f;
    public bool isGrounded = false;
    public bool isOnSlope = false;
    public bool isRight = false;
    public bool isLeft = false;

    private bool right = false;
    private bool left = false;
    private bool jump = false;

    private Vector3 TL; // Top Left corner of the box collider
    private Vector3 TR; // Top Right corner of the box collider
    private Vector3 BL; // Bottom Left corner of the box collider
    private Vector3 BR; // Bottom Right corner of the box collider
    private BoxCollider boxCollider;


    private float normalizedHorizontalSpeed = 0f;
    private float verticalRaySeparation;
    private float horizontalRaySeparation;

    private Vector3 velocity;

    private void Awake()
    {
        boxCollider = this.GetComponentInParent<BoxCollider>();
    }

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

        RaycastStartPoints();


        //////// first, we check for a slope below us before moving
        //////// only check slopes if we are going down and grounded
        //////if (deltaMovement.y < 0f) // && collisionState.wasGroundedLastFrame
        //////    handleVerticalSlope(ref deltaMovement);

        // now we check movement in the horizontal dir
        if (deltaMovement.x != 0f)
        {
            deltaMovement.x = HorizontalCollision(deltaMovement);
        }
            

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

    public void RaycastStartPoints()
    {
        var skinBounds = boxCollider.bounds;
        skinBounds.Expand(-2f * skinWidth);

        TL = new Vector3(skinBounds.min.x, skinBounds.max.y, 0f);
        TR = new Vector3(skinBounds.max.x, skinBounds.max.y, 0f);
        BL = new Vector3(skinBounds.min.x, skinBounds.min.y, 0f);
        BR = new Vector3(skinBounds.max.x, skinBounds.min.y, 0f);
    }

    private float HorizontalCollision(Vector3 deltaMovement)
    {
        isRight = deltaMovement.x > 0f;
        //isLeft = deltaMovement.x < 0f;
        float rayLength = Mathf.Abs(deltaMovement.x) + skinWidth;
        Vector3 rayDirection = isRight ? Vector3.right : Vector3.left;
        Vector3 firstRayStartPoint = isRight ? BR : BL;

        float colliderUseableHeight = boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2f * skinWidth);
        verticalRaySeparation = colliderUseableHeight / (horizontalRays - 1);

        for (int i = 0; i < horizontalRays; i++)
        {
            Vector3 ray = new Vector3(firstRayStartPoint.x, firstRayStartPoint.y + i * verticalRaySeparation, 0f);
            RaycastHit hit;

            Debug.DrawRay(ray, rayDirection * rayLength, Color.magenta);

            bool raycastHit = Physics.Raycast(ray, rayDirection, out hit, rayLength, platformMask);

            if (raycastHit)
            {
                // something something Darrell plz make slopes work

                deltaMovement.x = hit.point.x - ray.x;
                rayLength = Mathf.Abs(deltaMovement.x);
                //Debug.Log(deltaMovement.x);

                if (isRight)
                {
                    deltaMovement.x -= skinWidth;
                }
                else
                {
                    deltaMovement.x += skinWidth;
                }

                if (rayLength < skinWidth + error)
                {
                    break;
                }
                Debug.Log("Hmmmmmmmmmmmmmmmmm");
                //Debug.Log(hit.point.x + " " + hit.point.y);
            }
        }
        return deltaMovement.x;
    }
}
