/*
 * 
 * Authors: Spencer Wilson, Keiran Glynn, Andrew Ramirez
 * Date Created: 3/5/2018 @ 3:15 pm
 * Date Modified: 4/16/2018 @ 2:17 am (Tony Silva)
 * Project: CompSciClubSpring2018
 * File: FerroxMovement.cs
 * Description: This class houses the code for the movement of the ferrox.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class FerroxMovement : MonoBehaviour {

    public bool facingRight;
    public float speed;
    public float horizontal; // -1 = player facing left, 1 = player facing right
    private Rigidbody2D playerRB;
    public bool pauseInput;

    public GameObject ferroxGameObject;

    // Use this for initialization
    void Start () {

        facingRight = true;
        playerRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        horizontal = Input.GetAxis("Horizontal");

        if (pauseInput == true)
        {
            StartCoroutine(waiter());
        }
        else {
            if (Input.GetKey("d"))
            {
                transform.Translate(Vector2.right * Time.deltaTime * speed);
            }

            if (Input.GetKey("a"))
            {
                transform.Translate(Vector2.left * Time.deltaTime * speed);
            }
        }


        StartCoroutine("TurnIt");
    }

    IEnumerator waiter()
    {  
        //Wait for x seconds
        yield return new WaitForSecondsRealtime(1);

        pauseInput = false;
    }


    // Coroutine that flips the character
    public IEnumerator TurnIt()
    {
        yield return new WaitForFixedUpdate();

        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            transform.localScale = transform.localScale.x == 1 ? new Vector2(-1, 1) : Vector2.one;
        }

    }

    void OnCollisionEnter2D(Collision2D other) // this is allows for the player to become a child of the moving platform prefab
    {
        if (other.transform.tag == "MovingPlatform") //this finds the object with the tag "MovingPlatform"
        {
            transform.parent = other.transform; //this makes what is colliding with that tag into the child of that tag
        }        
    }

    void OnCollisionExit2D(Collision2D other) // this is allows for the player not be a child of the moving platform prefab
    {
        if (other.transform.tag == "MovingPlatform") //this finds the object with the tag "MovingPlatform"
        {
            transform.parent = null; //this makes what is colliding with that tag stop being a child of that tag
        }
    }

}
