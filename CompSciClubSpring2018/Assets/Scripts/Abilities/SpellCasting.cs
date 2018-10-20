/*
********************************************************************************
*Creator(s).........................................................Daniel Jaffe
*Created..............................................................10/05/2018
*Last Modified............................................@ 7:03PM on 10/12/2018
*Last Modified by...................................................Daniel Jaffe
*
*Description:   This script is used to create or destroy the spawner objects
*               needed to cast Yumi's spells. Note that you will need to attach
*               the three spell spawner objects to the their associated
*               gameObject variables on the script. 
********************************************************************************
 */
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    //Variables
    private bool spellOn;
    private GameObject spellSpawner;
    public GameObject spawnerEarth;
    public GameObject spawnerIce;
    public GameObject spawnerWind;


    //Used to check for if the spell is active
    public bool GetSpellStatus()
    {
        return spellOn;
    }

    //Used to instanciate an earth spell spawner object that can be used to cast the earth spell
    public void CallEarthSpell()
    {
        if (spellOn == false)
        {
            spellSpawner = Instantiate(spawnerEarth);
            spellOn = true;
        }
    }
    //Used to instanciate an ice spell spawner object that can be used to cast the earth spell
    public void CallIceSpell()
    {
        if (spellOn == false)
        {
            spellSpawner = Instantiate(spawnerIce);
            spellOn = true;
        }
    }
    //Used to instanciate a wind spell spawner object that can be used to cast the earth spell
    public void CallWindSpell()
    {
        if (spellOn == false)
        {
            spellSpawner = Instantiate(spawnerWind);
            spellOn = true;
        }
    }
    //Used to destroy the spell spawner and hence prevent further casting of spells
    public void DestroySpellSpawner()
    {
        if (spellOn == true)
        {
            Destroy(spellSpawner);
            spellOn = false;
        }
    }
}