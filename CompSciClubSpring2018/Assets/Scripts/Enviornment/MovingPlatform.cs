/*
 * Programmer:   Tony Silva 
 * Date Created: 03/12/2018 @  8:15 PM 
 * Last Updated: 03/20/2018 @  7:15 PM 
 * File Name:    MovingPlatform.cs 
 * Description:  This script will allow for a "platform" to move between two points
 */

// PUT HEADER UP HERE AND COMMENT YOUR CODE (THE MORE THOUROUGHLY, THE BETTER)I'm Tony and I need to comment my code. Oh yeaaaa
//^^ why yall hating?? ^^ XD

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public GameObject platform; //this is the Platform GameObject

    public float moveSpeed; //this is the speed at which the platform moves

    public Transform currentPoint; //this shows the point of where the platform is at any given frame

    public Transform[] points; //an array of points to which the platform is moving to

    public int pointSelection;

	// Use this for initialization
	void Start () {
        currentPoint = points[pointSelection]; //this changes the currentPoint variable to the point it needs to move to
	}
	
	// Update is called once per frame
	void Update () {

        platform.transform.position = 
            Vector3.MoveTowards(platform.transform.position, 
            currentPoint.position, Time.deltaTime * moveSpeed); //this creates a vector on which the platform moves

        if(platform.transform.position == currentPoint.position)//this checks to see which point the platform is at or moving to
        {
            pointSelection++; //this changes the variable in the array to move up 1 making the platform to move to the next point

            if(pointSelection == points.Length) //this checks to see if the the array is at the max amount of points so it does not go to a point that is non-existant
            {
                pointSelection = 0;//this sets the point back to 0
            }

            currentPoint = points[pointSelection]; //this changes the currentPoint variable to the point it needs to move to
        }

	}
}
