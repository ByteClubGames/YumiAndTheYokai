///*
// * Programmer:   Hunter Goodin 
// * Date Created: 02/16/2018 @  9:40 PM 
// * Last Updated: 06/2/2018 @ 11:35 PM 
// * File Name:    PlayerJumper.cs 
// * Description:  This script will be responsible for the player's movements. 
// */

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class HumanJump : MonoBehaviour
//{
//    public Rigidbody2D humanRB;
//    public float jumpForce = 7;

//    public float grChComeBack = 0.0f;

//    //public Vector2 firstPressPos = new Vector2(0, 0);
//    //public Vector2 secondPressPos;
//    //public Vector2 currentSwipe;

//    public bool isJumping;

//    public bool isGrounded; // Boolean variable than represents whether or not an object is grounded or not.

//    private void Update()
//    {
//        SpaceJump(); // Makes the player jump with the press of a spacebar. 
//        Jump();
//        // Swiper(); 
//        isJumping = false;
//    }

//    private void Jump() // Script that allows the player to jump.
//    {
//        if (isJumping && /*(actingPlayerObj.velocity.y == 0f) && */ isGrounded)
//        {
//            actingPlayerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

//            groundCheck.SetActive(false);
//            groundCheck.SetActive(true);
//        }

//        if (Input.GetKeyDown("up") && /* ( actingPlayerObj.velocity.y == 0f ) */ isGrounded)
//        {
//            actingPlayerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//        }
//        if (Input.GetKeyDown("w") && /* ( actingPlayerObj.velocity.y == 0f ) */ isGrounded)
//        {
//            actingPlayerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//        }
//        if (Input.GetKeyDown("space") && /* ( actingPlayerObj.velocity.y == 0f ) */ isGrounded)
//        {
//            actingPlayerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

//            groundCheck.SetActive(false);
//            groundCheck.SetActive(true);
//        }

//        //if (jump && /*(humanRB.velocity.y == 0f) &&*/ isGrounded)
//        //{
//        //    humanRB.AddForce(Vector2.up * Time.deltaTime * jumpForce);
//        //}
//    }

//    public void SetIsGrounded() // Allows the player to set isGrounded.
//    {
//        // isGrounded = value;

//        isGrounded = groundCheck.GetComponent<GroundCheck>().GetIsColling();
//    }

//    public void MakeJumpTrue()
//    {
//        isJumping = true;
//    }

//    private void SpaceJump() // Uses the space bar to jump. Useful for testing the game on a computer
//    {
//        if (Input.GetKey("space") && (humanRB.velocity.y == 0f) && isGrounded)
//        {
//            humanRB.AddForce(Vector2.up * Time.deltaTime * jumpForce);
//        }
//    }

//    //private void Swiper()
//    //{
//    //    //get user input
//    //    if (Input.GetMouseButtonDown(0))
//    //    {
//    //        //get first mouse position
//    //        firstPressPos.x = Input.mousePosition.x;
//    //        firstPressPos.y = Input.mousePosition.y;
//    //    }
//    //    if (Input.GetMouseButtonUp(0))
//    //    {
//    //        secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

//    //        currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
//    //    }

//    //    if (currentSwipe.y > 50 && currentSwipe.y != 0)
//    //    {
//    //        humanRB.transform.Translate(transform.up * Time.deltaTime * jumpSpeed);

//    //        firstPressPos = new Vector2(0, 0);
//    //        secondPressPos = new Vector2(0, 0);
//    //        currentSwipe = new Vector2(0, 0);
//    //    }
//    //}
//}
