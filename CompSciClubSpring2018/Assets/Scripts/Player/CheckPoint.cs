/*
 * Programmer:   Keiran Glynn
 * Date Created: 05/19/2018 @ 12:17 PM
 * Last Updated: 05/19/2018 @ 12:17 PM
 * File Name:    CheckPoint.cs 
 * Description:  This script is attached to the checkpoint object. When the player enters the collider of the checkpoint, it first checks to see if this
 * current checkpoint is the one that is currently active (the checkpoint that has been most recently visited). If the checkpoint is not currently active,
 * then it is made active by saving its unique name to the RespawnManager.cs, as well as the players position when the checkpoint is activated. If the player 
 * revisits the checkpoint that is currently active, nothing will happen. A checkpoint can be activated mmore than once if it is visited non-consecutively.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {
    private string checkPointName;
    private string saveName;
    private Vector3 savePos;

	// Use this for initialization
	void Start ()
    {
        checkPointName = this.name;
	}
	
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Human"))
        {
            if (checkPointName != saveName)
            {
                savePos = GameObject.Find("Player").transform.position;
                GameObject.Find("Human").GetComponent<RespawnManager>().SetName(checkPointName);
                GameObject.Find("Human").GetComponent<RespawnManager>().SetPos(savePos);
                Debug.Log("Checkpoint " + checkPointName + " is now active.");
            }
            else
            {
                Debug.Log("Checkpoint " + checkPointName + " is already active.");
            }
        }
        else
        {
            //This collision isn't important
        }        
    }

    public string GetName()
    {
        return checkPointName;
    }
}
