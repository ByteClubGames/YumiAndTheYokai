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
            StartCoroutine(Rotate(ring1, 60f, 0.3f)); // Rotates Ring1, the center ring.
            switch1 = false;
        }
        if(switch2)
        {
            StartCoroutine(Rotate(ring2, 60f, 0.3f)); // Rotates Ring2, the ring second from the center.
            switch2 = false;
        }
        if(switch3)
        {
            StartCoroutine(Rotate(ring3, 60f, 0.3f)); // Rotates Ring3, the ring third from the center.
            switch3 = false;
        }
        if(switch4)
        {
            StartCoroutine(Rotate(ring4, 60f, 0.3f)); // Rotates Ring4, the outermost ring.
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

    IEnumerator Rotate(GameObject ring, float byDegrees, float duration)
    {
        Vector3 currentE = ring.transform.eulerAngles; // Gets the Euler rotation values from the ring game object and stores them in a Vector3 variable.
        Vector3 newRotationE = new Vector3(currentE.x, currentE.y, currentE.z + byDegrees); // Stores the new Euler rotation values that the ring game object will slerp to in a Vector3 variable.
        Quaternion currentQ = ring.transform.rotation; // Assigns Quaternion variable currentQ the Quaterion rotation values of the ring game object.
        Quaternion newRotationQ = Quaternion.Euler(newRotationE); // Converts newRotationE to Quaternion format and stores it in newRotationQ.
        
        if(duration > 0f) // If duration is greater than 0.
        {
            float startT = Time.time; // startT is assigned the value of Time.time, represents the start.
            float endT = startT + duration; // endT is assigned the combined values of startT and duration.
            yield return null;

            while(Time.time <= endT)
            {
                float progress = (Time.time - startT) / duration; // Gives the perentage value between 0 and 1 that the slerp is currently at.
                ring.transform.rotation = Quaternion.Slerp(currentQ, newRotationQ, progress);
                yield return null;
            }
        }
        yield return null;
    }
}
