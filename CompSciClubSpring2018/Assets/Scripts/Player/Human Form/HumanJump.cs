/*
 * Programmer:   Hunter Goodin, Keiran Glynn 
 * Date Created: 02/16/2018 @  9:40 PM 
 * Last Modified: 06/24/2018 @ 2:07 PM 
 * File Name:    PlayerJumper.cs 
 * Description:  This script will be responsible for the player's movements. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanJump : MonoBehaviour
{
    public Rigidbody actingPlayerRB; // Populated with the player's human body's 2D Rigidbody in-engine. 
    public GameObject groundCheck; 
    public float jumpForce;

    public float grChComeBack = 0.0f; 

    //public Vector2 firstPressPos = new Vector2(0, 0);
    //public Vector2 secondPressPos;
    //public Vector2 currentSwipe;

    public bool isJumping; 

    public bool isGrounded; // Boolean variable than represents whether or not an object is grounded or not.

    private void Update()
    {
        SetIsGrounded(); 
        Jump(); 
        // Swiper(); 
        isJumping = false;
    }

    private void Jump() // Script that allows the player to jump.
    {
        if (isJumping && /*(actingPlayerObj.velocity.y == 0f) && */ isGrounded)
        {
            actingPlayerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            groundCheck.SetActive(false);
            groundCheck.SetActive(true);
        }

        if (  Input.GetKeyDown("up") && /* ( actingPlayerObj.velocity.y == 0f ) */ isGrounded)
        {
            actingPlayerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if ( Input.GetKeyDown("w") && /* ( actingPlayerObj.velocity.y == 0f ) */ isGrounded)
        {
            actingPlayerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if ( Input.GetKeyDown("space") && /* ( actingPlayerObj.velocity.y == 0f ) */ isGrounded )
        {
            actingPlayerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            groundCheck.SetActive(false);
            groundCheck.SetActive(true);
        }     
    }

    public void SetIsGrounded() // Allows the player to set isGrounded.
    {
        isGrounded = groundCheck.GetComponent<GroundCheck>().GetIsColliding();
    }

    public void MakeJumpTrue()
    {
        isJumping = true; 
    }


    /* The following code is a part of the "Swipe to Jump" system that was being implemented by Hunter. I beleive that it has become redunant and
     * outdated with the inclusion of the "Touch Input" scripts, located in the touch input folder in the assests menu. As Hunter is no longer with us, 
     * this section of code needs to be looked at in comparison to the other touch input scripts to determine what we really need to hold on to.
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
