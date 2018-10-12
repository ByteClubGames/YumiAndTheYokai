/*
 *  Authors: Ivonne Lopez, Darrell Wong
 *  Date Created: 9/21/2018
 *  Last Modified: 10/11/2018
 *  FlyingEnemy.cs
 *  Description: Script responsible for general(flap and grounded) movement and attack patterns. Calls on PlayerDetection.cs to actually detect the player's presence.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour {

    [Header("Horizontal Flight Boundary Points")]
    public Vector3 pos1;  //pos1 and pos2 are the initial positions of the enemy
    public Vector3 pos2;
    public float leftBoundry = 4f;   //how far the enemy will wander to the left when not under aggro
    public float rightBoundry = 4f;  // how far the enemy will wanted to the right when not under aggro
    public Vector3 startPos;


    [Header("Physics/ Enemy Attributes")]
    public float horizontalSpeed;
    public float verticalSpeed;
    public float amplitude;
    public float attackSpeed = 0.05f;
    public float returnSpeed = 0.05f;

    private Transform Player;
    private Vector3 directionOfPlayer;
    public Vector3 tempPosition; // This position vector is modified algebraically, and then the enemy's actual position is set equal to this temp position    
    private float progress = 0f; // Variable determining how the percentage completion of the Lerp for the horizontal component of movement
    private bool challenged = false; // Determines if the enemy should be attacking the player or not
    private PlayerDetection detectPlayer;
    Rigidbody rb;

    // Use this for initialization
    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        tempPosition = transform.position;

        pos1 = new Vector3(transform.position.x, transform.position.y, 0f);
        pos2 = new Vector3(transform.position.x + rightBoundry, transform.position.y, 0f);

        Player = GameObject.Find("Yumi").transform;
        detectPlayer = this.GetComponentInChildren<PlayerDetection>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        progress = Mathf.PingPong(Time.time * 0.1f, 1f);
        tempPosition.x = Mathf.Lerp(pos1.x, pos2.x, progress * 1);
        tempPosition.y = startPos.y + (Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude); ;

        Movement(tempPosition); 
	} 

    private void Movement(Vector3 nextPosition)     //nextPosition is the current position(before being challenged)
    {
        challenged = detectPlayer.PlayerDetected();
        
        if (challenged)
        {
            directionOfPlayer = (Player.transform.position - transform.position).normalized;
            //rb.AddForce(directionOfPlayer*attackSpeed);
            rb.MovePosition(transform.position + directionOfPlayer * attackSpeed * Time.deltaTime);
            //transform.Translate(directionOfPlayer * attackSpeed, Space.World);            
        }
        else
        {
            Vector3 directionOfNextPosition = (nextPosition - transform.position).normalized;
            if (Vector3.Distance(transform.position, nextPosition) < .05)
            {
                transform.position = nextPosition;
            }
            else
            {
                rb.MovePosition(transform.position + directionOfNextPosition * returnSpeed * Time.deltaTime);
            }
        }        
    }   
}
