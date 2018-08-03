/*
 * Programmer:   Hunter Goodin, Spencer Wilson
 * Date Created: 02/16/2018 @  4:00 PM 
 * Last Updated: 07/31/2018 @  3:17 PM 
 * File Name:    PlayerMovement.cs 
 * Description:  This script will be responsible for the player's movements. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;             // How fast the player character moves 
    public GameObject actingPlayerObj; 
    public bool isLeft;             // This will be toggled in-engine if the object this script is attached to is the left button 
    public bool isRight;            // This will be toggled in-engine if the object this script is attached to is the right button 
    public bool touched;           // This will be true if the object this script is attached to is being touched by the player's finger 
    public GameObject switcher; 

    private void FixedUpdate()
    {
        if ( touched && isLeft )      // If touched is true and isLeft is true 
        {
            actingPlayerObj.transform.Translate(-transform.right * Time.deltaTime * speed);
            //switcher.gameObject.GetComponent<YokaiSwitcher>().ChangeActiveSpawner(false);
        }  

        if ( touched && isRight )     // If touched is true and isRight is true 
        {
            actingPlayerObj.transform.Translate(transform.right * Time.deltaTime * speed);
            //switcher.gameObject.GetComponent<YokaiSwitcher>().ChangeActiveSpawner(true);
        }

        if ( Input.GetKey("a") || Input.GetKey("left") )
        {
            actingPlayerObj.transform.Translate(-transform.right * Time.deltaTime * speed);
            //switcher.gameObject.GetComponent<YokaiSwitcher>().ChangeActiveSpawner(false);
        }

        if ( Input.GetKey("d") || Input.GetKey("right") )
        {
            actingPlayerObj.transform.Translate(transform.right * Time.deltaTime * speed);
            //switcher.gameObject.GetComponent<YokaiSwitcher>().ChangeActiveSpawner(true);
        }

        touched = false; 
    }

    void Move()             // This function will be accessed by the TouchInput.cs script 
    { touched = true; }     // Make touched true 

    void StopMoving()       // This function will be accessed by the TouchInput.cs script 
    { touched = false; }    // make touch false 
}
