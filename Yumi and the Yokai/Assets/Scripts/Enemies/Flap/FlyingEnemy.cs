/********************************************************************************
*Creator(s)............................................Ivonne Lopez, Darrell Wong
*Created................................................................9/21/2018
*Last Modified................................................@ 2PM on 2/15/2019
*Last Modified by....................................................Ivonne Lopez
*
* Description:  Script responsible for general(flap and grounded) movement and 
*               attack patterns. Calls on PlayerDetection.cs to actually detect 
*               the player's presence.
*********************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{

    [Header("Horizontal Flight Boundary Points")]
    public float leftBoundry = 4f;   //how far the enemy will wander to the left when not under aggro
    public float rightBoundry = 4f; // how far the enemy will wanter to the right when not under aggro
    public bool facingRight = true; //animation by default is facing right 
    private Vector3 pos1;  //pos1 and pos2 are the initial positions of the enemy
    private Vector3 pos2;
    private Vector3 startPos;
    private Vector3 difference;     //how far away the 2 boundry points are. Used in calculating horizontal speeds


    [Header("Physics/ Enemy Attributes")]
    public float patrolSpeed = 0.1f;        //patrolSpeed needs to be between 0 and .5f. An increase of patrol Speed requires an increase of normalspeed
    public float amplitude;                 //amplitude of the enemy patrol pattern
    public float attackSpeed = 5f;          //speed of enemy when attacking player
    public float normalSpeed = 2f;          //speed of the enemy when not attacking

    private Transform Player;
    private Vector3 directionOfPlayer;
    private Vector3 tempPosition;            // This position vector is modified algebraically, and then the enemy's actual position is set equal to this temp position    
    private float progress = 0f;            // Variable determining how the percentage completion of the Lerp for the horizontal component of movement
    private bool challenged = false;        // Determines if the enemy should be attacking the player or not
    private PlayerDetection detectPlayer;
    Rigidbody rb;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        tempPosition = transform.position;

        pos1 = new Vector3(transform.position.x - leftBoundry, transform.position.y, 0f);
        pos2 = new Vector3(transform.position.x + rightBoundry, transform.position.y, 0f);

        Player = GameObject.Find("Yumi").transform;
        detectPlayer = this.GetComponentInChildren<PlayerDetection>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        pos1 = new Vector3(startPos.x - leftBoundry, startPos.y, 0f);
        pos2 = new Vector3(startPos.x + rightBoundry, startPos.y, 0f);
        difference = pos2 - pos1;

        progress = Mathf.PingPong(Time.time * patrolSpeed, 1f);

        Debug.DrawLine(pos1, pos2, Color.blue);

        tempPosition.x = Mathf.Lerp(pos1.x, pos2.x, progress); // interpolates pos1 and pos2 by progress
        tempPosition.y = startPos.y + (Mathf.Sin(Time.realtimeSinceStartup * normalSpeed) * amplitude); 

        Movement(tempPosition);
    

    }

    void Flip() //flips animation about the y-axis to match the direction of the animation's movement 
    {

        facingRight = !facingRight; 
        transform.Rotate(Vector3.up * 180); //rotates 180 degress about y-axis
    }

    private void Movement(Vector3 nextPosition)     //nextPosition is the current position(before being challenged)
    {
        challenged = detectPlayer.PlayerDetected();

        if (challenged)
        {
            directionOfPlayer = (Player.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + directionOfPlayer * attackSpeed * Time.deltaTime);
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
                rb.MovePosition(transform.position + directionOfNextPosition * normalSpeed * Time.deltaTime);
            }
        }
        if ((tempPosition.x - transform.position.x) > 0 && !facingRight) //flips the animation to face right
        {
            Flip();
        }
        else if ((tempPosition.x - transform.position.x) < 0 && facingRight) //flips the animation to face left 
        {
            Flip();
        }

    }
}
