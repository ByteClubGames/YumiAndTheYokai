/*
 * Programmer: Keiran Glynn, Spencer Wilson
 * Date Created: 07/23/2018 @ 12:30 AM
 * Last Modified: 07/31/2018 @ 3:17 PM
 * File Name: InputListener.cs
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    private PlayerController human;
    private PlayerController yokai;
    private PlayerController activePlayer;
    private YokaiSwitcher switcher;
    private Transform cameraTarget;

    private bool yumiActive;




    // Use this for initialization
    void Start()
    {
        human = GameObject.Find("Player-Human").GetComponent<PlayerController>();
        switcher = GameObject.Find("Player-Human").GetComponentInChildren<YokaiSwitcher>();        

        yumiActive = true;
        activePlayer = human;
    }

    void Update()
    {
        activePlayer = yumiActive ? human : yokai;
        Debug.Log(activePlayer);
                
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            activePlayer.CallLeft(true);
            activePlayer.CallRight(false);
            switcher.SetSpawnOffset(false);
        }        
        else if (Input.GetKey("d") || Input.GetKey("right"))
        {
            activePlayer.CallLeft(false);
            activePlayer.CallRight(true);
            switcher.SetSpawnOffset(true);
        }
        else
        {
            activePlayer.CallLeft(false);
            activePlayer.CallRight(false);
        }

        // Jumping
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up") || Input.GetKeyDown("space"))
        {
            activePlayer.CallJump();
        }



        // Character Swap
        if (Input.GetKeyDown("g"))
        {
            //switcherScript.SetFacingRight(facingRight);
            //switcherScript.SetProjection();
            switcher.ProjectYokai();
            yokai = GameObject.Find("Player-Ferrox(Clone)").GetComponent<PlayerController>();
            activePlayer.ClearCalls();
            SetYumiActive(false);
        }
    }

    public void SetYumiActive(bool active)
    {
        yumiActive = active ? true : false;        
    }
}
