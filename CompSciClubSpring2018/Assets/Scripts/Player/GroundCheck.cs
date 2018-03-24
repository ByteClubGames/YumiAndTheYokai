/*
 * 
 * Author: Spencer Wilson
 * Date Created: 3/23/2018 @ 11:59 am
 * Date Modified: 3/23/2018 @ 11:59 am
 * Project: CompSciClubSpring2018
 * File: GroundCheck.cs
 * Description: Checks if the player is colliding with the ground or not.
 * 
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    public GameObject character; // Public game object named character that holds the reference to the attached character. 
    // Use this for initialization
    private void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Platforms")
        {
            if(character.name == "Human")
            {
                character.GetComponent<HumanJump>().SetIsGrounded(true);
            }
            else if(character.name == "Ferrox")
            {
                character.GetComponent<FerroxJump>().SetIsGrounded(true);
            }
        }
    }
}
