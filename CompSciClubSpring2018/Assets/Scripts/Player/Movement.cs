/*
 * Programmer: Keiran Glynn
 * Date Created: 07/23/2018 @ 12:30 AM
 * Last Modified: 07/23/2018 @ 12:30 AM
 * File Name: Movement.cs
 * Description: Base movement class that all other movement classes will inherit from.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public CharacterController controller;
    public Vector3 movementVector;
    public float gravity = 18f;
    //public float maxCoyoteTime = .1f;
    //public float coyoteTime = 0f;
    public float jumpForce;
    public float jumpVelocity;
    public float movementSpeed;

    

    public void MoveLeft(bool left)
    {
        if (left)
        {
            movementVector.x = movementSpeed;
        }
        else
        {
            movementVector.x = 0f;
        }
        controller.Move(-movementVector * Time.deltaTime);
    }

    public void MoveRight(bool right)
    {
        if (right)
        {
            movementVector.x = movementSpeed;
        }
        else
        {
            movementVector.x = 0f;
        }        
        controller.Move(movementVector * Time.deltaTime);
    }

    public void Jump(bool jump)
    {
        if (controller.isGrounded)
        {
            Debug.Log("Player Grounded");
            jumpVelocity = -controller.stepOffset / Time.deltaTime;

            if (jump)
            {
                jumpVelocity = jumpForce;
            }
        }
        else
        {
            jumpVelocity -= gravity * Time.deltaTime;
        }

        //movementVector = Vector3.zero;
        movementVector.y = jumpVelocity;
        controller.Move(movementVector * Time.deltaTime);
    }


}
