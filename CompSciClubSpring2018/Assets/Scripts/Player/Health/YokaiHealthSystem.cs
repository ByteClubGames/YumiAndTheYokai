/*
********************************************************************************
*Creator(s)........................................................Hunter Goodin
*Created..............................................................12/15/2018
*Last Modified...........................................@12:55 PM on 12/21/2018
*Last Modified by..................................................Hunter Goodin
*
*Description: This script handle's the Yokai's health. Every second, the health 
*             will decrement. The health will also go down based on any 
*             potential values passed to DamageYokai(). When the health gets to 
*             zero, the yokai will despawn. 
********************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YokaiHealthSystem : MonoBehaviour
{
    public int health;                         // Base health (this will be more integrated into the mana system later )
    public float initialTime;                       // Explanation later... 
    // public float deltaTime;                         // Unused... keeping for prosperity and potential use 
    // public float finalTime;                         // Unused... keeping for prosperity and potential use 
    public float second;                     // second = one second long 
    private VisibilityController invisibleObjects;  // For the yokai to be able to view invisible things 
    private YokaiSwitcher switcher;                 // Toggle for the yokai 

    void Start()
    {
        switcher = GameObject.Find("Yumi").GetComponentInChildren<YokaiSwitcher>();                             // Gets all the child objs of the obj named "Yumi" and sorts them into switcher 
        invisibleObjects = GameObject.Find("SceneDirector").GetComponentInChildren<VisibilityController>();     // Gets all the child objs of the obj names "SceneDirector" and sorts them into invisibleObjects 
        SetTime();                                                                                              // Calls the SetTime() function 
    }

    void FixedUpdate()
    {
        if (health > 0)                     // If health is greater than 0 
        {
            if (Time.time > initialTime)    // And the current time is higher than the initialTime 
            {
                initialTime++;              // Incriment initialTime
                health--;                   // Decrement health 
            }
        }

        if (health <= 0)                                                                            // If health is less than or equal to 0... 
        {
            second = 1.0f;                                                                          // Make sure second is equal to one second again... 
            health = 60;                                                                            // Make sure health is equal to 60 again... 
            SetTime();                                                                              // Call the SetTime() function 
            switcher.DeleteYokai(GameObject.Find("Yokai(Clone)"));                                  // Delete the current instance of the Yokai prefab 
            invisibleObjects.SetInvisible();                                                        // Toggle SetInvisible 
            GameObject.Find("InputListener").GetComponent<InputListener>().SetYumiActive(true);     // Toggle InputListener 
        } 
    }

    public void DamageYokai(int dam)    // Called when the Yokai is hit by something that does damage 
    {
        health -= dam;          // Decrement the yokai's health 
    }

    public void SetTime()               // This function is VERY key for the timing to work... 
    {
        initialTime = Time.time;            // initialTime is set to the current time 
        // finalTime = initialTime + health;   // finalTime is set to the initialTime + the current health  // Commented out because I don't need it anymore. Keeping it for prosperity. 
    }
}
