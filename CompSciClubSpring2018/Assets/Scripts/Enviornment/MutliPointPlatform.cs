/**
 * Author(s): Brenda De los Santos
 * Date Created: 7/27/2018
 * Last Modified: 7/27/2018
 * File Name: MultiPointPlatform.cs
 * Description: This program provides the functionslity of a moving platform that travels to multiple points.
 **/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutliPointPlatform : MonoBehaviour {

    [SerializeField]
    public float speed;
    
    private Vector3 startPos; //Holds the start position of the platform
    private int index;//The index of the positions array

    private Vector3 nextPos;//Holds the next position of the platform

    [SerializeField]
    private Transform platform; //Reference to the actual platform
    [SerializeField]
    private Transform[] transformPos; //Reference to all positions


    // Use this for initialization
    void Start () {
        
        //initializes the start and next position
        startPos = platform.localPosition;
        nextPos = transformPos[index].localPosition;
        
    }
	
	// Update is called once per frame
	void Update () {
        
        Move();
        
        //Checks if the platform is nesr the next position to change direction
        if (Vector3.Distance(platform.localPosition, nextPos) <= 0.1 ) 
        {
            if (index == transformPos.Length) //Can be modified to make the platform move back and forth
            {
                index = 0;
                StartPosition(); 
                nextPos = transformPos[index].localPosition;
            }

            ChangeDestination(index);
            
            index++;
        }
    }

    //Moves position of platform from current position to the next
    private void Move()
    {
        platform.localPosition = Vector3.MoveTowards(platform.localPosition, nextPos, speed * Time.deltaTime);

    }

    //Changes the destination of the platform
    private void ChangeDestination(int index)
    {
        nextPos = transformPos[index].localPosition;

    }
    
    //Repositions the platform to the orginal position when finished
    private void StartPosition()
    {
        platform.localPosition = startPos;
    }
}
