using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEnabler : MonoBehaviour
{
    private string activeCharacter;
    public GameObject CharacterYumi;
    private GameObject CharacterYokai;
    private YumiMovement yumiMovementScript;
    private YokaiMovement yokaiMovementScript;
    private YokaiSwitcher characterSwitcher;
    private SpellCasting spellcaster;
    private VisibilityController invisibleObjects;

    

    // Start is called before the first frame update
    void Start()
    {
        activeCharacter = "Yumi";
    }

    // Update is called once per frame
    void Update()
    {
        GetCharacterStatus();
    }

    private void GetCharacterStatus()
    {
        if(activeCharacter == "Yumi")
        {

        }
        else if(activeCharacter == "Yokai")
        {

        }
        else
        {
            Debug.Log("The activeCharacter " + activeCharacter + "of the InputEnabler.cs is undefined.");
        }
    }

    public void RequestJump()
    {

    }
    public void RequestCancelJump()
    {

    }
    public void RequestLeft()
    {

    }
    public void RequestRight()
    {

    }
    public void RequestYokai()
    {
        if(activeCharacter == "Yumi")
        {
            activeCharacter = "Yokai";
            characterSwitcher.SpawnYokai();
            invisibleObjects.SetInvisible();
            
        }
        else
        {
            activeCharacter = "Yumi";
            characterSwitcher.DeleteYokai(GameObject.Find("Yokai(Clone)"));
            invisibleObjects.SetInvisible();
        }
    }
    public void RequestEarth()
    {
        if (activeCharacter == "Yumi")
        {
            if (spellcaster.GetSpellStatus())
            {
                spellcaster.DestroySpellSpawner();
            }
            spellcaster.CallEarthSpell();
        }
    }
    public void RequestIce()
    {
        if (activeCharacter == "Yumi")
        {
            if (spellcaster.GetSpellStatus())
            {
                spellcaster.DestroySpellSpawner();
            }
            spellcaster.CallIceSpell();
        }
    }
    public void RequestWind()
    {
        if (activeCharacter == "Yumi")
        {
            if (spellcaster.GetSpellStatus())
            {
                spellcaster.DestroySpellSpawner();
            }
            spellcaster.CallWindSpell();
        }
    }
    public void RequestCancelSpells()
    {
        if (activeCharacter == "Yumi")
        {
            spellcaster.DestroySpellSpawner();
        }
        else
        {
            activeCharacter = "Yumi";
            characterSwitcher.DeleteYokai(GameObject.Find("Yokai(Clone)"));
            invisibleObjects.SetInvisible();
        }
    }
}
