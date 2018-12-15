/*
 *  Authors: Ivonne Lopez
 *  Date Created: 9/21/2018
 *  Last Modified: 9/21/2018
 *  FlyingEnemy.cs
 *  Description: Script responsible for flap movement and attack patterns. Calls on PlayerDetection.cs to actually detect the player's presence.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Horizontal Flight Boundary Points")]
    public Vector3 pos1 = new Vector3(-4f, 5f, 0f);
    public Vector3 pos2 = new Vector3(4f, 5f, 0f);

    [Header("Physics/ Enemy Attributes")]
    public float horizontalSpeed;
    public float verticalSpeed;
    public float amplitude;
    public float attackSpeed = 0.1f;

    private Transform Player;
    private Vector3 directionOfPlayer;
    private Vector3 tempPosition; // This position vector is modified algebraically, and then the enemy's actual position is set equal to this temp position    
    private float progress = 0f; // Variable determining how the percentage completion of the Lerp for the horizontal component of movement
    private bool challenged = false; // Determines if the enemy should be attacking the player or not
    private PlayerDetection detectPlayer;


    // Use this for initialization
    void Awake ()
    {
        //default position
        tempPosition = transform.position;
        Player = GameObject.Find("Yumi").transform;
        detectPlayer = this.GetComponentInChildren<PlayerDetection>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        progress = Mathf.PingPong(Time.time * 0.1f, 1f);
        tempPosition.x = Mathf.Lerp(pos1.x, pos2.x, progress * 1);
        tempPosition.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude;

        Movement(tempPosition.x, tempPosition.y); 
	} 

    private void Movement(float x, float y)
    {
        challenged = detectPlayer.PlayerDetected();

        if (challenged)
        {
            directionOfPlayer = Player.transform.position - transform.position;
            directionOfPlayer = directionOfPlayer.normalized;
            transform.Translate(directionOfPlayer * attackSpeed, Space.World);            
        }
        else
        {
            transform.position = new Vector3(x, y, 0f);
        }        
    }   
}
