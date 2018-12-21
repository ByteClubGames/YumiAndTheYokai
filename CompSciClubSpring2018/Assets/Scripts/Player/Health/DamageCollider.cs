/*
********************************************************************************
*Creator(s)........................................................Hunter Goodin
*Created..............................................................12/14/2018
*Last Modified..........................................@ 12:55 PM on 12/21/2018
*Last Modified by..................................................Hunter Goodin
*
*Description: When the player collides with an object with this script attached
*             to it, the player will take damage. 
********************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public int damageDealtToYumi = 1;		// It's public so you can change it in-engine and on a per-prefab basis 
    public int damageDealtToYokai = 20;		// It's public so you can change it in-engine and on a per-prefab basis 

    private void OnTriggerEnter(Collider col)	// When an obj collides with this obj's trigger... 
    {
        if (col.gameObject.name == "Yumi")	// Check if the name is "Yumi" 
        {
            GameObject.Find("Yumi").GetComponent<YumiHealthSystem>().DamageDealer(damageDealtToYumi);	// Search for an obj called "Yumi", get the YumiHealthSystem script attached to it, call the DamageDealer function and pass the value damageDealtToYumi"
        }
        else if (col.gameObject.name == "Yokai(Clone)")
        {
            GameObject.Find("Yokai(Clone)").GetComponent<YokaiHealthSystem>().DamageYokai(damageDealtToYokai);	// Search for an obj called "Yokai(Clone)", get the YokaiHealthSystem script attached to it, call the DamageYokai function and pass the value damageDealtToYokai"
        }
    }
}
