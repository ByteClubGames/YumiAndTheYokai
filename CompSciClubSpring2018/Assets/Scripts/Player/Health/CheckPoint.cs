﻿/*
********************************************************************************
*Creator(s)........................................................Hunter Goodin
*Created..............................................................12/14/2018
*Last Modified...........................................@ 8:00 PM on 12/14/2018
*Last Modified by..................................................Hunter Goodin
*
*Description: This script handle's Yumi's health. When the DamageDealer() is
*             called from another function (IE: an enemy), it will decrement 
*             Yumi's health by decremented by the ammount passed. 
*             
*             When the health reaches zero, this script will call the
*             Checkpoint.CS script's Respawn() function and set Yumi's coords
*             back to the checkpoint's coords. It will then call this script's 
*             Respawn() function which will reset Yumi's HP back to 5; 
********************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameObject thisObj;
    private Vector3 checkpointPos = new Vector3(0, 0, 0);

    void Start()
    {
        thisObj = gameObject; 
        checkpointPos = new Vector3(thisObj.transform.position.x, thisObj.transform.position.y, thisObj.transform.position.z); 
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Yumi")
        {
            GameObject.Find("CheckpointController").GetComponent<CheckpointSystem>().lastCheckpointSetter(checkpointPos); 
        }
    }
}
