/*
 * Programmer:   Keiran Glynn
 * Date Created: 05/19/2018 @ 12:17 PM
 * Last Updated: 05/19/2018 @ 12:17 PM
 * File Name:    RespawnManager.cs 
 * Description:  This script is attached to the player, and is responsible for holding the data for the player's position when a checkpoint is activated.
 * When the player dies, the HumanHealth.cs script will utilize the data from this script in order to decide which checkpoint to use when respawning the 
 * player, and where that checkpoint was based on the saved position of the player.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour {
    private Vector3 savedPos; //Holds the position of where the player will respawn

	// Use this for initialization
	void Start ()
    {
        savedPos = GameObject.Find("Player").transform.position; // If the player dies before reaching a checkpoint, they have a respawn location at start
	}

    public void SetPos(Vector3 position) // Used by CheckPoint.cs to set the respawn position
    {
        savedPos = position;
    }

    public Vector3 GetPos() // Called on by CheckPoint.cs to get the saved respawn position
    {
        return savedPos;
    }
}
