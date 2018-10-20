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
    private PlayerController human;
    private PlayerController yokai;
    private PlayerController activePlayer; // Specifies which game object the movement is being called on (Yumi or Yokai)
    private YokaiSwitcher switcher; // Script responsible for spawning and deleting the Yokai
    private SpellCasting spellcaster;
    private VisibilityController invisibleObjects;
    private bool yumiActive; // Flag for if the human is currently active


    void Start()
    {
        human = GameObject.Find("Yumi").GetComponent<PlayerController>();
        switcher = GameObject.Find("Yumi").GetComponentInChildren<YokaiSwitcher>();
        spellcaster = GameObject.Find("Yumi").GetComponentInChildren<SpellCasting>(); //You will need a gameObject attached to Yumi called SpellAbilities. On that object, attach the script SpellCasting and the spell spawner objects to that script.
        invisibleObjects = GameObject.Find("SceneDirector").GetComponentInChildren<VisibilityController>();

        switcher.SetSpawnOffset(true); // If Yokai is spawned, do so on the right side of human by default
        activePlayer = human;
        yumiActive = true;
    }

    void Update()
    {
        activePlayer = yumiActive ? human : yokai; // Choose which character to call movement methods on
                                                   //Debug.Log(activePlayer + " is now active");

        if ((Input.GetKey("a") || Input.GetKey("left")) && (Input.GetKey("d") || Input.GetKey("right")))
        {
            activePlayer.CallLeft(false);
            activePlayer.CallRight(false);
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
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
        if (Input.GetKeyDown("f")) //Updated the mapping to "f" to be in line with most other games. 1-3 will be for spells. -Daniel Jaffe
        {
            if (GameObject.Find("Yokai(Clone)") == null)
            {
                switcher.SpawnYokai();
                invisibleObjects.SetVisible();
                yokai = GameObject.Find("Yokai(Clone)").GetComponent<PlayerController>();                
                activePlayer.ClearCalls();
                SetYumiActive(false);                
            }
            else
            {
                switcher.DeleteYokai(GameObject.Find("Yokai(Clone)"));
                invisibleObjects.SetInvisible();
                SetYumiActive(true);
            }
        }

        // Spell Casting!!! (Finally-- lol)
        if (Input.GetKeyDown("1") && yumiActive) //Press 1 and start the earth spell
        {
            if (spellcaster.GetSpellStatus())
            {
                spellcaster.DestroySpellSpawner();
            }
            spellcaster.CallEarthSpell();
        }
        if (Input.GetKeyDown("2") && yumiActive) //Press 2 and start the ice spell
        {
            if (spellcaster.GetSpellStatus())
            {
                spellcaster.DestroySpellSpawner();
            }
            spellcaster.CallIceSpell();
        }
        if (Input.GetKeyDown("3") && yumiActive) //Press 3 and start the wind spell
        {
            if (spellcaster.GetSpellStatus())
            {
                spellcaster.DestroySpellSpawner();
            }
            spellcaster.CallWindSpell();
        }
        if (Input.GetKeyDown("escape") || (!yumiActive && GameObject.Find("Yokai(Clone)") != null)) //Press escape and cancel out of spell casting mode. Also cancel if Yokai is spawned.
        {
            spellcaster.DestroySpellSpawner();
        }

    }

    public void SetYumiActive(bool active)
    {
        yumiActive = active ? true : false;
    }
}
