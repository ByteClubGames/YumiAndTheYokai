/* 
 * 
 * Author: Spencer Wilson
 * Date Created: 03/6/2018 @ 4:33 pm
 * Date Modified: 03/10/2018 @ 10:20 pm
 * Project: CompSciClubSpring2018
 * File: FollowActingCharacter.cs
 * Description: This script houses the code for the Main Camera to follow the acting character.
 * ////////////////////////////////////////////////////////////////////////////////////////////
 * TERMS: Acting Character: The current character the player is playing as.
 * 
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowActingCharacter : MonoBehaviour {

    public GameObject actingCharacter; // Stores the game object of the current acting character.
    private Transform actingCharacterTransform; // Holds the transform of the acting character.
    private Vector3 newPos; // Creating a variable that stores the new position that the camera moves too.

	void LateUpdate () // LateUpdate is called after Update and FixedUpdate have been called. Good for tracking stuff.
    {
        actingCharacterTransform = actingCharacter.transform; // Assigns the actingCharacterTransform variable with the value stored in actingCharacter.transform.
        newPos = new Vector3(actingCharacterTransform.position.x, actingCharacterTransform.position.y + 1.2f, -6.15f); // Getting the x,y coordinates of the current acting character and a z value of -20 and storing it into newPos. 
        gameObject.transform.position = newPos; // Setting the currently selected game object's position to that of the value stored in newPos.
	}

    public void setActingCharacter(GameObject newActingCharacter) // Public function that returns void named setActingCharacter. This function takes a GameObject datatype as an arguement.
    {
        actingCharacter = newActingCharacter; // Assigns newActingCharacter to actingCharacter.
    }
}
