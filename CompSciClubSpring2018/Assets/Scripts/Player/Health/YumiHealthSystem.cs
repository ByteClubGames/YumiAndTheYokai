/*
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

public class YumiHealthSystem : MonoBehaviour
{
    public int health = 5;
    public GameObject playerObj;

    void Update()
    {
        if ( health <= 0 )
        {
            gameObject.SetActive(false); 
            playerObj.transform.position = new Vector3( GameObject.Find("CheckpointController").GetComponent<CheckpointSystem>().lastCheckpointCoords.x,
                                                        GameObject.Find("CheckpointController").GetComponent<CheckpointSystem>().lastCheckpointCoords.y,
                                                        GameObject.Find("CheckpointController").GetComponent<CheckpointSystem>().lastCheckpointCoords.z );
            gameObject.SetActive(true); 
            ResetHealth(); 
        }
    }

    public void DamageDealer(int dam)
    {
        health = health - dam; 
    }

    public void ResetHealth()
    {
        health = 5; 
    }
}
