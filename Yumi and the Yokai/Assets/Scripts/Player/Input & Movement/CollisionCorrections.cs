/*
********************************************************************************
*Creator(s)...........................................Kieran Glynn, Darrell Wong
*Created..............................................................12/14/2018
*Last Modified............................................@ 7:34PM on 12/17/2018
*Last Modified by...................................................Darrell Wong
*
*Description:   This script houses static methods that are called on by the
*               PlayerController.cs. These methods check for collisions with
*               platforms, and modify the movement of the player as to not
*               allow it to pass through walls and floors.
********************************************************************************
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCorrections : MonoBehaviour {

    private Transform transform;
    private BoxCollider boxCollider;
    private Vector3 TL; // Top Left corner of the box collider
    private Vector3 TR; // Top Right corner of the box collider
    private Vector3 BL; // Bottom Left corner of the box collider
    private Vector3 BR; // Bottom Right corner of the box collider

    private LayerMask platformMask;

    private int numberVerticalRays;
    private int numberHorizontalRays;

    private float skinWidth;
    private float headCheck;
    private float maxClimbableSlope;
    private float error;
    private float transformHeight;
    private float transformWidth;

    //Hit detection flags
    private bool isClimbableSlope;
    private bool isGrounded;
    private bool isOnSlope;
    private bool isRight;
    private bool isOnWallRight;
    private bool isOnWallLeft;

    

    /// <summary>
    /// Constructor for the Collision Corrections class. This object holds data about the player's current position and the dimensions
    /// of the player's box collider. It includes methods for checking for collisions in the direction of movement, and adjusting the
    /// distance moved in order to avoid clipping.
    /// </summary>
    /// <param name="player_transform">The transform of the player.</param>
    /// <param name="box_collider">The box collider attached to the player.</param>
    /// <param name="platform_mask">Layermask containing the layers that you want to avoid clipping through.</param>
    /// <param name="number_of_vertical_rays">How many rays will be checking for the floor and roof.</param>
    /// <param name="number_of_horizontal_rays">How many rays will be checking for the walls.</param>
    /// <param name="skin_width">How long do the rays need to be when checking for collisions.</param>
    /// <param name="head_check">Length of a ray used for checking for low hanging roofs.</param>
    /// <param name="max_climbable_slope">How steep of a slope is too steep to walk on.</param>
    /// <param name="collision_check_error">How much leeway to use when checking for collisions. Having at
    /// least a small value here (less than 0.001f) can help with performance of collision checking.</param>
    public CollisionCorrections(Transform player_transform, BoxCollider box_collider, LayerMask platform_mask, 
        int number_of_vertical_rays, int number_of_horizontal_rays, float skin_width, float head_check,
        float max_climbable_slope, float collision_check_error)
    {
        // Initialize all values named in constructor definition
        transform = player_transform;
        boxCollider = box_collider;
        platformMask = platform_mask;
        numberVerticalRays = number_of_vertical_rays;
        numberHorizontalRays = number_of_horizontal_rays;
        skinWidth = skin_width;
        headCheck = head_check;
        error = collision_check_error;

        //Initialize all hit detection flags to false
        isClimbableSlope = false;
        isGrounded = false;
        isOnSlope = false;
        isRight = false;
        isOnWallRight = false;
        isOnWallLeft = false;

        //Initialize the positions for corners of the box collider
        RaycastStartPoints();
    }

    /// <summary>
    /// Method that calculates the local distance-offset of each of the four corners of the player's
    /// box collider.
    /// </summary>
    private void RaycastStartPoints()
    {
        var skinBounds = boxCollider.bounds;
        skinBounds.Expand(-2f * skinWidth);

        TL = new Vector3(skinBounds.min.x, skinBounds.max.y, 0f);
        TR = new Vector3(skinBounds.max.x, skinBounds.max.y, 0f);
        BL = new Vector3(skinBounds.min.x, skinBounds.min.y, 0f);
        BR = new Vector3(skinBounds.max.x, skinBounds.min.y, 0f);
    }
    
    /// <summary>
    /// Calculates the height and width of the transform
    /// </summary>
    private void TransformDimentions()
    {
        transformHeight = transform.localScale.y;
        transformWidth = transform.localScale.x;
    }

    /// <summary>
    /// Clear hit detection flags for wall sliding right and left
    /// </summary>
    private void ClearOnWall()
    {
        isOnWallLeft = isOnWallRight = false;
    }

    /// <summary>
    /// Check for collisions on the left/right axis. If a collision occurs, adjust the movement that was planned for this frame, such that 
    /// the player does not clip through a platform.
    /// </summary>
    /// <param name="deltaMovement">The ammount & direction of movement for this frame</param>
    /// <returns></returns>
    public Vector3 HorizontalCollision(Vector3 deltaMovement)
    {
        isRight = deltaMovement.x > 0f;
        float rayLength = Mathf.Abs(deltaMovement.x) + skinWidth;
        Vector3 rayDirection = isRight ? Vector3.right : Vector3.left;
        Vector3 firstRayStartPoint = isRight ? BR : BL;

        float colliderUseableHeight = boxCollider.size.y * Mathf.Abs(transformHeight) - (2f * skinWidth);
        float verticalRaySeparation = colliderUseableHeight / (numberHorizontalRays - 1);

        for (int i = 0; i < numberHorizontalRays; i++)
        {
            isClimbableSlope = false;
            Vector3 ray = new Vector3(firstRayStartPoint.x, firstRayStartPoint.y + i * verticalRaySeparation, 0f);
            RaycastHit hit;

            Debug.DrawRay(ray, rayDirection * rayLength, Color.magenta);

            bool raycastHit = Physics.Raycast(ray, rayDirection, out hit, rayLength, platformMask);

            //can only hit slope with the bottom horizontal raycast && isRight && slope is < 70
            if (isRight && (Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90f < maxClimbableSlope))
            {
                //print("Slope on right: " + (Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg ));

                if (i > 0) //case of a collision with a narrowing ceiling while walking
                {
                    isClimbableSlope = false;
                }
                else
                {
                    isClimbableSlope = true;
                }
            }

            if (!isRight && (Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg < maxClimbableSlope))
            {
                //print("Slope on left: " + Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg);

                if (i > 0) //case of a collision with a narrowing ceiling while walking
                {
                    isClimbableSlope = false;
                }
                else
                {
                    isClimbableSlope = true;
                }
            }

            if (raycastHit && !isClimbableSlope)
            {
                // something something Darrell plz make slopes work ... ok

                deltaMovement.x = hit.point.x - ray.x;
                rayLength = Mathf.Abs(deltaMovement.x);

                if (isRight && !isClimbableSlope)
                {
                    deltaMovement.x -= skinWidth;

                    if ((hit.transform.gameObject.tag == "WallJump") && !isGrounded)
                    {
                        //////////playerController.SetIsOnWallRight(true);
                        Debug.Log("isOnWallRight");
                    }
                }
                else if (!isRight && !isClimbableSlope)
                {
                    deltaMovement.x += skinWidth;

                    if ((hit.transform.gameObject.tag == "WallJump") && !isGrounded)
                    {
                        //////////playerController.SetIsOnWallLeft(true);
                        Debug.Log("isOnWallLeft");
                    }
                }

                if (rayLength < skinWidth + error)
                {
                    break;
                }
            }
        }

        //playerController.SetClimbableSlope(climbableSlope); .......... Not important to return to movement script
        //playerController.SetIsRight(isRight); .......... Not important to return to movement script
        return deltaMovement;
    }

    /// <summary>
    /// Check for collisions on the up/down axis. If a collision occurs, adjust the movement that was planned for this frame, such that 
    /// the player does not clip through a platform.
    /// </summary>
    /// <param name="deltaMovement">The ammount & direction of movement for this frame</param>
    /// <returns></returns>
    public Vector3 VerticalCollision(Vector3 deltaMovement)
    {
        bool isUp = deltaMovement.y > 0f;
        isRight = deltaMovement.x > 0f;
        float rayLength = Mathf.Abs(deltaMovement.y) + skinWidth;
        Vector3 rayDirection = isUp ? Vector3.up : -Vector3.up;
        Vector3 firstRayStartPoint = isUp ? TL : BL;
        firstRayStartPoint.x += deltaMovement.x;


        float colliderUseableWidth = boxCollider.size.x * Mathf.Abs(transformWidth) - (2f * skinWidth);
        float horizontalRaySeparation = colliderUseableWidth / (numberVerticalRays - 1);
        if (isRight)
        {
            //print("moving Right");

            for (int i = numberVerticalRays; i >= 0; i--)
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
                        //////////playerController.ClearOnWall();
                    }

                    if (!isUp && deltaMovement.y > 0.00001f)
                    {
                        isOnSlope = true;
                    }
                    else
                    {
                        //isOnSlope = false;
                    }

                    if (rayLength < skinWidth + error)
                    {
                        break;
                    }
                }
                else
                {
                    //isOnSlope = false;
                }
            }
        }
        else
        {
            //print("moving left");

            for (int i = 0; i < numberVerticalRays; i++)
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
                        //////////playerController.ClearOnWall();
                    }

                    if (!isUp && deltaMovement.y > 0.00001f)
                    {
                        isOnSlope = true;
                    }
                    else
                    {
                        //isOnSlope = false;
                    }

                    if (rayLength < skinWidth + error)
                    {
                        break;
                    }
                }
                else
                {
                    //isOnSlope = false;
                }
            }
        }

        //////////playerController.SetIsGrounded(isGrounded);
        //////////playerController.SetIsOnSlope(isOnSlope);
        //////////playerController.SetIsRight(isRight);
        return deltaMovement;
    }

    public Vector3 VerticalSlopeDetection(Vector3 deltaMovement)
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

    //Methods that give info about current state of collision. To be accessed by other scripts.
    #region Public Accessor Methods for Hit Detection Flags
    public bool getIsClimbableSlope()
    {
        return isClimbableSlope;
    }

    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public bool getIsOnSlope()
    {
        return isOnSlope;
    }

    public bool getIsRight()
    {
        return isRight;
    }

    public bool getIsOnWallRight()
    {
        return isOnWallRight;
    }

    public bool getIsOnWallLeft()
    {
        return isOnWallLeft;
    }
    #endregion

    #region LegacyCode
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
    #endregion
}
