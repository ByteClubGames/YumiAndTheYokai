/*
 *  Authors:  Darrell Wong
 *  Date Created: 10/9/2018
 *  Last Modified: 10/11/2018
 *  EnemyPig.cs
 *  Description: Script defines the rigidbody movement of the enemy pig to patrol between 2 points(leftBoundry & rightBoundry) and chase the player
 *  Calls on PlayerDetection.cs to actually detect the player's presence.
 *  
 *  Instructions: 
 *  1. Place enemy then set rightBoundry and leftBoundry. These boundries dictate how far to the left and right the enemy will patrol.
 *  2. Adjust player detection collider size.
 *  3. Max speeds and accelerations can be adjusted to taste.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPig : MonoBehaviour {

    [Header("Physics/ Enemy Attributes")]
    public float AttackMaxSpeed;
    public float patrolMaxSpeed;
    public float acceleration;          //determines magnitude of force applied to enemy
    public float leftBoundry;           //sets the left patrol range  
    public float rightBoundry;          //sets the right patrol range
    private Vector3 leftPos;            //location of left boundry
    private Vector3 rightPos;           //location of right boundry
    public float wallCheckRayLength = .5f;     //adjusting this value will determine how far away from walls the enemy will approach
    private Vector3 originalPosition;   

    [Header("Boolean triggers")] 

    private bool skidding;          //currently not being used
    private bool movingLeft;        //currently not being used
    private bool movingRight;       //currently not being used

    public bool challenged = false;           // Determines if the enemy should be attacking the player or not
    public bool patrolRight = true;

    [Header("Objects")]
    private PlayerDetection detectPlayer;
    Rigidbody enemy;
    private Transform Player;

    void Start () {
        enemy = GetComponent<Rigidbody>();
        enemy.isKinematic = false;

        Player = GameObject.Find("Yumi").transform;
        detectPlayer = this.GetComponentInChildren<PlayerDetection>();

        originalPosition = transform.position;
    }
	
	void FixedUpdate () {
        leftPos = new Vector3(originalPosition.x - leftBoundry, 0f, 0f);    //set leftpos
        rightPos = new Vector3(originalPosition.x + rightBoundry, 0f, 0f);  //set rightpos

        Debug.DrawLine(new Vector3(rightPos.x, enemy.transform.position.y, 0f), new Vector3(leftPos.x, enemy.transform.position.y, 0f), Color.blue);

        Movement();
    }

    private void Update()
    {
        challenged = detectPlayer.PlayerDetected();             //update challenged boolean every frame
    }

    private void Movement()     
    {

        if (challenged)                                         //when the player is within range
        {
            Vector3 directionOfPlayer = Player.transform.position - transform.position;
            moveToPosition(directionOfPlayer, AttackMaxSpeed);  //move towards the player
        }
        else
        {

            if (patrolRight)                               //This if-else block makes the enemy wander left to right based on left and right boundries
            {

                Vector3 directionToNext = rightPos - enemy.transform.position;      //finds the direction vector to the right boundry
                RaycastHit hit;
                Debug.DrawLine(enemy.transform.position, enemy.transform.position + Vector3.right * wallCheckRayLength, Color.red);

                if (directionToNext.x < 0 ||
                    Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, wallCheckRayLength))  //else if the enemy has passed the right boundry
                {
                    patrolRight = false;   //switch directions

                }
                else                                        //if the enemy is to the left of the boundry

                {
                    moveToPosition(directionToNext, patrolMaxSpeed);//move towards the boundry
                }
            }
            else                                            //the enemy has now switched directions
            {

                Vector3 directionToNext = leftPos - enemy.transform.position;
                RaycastHit hit;
                Debug.DrawLine(enemy.transform.position, enemy.transform.position + Vector3.left * wallCheckRayLength, Color.red);

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

    private void moveToPosition(Vector3 targetDirection, float speed)  //applies force towards targetDirection (targetDirection = targetPos - currentPos)
    {
        if (targetDirection.x >= 0)                             //if targetDirection is to the right
        {
            if (enemy.velocity.x < speed)                       //if enemy speed is less than max speed 
            {
                enemy.AddForce(Vector3.right * acceleration);   //add force to the right
            }
        }
        else                                                    // if target direction is to the left
        {
            if (enemy.velocity.x > -speed)
            {
                enemy.AddForce(Vector3.left * acceleration);    //add force to the left
            }
        }
    }

}
