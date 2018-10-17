/*
 *  Authors: Keiran Glynn, Darrell Wong
 *  Date Created: 9/21/2018
 *  Last Modified: 10/5/2018
 *  PlayerDetection.cs
 *  Description: This script will trigger a boolean if the Yumi or the Yokai enter a box collider (set collider as trigger). This bool value can be called on by
 *  other classes to tell enemy's or anything else that the player is nearby.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{

    private bool detected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Yumi" | other.name == "Player-Ferrox(Clone)")
        {
            detected = true;
        }
    }

    public bool PlayerDetected()
    {
        return detected;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Yumi" | other.name == "Player-Ferrox(Clone)")
        {
            detected = false;
        }
    }
}
