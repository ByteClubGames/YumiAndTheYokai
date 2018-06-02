/*
 * Programmer:   Keiran Glynn
 * Date Created: 05/19/2018 @ 12:17 PM
 * Last Updated: 05/19/2018 @ 12:17 PM
 * File Name:    RespawnManager.cs 
 * Description:  This script is attached to the player, and is responsible for holding the data for the player's position when a checkpoint is activated.
 * It also holds data for the name of the checkpoint that was last activated. When the player dies, the HumanHealth.cs script will utilize the data from this
 * script in order to decide which checkpoint to use when respawning the player, and where that checkpoint was based of the saved position of the player.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour {
    private string checkPointName = "noName";
    private Vector3 savedPos;

	// Use this for initialization
	void Start () {
        checkPointName = "noName";
        //savedPos = GameObject.Find("Player").transform.position;
	}	

    public void SetName(string name)
    {
        checkPointName = name;
    }

    public string GetName()
    {
        return checkPointName;
    }

    public void SetPos(Vector3 position)
    {
        savedPos = position;
    }

    public Vector3 GetPos()
    {
        return savedPos;
    }
}
