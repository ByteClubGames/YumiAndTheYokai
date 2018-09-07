/*
 * 
 * Authors: Spencer Wilson, Keiran Glynn, Hunter Goodin
 * Date Created:  03/05/2018 @  3:11 PM
 * Date Modified: 07/31/2018 @  3:30 PM
 * Project: CompSciClubSpring2018
 * File: PlayerController.cs
 * Description: This class controls the switching between the yokai and the human.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YokaiSwitcher : MonoBehaviour
{
    private float spawnOffset;

    private GameObject human; // Public game object that holds the human game object. 
    public GameObject yokai; // Public game object that holds a reference to the ferrox prefab.
    //private bool facingRight; // Declaring a private boolean variable that determines whether the player is facing left or right.
    //private bool isProjecting; // Declaring a private boolean variable that represents whether or not the player is projecting.


    private void Start()
    {
        human = GameObject.Find("Player-Human");        
    }

    public void SetSpawnOffset(bool spawnRight)
    {
        float spawnOffset = spawnRight ? 1.0f : -1.0f;
    }

    public void ProjectYokai()
    {
        Vector3 spawnLocation;        

        spawnLocation = new Vector3(human.transform.position.x + spawnOffset, human.transform.position.y, human.transform.position.z); // Instantiates the 
        Instantiate(yokai, spawnLocation, Quaternion.identity);
    }

    //public void SetProjection()
    //{
    //    SetIsProjecting(); // Calls on SetIsProjecting().
    //    SetProjectionState(); // Calls on SetProjectionState().
    //}

    //public void SetIsProjecting() // Sets isProjecting to the opposite boolean value it currently is at.
    //{
    //    isProjecting = !isProjecting;
    //}

    //public void SetFacingRight(bool input) // Sets facingRight to the value stored in bool.
    //{
    //    facingRight = input;
    //}    

}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// KEEPING CODE FOR REFERENCE
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// public Transform spawnerLeft; // Public transform that holds where the ferrox will spawn.
// public Transform spawnerRight;
// public bool activeSpawner = true; 
// public Vector3 spawnerLoc; 


// Use this for initialization
//public void Start ()
//   {
//       SetIsProjecting(); // Initializing isProjecting to be false at the start of the level.
//}

// Update is called once per frame
//public void Update ()
//   {
//       CheckInput(); // Checks if input has been recieved from device to determine the boolean value of isProjecting.
//       CheckIfProjecting(); // Determines what happens between the player's projections and other such things.
//}

//   public void CheckInput() // Check input for whether or not player has seleted to project or not.
//   {
//       if(Input.GetKeyUp("g")) // When "g" is pressed, switch the isProjecting value to it's opposite boolean value.
//       {
//           SetIsProjecting();
//       }
//   }

//   private void CheckIfProjecting() // Checks whether or not player is projecting and switches the to corresponding form.
//   {
//       if(isProjecting) // If the player is projecting, switch to the yokai form.
//       {
//           humanGameObject.GetComponent<HumanMovement>().SetIfActive(false); // Turns off the human's movement.
//           ferroxGameObject.SetActive(true); // Sets the ferroxGameObject to be active.
//           GameObject.Find("Main Camera").GetComponent<FollowActingCharacter>().setActingCharacter(ferroxGameObject); // Sets the acting character to the ferrox.
//           Debug.Log("Player is projecting");
//       }
//       else // If player isn't projecting, switch to the human form.
//       {
//           ferroxGameObject.SetActive(false); // Sets the ferroxGameObject to be at an inactive state.
//           humanGameObject.GetComponent<HumanMovement>().SetIfActive(true); // Turns back on the human's movement.
//           ferroxGameObject.transform.position = new Vector2(humanGameObject.transform.position.x + 1f, humanGameObject.transform.position.y); // Resetting the ferrox's position to the human form's position for next projection.
//           Debug.Log("Player is not projecting");
//       }
//   }

//   /* This script modularizes Spencer's script for swapping the value of 'isProjecting' between true and false. It is called on by this
//    * class, as well as other classes, like FerroxHealth.cs. Right now this class only changes the value of 'isProjecting to its opposite value.
//    * In the future, it may be benneficial to have it accept a true or false paramater so that the value of 'isProjecing can be 
//    * explicitly chosen. 
//    */
//    public void SetIsProjecting()
//   {
//       isProjecting = !isProjecting;
//   }
