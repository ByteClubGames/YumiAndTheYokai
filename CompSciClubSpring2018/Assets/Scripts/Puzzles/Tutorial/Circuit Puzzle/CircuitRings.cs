/*
 *  
 * Author: Spencer Wilson
 * Date Created: 3/16/2018 @ 6:26 pm
 * Date Modified: 3/6/2018 @ 6:27 pm
 * Project: CompSciClubSpring2018
 * File: CircuitRings.cs
 * Description: Script that controls the rotation of the circuit rings.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitRings : MonoBehaviour {

    public GameObject ring4; // Holds the reference to the Ring4 game object.
    public GameObject ring3; // Holds the reference to the Ring3 game object.
    public GameObject ring2; // Holds the reference to the Ring2 game object.
    public GameObject ring1; // Holds the reference to the Ring1 game object.

    public void Update()
    {
        RotateRing1();
    }

    public void RotateRing1() // Rotates Ring1, the center ring.
    {
        ring1.GetComponent<Transform>().Rotate(Vector3.right * 90 * Time.deltaTime);
    }

}
