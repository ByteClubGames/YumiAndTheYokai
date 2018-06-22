/*
 * Programmer:   Keiran Glynn
 * Date Created: 03/09/2018 @  12:30 PM 
 * Last Updated: 03/09/2018 @  12:30 PM 
 * File Name:    HumanHealth.cs 
 * Description:  This script determines weather or not the human component of the player is alive. It also houses a value for the human's 
 * hit point value. It includes a public method that can be called on by enemies to cause damage (removal of hit points) to the player.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHealth : MonoBehaviour {

    private bool humanAlive = true;    
    public int humanHP = 6; // This is the number of health points that the player has

    void Update() // Constantly calls isAlive() to make sure the player hasn't died
    {
        if (humanHP <= 0) // If the player's health points drop below zero, then method 'SetAlive()' sets the value of humanAlive to false
        {
            SetAlive();
        }

        IsAlive();
    }

    public void TakeDamage (int damage)
    {
        humanHP -= damage;
    }

    private void IsAlive() // When called, method checks if the bool 'humanAlive' is false. If its false, the player dies
    {
        if (humanAlive != true)
        {
            Debug.Log("Human has Died"); // Tells the console that the player died
        }
    }

    private void SetAlive() // When called, will change the bool 'humanAlive' to false.
    {
        if (humanHP <= 0)
        {
            humanAlive = false;
        }

        else
        {
            humanAlive = true;
        }
    }
}
