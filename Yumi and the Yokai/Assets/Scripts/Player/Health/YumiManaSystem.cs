/*
************************************************************************************************
*Creator(s)........................................................Hunter Goodin, Spencer Wilson
*Created...............................................................1/25/2018
*Last Modified...........................................@9:44 PM on 02/09/2019
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

    public YokaiSwitcher scriptYokaiSwitch;

    private bool lastNumSign; // lastNumberSign determines whether or not the last number passed to ManaModifierTimed was positive or negative. True = Positive, False = Negative.

    private float secondMeter;
    public int curMana = 50; 
    public int maxMana = 100;
    public int manaRegen = 10;
    public int manaDepleteYokai = -5;
    public int manaDepleteSpell = -30;

    #region Legacy Code - Hunter
    //public float second = 1.0f;
    //public float initialTime;
    #endregion

    #endregion

    void FixedUpdate()
    {
        if (!scriptYokaiSwitch.getIsProjecting()) // Regenerates mana while the yokai is not active.
        {
            ManaModifierTimed(manaRegen); // Calls on a method that regenerates 10 units of mana every second.
        }
        else if (scriptYokaiSwitch.getIsProjecting()) // When Yumi is using the yokai
        {
                ManaModifierTimed(manaDepleteYokai);
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

    #region Helper Functions

    public void SpellIsActive() // Called on outside of script if a spell is active. If so, it calls on ManaModifierSpell().
    {
        ManaModifierSpell(manaDepleteSpell);
    }

    private void ManaModifierSpell(int x) // Takes in some integer x and subtracts it from the current mana.
    {
        curMana += x; 
    }

    private void ManaModifierTimed(int x) // Takes in some integer x and modifies curMana's value on a per second basis.
    {
        if (lastNumSign != numSign(x))
            secondMeter = 0f;

        secondMeter += Time.unscaledDeltaTime; // Incrementing secondMeter by the number of seconds that passed between the current frame and the last frame.
        if(secondMeter >= 1f)
        {
            curMana += x;
            secondMeter = 0f; // Reset secondMeter to zero.
        }
        lastNumSign = numSign(x); // Assigns whether or not x is positive or not to lastNumSign.
    }

    // getCurMana()'s function is to help the spells and the projection scripts determine whether or not the player has enough mana to perform certain actions and for when the yokai is destroyed when the player runs out of mana.
    public int getCurMana()
    {
        return curMana;
    }

    public bool numSign(int f) // Determines wheter or not f is a positive number.
    {
        if (f - Mathf.Abs(f) == 0)
            return true;
        return false;
    }

    #endregion

    #region Legacy Code - Hunter
    //public void SetTime()               // This function is VERY key for the timing to work... 
    //{
    //    initialTime = Time.time;            // initialTime is set to the current time 
    //    // finalTime = initialTime + health;   // finalTime is set to the initialTime + the current health  // Commented out because I don't need it anymore. Keeping it for prosperity. 
    //}
    #endregion
}
