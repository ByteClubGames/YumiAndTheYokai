/*
********************************************************************************
*Creator(s)........................................................Hunter Goodin
*Created..............................................................12/14/2018
*Last Modified...........................................@ 2:55 PM on 12/21/2018
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
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class YumiHealthSystem : MonoBehaviour
{
    public int health = 5;														// Base health is set to 5 
    public GameObject playerObj;												// To be populated with the player obj 

    public string healthPath = "Assets/Scripts/Player/Health/CurrentHP.txt";	// This is what the health's .txt is. We saved the health in a .txt so the palyer can keep the current 

    public GameObject healthParent; 											// To be populated by the HealthAnimParent obj. This is for displaying health 
    public Transform[] hpArr;								                    // An empty array 

    void Start()	// On initialization... 
    {
        StreamReader reader = new StreamReader(healthPath);				// Creating a reder obj that reads from healthPath 
        healthPath = reader.ReadToEnd();								// Read the file 
        Convert.ToInt32(healthPath);									// Convert that str to an int 
        health = Int32.Parse(healthPath);								// Set the health int to the converted str 
        reader.Close();													// Close the reder 

        healthPath = "Assets/Scripts/Player/Health/CurrentHP.txt";		// Reset the .txt back to the path since Convert.ToInt actually litterally changes the value to the int for a bit 

        healthParent = GameObject.Find("HealthAnimParent");				// Set healthParent to the obj named "HealthAnimParent"

        hpArr = healthParent.GetComponentsInChildren<Transform>();		// Set hpArr to all the children in healthParent 

        ArrFix(); 														// hpArr is initially a little messed up and this function fixes it 
    }

    // hpArr is initially really bad... The first element is set to the parent obj. Then it grabs all the children's children and sorts them in front  
    // of their parents in the arr. We don't like that. We want to be able to toggle only the parent. The simplest solution is to just fix the arr 
    // so that's what this function does. Skips the first element of the hpArr and sorts every other child of after that (so it only grabs the parents). 
    // This will need to be changed if the heart model changes but it is an easy fix and the heart model shouldn't change anyways. 

    void ArrFix()
    {
        int newL = (hpArr.Length - 1) / 2;			// Calculate the new length of the newArr 
        Transform[] newArr = new Transform[newL];	// Create an empty array with a size of newL 
        int temp = 1; 								// This is for skipping some elements of the current arr 

        for (int i = 0; i < newL; i++)				// As long as i is less than the newL 
        {
            newArr[i] = hpArr[temp]; 				// Element i in the newArr is set to the temp element of hpArr 
            temp++;	// increment twice (this can be changed later if the health model becomes more complicated... just add more of these.)
            temp++; 
        }

        hpArr = newArr; // Set the hpArr to the newArr and we're all set 
    }

    void Update()
    {
        if ( health <= 0 ) // if health is less than or equal to 0 
        {
            gameObject.SetActive(false); 	// Setting the player to inactive for a sec while we change her location 
            playerObj.transform.position = new Vector3(GameObject.Find("CheckpointController").GetComponent<CheckpointSystem>().lastCheckpointCoords.x,
                                                        GameObject.Find("CheckpointController").GetComponent<CheckpointSystem>().lastCheckpointCoords.y,
                                                        GameObject.Find("CheckpointController").GetComponent<CheckpointSystem>().lastCheckpointCoords.z);	// TP her to the last checkpoint 
            gameObject.SetActive(true); 	// Set the player back to active 
            ResetHealth(); 					// Reset the health back to what we want her max health to be 
        }

        ArrLoop();	// This is for displaying the health 
    }

    public void HealthWriter() // Whatever causes the player to change scenes should call this function. 
    {
        StreamWriter writer = new StreamWriter(healthPath);		// Create a new writer obj that writes to healthPath 
        writer.Write(health);									// Write health to it 
        writer.Close();											// Close the writer 
    }

    // Right now, this sets some of the objects invisible. Later on, I believe that Brad has an animation for the health container breaking. 
    // but now that all of the heart containers are sorted into an array and this sorts through them, you can run the code that plays the 
    // animation instead of setting them to inactive. 
    void ArrLoop() // makes a certain ammount visable 
    {
        int b = 1; 	// this is set to 1 for now 

        for (int i = 1; i <= health; i++)		// as long as i is less than Yumi's current health ]... 
        {
            hpArr[i].gameObject.SetActive(true);	// Make that many heart containers visable (in order) 
            b = i; 									// Set b to whatever the last heart container changed was 
        }	

        for (int j = b; j < hpArr.Length; j++)	// Depending on how many the health is left... 
        {
            hpArr[j].gameObject.SetActive(false);	// Set the rest as invisible. 
        }
        // There are 10 heart containers in total. The max hHP is set to 5... However, if we decide to have a higher or lower max HP in the future 
        // or if we want Yumi to have some temporary hearts in the future, the option is there. Dan said that it's unlikely to go past 10 so I left it at that. 
    }

    public void DamageDealer(int dam)	// Called when we want to deal damage to Yumi 
    {
        health -= dam; 	// Decrement health by dam amount 
    }

    public void ResetHealth()	// Called when we want to set Yumi's health back to 5 
    {
        health = 5; 	// Set health to 5 
    }
}