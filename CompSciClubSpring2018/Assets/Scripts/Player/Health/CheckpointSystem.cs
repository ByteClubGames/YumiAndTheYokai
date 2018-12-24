/*
********************************************************************************
*Creator(s)........................................................Hunter Goodin
*Created..............................................................12/14/2018
*Last Modified...........................................@ 2:55 PM on 12/21/2018
*Last Modified by..................................................Hunter Goodin
*
*Description: This script controls all of the checkpoints in the scene. When the
*             player ccollides with an object, lastCheckpoint will be set to the
*             coordinates sent to lastCheckpointSetter(). 
********************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    public GameObject initialCheckpoint;  	// To be populated with the checkpoint we want to be Yumi's initial checkpoint 
    public Vector3 lastCheckpointCoords;	// The coords of above ^^^ 

    void Start()	// On initialization... 
    {
        if (initialCheckpoint == null)	// if initialCheckpoint isn't populated... (but really it should always be populated but this is a fail safe...)
        {
            lastCheckpointCoords = new Vector3( GameObject.Find("Yumi").GetComponent<Transform>().position.x, 
                                                GameObject.Find("Yumi").GetComponent<Transform>().position.y, 
                                                GameObject.Find("Yumi").GetComponent<Transform>().position.z ); // Set initialCheckpoint to Yumi's coords 
        }
        else							// otherwise... 
        {
            lastCheckpointCoords = new Vector3(initialCheckpoint.transform.position.x, initialCheckpoint.transform.position.y, initialCheckpoint.transform.position.z);	// lastCheckpointCoords = the coords of the initialCheckpoint obj 
        }
        // when the eplayer dies, the player is set to inactive before the player TP's to the new coords 
    }

    public void lastCheckpointSetter(Vector3 newCoords)		// When this is called... 
    {
        lastCheckpointCoords = new Vector3(newCoords.x, newCoords.y, newCoords.z);	//the lastCheckpointCoords are set to the coords passed to this function 
    }
}

#region OldCode 
/* Commented all of this out because I changed everything */

//{
//    private Vector3 savePos; // The position of the active checkpoint
//    private Vector3 currPos; // The position of this checkpoint

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.gameObject.CompareTag("Human")) // Is the colliding game object even the player (Should the checkpoint consider activating?)
//        {
//            savePos = GameObject.Find("Human").GetComponent<RespawnManager>().GetPos(); // Position that the player would currently respawn at
//            currPos = this.transform.parent.position; // Position of this checkpoint

//            if (savePos != currPos) // If this checkpoint isn't already active, do action...
//            {
//                savePos = currPos; // Makes this checkpoint the active checkpoint
//                GameObject.Find("Human").GetComponent<RespawnManager>().SetPos(savePos); // Tells RespawnManager.cs to use this new position to respawn the player
//                Debug.Log("Checkpoint " + this.transform.parent.name + " is now active.");
//            }
//            else
//            {
//                Debug.Log("Checkpoint " + this.transform.parent.name + " is already active.");
//            }
//        }
//        else
//        {
//            //This collision isn't important
//        }
//    }
//}

#endregion