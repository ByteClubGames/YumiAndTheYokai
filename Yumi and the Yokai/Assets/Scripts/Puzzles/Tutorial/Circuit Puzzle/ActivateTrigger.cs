/*
 * 
 * Author: Spencer Wilson, Keiran Glynn
 * Date Created: 3/18/2018 @ 5:29 pm
 * Date Modified: 5/12/2018 @ 12:38 pm
 * Project: CompSciClubSpring2018
 * File: ActivateTrigger.cs
 * Description: This script is responsible for triggering the rotation function of the circuit puzzle when the astral enters the collider for the switch. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour {

    public int switchNum; // Public integer that holds the switch number.

    bool greenLight = true; // Used as a flag to stop repeated presses of the activation switch

    public void OnTriggerEnter2D(Collider2D col) // Detects 2D colliders. If the collider tag is named "Ferrox", it triggers the according ring to rotate.
    {
        
        if (col.gameObject.tag == "Ferrox" && greenLight)
        {
            greenLight = false; // Set to false so that repeated collisions of the ferrox with the switch do nothing
            Debug.Log("Ferrox is colliding");
            GameObject.Find("Circuit Puzzle").GetComponent<CircuitPuzzleController>().ActivateSwitch(switchNum - 1); // Supplies the CiruitPuzzleController with
                                                                                                                     // which switch was pressed
        }
    }

    public void SetGreenLight() // Used to reset the flag and allow another activation of the switch
    {
        greenLight = true;
    }
}
