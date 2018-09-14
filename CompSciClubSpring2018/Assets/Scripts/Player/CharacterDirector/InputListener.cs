/*
 * Programmer: Keiran Glynn, Spencer Wilson
 * Date Created: 07/23/2018 @ 12:30 AM
 * Last Modified: 09/07/2018 @ 7:41 PM
 * File Name: InputListener.cs
 * Description: This class is responsible listening to the keyboard, and calling the corresponding actions (movement, spells, etc). 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InputListener : MonoBehaviour
{
    private PlayerController human;
    private PlayerController yokai;
    private PlayerController activePlayer; // Specifies which game object the movement is being called on (Yumi or Yokai)
    private YokaiSwitcher switcher; // Script responsible for spawning and deleting the Yokai
    private CinemachineBrain cameraBrain;

    private bool yumiActive; // Flag for if the human is currently active

        
    void Start()
    {
        human = GameObject.Find("Player-Human").GetComponent<PlayerController>();        
        switcher = GameObject.Find("Player-Human").GetComponentInChildren<YokaiSwitcher>();
        cameraBrain = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();

        switcher.SetSpawnOffset(true); // If Yokai is spawned, do so on the right side of human by default
        activePlayer = human;
        yumiActive = true;        
    }

    void Update()
    {
        activePlayer = yumiActive ? human : yokai; // Choose which character to call movement methods on
        Debug.Log(activePlayer + " is now active");
                
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            activePlayer.CallLeft(true);
            activePlayer.CallRight(false);
            switcher.SetSpawnOffset(false); // If Yokai is spawned, do so on the left side of human
        }        
        else if (Input.GetKey("d") || Input.GetKey("right"))
        {
            activePlayer.CallLeft(false);
            activePlayer.CallRight(true);
            switcher.SetSpawnOffset(true); // If Yokai is spawned, do so on the right side of human
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
        if (Input.GetKeyDown("1") || Input.GetKeyDown("y"))
        {            
            if (GameObject.Find("Player-Ferrox(Clone)") == null)
            {
                switcher.SpawnYokai();
                yokai = GameObject.Find("Player-Ferrox(Clone)").GetComponent<PlayerController>();
                activePlayer.ClearCalls();
                SetYumiActive(false);
                //cameraBrain.
            }
            else
            {
                switcher.DeleteYokai(GameObject.Find("Player-Ferrox(Clone)"));
                SetYumiActive(true);
            }
        }
    }

    public void SetYumiActive(bool active)
    {
        yumiActive = active ? true : false;        
    }
}
