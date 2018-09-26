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
using Cinemachine;

public class PlayerController : MonoBehaviour {

    public enum CharacterName {
        Yokai,
        Yumi
    }

    public CharacterName characterName;

    [Header("Physics/ Player Attributes")]
    public float gravity = -25f; // Negative input value
    public float jumpSpeed = 3f;
    public float horizontalSpeed = 8f; // Movement speed along horizontal axis
    public float maxClimbableSlope = 50f; //in degrees
    public float skinWidth; // Acts as an inset start point on the collider for the Ray orgin points

    [Header("Collision Detection")]
    [Range( 2, 12)]
    public int horizontalRays = 8;
    [Range(2, 10)]
    public int verticalRays = 4;
    public float error = .01f;
    public LayerMask platformMask;

    #region Hit Detection Flags    
    private bool climbableSlope;
    private bool isGrounded = false;
    private bool isOnSlope = false;
    private bool isRight = false;
    #endregion

    #region Movement Call Flags
    private bool right = false;
    private bool left = false;
    private bool jump = false;
    #endregion

    #region Box Collider Bounds
    private BoxCollider boxCollider;
    private Vector3 TL; // Top Left corner of the box collider
    private Vector3 TR; // Top Right corner of the box collider
    private Vector3 BL; // Bottom Left corner of the box collider
    private Vector3 BR; // Bottom Right corner of the box collider

    private void RaycastStartPoints()
    {
        var skinBounds = boxCollider.bounds;
        skinBounds.Expand(-2f * skinWidth);

        TL = new Vector3(skinBounds.min.x, skinBounds.max.y, 0f);
        TR = new Vector3(skinBounds.max.x, skinBounds.max.y, 0f);
        BL = new Vector3(skinBounds.min.x, skinBounds.min.y, 0f);
        BR = new Vector3(skinBounds.max.x, skinBounds.min.y, 0f);
    }
    #endregion

    private float normalizedHorizontalSpeed = 0f;
    private float verticalRaySeparation;
    private float horizontalRaySeparation;

    private Vector3 velocity;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Transform yumi;
    private CinemachineVirtualCamera main_follow_camera;

    private void Awake()
    {
        boxCollider = this.GetComponent<BoxCollider>();
    }

    private void Start()
    {
        main_follow_camera = GameObject.Find("Main Follow Camera").GetComponent<CinemachineVirtualCamera>();
        yumi = GameObject.Find("Yumi").transform;

        animator = this.gameObject.GetComponentInChildren<Animator>();
        spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        if (characterName == CharacterName.Yumi) {
            
        }
        if (characterName == CharacterName.Yokai) {
            main_follow_camera.Follow = this.gameObject.transform;
        }
    }

    private void OnDestroy()
    {
        if (characterName == CharacterName.Yokai) {
            CinemachineVirtualCamera main_follow_camera = GameObject.Find("Main Follow Camera").GetComponent<CinemachineVirtualCamera>();
            Transform yumi = GameObject.Find("Yumi").transform;
            main_follow_camera.Follow = yumi;
        }
    }

    void Update()
    {

        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        /* If the player is grounded, set vertical velocity to zero. This stops the player from accelerating downward */
        if (isGrounded)
        {
            velocity.y = 0;
        }

        
        /* The following selection decides the directioin of horizontal movement (right, left, none) */
        if (right)
        {
            if (!currentStateInfo.IsName("yokai_run")) {
                spriteRenderer.flipX = false;
                animator.Play("yokai_run");
            }
            normalizedHorizontalSpeed = 1;
        }
        else if (left)
        {
            if (!currentStateInfo.IsName("yokai_run"))
            {
                spriteRenderer.flipX = true;
                animator.Play("yokai_run");
            }
            normalizedHorizontalSpeed = -1;            
        }
        else
        {
            normalizedHorizontalSpeed = 0;            
        }
        
        /* This selection will make the player jump given it is touching the ground platform and spacebar is pressed*/
        if (isGrounded && jump)
        {
            jump = false;
            isGrounded = false;            
            velocity.y = Mathf.Sqrt(2f * jumpSpeed * -gravity);            
        }

        if (characterName == CharacterName.Yokai) {
            if (!right && !left && !jump)
            {
                animator.Play("yokai_idle");
            }
            if (jump) {
                Debug.Log(jump);
            }
        }
        if (characterName == CharacterName.Yumi) {
            animator.Play("");
        }

        //Horizontal velocity
        velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * horizontalSpeed, Time.deltaTime * 20f);

