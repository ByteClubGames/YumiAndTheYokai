/*
********************************************************************************
*Creator(s)...........................................Kieran Glynn, Darrell Wong
*Created..............................................................12/14/2018
*Last Modified............................................@ 4:42PM on 12/14/2018
*Last Modified by...................................................Keiran Glynn
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

    public static Vector3 HorizontalCollision(PlayerController playerController,Vector3 deltaMovement, BoxCollider boxCollider, Vector3 BL, Vector3 BR, 
        float transformHeight, float verticalRaySeparation, float maxClimbableSlope, float skinWidth, float error, int horizontalRays, 
        bool climbableSlope, bool isRight, bool isGrounded, bool isOnWallRight, bool isOnWallLeft, LayerMask platformMask)
    {
        isRight = deltaMovement.x > 0f;
        float rayLength = Mathf.Abs(deltaMovement.x) + skinWidth;
        Vector3 rayDirection = isRight ? Vector3.right : Vector3.left;
        Vector3 firstRayStartPoint = isRight ? BR : BL;

        float colliderUseableHeight = boxCollider.size.y * Mathf.Abs(transformHeight) - (2f * skinWidth);
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

                    if ((hit.transform.gameObject.tag == "WallJump") && !isGrounded)
                    {
                        playerController.SetIsOnWallRight(true);
                        Debug.Log("isOnWallRight");
                    }
                }
                else if (!isRight && !climbableSlope)
                {
                    deltaMovement.x += skinWidth;

                    if ((hit.transform.gameObject.tag == "WallJump") && !isGrounded)
                    {
                        playerController.SetIsOnWallLeft(true);
                        Debug.Log("isOnWallLeft");
                    }
                }

                if (rayLength < skinWidth + error)
                {
                    break;
                }
            }
        }

        playerController.SetClimbableSlope(climbableSlope);
        playerController.SetIsRight(isRight);
        return deltaMovement;
    }

    public static Vector3 VerticalCollision(PlayerController playerController, Vector3 deltaMovement, BoxCollider boxCollider, Vector3 TL, Vector3 BL,
        float transformWidth, float horizontalRaySeparation, float skinWidth, float error, int verticalRays, bool isOnSlope, bool isRight, 
        bool isGrounded, LayerMask platformMask)
    {
        bool isUp = deltaMovement.y > 0f;
        isRight = deltaMovement.x > 0f;
        float rayLength = Mathf.Abs(deltaMovement.y) + skinWidth;
        Vector3 rayDirection = isUp ? Vector3.up : -Vector3.up;
        Vector3 firstRayStartPoint = isUp ? TL : BL;
        firstRayStartPoint.x += deltaMovement.x;


        float colliderUseableWidth = boxCollider.size.x * Mathf.Abs(transformWidth) - (2f * skinWidth);
        horizontalRaySeparation = colliderUseableWidth / (verticalRays - 1);
        if (isRight)
        {
            //print("moving Right");

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
                        playerController.ClearOnWall();
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
            //print("moving left");

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
                        playerController.ClearOnWall();
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

        playerController.SetIsGrounded(isGrounded);
        playerController.SetIsOnSlope(isOnSlope);
        playerController.SetIsRight(isRight);
        return deltaMovement;
    }

    public static Vector3 VerticalSlopeDetection(PlayerController playerController, Vector3 deltaMovement, BoxCollider boxCollider, Vector3 BL, Vector3 BR,
        float skinWidth,float maxClimbableSlope, LayerMask platformMask)
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
