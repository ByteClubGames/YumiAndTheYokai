/*
 * Programmer:   Keiran Glynn
 * Date Created: 05/19/2018 @ 12:17 PM
 * Last Updated: 05/19/2018 @ 12:17 PM
 * File Name:    CheckPoint.cs 
 * Description:  This script is attached to the checkpoint object. When the player enters the collider of the checkpoint, it first checks to see if this
 * current checkpoint is the one that is currently active (the checkpoint that has been most recently visited). If the checkpoint is not currently active,
 * then it is made active by saving its unique position to the RespawnManager.cs. If the player revisits the checkpoint that is currently active, nothing 
 * will happen. A checkpoint can be activated mmore than once if it is visited non-consecutively.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {
    private Vector3 savePos; // The position of the active checkpoint
    private Vector3 currPos; // The position of this checkpoint
	
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Human")) // Is the colliding game object even the player (Should the checkpoint consider activating?)
        {
            savePos = GameObject.Find("Human").GetComponent<RespawnManager>().GetPos(); // Position that the player would currently respawn at
            currPos = this.transform.parent.position; // Position of this checkpoint

            if (savePos != currPos) // If this checkpoint isn't already active, do action...
            {
                savePos = currPos; // Makes this checkpoint the active checkpoint
                GameObject.Find("Human").GetComponent<RespawnManager>().SetPos(savePos); // Tells RespawnManager.cs to use this new position to respawn the player
                Debug.Log("Checkpoint " + this.transform.parent.name + " is now active.");
            }
            else
            {
                Debug.Log("Checkpoint " + this.transform.parent.name + " is already active.");
            }
        }
        else
        {
            //This collision isn't important
        }
    }
}
