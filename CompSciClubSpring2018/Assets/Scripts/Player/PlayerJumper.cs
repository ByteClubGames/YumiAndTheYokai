/*
 * Programmer:   Hunter Goodin 
 * Date Created: 02/16/2018 @  9:40 PM 
 * Last Updated: 02/16/2018 @ 11:35 PM 
 * File Name:    PlayerJumper.cs 
 * Description:  This script will be responsible for the player's movements. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumper : MonoBehaviour
{
    public Rigidbody playerRB;
    public float jumpForce = 7; 

    private Vector2 firstPressPos = new Vector2(0, 0);
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;

    private bool touched;
    public bool isJump;

    public LayerMask groundLayers;
    public CapsuleCollider col;

    private void Update()
    {
        Swiper();  
    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * .9f, groundLayers);
    }

    // Swipe Controls 
    private void Swiper()
    {
        //get user input
        if (Input.GetMouseButtonDown(0))
        {
            //get first mouse position
            firstPressPos.x = Input.mousePosition.x;
            firstPressPos.y = Input.mousePosition.y;
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
        }

        if (IsGrounded() && currentSwipe.y > 50 && currentSwipe.y != 0)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            firstPressPos = new Vector2(0, 0);
            secondPressPos = new Vector2(0, 0);
            currentSwipe = new Vector2(0, 0);
        }
    }

    private bool IsSwipe()
    {
        if(currentSwipe.y > 50 && currentSwipe.y != 0)
        {
            return true; 
        }
        else
        {
            return false; 
        }
    }

    // Button Controls
    private void ButtonJump()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (touched && isJump)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnMouseDown()
    {
        touched = true;
    }

    private void OnMouseUp()
    {
        touched = false;
    }
}
