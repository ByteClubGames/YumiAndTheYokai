/*
 *  
 * Author: Spencer Wilson, Keiran Glynn
 * Date Created: 3/16/2018 @ 8:38 pm
 * Date Modified: 5/12/2018 @ 12:18 pm
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
    public GameObject switch1GO; // References the switch1 game object
    public GameObject switch2GO; // References the switch2 game object
    public GameObject switch3GO; // References the switch3 game object
    public GameObject switch4GO; // References the switch4 game object
    public float spinAngle1 = 30f; // Determines what angle ring1 will rotate at
    public float spinAngle2 = 30f; // Determines what angle ring2 will rotate at
    public float spinAngle3 = 30f; // Determines what angle ring3 will rotate at
    public float spinAngle4 = 30f; // Determines what angle ring4 will rotate at
    public float spinTime = 3f; // Determines how long a ring will take to complete its rotation

    public GameObject levelExit; // Holds the reference to the level exit / door.

    private bool switch1; // Declared a private boolean variable named switch1 that represents whether or not switch1 has been activated.
    private bool switch2; // Declared a private boolean variable named switch2 that represents whether or not switch2 has been activated
    private bool switch3; // Declared a private boolean variable named switch3 that represents whether or not switch3 has been activated.
    private bool switch4; // Declared a private boolean variable named switch4 that represents whether or not switch4 has been activated.

    private bool canSwitch1Active;
    private bool canSwitch2Active;
    private bool canSwitch3Active;
    private bool canSwitch4Active;

    private bool puzzleActive; // Private boolean value that represents whether or not the puzzle is active or not.
    private bool levelExitAccess; // Private boolean value that represents whether or not the player can access the level exit.

    private void Start()
    {       
        switch1 = false; // Initializes switch1 to false at the start of the game.
        switch2 = false; // Initializes switch2 to false at the start of the game.
        switch3 = false; // Initializes switch3 to false at the start of the game
        switch4 = false; // Initializes switch4 to false at the start of the game.

        canSwitch1Active = true;
        canSwitch2Active = true;
        canSwitch3Active = true;
        canSwitch4Active = true;

        levelExitAccess = false; // Game starts with the level exit initially locked.
    }

    public void Update()
    {
        IsPuzzleActive();        
    }

    public void IsPuzzleActive() // Function that checks if the puzzle has been powered on or not.
    {
        if(switch1 || switch2 || switch3 || switch4) //If a switch is being pushed and the ring should rotate, then do action
        {
            SwitchControl(); // responsible for deciding which ring should rotate based on the switch that was activated
        }
    }

    private void SwitchControl() // Private function named SwitchControl that checks whether or not the switches have been activated or not.
    {
        if(switch1)
        {
            canSwitch1Active = false; // Player cannot activate switch1 until after the ring completes it's rotation.
            StartCoroutine(Rotate(ring1, Vector3.forward, spinAngle1, spinTime, switch1GO)); // Rotates Ring1, the center ring.
            canSwitch1Active = true; // 
            switch1 = false;
        }
        if(switch2)
        {
            canSwitch2Active = false; // Player cannot activate switch2 until after the ring completes it's rotation.

            StartCoroutine(Rotate(ring2, Vector3.forward, spinAngle2, spinTime, switch2GO)); // Rotates Ring2, the ring second from the center.
            canSwitch2Active = true;
            switch2 = false;
        }
        if(switch3)
        {
            canSwitch3Active = false; // Player cannot activate switch3 until after the ring completes it's rotation.

            StartCoroutine(Rotate(ring3, Vector3.forward, spinAngle3, spinTime, switch3GO)); // Rotates Ring3, the ring third from the center.
            canSwitch3Active = true;
            switch3 = false;
        }
        if(switch4)
        {
            canSwitch4Active = false; // Player cannot activate switch4 until after the ring completes it's rotation.

            StartCoroutine(Rotate(ring4, Vector3.forward, spinAngle4, spinTime, switch4GO)); // Rotates Ring4, the outermost ring.
            canSwitch4Active = true;
            switch4 = false;
        }
    }

    public void ActivateSwitch(int i) // Takes an integer value to establish which switch to activate. Will activate when called by ActivateTrigger.cs
    {
        if(i == 0 && canSwitch1Active)
        {
            switch1 = true;
            Debug.Log("Switch1 Activated");
        }
        else if(i == 1 && canSwitch2Active)
        {
            switch2 = true;
            Debug.Log("Switch2 Activated");
        }
        else if(i == 2 && canSwitch3Active)
        {
            switch3 = true;
            Debug.Log("Switch3 Activated");
        }
        else if(i == 3 && canSwitch4Active)
        {
            switch4 = true;
            Debug.Log("Switch4 Activated");
        }
    }
    
    IEnumerator Rotate(GameObject ring, Vector3 axis, float angle, float duration, GameObject switchObj)
    {
        Quaternion current = ring.transform.rotation; // holds the current rotation position of the given ring
        Quaternion to = ring.transform.rotation; // Will hold the desired rotation position of the given ring
        to *= Quaternion.Euler(axis * angle); // Modifies the "to" rotation position to what we want it to be

        float elapsed = 0f;
        while(elapsed <= duration) // Compares how much time has elapsed out of how much time we want it to take to rotate the ring
        {
            ring.transform.rotation = Quaternion.Slerp(current, to, elapsed / duration); // "Moves" (rotates) the ring towards its desired rotation
            elapsed += Time.deltaTime; // Measures how much time has passed while completing the rotate movement
            yield return null;
        }
        ring.transform.rotation = to;
        switchObj.GetComponent<ActivateTrigger>().SetGreenLight(); // Calls a function that stops the switch from being activated a second time and tells it that
                                                                   // it can be activated again.
    }    
}