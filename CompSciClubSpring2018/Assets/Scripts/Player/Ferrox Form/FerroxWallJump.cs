/*
 * 
 * Authors: Spencer Wilson, Andrew Ramirez
 * Date Created: 3/5/2018 @ 3:16 pm
 * Date Modified: 4/7/2018 @ 10:20am
 * Project: CompSciClubSpring2018
 * File: FerroxWallJump.cs
 * Description: This class houses the code for the ferrox's wall jump.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerroxWallJump : MonoBehaviour {

    //FerroxJump jump;
  //  FerroxMovement movement;

    public GameObject ferroxGameObject;

    public float speed; 
    public float distance = 0.6f; // determines the length of the the hitbox for wall jumps
    public bool touchingWall; // is the player touching wall


    // Use this for initialization
    void Start()
    {
        // default speed for wall jump, this affects the height of the jump
        speed = 16;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // a raycast to detect wall collision
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);

        // if jump button pressed and not grounded and collider exists and touching a "wall"
        if ((hit.collider != null) && (touchingWall == true))
        {
            // checks if player wants to walljump
            if (((Input.GetKey("a") && GetComponent<FerroxMovement>().facingRight) || (Input.GetKey("d")) && GetComponent<FerroxMovement>().facingRight == false)) {
                {
                 
                    // flip the player character
                    StartCoroutine("TurnIt");

                    // sets velocity to a new vector2, modify the parameters to adjust the walljump, note that it follows this format Vector2(x, y)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(speed / 3 * hit.normal.x, speed);
                    

                    // pause input to prevent infinite wall jumping
                    GetComponent<FerroxMovement>().pauseInput = true;

                    ///////////////////////// need to finish //////////////////////////
                    // temporarily slow down the players air speed after a jump
                  //  movement.speed = movement.wallJumpAirSpeed;

                    // sets the speed back to normal once back on the ground
                    if (GetComponent<FerroxJump>().isGrounded == true)
                    {
                     //   movement.speed = movement.wallJumpAirSpeed * 2;
                    }
                    ///////////////////////////////////////////////////////
                }
            }
        }
    }

    // checks if the player is touching a wall
    private void OnCollisionEnter2D(Collision2D collision)
    {
            touchingWall = collision.gameObject.tag == "wall";
    }

    // draws hitbox to detect walls; hint turn on gizmos to view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * distance);
    }

    // Coroutine that flips the character
    public IEnumerator TurnIt()
    {
        yield return new WaitForFixedUpdate();


        transform.localScale = transform.localScale.x == 1 ? new Vector2(-1, 1) : Vector2.one;
        ferroxGameObject.GetComponent<FerroxMovement>().facingRight = !ferroxGameObject.GetComponent<FerroxMovement>().facingRight;

    }
}
