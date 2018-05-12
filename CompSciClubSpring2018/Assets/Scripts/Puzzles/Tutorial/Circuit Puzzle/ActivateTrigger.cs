/*
 * 
 * Author: Spencer Wilson
 * Date Created: 3/18/2018 @ 5:29 pm
 * Date Modified: 3/19/2018 @ 4:38 pm
 * Project: CompSciClubSpring2018
 * File: ActivateTrigger.cs
 * Description: This script activates the according switch upon colliding with the ferrox game object.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour {

    public int switchNum; // Public integer that holds the switch number.

    bool greenLight = true;

    public void OnTriggerEnter2D(Collider2D col) // Detects 2D colliders. If the collider tag is named "Ferrox", it triggers the according ring to rotate.
    {
        
        if (col.gameObject.tag == "Ferrox" && greenLight)
        {
            greenLight = false;
            Debug.Log("Ferrox is colliding");
            GameObject.Find("Circuit Puzzle").GetComponent<CircuitPuzzleController>().ActivateSwitch(switchNum - 1);
        }
    }

    public void SetGreenLight()
    {
        greenLight = true;
    }
}
