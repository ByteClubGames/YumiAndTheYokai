/**
 * Author: Brenda De los Santos
 * Date Created: 7/25/18
 * Last Modified: 7/26/18 
 * File Name:SimpleMovingPlatform.cs
 * Description: This program enables a platform to travel between two points. 
 * How to use: To make the moving platform, create an empty gameobject and add this script. In that gameobject, make a cube 
 * and another empty gameobject. Be sure to only change the parent position when relocating the platform and zero-out positions 
 * of the children. Next, position the child gameobject to where you would like the platform to move. In the Inspector tab of 
 * the parent, place the platform in the Child Transform slot and the child empty gameobject in Transform B slot.
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovingPlatform : MonoBehaviour {

    private Vector3 originPos;//Starting/current position
    private Vector3 posB;//A position
    private Vector3 nextPos;//Used to store the next position
    //Vector3[] positions = new Vector3[2];

    [SerializeField]
    //Sets the speed at which the platform moves
    public float speed; 
    
    [SerializeField]
    //Used to obtain the position of the platform
    private Transform childTransform;

    [SerializeField]
    //Used to obtain the position of the game object
    private Transform transformB;

	// Use this for initialization
	void Start ()
    {
        originPos = childTransform.localPosition;//Retrieves the platform's current position and assigns that to originPos
        posB = transformB.localPosition;//Retrieves the position of a game object and assigns that to posB
        nextPos = posB;
	}
	
	// Update is called once per frame
	void Update () {

        //Moves the platform from its original position to the next
        Move();

        //Determines whether the platform need to change position
        if (Vector3.Distance(childTransform.localPosition, nextPos) <= 0.1)
        {
            ChangeDestination();
        }
	}

    //Move() makes the platform move from it original position to the next
    private void Move()
    {
        childTransform.localPosition = Vector3.MoveTowards(childTransform.localPosition, nextPos, speed * Time.deltaTime);
    }

    //ChangeDestination() determines if the platform needs to move from its current position to the next
    private void ChangeDestination()
    {
        if (nextPos != originPos)
        {
            nextPos = originPos;
        }
        else
        {
            nextPos = posB;
        }
    }
}
