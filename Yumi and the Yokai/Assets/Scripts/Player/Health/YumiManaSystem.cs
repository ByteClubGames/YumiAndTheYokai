/*
************************************************************************************************
*Creator(s)........................................................Hunter Goodin, Spencer Wilson
*Created...............................................................1/25/2018
*Last Modified...........................................@11:30 AM on 02/02/2019
*Last Modified by..................................................Spencer Wilson
*
*Description: This script handle's the Yumi's Mana system. 
************************************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumiManaSystem : MonoBehaviour
{
    #region Variables

    private float secondMeter;
    public float currentTime;
    public int curMana = 50; 
    public int maxMana = 100;
    public int manaRegen = 10;
    public int manaDepleteYokai = -5;
    public int manaDepleteSpell = -30;

    public YokaiSwitcher script;

    #region Legacy Code - Hunter
    //public float second = 1.0f;
    //public float initialTime;
    #endregion

    #endregion

    void Start() // MAY DELETE, LEAVE UP TO KIERAN WHETHER OR NOT HE WOULD WANT THE VARIABLES INITIALIZED OR NOT
    {

    }

    void FixedUpdate()
    {
        if (/*YOKAI IS NOT ACITVE AND PLAYER IS NOT CASTING SPELLS && curMana < maxMana*/ !script.getIsProjecting() /*Input.GetKey("y")*/) // Regenerates mana while the yokai is not active and the player is not casting any spells.
        {
            ManaModifier(manaRegen); // Calls on a method that regenerates 10 units of mana every second.
        }
        else
        {
            if (/*While Yokai is in scene*/ script.getIsProjecting() /*Input.GetKey("z")*/) // When Yumi is using the yokai
            {
                ManaModifier(manaDepleteYokai);
            }
            else if (Input.GetKeyDown("x")) // When Yumi is using spells.
            {
                UseManaSpell(manaDepleteSpell);
            }
        }

        if (curMana > maxMana) // Checks if the current mana value is above the maximum mana. If so, set the current mana value max mana's value.
        {
            curMana = maxMana;
        }
        if (curMana <= 0) // Checks if the current mana value is below zero. If so, set the current mana value to zero.
        {
            curMana = 0;
        }
        #region Legacy Code - Hunter
        //else if (curMana < maxMana)
        //{
        //    if (Time.time > initialTime)    // And the current time is higher than the initialTime 
        //    {
        //        initialTime++;              // Incriment initialTime
        //        curMana++;                  // Decrement health 
        //    }
        //}
        #endregion
    }

    private void UseManaSpell(int x) // Takes in some integer x and subtracts it from the current mana.
    {
        curMana += x; 
    }

    private void ManaModifier(int x)
    {
        currentTime = Time.time;

        secondMeter += Time.unscaledDeltaTime; // Incrementing secondMeter by the number of seconds that passed between the current frame and the last frame.
        if(secondMeter >= 1f)
        {
            curMana += x;
            secondMeter = 0f; // Reset secondMeter to zero.
        }
    }

    #region Legacy Code - Hunter
    //public void SetTime()               // This function is VERY key for the timing to work... 
    //{
    //    initialTime = Time.time;            // initialTime is set to the current time 
    //    // finalTime = initialTime + health;   // finalTime is set to the initialTime + the current health  // Commented out because I don't need it anymore. Keeping it for prosperity. 
    //}
    #endregion
}