        // Gravity (Vertical component of Velocity if not jumping)
        velocity.y += gravity * Time.deltaTime;


        /*The velocities (speed and directions that we would like to move in) are multiplied by Time to turn them into a position that
         * we would like to move towards. The Move function then passes then modifies these positions based on if the player is colldiding
         * with the world. After the movement positions are modified to prevent passing through walls and floors, the player is moved using 
         * transform.translate. */
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
        if (isGrounded)
        {            
            jump = true;            
        }
    }

    public void ClearCalls()
    {
        right = left = false;
    }
    #endregion

    public void Move(Vector3 deltaMovement)
    {
        /* Reset of collsion flags for detecting the ground */
        isGrounded = false;
        isOnSlope = false;

        /* Recalculates the corners of our collider box so that the detection rays can be casted accurately */
        RaycastStartPoints();


        
        if (deltaMovement.y < 0f)
        {
            deltaMovement = VerticalSlopeDetection(ref deltaMovement); 
        }
            

        /* Check for collisions to the left and right, and modify the position we want to move towards if we aren't supposed to 
         * go there. */
        if (deltaMovement.x != 0f)
        {
            deltaMovement = HorizontalCollision(deltaMovement);
        }


        /* Check for collisions with the ceilings and floors, and modify the position we want to move towards if we aren't supposed to 
         * go there. */
        if (deltaMovement.y != 0f)
        {
            deltaMovement = VerticalCollision(deltaMovement);            
        }
            

        /* Given our 2.5D game, we should always be zero in the z-axis. Once all of the movement positions have been modified based on 
         * the players collisions, use transform.Translate to move the player towards those positions */
        deltaMovement.z = 0;
        transform.Translate(deltaMovement, Space.World);

        // only calculate velocity if we have a non-zero deltaTime
        if (Time.deltaTime > 0f)
            velocity = deltaMovement / Time.deltaTime;

        
        // to be used more in slope detection when its fixed
        if (isOnSlope)
            velocity.y = 0;
    }


    /*Rays are casted from the bottom of the box collider to the top using a for-loop. The direction they are casted in is decided based on
     * the current movement direction. If a collision is detected, it will subtract from the target position as to prevent the player from 
     * moving past the location in which the ray struck the wall. This new modified target position is returned to the original movement 
     * function. */
    private Vector3 HorizontalCollision(Vector3 deltaMovement)
    {
        isRight = deltaMovement.x > 0f;
        float rayLength = Mathf.Abs(deltaMovement.x) + skinWidth;
        Vector3 rayDirection = isRight ? Vector3.right : Vector3.left;
        Vector3 firstRayStartPoint = isRight ? BR : BL;

        float colliderUseableHeight = boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2f * skinWidth);
        verticalRaySeparation = colliderUseableHeight / (horizontalRays - 1);

        for (int i = 0; i < horizontalRays; i++)
        {
            climbableSlope = false;
            Vector3 ray = new Vector3(firstRayStartPoint.x, firstRayStartPoint.y + i * verticalRaySeparation, 0f);
            RaycastHit hit;

            Debug.DrawRay(ray, rayDirection * rayLength, Color.magenta);

            bool raycastHit = Physics.Raycast(ray, rayDirection, out hit, rayLength, platformMask);

            // can only hit slope with the bottom horizontal raycast && isRight && slope is <70
            if (isRight && (Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90f < maxClimbableSlope))
            {
                //print("Slope on right: " + (Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg ));
                climbableSlope = true;
            }

            if (!isRight && (Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg < maxClimbableSlope))
            {
                //print("Slope on left: " + Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg);
                climbableSlope = true;
            }

            if (raycastHit && !climbableSlope)
            {
                // something something Darrell plz make slopes work ... ok

                deltaMovement.x = hit.point.x - ray.x;
                rayLength = Mathf.Abs(deltaMovement.x);

                if (isRight && !climbableSlope)
                {
                    deltaMovement.x -= skinWidth;
                }
                else if (!isRight && !climbableSlope)
                {
                    deltaMovement.x += skinWidth;
                }

                if (rayLength < skinWidth + error)
                {
                    break;
                }
            }
        }

        return deltaMovement;
    }

    /*Rays are casted from the left of the box collider to the right using a for-loop. The direction they are casted in is decided based on
     * the current movement direction. If a collision is detected, it will subtract from the target position as to prevent the player from 
     * moving past the location in which the ray struck the floor/ ceiling. This new modified target position is returned to the original 
     * movement function. */
    private Vector3 VerticalCollision(Vector3 deltaMovement)
    {
        bool isUp = deltaMovement.y > 0f;
        isRight = deltaMovement.x > 0f;
        float rayLength = Mathf.Abs(deltaMovement.y) + skinWidth;
        Vector3 rayDirection = isUp ? Vector3.up : -Vector3.up;
        Vector3 firstRayStartPoint = isUp ? TL : BL;
        firstRayStartPoint.x += deltaMovement.x;
        

        float colliderUseableWidth = boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2f *skinWidth);
        horizontalRaySeparation = colliderUseableWidth / (verticalRays - 1);
        if (isRight)
        {
            print("moving Right");

            for (int i = verticalRays; i >= 0; i--)
            {
                Vector3 ray = new Vector3(firstRayStartPoint.x + i * horizontalRaySeparation, firstRayStartPoint.y, 0f);
                RaycastHit hit;

                Debug.DrawRay(ray, rayDirection * rayLength, Color.magenta);

                bool raycastHit = Physics.Raycast(ray, rayDirection, out hit, rayLength, platformMask);

                if (raycastHit)
                {
                    deltaMovement.y = hit.point.y - ray.y;
                    rayLength = Mathf.Abs(deltaMovement.y);

                    if (isUp)
                    {
                        deltaMovement.y -= skinWidth;
                    }
                    else
                    {
                        deltaMovement.y += skinWidth;
                        isGrounded = true;
                    }

                    if (!isUp && deltaMovement.y > 0.00001f)
                    {
                        isOnSlope = true;
                    }

                    if (rayLength < skinWidth + error)
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            print("moving left");

            for (int i = 0; i < verticalRays; i++)
            {
                Vector3 ray = new Vector3(firstRayStartPoint.x + i * horizontalRaySeparation, firstRayStartPoint.y, 0f);
                RaycastHit hit;

                Debug.DrawRay(ray, rayDirection * rayLength, Color.magenta);

                bool raycastHit = Physics.Raycast(ray, rayDirection, out hit, rayLength, platformMask);

                if (raycastHit)
                {
                    deltaMovement.y = hit.point.y - ray.y;
                    rayLength = Mathf.Abs(deltaMovement.y);

                    if (isUp)
                    {
                        deltaMovement.y -= skinWidth;
                    }
                    else
                    {
                        deltaMovement.y += skinWidth;
                        isGrounded = true;
                    }

                    if (!isUp && deltaMovement.y > 0.00001f)
                    {
                        isOnSlope = true;
                    }

                    if (rayLength < skinWidth + error)
                    {
                        break;
                    }
                }
            }
        }
        

        return deltaMovement;
    }

    private Vector3 VerticalSlopeDetection(ref Vector3 deltaMovement)
    {
        // slope check from the center of our collider
        float centerOfCollider = (BL.x + BR.x) * 0.5f;
        Vector3 rayDirection = Vector3.down;

        // the ray distance is based on our slopeLimit
        float slopeLimitTangent = Mathf.Tan(maxClimbableSlope * Mathf.Deg2Rad);
        float slopeCheckRayDistance = slopeLimitTangent * (BR.x - centerOfCollider);

        var slopeRay = new Vector3(centerOfCollider, BL.y);
        Debug.DrawRay(slopeRay, rayDirection * slopeCheckRayDistance, Color.yellow);
        RaycastHit hit;
        bool raycastHit = Physics.Raycast(slopeRay, rayDirection, out hit, slopeCheckRayDistance, platformMask);        
        if (raycastHit)
        {
            // bail out if we have no slope
            var angle = Vector3.Angle(hit.normal, Vector2.up);
            if (angle == 0)
                return Vector3.zero;

            // we are moving down the slope if our normal and movement direction are in the same x direction
            var isMovingDownSlope = Mathf.Sign(hit.normal.x) == Mathf.Sign(deltaMovement.x);
            if (isMovingDownSlope)
            {                
                // we add the extra downward movement here to ensure we "stick" to the surface below
                deltaMovement.y += hit.point.y - slopeRay.y - skinWidth;                
                //collisionState.movingDownSlope = true;
                //collisionState.slopeAngle = angle;
            }
        }
        return deltaMovement;
    }

    //bool handleHorizontalSlope(ref Vector3 deltaMovement, float angle)
    //{
    //    // disregard 90 degree angles (walls)
    //    if (Mathf.RoundToInt(angle) == 90)
    //        return false;

    //    // if we can walk on slopes and our angle is small enough we need to move up
    //    if (angle < slopeLimit)
    //    {
    //        // we only need to adjust the deltaMovement if we are not jumping
    //        // TODO: this uses a magic number which isn't ideal! The alternative is to have the user pass in if there is a jump this frame
    //        if (jump  & isGrounded)
    //        {
    //            // apply the slopeModifier to slow our movement up the slope
    //            var slopeModifier = slopeSpeedMultiplier.Evaluate(angle);
    //            deltaMovement.x *= slopeModifier;

    //            // we dont set collisions on the sides for this since a slope is not technically a side collision.
    //            // smooth y movement when we climb. we make the y movement equivalent to the actual y location that corresponds
    //            // to our new x location using our good friend Pythagoras
    //            deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * deltaMovement.x);
    //            var isGoingRight = deltaMovement.x > 0;

    //            // safety check. we fire a ray in the direction of movement just in case the diagonal we calculated above ends up
    //            // going through a wall. if the ray hits, we back off the horizontal movement to stay in bounds.
    //            var ray = isGoingRight ? _raycastOrigins.bottomRight : _raycastOrigins.bottomLeft;
    //            RaycastHit2D raycastHit;
    //            if (collisionState.wasGroundedLastFrame)
    //                raycastHit = Physics2D.Raycast(ray, deltaMovement.normalized, deltaMovement.magnitude, platformMask);
    //            else
    //                raycastHit = Physics2D.Raycast(ray, deltaMovement.normalized, deltaMovement.magnitude, platformMask & ~oneWayPlatformMask);

    //            if (raycastHit)
    //            {
    //                // we crossed an edge when using Pythagoras calculation, so we set the actual delta movement to the ray hit location
    //                deltaMovement = (Vector3)raycastHit.point - ray;
    //                if (isGoingRight)
    //                    deltaMovement.x -= _skinWidth;
    //                else
    //                    deltaMovement.x += _skinWidth;
    //            }

    //            _isGoingUpSlope = true;
    //            collisionState.below = true;
    //        }
    //    }
    //    else // too steep. get out of here
    //    {
    //        deltaMovement.x = 0;
    //    }

    //    return true;
    //}
}
