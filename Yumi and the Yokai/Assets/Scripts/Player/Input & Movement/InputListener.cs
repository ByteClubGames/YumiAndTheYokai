/*
 * Programmer: Keiran Glynn, Daniel Jaffe, Spencer Wilson, Michael Sanchez
 * Date Created: 07/23/2018 @ 12:30 AM
 * Last Modified: 10/20/2018 @ 12:34 AM
 * Last Modification By: Daniel Jaffe
 * File Name: InputListener.cs
 * Description: This class is responsible listening to the keyboard, and calling the corresponding actions (movement, spells, etc). 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    InputEnabler InputEnabler;
    Dictionary<string, string> ControlList;

    void Start()
    {
        Dictionary<string, string> ControlList = new Dictionary<string, string>();
        AssignControls();
    }

    

    void Update()
    {
        if (CheckForInput())
        {
            JumpInput();
            CancelJumpInput();
            LeftInput();
            RightInput();
            YokaiInput();
            SpellInput();
        }
        
        // CheckForControlRemapping();
    }

    /// <summary>
    /// Define a keyboard-key for each action by assigning the keyboard-key as a value in this dictionary.
    /// </summary>
    private void AssignControls()
    {        
        ControlList.Add("jump1", "w");
        ControlList.Add("jump2", "up");
        ControlList.Add("jump3", "space");
        ControlList.Add("left1", "a");
        ControlList.Add("left2", "left");
        ControlList.Add("right1", "d");
        ControlList.Add("right2", "right");
        ControlList.Add("yokai", "f");
        ControlList.Add("earth", "1");
        ControlList.Add("ice", "2");
        ControlList.Add("wind", "3");
        ControlList.Add("cancel spells", "escape");
        
        //ControlList.Add("action", "key");
    }

    /// <summary>
    /// Method to edit the keyboard-key associated with a given action.
    /// </summary>
    /// <param name="action">The action that you want to re-assign.</param>
    /// <param name="inputKey">They keyboard-key that you want to assign the action to.</param>
    public void EditControls(string action, string inputKey)
    {
        ControlList[action] = inputKey;
    }

    /// <summary>
    /// Checks to see if any buttons have been pressed this frame, or if a button has been held from a previous frame.
    /// </summary>
    /// <returns></returns>
    private bool CheckForInput()
    {
        return (Input.anyKey && Input.anyKeyDown);
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(ControlList["jump1"]))
        {
            InputEnabler.RequestJump();
        }
        else if (Input.GetKeyDown(ControlList["jump2"]))
        {
            InputEnabler.RequestJump();
        }
        else if (Input.GetKeyDown(ControlList["jump3"]))
        {
            InputEnabler.RequestJump();
        }
    }

    private void CancelJumpInput()
    {
        if (Input.GetKeyUp(ControlList["jump1"]))
        {
            InputEnabler.RequestCancelJump();
        }
        else if (Input.GetKeyUp(ControlList["jump2"]))
        {
            InputEnabler.RequestCancelJump();
        }
        else if (Input.GetKeyUp(ControlList["jump3"]))
        {
            InputEnabler.RequestCancelJump();
        }
    }

    private void LeftInput()
    {
        if (Input.GetKey(ControlList["left1"]))
        {
            InputEnabler.RequestLeft();
        }
        else if (Input.GetKey(ControlList["left2"]))
        {
            InputEnabler.RequestLeft();
        }
    }

    private void RightInput()
    {
        if (Input.GetKey(ControlList["right1"]))
        {
            InputEnabler.RequestRight();
        }
        else if (Input.GetKey(ControlList["right2"]))
        {
            InputEnabler.RequestRight();
        }
    }

    private void YokaiInput()
    {
        if (Input.GetKey(ControlList["yokai"]))
        {
            InputEnabler.RequestYokai();
        }
    }

    private void SpellInput()
    {
        if (Input.GetKeyDown(ControlList["earth"]))
        {
            InputEnabler.RequestEarth();
        }
        else if (Input.GetKeyDown(ControlList["ice"]))
        {
            InputEnabler.RequestIce();
        }
        else if (Input.GetKeyDown(ControlList["wind"]))
        {
            InputEnabler.RequestWind();
        }

        if (Input.mouseScrollDelta.y != 0f)
        {
            //SwapActiveSpell();
        }

        if (Input.GetKeyDown(ControlList["cancel spells"]))
        {
            InputEnabler.RequestCancelSpells();
        }
    }
}
