/*
 * Programmer:   Hunter Goodin 
 * Date Created: 02/16/2018 @  7:15 PM 
 * Last Updated: 02/16/2018 @  9:35 PM 
 * File Name:    PlayerMovement.cs 
 * Description:  This script will be responsible for the player's movements. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpSpeed; 
    public Rigidbody playerRB;
    public bool isJump;
    public bool isLeft;
    public bool isRight;
    public bool touched; 

    private void FixedUpdate()
    {
        if (touched && isLeft)
        {
            playerRB.transform.Translate(-transform.right * Time.deltaTime * speed);
        }
        if (touched && isRight)
        {
            playerRB.transform.Translate(transform.right * Time.deltaTime * speed);
        }
        if (touched && isJump)
        {
            playerRB.transform.Translate(transform.up * Time.deltaTime * jumpSpeed);
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
