/*
********************************************************************************
*Creator(s)........................................................Hunter Goodin
*Created..............................................................12/14/2018
*Last Modified...........................................@ 8:00 PM on 12/14/2018
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
    public GameObject initialCheckpoint;  
    public Vector3 lastCheckpointCoords;

    void Start()
    {
        if (initialCheckpoint == null)
        {
            lastCheckpointCoords = new Vector3( GameObject.Find("Yumi").GetComponent<Transform>().position.x, 
                                                GameObject.Find("Yumi").GetComponent<Transform>().position.y, 
                                                GameObject.Find("Yumi").GetComponent<Transform>().position.z ); 
        }
        else
        {
            lastCheckpointCoords = new Vector3(initialCheckpoint.transform.position.x, initialCheckpoint.transform.position.y, initialCheckpoint.transform.position.z);
        }
        // when the eplayer dies, set the player to inactive before it moves position, then set it to active again 
    }

    public void lastCheckpointSetter(Vector3 newCoords)
    {
        lastCheckpointCoords = new Vector3(newCoords.x, newCoords.y, newCoords.z); 
    }
}

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
