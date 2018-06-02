/*
 * Programmer:   Keiran Glynn
 * Date Created: 03/09/2018 @  3:07 PM 
 * Last Updated: 03/19/2018 @  12:20 AM 
 * File Name:    FerroxHealth.cs 
 * Description:  This script determines weather or not the Ferrox component of the player is alive. It includes a public method that can be  
 * called on by enemies to cause damage (removal of hit points) to the Ferrox. The Ferrox has a int value for its number of hit points (1 hp)
 * that will be depleted upon taking damage. When the Ferrox dies, the player will once again become active, and the Ferrox's postion will be 
 * reset.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerroxHealth : MonoBehaviour {

    private bool ferroxAlive = true;
    public int maxHealth = 1;
    public int ferroxHP = 1; // This is the number of health points that the ferrox has

    private GameObject playerGameObject;
    private GameObject humanGameObject; // Public game object that holds the little girl game object.
    private GameObject ferroxGameObject; // Public game object that holds the ferrox's game object.

    void Start()
    {
        playerGameObject = GameObject.Find("Player");
        humanGameObject = GameObject.Find("Human");
        ferroxGameObject = GameObject.Find("Ferrox");
    }

    void Update() // Constantly calls isAlive() to make sure the player hasn't died
    {
        SetAlive();
        IsAlive();
    }

    public void TakeDamage(int damage) // Can be called on by other classes to remove HP from the player.
    {
        if(damage >= 0)
        {
            ferroxHP -= damage;
        }
        else
        {
            Debug.Log("Enter in a positive damage amount for damgage taken to Astral!");
        }
    }

    public void giveHealth(int health) // Can be called on by other classes to remove HP from the player.
    {
        if (ferroxHP + health > maxHealth)
        {
            ferroxHP = maxHealth;
        }
        else if (health < 0)
        {
            Debug.Log("Enter in a positive health amount for health given to Astral!");
        }
        else
        {
            ferroxHP += health;
        }
    }

    private void IsAlive() // When called, method checks if the bool 'ferroxAlive' is false. If its false, the ferrox dies
    {
        if (ferroxAlive != true)
        {
            ferroxHP = maxHealth; // Resets the Ferrox's health value to 1 HP
            ferroxGameObject.SetActive(false); // Sets the ferroxGameObject to be at an inactive state.
            humanGameObject.GetComponent<HumanMovement>().SetIfActive(true); // Turns back on the human's movement.
            ferroxGameObject.transform.position = new Vector2(humanGameObject.transform.position.x + 1f, humanGameObject.transform.position.y); // Resetting the ferrox's position to the human form's position for next projection.
            playerGameObject.GetComponent<YokaiSwitcher>().SetIsProjecting(); // Makes Ferrox dissapear back into player
            Debug.Log("Ferrox has Died"); // Tells the console that the ferrox died :'(            
        }
    }

    private void SetAlive() // If the ferrox's health points drop below zero, then method 'SetAlive()' sets the value of ferroxAlive to false
    {
        if (ferroxHP <= 0) 
        {
            ferroxAlive = false;
        }

        else
        {
            ferroxAlive = true;
        }
    }

}
