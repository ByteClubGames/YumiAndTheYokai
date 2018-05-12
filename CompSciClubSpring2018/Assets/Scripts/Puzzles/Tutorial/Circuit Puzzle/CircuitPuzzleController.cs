/*
 *  
 * Author: Spencer Wilson
 * Date Created: 3/16/2018 @ 8:38 pm
 * Date Modified: 3/23/2018 @ 10:18 pm
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
    public GameObject switch1GO;
    public GameObject switch2GO;
    public GameObject switch3GO;
    public GameObject switch4GO;
    public float spinAngle1 = 30f;
    public float spinAngle2 = 30f;
    public float spinAngle3 = 30f;
    public float spinAngle4 = 30f;
    public float spinTime = 3f;

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
        if(switch1 || switch2 || switch3 || switch4)
        {
            SwitchControl();
        }
    }

    private void SwitchControl() // Private function named SwitchControl that checks whether or not the switches have been activated or not.
    {
        if(switch1)
        {
            canSwitch1Active = false; // Player cannot activate switch1 until after the ring completes it's rotation.
            //Vector3 axis1 = ring1.transform.rotation.z;
            StartCoroutine(Rotate(ring1, Vector3.forward, spinAngle1, spinTime, switch1GO)); // Rotates Ring1, the center ring.
            canSwitch1Active = true; // 
            switch1 = false;
            //switch1GO.GetComponent<ActivateTrigger>().SetGreenLight();
        }
        if(switch2)
        {
            canSwitch2Active = false; // Player cannot activate switch2 until after the ring completes it's rotation.

            StartCoroutine(Rotate(ring2, Vector3.forward, spinAngle2, spinTime, switch2GO)); // Rotates Ring2, the ring second from the center.
            canSwitch2Active = true;
            switch2 = false;
            //switch2GO.GetComponent<ActivateTrigger>().SetGreenLight();
        }
        if(switch3)
        {
            canSwitch3Active = false; // Player cannot activate switch3 until after the ring completes it's rotation.

            StartCoroutine(Rotate(ring3, Vector3.forward, spinAngle3, spinTime, switch3GO)); // Rotates Ring3, the ring third from the center.
            canSwitch3Active = true;
            switch3 = false;
            //switch3GO.GetComponent<ActivateTrigger>().SetGreenLight();
        }
        if(switch4)
        {
            canSwitch4Active = false; // Player cannot activate switch4 until after the ring completes it's rotation.

            StartCoroutine(Rotate(ring4, Vector3.forward, spinAngle4, spinTime, switch4GO)); // Rotates Ring4, the outermost ring.
            canSwitch4Active = true;
            switch4 = false;
            //switch4GO.GetComponent<ActivateTrigger>().SetGreenLight();
        }
    }

    public void ActivateSwitch(int i) // Takes an integer value to establish which switch to activate.
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

    //IEnumerator WaitAmountOfSeconds(bool switchNum, float duration) // work on
    //{
    //    float start = Time.time;
    //    float end = start + duration;
    //    float progress = 0f;
    //    while(progress < 1)
    //    {
    //        progress = (Time.time - start) / duration;
    //    }
    //    switchNum = false;
    //    yield return null;
    //}


    IEnumerator Rotate(GameObject ring, Vector3 axis, float angle, float duration, GameObject switchObj)
    {
        Quaternion current = ring.transform.rotation;
        Quaternion to = ring.transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0f;
        while(elapsed <= duration)
        {
            ring.transform.rotation = Quaternion.Slerp(current, to, elapsed / duration);
            elapsed += Time.deltaTime;
            Debug.Log("RotateScript made it to here!");
            yield return null;
        }
        ring.transform.rotation = to;
        switchObj.GetComponent<ActivateTrigger>().SetGreenLight();
    }

    //IEnumerator Rotate(GameObject ring, float byDegrees, float duration) // Coroutine function rotates the puzzle rings.
    //{
    //    Vector3 currentE = ring.transform.eulerAngles; // Gets the Euler rotation values from the ring game object and stores them in a Vector3 variable.
    //    Vector3 newRotationE = new Vector3(currentE.x, currentE.y, Mathf.Round(currentE.z + byDegrees)); // Stores the new Euler rotation values that the ring game object will slerp to in a Vector3 variable.
    //    Quaternion currentQ = ring.transform.rotation; // Assigns Quaternion variable currentQ the Quaterion rotation values of the ring game object.
    //    Quaternion newRotationQ = Quaternion.Euler(newRotationE); // Converts newRotationE to Quaternion format and stores it in newRotationQ.
        
    //    if(duration > 0f) // If duration is greater than 0.
    //    {
    //        float startT = Time.time; // startT is assigned the value of Time.time, represents the start.
    //        float endT = startT + duration; // endT is assigned the combined values of startT and duration.
    //        ring.transform.rotation = currentQ; // Assigns the rings current rotation to the values stored in currentQ.
    //        yield return null;

    //        while(Time.time < endT) // While loop that slerps the ring for a duration of time.
    //        {
    //            float progress = (Time.time - startT) / duration; // Gives the perentage value between 0 and 1 that the slerp is currently at.
    //            ring.transform.rotation = Quaternion.Slerp(currentQ, newRotationQ, progress);
    //            yield return null;
    //        }
    //        ring.transform.rotation = newRotationQ; // Sets the ring's rotation to the final rotation that it should end at.
    //    }
    //    yield return null;
    //}    
}