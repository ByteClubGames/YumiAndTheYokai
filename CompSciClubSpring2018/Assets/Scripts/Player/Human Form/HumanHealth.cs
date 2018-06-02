/*
 * Programmer:   Keiran Glynn
 * Date Created: 03/09/2018 @  12:30 PM 
 * Last Updated: 05/19/2018 @  12:30 AM 
 * File Name:    HumanHealth.cs 
 * Description:  This script determines weather or not the human component of the player is alive. It also houses a value for the human's 
 * hit point value. It includes a public method that can be called on by enemies to cause damage (removal of hit points) to the player.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHealth : MonoBehaviour {

    private bool humanAlive = true;
    public int maxHealth = 6;
    public int humanHP = 6; // This is the number of health points that the player has

    void Update() // Constantly calls isAlive() to make sure the player hasn't died
    {
        if (humanHP <= 0) // If the player's health points drop below zero, then method 'SetAlive()' sets the value of humanAlive to false
        {
            SetAlive();
        }

        IsAlive();
    }

    public void TakeDamage(int damage) // Can be called on by other classes to remove HP from the player.
    {
        if (damage >= 0)
        {
            humanHP -= damage;
        }
        else
        {
            Debug.Log("Enter in a positive damage amount for damgage taken to human!");
        }
    }

    public void giveHealth(int health) // Can be called on by other classes to remove HP from the player.
    {
        if (humanHP + health > maxHealth)
        {
            humanHP = maxHealth;
        }
        else if (health < 0)
        {
            Debug.Log("Enter in a positive health amount for health given to human!");
        }
        else
        {
            humanHP += health;
        }
    }

    private void IsAlive() // When called, method checks if the bool 'humanAlive' is false. If its false, the player dies
    {
        if (humanAlive != true)
        {
            Respawn();
        }
    }

    private void SetAlive() // When called, will change the bool 'humanAlive' to false.
    {
        if (humanHP <= 0)
        {
            humanHP = maxHealth;
            humanAlive = false;
        }

        else
        {
            humanAlive = true;
        }
    }

    private void Respawn()
    {
        //humanHP = maxHealth;
        Debug.Log("Human has Died"); // Tells the console that the player died
        GameObject.Find("Human").transform.position = GameObject.Find("Human").GetComponent<RespawnManager>().GetPos();
    }
}
