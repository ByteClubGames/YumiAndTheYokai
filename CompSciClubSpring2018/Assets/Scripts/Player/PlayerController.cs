/*
 * 
 * Authors: Spencer Wilson
 * Date Created: 3/5/2018 @ 3:11 pm
 * Date Modified: 3/5/2018 @ 5:11 pm
 * Project: CompSciClubSpring2018
 * File: PlayerController.cs
 * Description: This class controls the interaction between the player's physical and yokai form as well as any extraneous things.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject humanGameObject; // Public game object that holds the little girl game object.
    public GameObject ferroxGameObject; // Public game object that holds the ferrox's game object.
    private bool isProjecting; // Creating a private boolean variable that represents whether or not the player is projecting.
	// Use this for initialization
	public void Start () {
        isProjecting = false; // Initializing isProjecting to be false at the start of the level.
	}
	
	// Update is called once per frame
	public void Update ()
    {
        CheckInput(); // Checks if input has been recieved from device to determine the boolean value of isProjecting.
        CheckIfProjecting(); // Determines what happens between the player's projections and other such things.
	}

    public void CheckInput() // Check input for whether or not player has seleted to project or not.
    {
        if(Input.GetKeyUp("g")) // When "g" is pressed, switch the isProjecting value to it's opposite boolean value.
        {
            isProjecting = !isProjecting;
        }
    }

    private void CheckIfProjecting() // Checks whether or not player is projecting and switches the to corresponding form.
    {
        if(isProjecting) // If the player is projecting, switch to the yokai form.
        {
            ferroxGameObject.SetActive(true); // Sets the ferroxGameObject to be active.
            // set input for if player is projecting
            Debug.Log("Player is projecting");
        }
        else // If player isn't projecting, switch to the human form.
        {
            ferroxGameObject.SetActive(false); // Sets the ferroxGameObject to be at an inactive state.
            ferroxGameObject.transform.position = humanGameObject.transform.position; // Resetting the ferrox's position to the human form's position for next projection.
            Debug.Log("Player is not projecting");
        }
    }



}
