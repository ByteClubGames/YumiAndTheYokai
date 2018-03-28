/*
 * 
 * Authors: Spencer Wilson, Andrew Ramirez
 * Date Created: 3/5/2018 @ 3:16 pm
 * Date Modified: 3/24/2018 @ 9:00 am
 * Project: CompSciClubSpring2018
 * File: FerroxWallJump.cs
 * Description: This class houses the code for the ferrox's wall jump.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerroxWallJump : MonoBehaviour {

    FerroxJump jump;

    public GameObject ferroxGameObject;

    public float speed;
    public float distance = 0.6f; // determines the length of the the hitbox for wall jumps
    public bool touchingWall; // is the player touching wall

    private bool touchingFlaggedWall;

    public GameObject flaggedWall;

    // Use this for initialization
    void Start()
    {
        // uses jump to check if the player is grounded or not
        jump = GetComponent<FerroxJump>();

        // default speed for wall jump
        speed = 8;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // for wall jump hit detection
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);

        // if jump button pressed and not grounded and collider exists and touching a "wall"
        if (Input.GetKey("space") && !jump.isGrounded && hit.collider != null && touchingWall && !touchingFlaggedWall)
        {
            {       
                // sets velocity to a new vector2
                GetComponent<Rigidbody2D>().velocity = new Vector2(speed * hit.normal.x, speed);

                // flip the player character
                StartCoroutine("TurnIt");
            }

        }
    }

    // ***Note, wall objects NEED to be tagged as "wall" 
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "wall" && collision.gameObject != flaggedWall)
        {
            touchingWall = collision.gameObject.tag.Equals("wall");
            flaggedWall = collision.gameObject;
            touchingFlaggedWall = true;
        }
        touchingFlaggedWall = false;
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
