/*
 *  Authors: Keiran Glynn
 *  Date Created: 9/21/2018
 *  Last Modified: 9/21/2018
 *  PlayerDetection.cs
 *  Description: This script will trigger a boolean if the Yumi or the Yokai enter a box collider (set collider as trigger). This bool value can be called on by
 *  other classes to tell enemy's or anything else that the player is nearby.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour {

    private bool detected;	

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player-Human" | other.name == "Player-Ferrox(Clone)")
        {
            detected = true;
        }
        else
        {
            detected = false;
        }
    }

    public bool PlayerDetected()
    {
        return detected;
    }
}
