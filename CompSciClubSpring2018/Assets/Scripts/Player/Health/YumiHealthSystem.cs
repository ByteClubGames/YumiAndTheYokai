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
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class YumiHealthSystem : MonoBehaviour
{
    public int health = 5;
    public GameObject playerObj;

    public string healthPath = "Assets/Scripts/Player/Health/CurrentHP.txt";

    public GameObject healthParent; 
    public Transform[] hpArr = new Transform[0];

    void Start()
    {
        StreamReader reader = new StreamReader(healthPath);
        healthPath = reader.ReadToEnd();
        Convert.ToInt32(healthPath);
        health = Int32.Parse(healthPath);
        reader.Close();

        healthPath = "Assets/Scripts/Player/Health/CurrentHP.txt";

        healthParent = GameObject.Find("HealthAnimParent");

        hpArr = healthParent.GetComponentsInChildren<Transform>();

        ArrFix(); 
    }

    void ArrFix()
    {
        int newL = (hpArr.Length - 1) / 2;
        Transform[] newArr = new Transform[newL];
        int temp = 1; 

        for (int i = 0; i < newL; i++)
        {
            newArr[i] = hpArr[temp];
            temp++;
            temp++; 
        }

        hpArr = newArr; 
    }

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

        ArrLoop();
    }

    public void HealthWriter() // Whatever causes the player to change scenes should call this function. 
    {
        StreamWriter writer = new StreamWriter(healthPath);
        writer.Write(health);
        writer.Close();
    }

    void ArrLoop() // makes a certain ammount visable 
    {
        int b = 1; 

        for (int i = 1; i <= health; i++)
        {
            hpArr[i].gameObject.SetActive(true);
            b = i; 
        }


        for (int j = b; j < hpArr.Length; j++)
        {
            hpArr[j].gameObject.SetActive(false);
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