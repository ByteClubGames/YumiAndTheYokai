/*
********************************************************************************
*Creator(s)........................................................Hunter Goodin
*Created..............................................................12/14/2018
*Last Modified...........................................@ 3:00 PM on 12/21/2018
*Last Modified by..................................................Hunter Goodin
*
*Description: This script will essentially be a controller for the whole 
*             checkpoint system. This should be attached to a checkpoint 
*             controller object. It can probably be Yumi or the existing scene 
*             controller. 
********************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private GameObject thisObj;								// An obj reference that will be populated with the obj this script is attatched to 
    private Vector3 checkpointPos = new Vector3(0, 0, 0);	// Initializing a Vector3 with base coords to be changed in Start() 

    void Start()										// On initialization... 
    {
        thisObj = gameObject; 								// thisObj = the object this script is attatched to 
        checkpointPos = new Vector3(thisObj.transform.position.x, thisObj.transform.position.y, thisObj.transform.position.z); // checkpointPos is set to the coords of thisObj 
    }

    public void OnTriggerEnter(Collider col)			// When an obj collides with the trigger... 
    {
        if(col.gameObject.name == "Yumi")					// Check if the collision's name is "Yumi" 
        {
            GameObject.Find("CheckpointController").GetComponent<CheckpointSystem>().lastCheckpointSetter(checkpointPos); 	// Search for an obj called "CheckpointController, get the CheckpointSystem script attached to it, call the lastCheckpointSetter function and pass the value checkpointPos"
        }
    }
}
