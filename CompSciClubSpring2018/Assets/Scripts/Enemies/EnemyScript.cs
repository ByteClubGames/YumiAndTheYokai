/*
 *  Authors: Darrell Wong
 *  Date Created: 10/9/2018
 *  Last Modified: 10/11/2018
 *  EnemyPig.cs
 *  Description: Script responsible for general(flap and grounded) movement and attack patterns. Calls on PlayerDetection.cs to actually detect the player's presence.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    public float AttackMaxSpeed;
    public float patrolMaxSpeed;
    public float acceleration;
    public float leftBoundry;   //sets the left patrol range  NOTE: it is easiest to leave the leftBOundry as zero and place the enemy on the left most boundry
    public float rightBoundry;  //sets the right patrol range
    private Vector3 rightPos;
    private Vector3 leftPos;


    private bool skidding;
    private bool movingLeft;
    private bool movingRight;

    private Vector3 originalPosition;
    public bool offPatrol = false;    //after chasing the player, this boolean indicates that the enemy needs to return to its original position
    public bool patrolRight = true;
    public float wallCheckRayLength = .5f;  //adjusting this value will determine how far away from walls the enemy will approach

    private PlayerDetection detectPlayer;
    public bool challenged = false; // Determines if the enemy should be attacking the player or not

    Rigidbody enemy;
    private Transform Player;

    void Start () {
        enemy = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        leftPos = new Vector3(originalPosition.x - leftBoundry, originalPosition.y, 0f);
        rightPos = new Vector3(originalPosition.x + rightBoundry, originalPosition.y, 0f);
        enemy.isKinematic = false;

        Player = GameObject.Find("Yumi").transform;
        detectPlayer = this.GetComponentInChildren<PlayerDetection>();
    }
	
	void FixedUpdate () {
        //progress = Mathf.PingPong(Time.time * 0.1f, 1f);
        //currentPosition.x = Mathf.Lerp(leftBoundry, rightBoundry, progress * 1);

        leftPos = new Vector3(originalPosition.x - leftBoundry, 0f, 0f);
        rightPos = new Vector3(originalPosition.x + rightBoundry, 0f, 0f);
        Debug.DrawLine(new Vector3(rightPos.x, enemy.transform.position.y, 0f), new Vector3(leftPos.x, enemy.transform.position.y, 0f), Color.blue);

        Movement();
    }

    private void Update()
    {
        challenged = detectPlayer.PlayerDetected();
    }

    private void Movement()     //nextPosition is the current position(before being challenged)
    {

        if (challenged)
        {
            Vector3 directionOfPlayer = Player.transform.position - transform.position;
            moveToPosition(directionOfPlayer, AttackMaxSpeed);
            offPatrol = true;
        }
        else
        {
            if (offPatrol)  // offPatrol means that the enemy is returning to rightpos after attacking the player
            {

                if (enemy.transform.position.x >= rightPos.x)
                {
                    patrolRight = true;
                }
                if (enemy.transform.position.x <= leftPos.x)
                {
                    patrolRight = false;
                }
                else
                {
                    patrolRight = true;
                }
                offPatrol = false;

            }

            else //when offPatrol is false, the enemy will wander between rightpos and leftpos
            {
                if (patrolRight)      //This if-else block makes the enemy wander left to right based on left and right boundries
                {

                    Vector3 directionToNext = rightPos - enemy.transform.position;      //finds the direction vector to the right boundry
                    RaycastHit hit;
                    Debug.DrawLine(enemy.transform.position, enemy.transform.position + Vector3.right*wallCheckRayLength, Color.red);

                    if (directionToNext.x < 0 ||
                        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, wallCheckRayLength))     //else if the enemy has passed the right boundry
                    {
                        patrolRight = false;   //switch directions

                    }
                    else         //if the enemy is to the left of the boundry

                    {
                        moveToPosition(directionToNext, patrolMaxSpeed);        //move towards the boundry
                    }
                }
                else   //the enemy has now switched directions
                {
                    
                    Vector3 directionToNext = leftPos - enemy.transform.position;
                    RaycastHit hit;
                    Debug.DrawLine(enemy.transform.position, enemy.transform.position + Vector3.left*wallCheckRayLength, Color.red);

                    if (directionToNext.x > 0 ||
                        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, wallCheckRayLength))
                    {
                        patrolRight = true;
                    }
                    else
                    {
                        moveToPosition(directionToNext, patrolMaxSpeed);
                    }
                }
            }
        }
    }

    private void moveToPosition(Vector3 targetDirection, float speed)  //takes the direction of where the object needs to move (targetDirection = targetPos - currentPos)
    {
        if (targetDirection.x >= 0)
        {
            if (enemy.velocity.x < speed)
            {
                enemy.AddForce(Vector3.right * acceleration);
                //enemy.velocity = new Vector3(enemy.velocity.x + acceleration, enemy.velocity.y, 0f);
            }
        }
        else
        {
            if (enemy.velocity.x > -speed)
            {
                enemy.AddForce(Vector3.left * acceleration);
            }
        }
    }

}
