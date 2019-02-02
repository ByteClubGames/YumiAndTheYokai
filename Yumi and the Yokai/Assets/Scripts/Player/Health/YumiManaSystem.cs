/*
********************************************************************************
*Creator(s)........................................................Hunter Goodin
*Created...............................................................1/25/2018
*Last Modified...........................................@11:30 AM on 02/02/2019
*Last Modified by..................................................Hunter Goodin
*
*Description: This script handle's the Yumi's Mana system. 
********************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumiManaSystem : MonoBehaviour
{
    public int curMana = 50; 
    public int maxMana = 99;
    public float second = 1.0f;
    public float initialTime;

    void Start()
    {
        SetTime();
    }

    void FixedUpdate()
    {
        if(Input.GetKeyUp("y"))
        {
            UseMana(); 
        }

        if (curMana > maxMana)
        {
            curMana = maxMana;
        }
        else if (curMana < maxMana)
        {
            if (Time.time > initialTime)    // And the current time is higher than the initialTime 
            {
                initialTime++;              // Incriment initialTime
                curMana++;                   // Decrement health 
            }
        }
    }

    public void UseMana()
    {
        curMana -= 33; 
    }

    public void SetTime()               // This function is VERY key for the timing to work... 
    {
        initialTime = Time.time;            // initialTime is set to the current time 
        // finalTime = initialTime + health;   // finalTime is set to the initialTime + the current health  // Commented out because I don't need it anymore. Keeping it for prosperity. 
    }
}
