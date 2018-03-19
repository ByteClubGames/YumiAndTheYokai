/*
 *  
 * Author: Spencer Wilson
 * Date Created: 3/16/2018 @ 8:38 pm
 * Date Modified: 3/18/2018 @ 4:57 pm
 * Project: CompSciClubSpring2018
 * File: CircuitPuzzle.cs
 * Description: Script that controls the circuit puzzle.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitPuzzleController : MonoBehaviour {

    public GameObject ring1; // Holds the reference to the Ring1 game object.
    public GameObject ring2; // Holds the reference to the Ring2 game object.
    public GameObject ring3; // Holds the reference to the Ring3 game object.
    public GameObject ring4; // Holds the reference to the Ring4 game object.

    private bool switch1; // Declared a private boolean variable named switch1 that represents whether or not switch1 has been activated.
    private bool switch2; // Declared a private boolean variable named switch2 that represents whether or not switch2 has been activated
    private bool switch3; // Declared a private boolean variable named switch3 that represents whether or not switch3 has been activated.
    private bool switch4; // Declared a private boolean variable named switch4 that represents whether or not switch4 has been activated.

    private void Start()
    {
        switch1 = false; // Initializes switch1 to false at the start of the game.
        switch2 = false; // Initializes switch2 to false at the start of the game.
        switch3 = false; // Initializes switch3 to false at the start of the game
        switch4 = false; // Initializes switch4 to false at the start of the game.
    }

    public void Update()
    {
        SwitchControl(); // Checks if any of the switches have been activated and responses accordingly based upon their status.
    }

    private void SwitchControl() // Private function named SwitchControl that checks whether or not the switches have been activated or not.
    {
        if(switch1)
        {
            StartCoroutine(Rotate(ring1, ring1.transform.rotation, 1)); // Rotates Ring1, the center ring.
            switch1 = false;
        }
        if(switch2)
        {
            StartCoroutine(Rotate(ring2, ring2.transform.rotation, -1)); // Rotates Ring2, the ring second from the center.
            switch2 = false;
        }
        if(switch3)
        {
            StartCoroutine(Rotate(ring3, ring3.transform.rotation, 1)); // Rotates Ring3, the ring third from the center.
            switch3 = false;
        }
        if(switch4)
        {
            StartCoroutine(Rotate(ring4, ring4.transform.rotation, -1)); // Rotates Ring4, the outermost ring.
            switch4 = false;
        }
    }

    public void ActivateSwitch(int i) // Takes an integer value to establish which switch to activate.
    {
        if(i == 0)
        {
            switch1 = true;
        }
        else if(i == 1)
        {
            switch2 = true;
        }
        else if(i == 2)
        {
            switch3 = true;
        }
        else if(i == 3)
        {
            switch4 = true;
        }
    }

    //public void RotateRing1()
    //{
    //    //ring1.GetComponent<Transform>().Rotate(new Vector3(0, 0, 1) * 315 * Time.deltaTime);
    //}

    //public void RotateRing2()
    //{
    //    //ring2.GetComponent<Transform>().Rotate(new Vector3(0, 0, -1) * 90 * Time.deltaTime);
    //}

    //public void RotateRing3()
    //{
    //    //ring3.GetComponent<Transform>().Rotate(new Vector3(0, 0, 1) * 90 * Time.deltaTime);
    //}

    //public void RotateRing4()
    //{
    //    //ring4.GetComponent<Transform>().Rotate(new Vector3(0, 0, -1) * 90 * Time.deltaTime);
    //}

    IEnumerator Rotate(GameObject ring, Quaternion targetRotation, float direction)
    {
        targetRotation *= Quaternion.AngleAxis(60f, direction * Vector3.forward);
        ring.transform.rotation = Quaternion.Lerp(ring.transform.rotation, targetRotation, 10 * 20f * Time.deltaTime);
        yield return null;
    }
}
