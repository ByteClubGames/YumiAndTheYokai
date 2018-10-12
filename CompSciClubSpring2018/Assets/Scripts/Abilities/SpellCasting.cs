/*
********************************************************************************
*Creator(s).........................................................Daniel Jaffe
*Created..............................................................10/05/2018
*Last Modified........................................................10/05/2018
*Last Modified by...................................................Daniel Jaffe
*
*Description:   This script is used to create the spawner objects needed to cast
*               Yumi's spells. Note that you will need to attach the three spell
*               spawner objects to the their associated gameObject variables on
*               the script. Note additionally that this just creates the spawner
*               and you will need to utilize a 2nd script to see that they are
*               properly destroyed after casting.
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

    public void DestroySpellSpawner()
    {
        if (spellOn == true)
        {
            Destroy(spellSpawner);
            spellOn = false;
        }
    }
}