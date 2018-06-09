/*
 * 
 * Authors: Spencer Wilson, Keiran Glynn, Hunter Goodin
 * Date Created:  03/05/2018 @  3:11 pm
 * Date Modified: 06/10/2018 @  9:20 pm
 * Project: CompSciClubSpring2018
 * File: PlayerController.cs
 * Description: This class controls the interaction between the player's physical and yokai form as well as any extraneous things.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YokaiSwitcher : MonoBehaviour
{
    public GameObject humanGameObject;  // Public game object that holds the human game object. 
    public GameObject ferroxPrefab;     // Public game object that holds a reference to the ferrox prefab. 
    public Transform spawnerLeft;           // Public transform that holds where the ferrox will spawn. \
    public Transform spawnerRight;
    public bool activeSpawner = true; 
    public Vector3 spawnerLoc; 
    public bool instaProject = false; 

    public void Update()
    {
        CheckInput();
        CheckIfProjecting();
    }

    public void CheckInput()
    {
        if (Input.GetKeyUp("g")) // When "g" is pressed, switch the isProjecting value to it's opposite boolean value.
        {
            instaProject = true; 
        }
    }

    public void CheckIfProjecting()
    {
        if (instaProject)
        {
            if (activeSpawner == true)
            {
                spawnerLoc = spawnerRight.position;
                Instantiate(ferroxPrefab, spawnerLoc, Quaternion.identity);
                instaProject = false;
            }
            if (activeSpawner == false)
            {
                spawnerLoc = spawnerLeft.position;
                Instantiate(ferroxPrefab, spawnerLoc, Quaternion.identity);
                instaProject = false;
            }
        }
    }

    public void SetIsProjecting()
    {
        instaProject = !instaProject;
    }

    public void ChangeActiveSpawner(bool newDir)
    {
        activeSpawner = newDir; 
    }


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
}
