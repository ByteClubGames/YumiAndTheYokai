/*
 * Programmer:   Hunter Goodin, Keiran Glynn 
 * Date Created: 02/16/2018 @  9:40 PM 
 * Last Modified: 06/25/2018 @ 6:19 PM 
 * File Name:    PlayerJumper.cs 
 * Description:  This script makes the human-player jump. It is in the process of being modularized so that it will control the "active player" game object (like the human, ferrox, etc), so stay tuned. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanJump : MonoBehaviour
{
    public Rigidbody actingPlayerRB; // Populated with the player's human body's 2D Rigidbody in-engine. 
    //public GameObject groundCheck;
    public float jumpForce;
    //public float grChComeBack = 0.0f;
    public int defaultGroundFrames = 30;
    private int groundFrames = -1;
    public int defaultAirFrames = 30;
    private int airFrames = -1;
    private bool jumpNow = false;

    private Vector3 colliderCenter;
    private Vector3 colliderMin;

    //public Vector2 firstPressPos = new Vector2(0, 0);
    //public Vector2 secondPressPos;
    //public Vector2 currentSwipe;

    public bool isJumping;

    public bool isGrounded; // Boolean variable than represents whether or not an object is grounded or not.

    private void Update()
    {
        if (!isGrounded) SetIsGrounded();
        if (isGrounded && groundFrames > 0) groundFrames--;
        if (isGrounded && groundFrames <= 0) isGrounded = false;
        if (!isGrounded) airFrames--;
        if (airFrames <= 0) jumpNow = false;
        Jump();
        // Swiper(); 
        isJumping = false;
    }

    private void Jump() // Script that allows the acting player to jump.
    {
        if (Input.GetKeyDown("space") || Input.GetKeyDown("w") || Input.GetKeyDown("up") || isJumping)
        {
            jumpNow = true;
            airFrames = defaultAirFrames;
        }
        if (jumpNow && isGrounded)
        {
            actingPlayerRB.velocity = Vector3.zero;
            actingPlayerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpNow = false;
        }
    }

    /* The following method uses a CheckCapsule (similar to a raycast) to see if the player is standing on the ground, and therefore able to jump. It takes the position of the bottom of the character's capsule collider,
     * and casts a capsule downward a small amount below the player checking for ground on the layer "Platforms." If it detects ground beneath the player's feet, it will change the isGrounded boolean to
     * true, allowing the player to jump via the Jump method.
     */
    public void SetIsGrounded()
    {
        colliderCenter = GameObject.Find("Player-Human").GetComponent<CapsuleCollider>().bounds.center;
        colliderMin = GameObject.Find("Player-Human").GetComponent<CapsuleCollider>().bounds.min;
        int layerMask = (1 << 16); // *** Layermask is very finnicky, so be carefull. Look up the correct format for setting it to the desired layer(ex: (1<<10)) ***
        isGrounded = Physics.CheckCapsule(colliderCenter, new Vector3(colliderCenter.x, colliderMin.y - 0.1f, colliderCenter.z), 0.15f, layerMask); // boolean requirement used in parameters of Jump function (see above)
        groundFrames = defaultGroundFrames;
    }
    
    public void MakeJumpTrue() //I beleieve that this method is a part of Hunter's unfinished touch-triggered jump. Possibly check the touch input scripts for more info - Keiran
    {
        isJumping = true; 
    }


    /* The following code is a part of the "Swipe to Jump" system that was being implemented by Hunter. I beleive that it has become redunant and
     * outdated with the inclusion of the "Touch Input" scripts, located in the touch input folder in the assests menu. As Hunter is no longer with us, 
     * this section of code needs to be looked at in comparison to the other touch input scripts to determine what we really need to hold on to - Keiran
     */
     
    //private void Swiper()
    //{
    //    //get user input
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        //get first mouse position
    //        firstPressPos.x = Input.mousePosition.x;
    //        firstPressPos.y = Input.mousePosition.y;
    //    }
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

    //        currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
    //    }

    //    if (currentSwipe.y > 50 && currentSwipe.y != 0)
    //    {
    //        humanRB.transform.Translate(transform.up * Time.deltaTime * jumpSpeed);

    //        firstPressPos = new Vector2(0, 0);
    //        secondPressPos = new Vector2(0, 0);
    //        currentSwipe = new Vector2(0, 0);
    //    }
    //}
}
